using Service.LoadBalancer.Node;
using Service.LoadBalancer.Notification;
using Service.LoadBalancer.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Service.LoadBalancer.Diagnostics
{
    public class DiagnosticsRunner : ThreadRunnerBase
    {

        #region ThreadRunnerBase implementation

        protected override void ExecuteThread()
        {
            while (m_fRunThread)
            {
                foreach(var kvpNode in LoadBalancerService.Nodes)
                {
                    var node = kvpNode.Value;
                    var task = Task.Factory
                        .StartNew(() => { node.CheckAvailability(new SocketAvailabilityChecker()); })
                        .ContinueWith((taskOriginal) => { node.HandleNotifications(new NodeDownNotification(node)); });  
                }
                Thread.Sleep(1000);
            }
        } 

        #endregion

    }
}
