using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Soap;     // need to add reference to System.Runtime.Serialization.Formatters.Soap.dll
using System.Runtime.Serialization.Formatters.Binary;

namespace SerializableTypeDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Data data = new Data();
            using (FileStream fileStream = new FileStream(@"SoapData.txt",
                FileMode.OpenOrCreate, FileAccess.Write))
            {
                SoapFormatter soapFormatter = new SoapFormatter();
                soapFormatter.Serialize(fileStream, data);
                fileStream.Close();
            }
        }
    }
}
