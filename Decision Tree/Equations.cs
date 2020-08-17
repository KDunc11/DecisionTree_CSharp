using System;
using System.Collections.Generic;
using System.Text;

namespace ML_Decision_Tree
{
    class Equations
    {
        public static double ExpectedInfo(List<Tuple> tupleList, Attribute tupleClasses)
        {
            double sum = 0;
            for (int i = 0; i < tupleClasses.Values.Length; ++i)
            {
                int classCount = 0;
                for (int j = 0; j < tupleList.Count; ++j)
                {
                    if (tupleList[j].Class == tupleClasses.Values[i])
                    {
                        classCount++;
                    }
                }
                if (classCount != 0)
                {
                    sum += -1 * Math.Log((double)classCount / tupleList.Count, 2) * classCount / tupleList.Count;
                }
            }

            return sum;
        }

        public static double ExpectedInfoWithPartition(List<Tuple> fullTupleList, List<Tuple>[] tuplePartitions, Attribute tupleClasses)
        {
            double sum = 0;
            for (int n = 0; n < tuplePartitions.Length; ++n)
            {
                sum += (double)tuplePartitions[n].Count / fullTupleList.Count * ExpectedInfo(tuplePartitions[n], tupleClasses);
            }
            return sum;
        }

        public static bool SubListClassesAreSame(List<Tuple> tupleList)
        {
            string firstTupleClass = tupleList[0].Class;
            foreach (Tuple tuple in tupleList)
            {
                if (tuple.Class != firstTupleClass)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
