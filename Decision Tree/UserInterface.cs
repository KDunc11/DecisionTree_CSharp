using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace ML_Decision_Tree
{
    class UserInterface
    {
        //Default Constructor
        public UserInterface() { }

        /// <summary>
        /// Takes command line parameters to build Tuples and Decision Trees, also classifies any data sets provided
        /// </summary>
        /// <param name="args"></param>
        public static void Run(string[] args)
        {
            //bool gettingInput = true;
            Console.WriteLine("{0}", OpenMsg); //print opening message to console
            Console.WriteLine("Press Enter to Train a Decision Tree and Classify data");
            Console.ReadLine();
            Console.WriteLine("\n");

            LoadTuples(args);

            try
            {
                LabelData(args);
            }
            catch (Exception e)
            {
                Console.WriteLine("\n" + e.Message);
            }
            Close();
            //Console.WriteLine("1. Enter Training Tuples, will need to specify format and either enter in Tuples manually or load a file.");
            //Console.WriteLine("2. Load Training Tuples From Pre-Existing File/Files (provided via command line arguments or through a provided file).");
            //Console.WriteLine("3. Run Data Set");
            //Console.WriteLine("0. Quit Program");

            //int input;

            //while (gettingInput)
            //{
            //    Console.Write("\nEnter a Number: ");
            //    input = Convert.ToInt32(Console.ReadLine());

            //    if (input < 0 || input > 3)
            //        continue;
            //    else
            //    {
            //        if (input == 1)
            //        {
            //            EnterTuples(); //Lets user format tuples
            //            LoadTuples(args);
            //            Close();
            //        }
            //        else if (input == 2)
            //        {
            //            LoadTuples(args);
            //            // Close();
            //        }
            //        else if (input == 3)
            //        {
            //            LabelData(args);
            //        }
            //        else
            //        {
            //            Close();
            //            gettingInput = false;
            //        }
            //    }
            //}
        }

        /// <summary>
        /// Closes the user interface
        /// </summary>
        public static void Close()
        {
            Console.WriteLine("{0}", CloseMsg);
            Pause();
        }

        public static void Pause()
        {
            Console.WriteLine("Press any key to continue . . . ");
            Console.ReadKey(true);
        }

        //public static void EnterTuples()
        //{
        //    int numOfAttributes;
        //    Console.WriteLine("\nBegin by Entering Tuple Format as Follows.\n\n");
        //    Console.Write("Enter the number of Attributes to be used: ");
        //    numOfAttributes = Convert.ToInt32(Console.ReadLine());

        //    for (int i = 0; i < numOfAttributes; ++i)
        //    {
        //        //Handling Attributes
        //        string attributeName;
        //        Console.Write("\nEnter the name of Attribute {0}: ", i + 1);
        //        attributeName = Console.ReadLine();

        //        //Handling Attribute Values
        //        string[] attrVal;
        //        string val;
        //        Console.Write("\nEnter Values space separated (Ex. 0 1 2) for Attribute {0}: ", /*j + 1,*/ i + 1);
        //        val = Console.ReadLine();
        //        attrVal = val.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        //        //Add to the Attribute List
        //        attributes.Add(new Attribute(attributeName, attrVal));
        //    }

        //    Console.Write("\nEnter User Label Name (Ex. Answer): ");
        //    string labelName = Console.ReadLine();

        //    Console.Write("\nEnter Values space separated (Ex. 0 1 2) for User Label: ");
        //    string labelVal = Console.ReadLine();

        //    Console.WriteLine("\nAttributes successfully entered\n");

        //    string attrCount = Convert.ToString(attributes.Count);
        //    string[] attrNames = new string[attributes.Count];
        //    string[] attrValues = new string[attributes.Count];

        //    for (int i = 0; i < attributes.Count; ++i)
        //    {
        //        attrNames[i] = attributes[i].AttributeName;

        //        string vals = "";

        //        foreach (string str in attributes[i].Values)
        //        {
        //            vals += str;
        //            vals += " ";
        //        }

        //        attrValues[i] = vals;
        //    }


        //    string filePath;
        //    Console.Write("\nEnter File Name (followed by .txt): ");
        //    filePath = Console.ReadLine();

        //    using (StreamWriter output = new StreamWriter(filePath))
        //    {
        //        output.WriteLine(attrCount);

        //        for (int i = 0; i < attributes.Count; ++i)
        //        {
        //            output.Write(attrNames[i] + " ");
        //            output.Write(attrValues[i] + "\n");
        //        }

        //        output.Write(labelName + " " + labelVal);
        //    }

        //    Console.WriteLine("\nSaved to file {0}.", filePath);
        //}

        /// <summary>
        /// Loads data from a training file provided via command line
        /// </summary>
        /// <param name="args"></param>
        public static void LoadTuples(string[] args)
        {
            //int loadChoice;

            //Console.WriteLine("\nDo you have a tuple format file?");
            //Console.WriteLine("1: Yes");
            //Console.WriteLine("2: No");
            //Console.Write("\nEnter a number: ");

            //loadChoice = Convert.ToInt32(Console.ReadLine());

            //if (loadChoice == 1)
            //{
            //    string filePath;

            //    Console.Write("\nEnter the tuple format file's path: ");
            //    filePath = Console.ReadLine();

            //    try
            //    {
            //        string[] inputLines = File.ReadAllLines(filePath);
            //        string[] attributeNames = new string[Convert.ToInt32(inputLines[0])]; //the # of attributes is always the first line

            //        ///for handeling continuous values
            //        List<string> continuousAttributeNames = new List<string>();
            //        Dictionary<string, List<double>> continuousAttributeValues = new Dictionary<string, List<double>>();
            //        List<int> continousIndexes = new List<int>();
            //        ///

            //        for (int n = 0; n < Convert.ToInt32(inputLines[0]); ++n) //iterate through all lines read in from the file
            //        {
            //            string[] attributeInfo = inputLines[n + 1].Split(' ', StringSplitOptions.RemoveEmptyEntries); //split the current line into words
            //                                                                                                          //string name = attributeInfo[0];
            //            string[] attributeValues = new string[attributeInfo.Length - 1];
            //            Array.Copy(attributeInfo, 1, attributeValues, 0, attributeInfo.Length - 1);
            //            attributes.Add(new Attribute(attributeInfo[0], attributeValues));
            //            attributeNames[n] = attributeInfo[0];
            //            if (attributeInfo[1] == "continuous") //account for every attribute marked "continuous"
            //            {
            //                continuousAttributeNames.Add(attributeInfo[0]);
            //                continousIndexes.Add(n);
            //            }
            //        }

            //        //next read in the user label values
            //        string[] labels = inputLines[attributes.Count + 1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            //        string[] AllLabels = new string[labels.Length - 1];
            //        Array.Copy(labels, 1, AllLabels, 0, AllLabels.Length);
            //        Attribute userLabels = new Attribute(labels[0], AllLabels);

            //        Console.Write("\nEnter the file path for the corresponding training data: ");
            //        string trainingDataPath = Console.ReadLine();

            //        string[] inputTrainingData = File.ReadAllLines(trainingDataPath);

            //        string[] allInput = new string[inputLines.Length + inputTrainingData.Length];

            //        for (int i = 0; i < inputLines.Length; ++i)
            //            allInput[i] = inputLines[i];

            //        for (int j = inputLines.Length; j < inputLines.Length + inputTrainingData.Length; ++j)
            //            allInput[j] = inputTrainingData[j - inputLines.Length];

            //        tree = BuildTree(allInput, attributes, attributeNames, continuousAttributeNames, continuousAttributeValues,
            //            continousIndexes, userLabels);
            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine(e.Message);
            //    }
            //}
            //else
            //{
      
                string[] inputLines = File.ReadAllLines(args[0]);
                List<Attribute> attributeArray = new List<Attribute>();
                string[] attributeNames = new string[Convert.ToInt32(inputLines[0])]; //the # of attributes is always the first line

                ///for handeling continuous values
                List<string> continuousAttributeNames = new List<string>();
                Dictionary<string, List<double>> continuousAttributeValues = new Dictionary<string, List<double>>();
                List<int> continousIndexes = new List<int>();
                ///

                for (int n = 0; n < Convert.ToInt32(inputLines[0]); ++n) //iterate through all lines read in from the file
                {
                    string[] attributeInfo = inputLines[n + 1].Split(' ', StringSplitOptions.RemoveEmptyEntries); //split the current line into words
                                                                                                                  //string name = attributeInfo[0];
                    string[] attributeValues = new string[attributeInfo.Length - 1];
                    Array.Copy(attributeInfo, 1, attributeValues, 0, attributeInfo.Length - 1);
                    attributeArray.Add(new Attribute(attributeInfo[0], attributeValues));
                    attributeNames[n] = attributeInfo[0];
                    if (attributeInfo[1] == "continuous") //account for every attribute marked "continuous"
                    {
                        continuousAttributeNames.Add(attributeInfo[0]);
                        continousIndexes.Add(n);
                    }
                }

                //next read in the user label values
                string[] labels = inputLines[attributeArray.Count + 1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                string[] AllLabels = new string[labels.Length - 1];
                Array.Copy(labels, 1, AllLabels, 0, AllLabels.Length);
                Attribute userLabels = new Attribute(labels[0], AllLabels);

                tree = BuildTree(inputLines, attributeArray, attributeNames, continuousAttributeNames, continuousAttributeValues,
                    continousIndexes, userLabels);
          //  }

            Console.WriteLine("\n");

            tree.Print();
        }

        /// <summary>
        /// Splits data into Tuples and builds a Decision Tree
        /// </summary>
        /// <param name="inputLines"></param>
        /// <param name="attributeArray"></param>
        /// <param name="attributeNames"></param>
        /// <param name="continuousAttributeNames"></param>
        /// <param name="continuousAttributeValues"></param>
        /// <param name="continousIndexes"></param>
        /// <param name="userLabels"></param>
        /// <returns></returns>
        public static Tree BuildTree(string[] inputLines, List<Attribute> attributeArray, string[] attributeNames, List<string> continuousAttributeNames,
            Dictionary<string, List<double>> continuousAttributeValues, List<int> continousIndexes, Attribute userLabels)
        {
            List<Tuple> tuples = new List<Tuple>();

            //build the continuous attributes
            List<double>[] continuousVals = new List<double>[continuousAttributeNames.Count];
            for (int i = 0; i < continuousVals.Length; ++i)
            {
                continuousVals[i] = new List<double>();
            }
            for (int i = 0; i < inputLines.Length - attributeArray.Count - 2; ++i)
            {
                string[] tupleInfo = inputLines[i + 2 + attributeArray.Count].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                string[] tupleAttributes = new string[tupleInfo.Length - 1];
                Array.Copy(tupleInfo, 0, tupleAttributes, 0, tupleAttributes.Length);
                tuples.Add(new Tuple(tupleInfo[tupleInfo.Length - 1], attributeNames, tupleAttributes));

                int continuousCount = 0;

                for (int j = 0; j < tupleAttributes.Length; ++j)
                {
                    if (continousIndexes.Contains(j))
                    {
                        continuousVals[continuousCount].Add(Convert.ToDouble(tupleAttributes[j]));
                        ++continuousCount;
                    }
                }
            }

            for (int i = 0; i < continuousAttributeNames.Count; i++)
            {
                continuousAttributeValues.Add(continuousAttributeNames[i], continuousVals[i]);
                continuousAttributeValues[continuousAttributeNames[i]].Sort();
            }
            foreach (string contName in continuousAttributeNames)
            {
                double split;
                split = (continuousAttributeValues[contName][0] + continuousAttributeValues[contName][1]) / 2;
                double minExpectInfo = Double.MaxValue;

                for (int i = 0; i < tuples.Count - 1; ++i)
                {
                    double splitPoint = (continuousAttributeValues[contName][0] + continuousAttributeValues[contName][i + 1]) / 2;
                    List<Tuple>[] subList = new List<Tuple>[2];


                    for (int j = 0; j < subList.Length; ++j)
                    {
                        subList[j] = new List<Tuple>();
                    }
                    foreach (Tuple tuple in tuples)
                    {
                        if (Convert.ToDouble(tuple.AttributeValues[contName]) <= splitPoint)
                        {
                            subList[0].Add(tuple);
                        }
                        else
                        {
                            subList[1].Add(tuple);
                        }
                    }
                    double expectedInfo = Equations.ExpectedInfoWithPartition(tuples, subList, userLabels);

                    if (expectedInfo < minExpectInfo)
                    {
                        minExpectInfo = expectedInfo;
                        split = splitPoint;
                    }
                }
                for (int i = 0; i < tuples.Count; ++i)
                {
                    if (split != 0)
                    {
                        if (Convert.ToDouble(tuples[i].AttributeValues[contName]) <= split)
                        {
                            tuples[i].AttributeValues[contName] = " <= " + split.ToString("F1");
                        }
                        else
                        {
                            tuples[i].AttributeValues[contName] = " > " + split.ToString("F1");
                        }
                    }
                    else
                    {
                        if (Convert.ToDouble(tuples[i].AttributeValues[contName]) <= split)
                        {
                            Convert.ToInt32(split);
                            tuples[i].AttributeValues[contName] = " <= " + split.ToString();
                        }
                        else
                        {
                            Convert.ToInt32(split);
                            tuples[i].AttributeValues[contName] = " > " + split.ToString();
                        }
                    }
                }

                int contAttributeIndex = -1;

                for (int i = 0; i < attributeArray.Count; ++i)
                {
                    if (attributeArray[i].AttributeName == contName)
                    {
                        contAttributeIndex = i;
                        break;
                    }
                }

                attributeArray[contAttributeIndex].Values = new string[2];
                if (split != 0)
                {
                    attributeArray[contAttributeIndex].Values[0] = " <= " + split.ToString("F1");
                    attributeArray[contAttributeIndex].Values[1] = " > " + split.ToString("F1");
                }
                else //split = 0 so no need for the extra decimal place -> 0.0
                {
                    attributeArray[contAttributeIndex].Values[0] = " <= " + split.ToString();
                    attributeArray[contAttributeIndex].Values[1] = " > " + split.ToString();
                }

            }

            //build the tree
            Tree decisionTree = new Tree(string.Empty, string.Empty);
            decisionTree.TreeRecursion(tuples, decisionTree.Root, attributeArray, userLabels, continuousAttributeNames);

            return decisionTree;
        }

        /// <summary>
        /// Labels data sets provided via command lines
        /// </summary>
        /// <param name="args"></param>
        public static void LabelData(string[] args)
        {
            List<decimal> accuracies = new List<decimal>();
            List<decimal> precisions = new List<decimal>();
            List<decimal> recalls = new List<decimal>();
            int totalTP = 0;
            int totalFP = 0;
            int totalTN = 0;
            int totalFN = 0;

            for (int z = 1; z < args.Length; ++z)
            {
                // Console.Write("Enter the file path of the desired data set to run through the decision tree: ");
                string filePath = args[z];

                string[] file = File.ReadAllLines(filePath);
                int attrCount = Convert.ToInt32(file[0]);
                string[] userLabels = new string[file.Length - attrCount - 1];
                string[] attrNames = new string[attrCount];

                for (int i = 0; i < attrCount; ++i)
                {
                    attrNames[i] = file[i + 1];
                }


                for (int i = 0; i < file.Length - attrNames.Length - 1; ++i)
                {
                    string[] tupleVals = file[i + attrNames.Length + 1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    userLabels[i] = tupleVals[tupleVals.Length - 1]; //Last value in tuple is the user label, want to keep track of the original labels for accuracy test
                    tupleVals[tupleVals.Length - 1] = "Null";
                    string[] attributeVals = new string[tupleVals.Length - 1];
                    Array.Copy(tupleVals, 0, attributeVals, 0, attributeVals.Length);
                    tuples.Add(new Tuple(tupleVals[tupleVals.Length - 1], attrNames, attributeVals));
                }

                userLabelsDataSet = userLabels; //store the user labels in order for reference later
                string[] uniqueLabels = userLabels.Distinct().ToArray();
                Array.Sort(uniqueLabels);

                int wordCnt = 0;

                foreach (string str in uniqueLabels)
                {
                    char ch;

                    for (int i = 0; i < str.Length; ++i)
                    {
                        ch = str[i];

                        if (System.Char.IsDigit(ch))
                            break;
                        else if (System.Char.IsLetter(ch) && i == str.Length - 1)
                            wordCnt++;
                    }
                }

                if (wordCnt == uniqueLabels.Length)
                    Array.Reverse(uniqueLabels);

                Attribute labels = new Attribute("labels", uniqueLabels);

                tree.Classify(tuples, tree.Root, attributes, labels);

                int truePositive = 0;
                int trueNegative = 0;
                int falsePositive = 0;
                int falseNegative = 0;

                for (int i = 0; i < tuples.Count && i < userLabelsDataSet.Length; ++i)
                {
                    if (tuples[i].Class == userLabelsDataSet[i] && tuples[i].Class == uniqueLabels[0])
                        truePositive++;
                    if (tuples[i].Class == userLabelsDataSet[i] && tuples[i].Class == uniqueLabels[1])
                        trueNegative++;
                    if (tuples[i].Class != userLabelsDataSet[i] && tuples[i].Class == uniqueLabels[0])
                        falseNegative++;
                    if (tuples[i].Class != userLabelsDataSet[i] && tuples[i].Class == uniqueLabels[1])
                        falsePositive++;
                }

                Matrix = new ConfusionMatrix(truePositive, trueNegative, falsePositive, falseNegative);
                totalTP += truePositive;
                totalFP += falsePositive;
                totalTN += trueNegative;
                totalFN += falseNegative;

                tree.CalcAccuracy(truePositive, trueNegative, falsePositive, falseNegative);
                tree.CalcPrecision(truePositive, falsePositive);
                tree.CalcRecall(truePositive, falseNegative);
                accuracies.Add(tree.Accuracy);
                precisions.Add(tree.Precision);
                recalls.Add(tree.Recall);

                Console.WriteLine("\n\tTuples Classified: {0}", tuples.Count);
                Console.WriteLine("\n\tConfusion Matrix:");
                Matrix.Print();

                decimal accuracy = tree.Accuracy * 100;
                decimal precision = tree.Precision * 100;
                decimal recall = tree.Recall * 100;

                string strAccr = Convert.ToString(accuracy);
                
                if (strAccr.Length > 5)
                    strAccr = strAccr.Substring(0, 5);
                
                strAccr = strAccr + "%";

                string strPrec = Convert.ToString(precision);
                
                if (strPrec.Length > 5)
                    strPrec = strPrec.Substring(0, 5);
                
                strPrec = strPrec + "%";

                string strRec = Convert.ToString(recall);
                
                if (strRec.Length > 5)
                    strRec = strRec.Substring(0, 5);
                
                strRec = strRec + "%";

                Console.WriteLine("\n\tAccuracy: {0}", strAccr);
                Console.WriteLine("\n\tPrecision: {0}", strPrec);
                Console.WriteLine("\n\tRecall: {0}", strRec);
            } 

            if (args.Length - 1 != 1)
            {
                decimal avgAccuracy = 0;
                decimal avgPrecision = 0;
                decimal avgRecall = 0;

                for (int i = 0; i < accuracies.Count && i < precisions.Count && i < recalls.Count; ++i)
                {
                    avgAccuracy += accuracies[i];
                    avgPrecision += precisions[i];
                    avgRecall += recalls[i];
                }

                avgAccuracy = (avgAccuracy / accuracies.Count);
                avgPrecision = (avgPrecision / precisions.Count);
                avgRecall = (avgRecall / recalls.Count);


                Console.WriteLine("\n\n_Final Report_");
                
                ConfusionMatrix final = new ConfusionMatrix(totalTP, totalTN, totalFP, totalFN);

                Console.WriteLine("\nData Set Confusion Matrix:");
                final.Print();

                avgAccuracy = avgAccuracy * 100;
                avgPrecision = avgPrecision * 100;
                avgRecall = avgRecall * 100;

                string strAvgAcc = Convert.ToString(avgAccuracy);
                
                if (strAvgAcc.Length > 5)
                    strAvgAcc = strAvgAcc.Substring(0, 5);
                
                strAvgAcc = strAvgAcc + "%";

                string strAvgPre = Convert.ToString(avgPrecision);
                
                if (strAvgPre.Length > 5)
                    strAvgPre = strAvgPre.Substring(0, 5);
                
                strAvgPre = strAvgPre + "%";

                string strAvgRec = Convert.ToString(avgRecall);
                
                if (strAvgRec.Length > 5)
                    strAvgRec = strAvgRec.Substring(0, 5);
                
                strAvgRec = strAvgRec + "%";

                Console.WriteLine("\nAverage Accuracy: {0}", strAvgAcc);
                Console.WriteLine("\nAverage Precision: {0}", strAvgPre);
                Console.WriteLine("\nAverage Recall: {0}", strAvgRec);
            }
        } //end of LabelData function

        //Member Variables
        public static List<Attribute> attributes { get; set; } = new List<Attribute>();
        public static List<Tuple> tuples { get; set; } = new List<Tuple>();
        public static List<Tuple> dataSetTuples { get; set; } = new List<Tuple>();
        public static Tree tree { get; set; }
        public static string[] userLabelsDataSet { get; set; }
        public static ConfusionMatrix Matrix { get; set; }

        private readonly static string OpenMsg = "Welcome to C# Decision Tree! Follow the steps below.\n\n";
        private readonly static string CloseMsg = "\nProgram Closed.";
    }
}

//Training Set
// "C:\Users\Kyle\Desktop\NNU\StockQuotes07-TrainingSet.txt"

//Complete Data Set
// C:\Users\Kyle\Desktop\NNU\StockQuotes07-ClassifyDataSet.txt

// Data Sets
// C:\Users\Kyle\Desktop\NNU\StockQuotes07 - DataSet.txt
// C:\Users\Kyle\Desktop\NNU\StockQuotes07 - DataSet(Complete).txt