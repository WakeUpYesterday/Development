using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Attribute
{
    using Attribute = System.Attribute;

    [AttributeUsage(AttributeTargets.All)]
    public class CustomAttribute : Attribute
    {
        public readonly string Url;

        public string Topic               // Topic is a named parameter
        {
            get
            {
                return topic;
            }
            set
            {

                topic = value;
            }
        }

        public CustomAttribute(string url)  // url is a positional parameter
        {
            this.Url = url;
        }

        private string topic;
    }
}