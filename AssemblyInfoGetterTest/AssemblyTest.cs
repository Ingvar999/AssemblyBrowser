using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AssemblyInfoGetterLib;

namespace AssemblyInfoGetterTest
{
    [TestClass]
    public class AssemblyTest
    {
        private Node Setup()
        {
            IInfoGetter getter = new AssemblyInfoGetter();
            return getter.GetFileInfo("TestDll.dll");
        }

        [TestMethod]
        public void LoadingAssemblyTest()
        {
            Node actual = Setup();
            Assert.IsNotNull(actual);
        }

        [TestMethod]
        public void AssemblyNameTest()
        {
            Node actual = Setup();
            Assert.IsTrue(actual.Content.StartsWith("TestDll"));
        }

        [TestMethod]
        public void CompositeNamespaceTest()
        {
            Node actual = Setup();
            Assert.AreEqual("TestDll", actual.Nodes[0].Content);
            Assert.AreEqual("TestClass1", actual.Nodes[0].Nodes[0].Content);
        }

        [TestMethod]
        public void TypeNameTest()
        {
            Node actual = Setup();
            Assert.AreEqual("Class1", actual.Nodes[0].Nodes[0].Nodes[0].Content);
        }

        [TestMethod]
        public void FieldsIdentifyTest()
        {
            Node actual = Setup();
            Assert.AreEqual("Fields", actual.Nodes[0].Nodes[0].Nodes[0].Nodes[0].Content);
        }

        [TestMethod]
        public void PropertiesIdentifyTest()
        {
            Node actual = Setup();
            Assert.AreEqual("Properties", actual.Nodes[0].Nodes[0].Nodes[0].Nodes[1].Content);
        }

        [TestMethod]
        public void MethodsIdentifyTest()
        {
            Node actual = Setup();
            Assert.AreEqual("Methods", actual.Nodes[0].Nodes[0].Nodes[0].Nodes[2].Content);
        }

        [TestMethod]
        public void ExtensionMethodsIdentifyTest()
        {
            Node actual = Setup();
            Assert.AreEqual("Extension methods", actual.Nodes[0].Nodes[0].Nodes[0].Nodes[3].Content);
        }
    }
}
