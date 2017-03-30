using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Service.LoadBalancer.Diagnostics
{
    public class SocketAvailabilityChecker : ICheckAvailability
    {

        #region Declarations

        protected readonly int m_ronTimeout = 500;

        #endregion

        #region ICheckAvailability implementation

        public bool IsAvailable(string host, int port)
        {
            using (var socket = new Socket(SocketType.Stream, ProtocolType.Tcp))
            {
                socket.ReceiveTimeout = socket.SendTimeout = m_ronTimeout;
                try
                {
                    var result = socket.BeginConnect(host, port, null, null);
                    return result.AsyncWaitHandle.WaitOne(m_ronTimeout, true);
                }
                catch (Exception)
                {
                    return false;
                }
            }
        } 

        #endregion

    }
}
