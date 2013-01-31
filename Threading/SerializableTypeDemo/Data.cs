using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SerializableTypeDemo
{
    [Serializable]
    public class Data
    {
        [NonSerialized]
        private string privateData = "This is private data";

        public string publicData = "This is public data";        
        
        /// <summary>
        /// Property which contains public data
        /// </summary>
        public string PrivateData
        {
            get { return privateData; }
            set { privateData = value; }
        }
    }
}
