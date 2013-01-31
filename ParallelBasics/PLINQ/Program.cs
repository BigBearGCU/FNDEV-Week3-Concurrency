using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.NetworkInformation;

namespace PLINQ
{
    class Program
    {
        static void Main(string[] args)
        {
            //calculate prime numbers
            IEnumerable<int> numbers = Enumerable.Range(3, 100000 - 3);
            //run query in parallel
            var parallelQuery =
            from n in numbers.AsParallel()
            where Enumerable.Range(2, (int)Math.Sqrt(n)).All(i => n % i > 0)
            select n;
            //get results and print
            int[] primes = parallelQuery.ToArray();
            foreach (int prime in primes)
            {
                Console.WriteLine("Prime number " + prime);
            }
            Console.ReadKey();

            //degree of parallelism, use for I/O tasks, forces the LINQ
            //to be parallel
            string[] sites ={"www.albahari.com",
                            "www.linqpad.net",
                            "www.oreilly.com",
                            "www.google.com",
                            "www.takeonit.com",
                            "stackoverflow.com"
                           };
            var degreeParallel = from site in sites.AsParallel().WithDegreeOfParallelism(6)
                                 let p = new Ping().Send(site)
                                 select new
                                 {
                                     site,
                                     Result = p.Status,
                                     Time = p.RoundtripTime
                                 };
            foreach (var result in degreeParallel.ToArray())
            {
                Console.WriteLine("Result of ping "+result.site+" "+result.Result.ToString()+" "+result.Time.ToString());
            }
            Console.ReadKey();
        }
    }
}
