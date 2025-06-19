using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Channels;
using SubsystemBL;

namespace Subsystem
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            TcpChannel tcpChannel = new TcpChannel(8282);
            ChannelServices.RegisterChannel(tcpChannel);
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(SystemInfo), "system_info_service", WellKnownObjectMode.SingleCall);
        }
    }
}
