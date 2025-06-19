using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SubsystemBL
{
    public class SystemInfo : MarshalByRefObject, ISystemInfo
    {
        public string GetSystemName()
        {
            return "The Big Sub";
        }
    }
}
