using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelForEach
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> numList = new List<int>();
            foreach (int i in numList)
            {
                //do work
            }

            Parallel.ForEach(numList, item =>
            {
                //do work
            });
        }
    }
}
