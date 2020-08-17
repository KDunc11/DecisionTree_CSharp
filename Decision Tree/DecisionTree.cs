//////////////////////////////////////////////////////////////////////////////////
//                                                                              //
// Name: Kyle Duncan                                                            //
//                                                                              //
// Assignment: Decision Tree                                                    //
//                                                                              //
// Description: This program uses command line arguments to read in training    //
// data from files and then extract data from the input files to create tuples. //
// The tuples are then processed to form a decision tree based off of each      //
// attribute in the tuples and their corresponding information gain.            //
//                                                                              //
// Collaborated with Garrisen Cizmich and Jeffrey Fairbanks                     //
//                                                                              //
// ***Updated Description: This program uses either command line arugments or a //
// file to load in training data. In order to load in a file a tuple format     //
// file needs to be provided or created through the user interface. This        //
// program then extract data from the input files to create tuples. The tuples  // 
// are then processed to form a decision tree based off of each attribute in    // 
// the tuples and their corresponding information gain.***                      //
//                                                                              //
//////////////////////////////////////////////////////////////////////////////////

using System;
using System.IO;
using System.Collections.Generic;
using System.Data;

namespace ML_Decision_Tree
{ 
    class DecisionTree
    {
        static void Main(string[] args)
        {
           UserInterface.Run(args);
        }
    }
}