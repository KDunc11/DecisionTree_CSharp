using System;
using System.Collections.Generic;
using System.Text;

namespace ML_Decision_Tree
{
    class ConfusionMatrix
    {
        public ConfusionMatrix(int tP, int tN, int fP, int fN)
        {
            TruePositive = tP;
            FalsePositive = fP;
            TrueNegative = tN;
            FalseNegative = fN;
        }

        public void Print()
        {
            Console.WriteLine($"\t [[ {TruePositive}, {FalseNegative} ]\n\t  [ {FalsePositive}, {TrueNegative} ]]");
        }

        private int TruePositive { get; }
        private int FalsePositive { get; }
        private int TrueNegative { get; }
        private int FalseNegative { get; }
    }
}
