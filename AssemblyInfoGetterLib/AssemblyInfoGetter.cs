using System;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Reflection;
using System.Collections.ObjectModel;
using System.Linq;

namespace AssemblyInfoGetterLib
{
    public class AssemblyInfoGetter : IInfoGetter
    {
        private Assembly assembly;

        public Node GetFileInfo(string fileName)
        {
            assembly = Assembly.LoadFrom(fileName);
            Node info = new Node(assembly.FullName);
            foreach (var type in assembly.GetTypes())
            {
                GetTypeInfo(SearchNamespaceEntry(info.Nodes, type.ToString()), type);
            }
            return info;
        }

        private ObservableCollection<Node> SearchNamespaceEntry(ObservableCollection<Node> tree, string typeName)
        {
            int dotIndex = typeName.IndexOf('.');
            if (dotIndex == -1)
            {
                return tree;
            }
            Node match = null;
            string namespaceName = typeName.Substring(0, dotIndex);
            foreach (var node in tree)
            {
                if (node.Content == namespaceName)
                {
                    match = node;
                    break;
                }
            }
            if (match == null)
            {
                match = new Node(namespaceName);
                tree.Add(match);
            }
            return SearchNamespaceEntry(match.Nodes, typeName.Substring(dotIndex + 1));
        }

        private void GetTypeInfo(ObservableCollection<Node> tree, Type type)
        {
            if (!type.IsDefined(typeof(ExtensionAttribute), false))
            {
                Node typeInfo = new Node(type.Name);
                tree.Add(typeInfo);

                GetMembersInfo(typeInfo.Nodes, type.GetFields(), "Fields");
                GetMembersInfo(typeInfo.Nodes, type.GetProperties(), "Properties");
                GetMembersInfo(typeInfo.Nodes, type.GetMethods(), "Methods");
                GetMembersInfo(typeInfo.Nodes, GetExtensionMethods(type), "Extension methods");
            }
        }

        private void GetMembersInfo(ObservableCollection<Node> tree, object[] members, string header)
        {
            if (members.Length > 0)
            {
                Node membersInfo = new Node(header);
                tree.Add(membersInfo);
                foreach (var member in members)
                {
                    Node memberInfo = new Node(member.ToString());
                    membersInfo.Nodes.Add(memberInfo);
                }
            }
        }

        private MethodInfo[] GetExtensionMethods(Type extendedType)
        {
            List<MethodInfo> list = new List<MethodInfo>();
            foreach(var type in assembly.GetTypes())
            {
                if (type.IsSealed && !type.IsGenericType && !type.IsNested)
                {
                    foreach(var method in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                    {
                        if (method.IsDefined(typeof(ExtensionAttribute), false))
                        {
                            if (method.GetParameters()[0].ParameterType == extendedType)
                            {
                                list.Add(method);
                            }
                        }
                    }
                }
            }
            return list.ToArray();
        }
    }
}
