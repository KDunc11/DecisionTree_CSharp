using System;
using System.Collections.Generic;
using System.Text;

namespace ML_Decision_Tree
{
    public class Attribute
    {
        //Constructor
        public Attribute(string name, string[] values)
        {
            AttributeName = name;
            Values = new string[values.Length];
            values.CopyTo(Values, 0);
        }

        //Member Variables
        public string AttributeName { get; set; }
        public string[] Values { get; set; }
    }
}
