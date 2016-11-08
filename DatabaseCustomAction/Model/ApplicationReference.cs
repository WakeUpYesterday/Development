using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseCustomAction.Model
{
   public class ApplicationReference
    {
        public int Id { get; set; }
        public string ApplicationOrDeviceType { get; set; }
        public string ApplicationRootName { get; set; }
        public string ApplicationInitPage { get; set; }
    }
}
