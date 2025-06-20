using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubsystemBL;


namespace BigStoreBL
{
    public class SubsystemManager
    {
        public SubsystemManager() { }

        public string GetSubsystemName()
        {
            ISystemInfo obj = (ISystemInfo)Activator.GetObject(typeof(ISystemInfo),
                  "tcp://subsystem:8282/system_info_service");

            return obj.GetSystemName();
        }
    }
}
