using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDll
{
    namespace TestClass1
    {
        public class Class1
        {
            public int field;
            public int Property { get; set; }
            public void Method1() { }
            public int[] arr;
        }

        public static class Class1Ext
        {
            public static void Method2(this Class1 obj) { }
            public static void Method3(this Class1 obj, string str) { }
        }
    }
}
