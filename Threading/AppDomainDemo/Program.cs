//#define UNRESTRICTED    // define to apply security restriction to new AppDomain

using System;
using System.Reflection;
using System.Security;
using System.Security.Policy;
using Risky;



namespace AppDomainDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("RiskyType object in CURRENT APPDOMAIN");
                // risk1 is instance of RiskyType main AppDomain
                RiskyType risk1 = new RiskyType();

#if(UNRESTRICTED)
                // show process ID and AppDomain name for RiskType instance
                risk1.ProcessID();
                risk1.AppDomainName();
#endif

                // invoke safe and risky methods in first AppDomain
                string safeResult = risk1.LowRisk();
                Console.WriteLine(safeResult);
                string riskyResult = risk1.HighRisk("risky.txt");
                Console.Write(riskyResult);
                Console.WriteLine("\n");

#if(UNRESTRICTED)
                // create new AppDomain with same privileges as first AppDomain 
                // to show processID/appdomain name info
                AppDomain restrictedDomain = AppDomain.CreateDomain("restrictedDomain");    
#else
                // create new AppDomain with privileges restricted by security policy
                Evidence ev = new Evidence();
                ev.AddHost(new Zone(SecurityZone.Internet));
                AppDomain restrictedDomain = AppDomain.CreateDomain("restrictedDomain", ev);
#endif

                Console.WriteLine("RiskyType object in RESTRICTED APPDOMAIN");

                // use reflection to load the assembly Risky into new app domain and create an instance of RiskyType
                // the instance, risk2, is a transparent proxy to an instance of RiskyType in restricted domain
                // RiskyType must subclass MarshalByRefObject 
                RiskyType risk2 = (RiskyType)restrictedDomain.CreateInstanceAndUnwrap(
                    "Risky", "Risky.RiskyType");

#if(UNRESTRICTED)
                // show process ID and AppDomain name for RiskType instance accessed through proxy
                risk2.ProcessID();
                risk2.AppDomainName();
#endif

                // invoke safe and risky methods in new AppDomain
                safeResult = risk2.LowRisk();
                Console.WriteLine(safeResult);
                riskyResult = risk2.HighRisk("risky.txt");
                Console.Write(riskyResult);
                Console.WriteLine("\n");
            }
            catch (SecurityException e)
            {
                Console.WriteLine("SecurityException: {0}", e.Message);
            }
            finally
            {
                Console.ReadKey();
            }

        }
    }
}
