using System;
using System.Collections.Generic;
using System.Text;

namespace ML_Decision_Tree
{
    public class Tuple
    {
        //Constructor
        public Tuple()
        {
            AttributeValues = new Dictionary<string, string>();
        }

        public Tuple(string tupleClass, string[] attributeNames, string[] attributeValues)
        {
            Class = tupleClass;
            AttributeValues = new Dictionary<string, string>();

            for (int i = 0; i < attributeNames.Length; ++i)
            {
                AttributeValues.Add(attributeNames[i], attributeValues[i]);
            }
        }

        public void SetClass(string tupleClass)
        {
            Class = tupleClass;
        }

        //Member Variables
        public Dictionary<string, string> AttributeValues { get; }

        public string Class { get; set;  }
    }
}
