using System;
using System.Collections.Generic;
using System.Text;

namespace ML_Decision_Tree
{
    public class TreeNode
    {
        //Default Constructor
        public TreeNode()
        {
            Rule = string.Empty;
            NextAttribute = string.Empty;
        }

        //Constructor
        public TreeNode(string rule, string nextAttribute)
        {
            Rule = rule;
            NextAttribute = nextAttribute;
        }

        //Member Functions
        public void AddChild(string rule, string nextAttribute)
        {
            Children.Add(new TreeNode(rule, nextAttribute));
        }

        //Member Variables
        public List<TreeNode> Children { get; } = new List<TreeNode>();
        public string Rule { get; set; }
        public string NextAttribute { get; set; }
    }
}
