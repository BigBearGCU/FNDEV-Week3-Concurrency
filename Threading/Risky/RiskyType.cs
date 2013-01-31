using System;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace Risky
{
    // potentially unsafe third-party code
    public class RiskyType : MarshalByRefObject    
    {
        public string LowRisk()
        {
            return "No security risk here...";
        }
        
        public string HighRisk(string filename)
        {
            using (FileStream stm = new FileStream(filename,
                    FileMode.Open, FileAccess.Read))
            {
                StreamReader reader = new StreamReader(stm, Encoding.ASCII);
                StringBuilder strb = new StringBuilder();
                string str;
                do
                {
                    str = reader.ReadLine();
                    strb.Append(str + "\n");
                } while (str != null);
                return strb.ToString();
            }
        }

        public void AppDomainName()
        {
            Console.WriteLine("App Domain is {0}", AppDomain.CurrentDomain.FriendlyName);
        }

        public void ProcessID()
        {
            Console.WriteLine("Process ID is {0}",  Process.GetCurrentProcess().Id.ToString());
        }
    }
}
