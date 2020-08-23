using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Dynamic;
using System.IO;

namespace ML_Decision_Tree
{
    public class Tree
    {
        //Default Constructor
        public Tree()
        {
            Root = new TreeNode();
        }

        //Constructor
        public Tree(string rule, string nextAttribute)
        {
            Root = new TreeNode(rule, nextAttribute);
        }

        //Member Functions

        //recursively build the tree by splitting on a particular attribute, based on information gain
        public void TreeRecursion(List<Tuple> tupleArray, TreeNode parentNode, List<Attribute> attributeArray, Attribute userLabels, List<string> continuousAttributeNames, TreeNode prevNode = null, int childIndex = -1)
        { 
            if (tupleArray.Count > 0 && attributeArray.Count > 0 && !Equations.SubListClassesAreSame(tupleArray))
            {
                string bestAttribute = string.Empty;
                double minExpectInfo = double.MaxValue;

                foreach (Attribute attribute in attributeArray)
                {
                    Dictionary<string, List<Tuple>> attrSubLists = new Dictionary<string, List<Tuple>>();

                    foreach (string value in attribute.Values)
                    {
                        attrSubLists.Add(value, new List<Tuple>());
                    }
                    foreach (Tuple t in tupleArray)
                    {
                        attrSubLists[t.AttributeValues[attribute.AttributeName]].Add(t);
                    }

                    List<Tuple>[] attrSubListArray = new List<Tuple>[attrSubLists.Count];

                    for (int i = 0; i < attrSubLists.Count; ++i)
                    {
                        attrSubListArray[i] = attrSubLists[attribute.Values[i]];
                    }

                    double expectedInfo = Equations.ExpectedInfoWithPartition(tupleArray, attrSubListArray, userLabels);

                    if (expectedInfo < minExpectInfo)
                    {
                        minExpectInfo = expectedInfo;
                        bestAttribute = attribute.AttributeName;
                    }
                }

                parentNode.NextAttribute = bestAttribute;

                Dictionary<string, List<Tuple>> subLists = new Dictionary<string, List<Tuple>>();

                int bestAttributeIndex = -1;

                for (int i = 0; i < attributeArray.Count; ++i)
                {
                    if (attributeArray[i].AttributeName == bestAttribute)
                    {
                        bestAttributeIndex = i;
                        break;
                    }
                }

                foreach (string value in attributeArray[bestAttributeIndex].Values)
                {
                    subLists.Add(value, new List<Tuple>());
                }
                foreach (Tuple t in tupleArray)
                {
                    subLists[t.AttributeValues[attributeArray[bestAttributeIndex].AttributeName]].Add(t);
                }

                List<Tuple>[] subListArray = new List<Tuple>[subLists.Count];

                for (int i = 0; i < subLists.Count; ++i)
                {
                    subListArray[i] = subLists[attributeArray[bestAttributeIndex].Values[i]];
                }

                List<Attribute> prunedAttributeArray = new List<Attribute>(attributeArray);
                prunedAttributeArray.RemoveAt(bestAttributeIndex);

                //for (int i = 0; i < subListArray.Length; ++i)
                int index = 0;
                int iteration = 0;
                while(iteration < subListArray.Length)
                {
                    if (continuousAttributeNames.Contains(bestAttribute))
                    {
                        parentNode.AddChild(attributeArray[bestAttributeIndex].Values[iteration] + ":", string.Empty);
                    }
                    else
                    {
                        parentNode.AddChild("=" + attributeArray[bestAttributeIndex].Values[iteration] + ":", string.Empty);
                    }
                    TreeRecursion(subListArray[iteration], parentNode.Children[index], prunedAttributeArray, userLabels, continuousAttributeNames, parentNode, index);
                    if (pruned_status)
                    {
                        parentNode.Children.RemoveAt(index);
                        pruned_status = false;
                        index = 0;
                        iteration++;
                        continue;
                    }
                    iteration++;
                    index++;
                }
            }
            else if (tupleArray.Count > 0 && attributeArray.Count == 0)
            {
                Dictionary<string, int> classCount = new Dictionary<string, int>();

                foreach (string tupleClass in userLabels.Values)
                {
                    classCount.Add(tupleClass, 0);
                }

                foreach (Tuple tuple in tupleArray)
                {
                    ++classCount[tuple.Class];
                }

                int maxFrequency = int.MinValue;
                string modeClass = string.Empty;

                foreach (string tupleClass in userLabels.Values)
                {
                    if (classCount[tupleClass] > maxFrequency)
                    {
                        maxFrequency = classCount[tupleClass];
                        modeClass = tupleClass;
                    }
                }

                parentNode.NextAttribute = modeClass;
            }
            else if (tupleArray.Count == 0) //no tuples left left to branch off of
            {
                if (childIndex != -1) //prune the child
                    pruned_status = true;
                else
                    parentNode.NextAttribute = "No data left";
            }
            else //all the tuples are of the same class
            {
                parentNode.NextAttribute = tupleArray[0].Class;
            }
        }

        //classify tuples after training a tree
        public void Classify(List<Tuple> tuples, TreeNode currentNode, List<Attribute> attributes, Attribute userLabels)
        {
            List<Tuple> classifiedTuples = new List<Tuple>();

            foreach (Tuple t in tuples) //go through the entire list of tuples and classify each one
            {
                bool classified = false;

                while (!classified)
                {
                    foreach (TreeNode node in currentNode.Children)
                    {
                        if (currentNode.NextAttribute == "No data left")
                        {
                            t.SetClass("No data left");
                            classified = true;
                            classifiedTuples.Add(t);
                            break;
                        }

                        else
                        {
                            string attributeVal = t.AttributeValues[currentNode.NextAttribute];

                            if (attributeVal.Any(Char.IsDigit))
                            {
                                if (node.Rule.Contains("<="))
                                {
                                    string rule = node.Rule.Substring(3);
                                    rule = rule.TrimEnd(':');
                                    rule = rule.Trim();

                                    if (Convert.ToDecimal(attributeVal) <= Convert.ToDecimal(rule))
                                        currentNode = node;
                                }

                                if (node.Rule.Contains(">"))
                                {
                                    string rule = node.Rule.Substring(2);
                                    rule = rule.TrimEnd(':');
                                    rule = rule.Trim();


                                    if (Convert.ToDecimal(attributeVal) > Convert.ToDecimal(rule))
                                        currentNode = node;
                                }

                                if (node.Rule == attributeVal)
                                {
                                    currentNode = node;
                                }

                                if (node.Children.Count == 0)
                                {
                                    var leaf = CheckContinuousLeaf(node, userLabels, attributeVal);

                                    if (leaf.Item1)
                                    {
                                        t.SetClass(leaf.Item2);
                                        classified = true;
                                        classifiedTuples.Add(t);
                                        break;
                                    }
                                    else
                                    {
                                        t.SetClass(userLabels.Values[userLabels.Values.Length - 1]);
                                        classified = true;
                                        classifiedTuples.Add(t);
                                        break;
                                    }
                                }
                                //if (node.NextAttribute == userLabels.Values[0] && node.Children.Count == 0)
                                //{
                                //    if (node.Rule.Contains("<="))
                                //    {
                                //        string rule = node.Rule.Substring(3);
                                //        rule = rule.TrimEnd(':');
                                //        rule = rule.Trim();


                                //        if (Convert.ToDecimal(attributeVal) <= Convert.ToDecimal(rule))
                                //        {
                                //            t.SetClass(node.NextAttribute);
                                //            classified = true;
                                //            classifiedTuples.Add(t);
                                //            break;
                                //        }
                                //    }
                                //    else if (node.Rule.Contains(">"))
                                //    {
                                //        string rule = node.Rule.Substring(2);
                                //        rule = rule.TrimEnd(':');
                                //        rule = rule.Trim();

                                //        if (Convert.ToDecimal(attributeVal) > Convert.ToDecimal(rule))
                                //        {
                                //            t.SetClass(node.NextAttribute);
                                //            classified = true;
                                //            classifiedTuples.Add(t);
                                //            break;
                                //        }
                                //    }
                                //}

                                //if (currentNode.NextAttribute == userLabels.Values[1] && node.Children.Count == 0)
                                //{
                                //    if (node.Rule.Contains(">"))
                                //    {
                                //        string rule = node.Rule.Substring(2);
                                //        rule = rule.TrimEnd(':');
                                //        rule = rule.Trim();

                                //        if (Convert.ToDecimal(attributeVal) > Convert.ToDecimal(rule))
                                //        {
                                //            t.SetClass(node.NextAttribute);
                                //            classified = true;
                                //            classifiedTuples.Add(t);
                                //            break;
                                //        }
                                //    }
                                //    else if (node.Rule.Contains("<="))
                                //    {
                                //        string rule = node.Rule.Substring(3);
                                //        rule = rule.TrimEnd(':');
                                //        rule = rule.Trim();


                                //        if (Convert.ToDecimal(attributeVal) <= Convert.ToDecimal(rule))
                                //        {
                                //            t.SetClass(node.NextAttribute);
                                //            classified = true;
                                //            classifiedTuples.Add(t);
                                //            break;
                                //        }
                                //    }
                                //}
                            }
                            else
                            {
                                string rule = node.Rule;
                                rule = rule.Trim('=');
                                rule = rule.TrimEnd(':');
                                rule = rule.Trim();

                                if (rule == attributeVal)
                                    currentNode = node;

                                if (node.Children.Count == 0)
                                {
                                    var leaf = CheckDiscreteLeaf(node, userLabels);
                                    
                                    if (leaf.Item1)
                                    {
                                        t.SetClass(leaf.Item2);
                                        classified = true;
                                        classifiedTuples.Add(t);
                                        break;
                                    }
                                    else
                                    {
                                        t.SetClass(userLabels.Values[userLabels.Values.Length - 1]);
                                        classified = true;
                                        classifiedTuples.Add(t);
                                        break;
                                    }
                                }
                                //if (node.NextAttribute == userLabels.Values[0] && node.Children.Count == 0)
                                //{
                                //    t.SetClass(node.NextAttribute);
                                //    classified = true;
                                //    classifiedTuples.Add(t);
                                //    break;
                                //}

                                //if (node.NextAttribute == userLabels.Values[1] && node.Children.Count == 0)
                                //{
                                //    t.SetClass(node.NextAttribute);
                                //    classified = true;
                                //    classifiedTuples.Add(t);
                                //    break;
                                //}
                            }
                        }
                    }
                }
                currentNode = Root;
            }
            tuples = classifiedTuples;
        }

        public void CalcAccuracy(decimal truePos, decimal trueNeg, decimal falsePos, decimal falseNeg)
        {
            if ((truePos + trueNeg + falseNeg + falsePos) != 0)
                Accuracy = (truePos + trueNeg) / (truePos + trueNeg + falseNeg + falsePos);
            else
                Accuracy = 0;
        }

        public void CalcPrecision(decimal truePos, decimal falsePos)
        {
            if ((truePos + falsePos) != 0)
                Precision = (truePos / (truePos + falsePos));
            else
                Precision = 0;
        }

        public void CalcRecall(decimal truePos, decimal falseNeg)
        {
            if ((truePos + falseNeg) != 0)
                Recall = truePos / (truePos + falseNeg);
            else
                Recall = 0;
        }

        public void Print(StreamWriter writer) //calls the private function to recusrively print the tree using a depth first search
        {
            PrintDFS(Root, writer);
        }

        private void PrintDFS(TreeNode node, StreamWriter writer, int tabs = 0)
        {
            if (node != null) //checks if the tree is empty or not
            {
                if (node.Children.Count != 0) //if there are children then iterate through them
                {
                    for (int i = 0; i < node.Children.Count; ++i)
                    {
                        if (tabs == 0)
                        {
                            writer.Write(node.NextAttribute);
                            writer.WriteLine(node.Children[i].Rule);
                            Console.Write(node.NextAttribute); //Prints the node with the next highest info gain
                            Console.WriteLine(node.Children[i].Rule); //Prints the rule for each child
                        }
                        else
                        {
                            for (int j = 0; j < tabs; ++j) //formats the output
                            {
                                writer.Write("\t");
                                Console.Write("\t");
                            }

                            writer.Write(node.NextAttribute);
                            writer.WriteLine(node.Children[i].Rule);
                            Console.Write(node.NextAttribute); //Prints the node with the next highest info gain
                            Console.WriteLine(node.Children[i].Rule); //Prints the rule for each child
                        }

                        PrintDFS(node.Children[i], writer, ++tabs);
                        tabs--;
                    }
                }

                else //no children so a leaf node is reached
                {
                    for (int j = 0; j < tabs; ++j) //formats the output
                    {
                        writer.Write("\t");
                        Console.Write("\t");
                    }

                    writer.WriteLine(node.NextAttribute);
                    Console.WriteLine(node.NextAttribute); //Prints the value of the leaf node
                }
            }
            else
                Console.WriteLine("Cannot print, the tree was empty");
        }

        private (bool, string) CheckDiscreteLeaf(TreeNode node, Attribute userLabels)
        {
            bool classified = false;
            string label = "";

            foreach (string value in userLabels.Values)
            {
                if (value == node.NextAttribute)
                {
                    label = value;
                    classified = true;
                    break;
                }
            }

            return (classified, label);
        }

        private (bool, string) CheckContinuousLeaf(TreeNode node, Attribute userLabels, string attributeValue)
        {
            bool classified = false;
            string label = "";

            foreach (string value in userLabels.Values)
            {
                if (value == node.NextAttribute)
                {
                    if (node.Rule.Contains("<="))
                    {
                        string rule = node.Rule.Substring(3);
                        rule = rule.TrimEnd(':');
                        rule = rule.Trim();

                        if (Convert.ToDecimal(attributeValue) <= Convert.ToDecimal(rule))
                        {
                            label = value;
                            classified = true;
                            break;
                        }
                    }

                    else if (node.Rule.Contains(">"))
                    {
                        string rule = node.Rule.Substring(2);
                        rule = rule.TrimEnd(':');
                        rule = rule.Trim();

                        if (Convert.ToDecimal(attributeValue) > Convert.ToDecimal(rule))
                        {
                            label = value;
                            classified = true;
                            break;
                        }
                    }
                }
            }
            return (classified, label);
        }

        //Member Variables
        public TreeNode Root { get; }
        public decimal Accuracy { get; set; }
        public decimal Precision { get; set; }
        public decimal Recall { get; set; }

        private bool pruned_status = false;
    }
}
