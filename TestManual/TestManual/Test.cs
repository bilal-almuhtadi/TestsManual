using System;
using System.Collections.Generic;
using System.Text;

namespace TestManual
{
    public class Test
    {
        public string name;
        public string lab;
        public List<Leaf> list;

        public Test()
        {
            list = new List<Leaf>();
        }

        Test(string name, string lab, List<Leaf> list)
        {
            this.name = name;
            this.lab = lab;
            this.list = list;
        }
    }

    public class Leaf
    {
        public string name;
        public string val;

        public Leaf()
        {
        }
        public Leaf(string name, string val) 
        {
            this.name = name;
            this.val = val;
        }
    }
}
