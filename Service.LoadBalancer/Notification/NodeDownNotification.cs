using Service.LoadBalancer.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.LoadBalancer.Notification
{
    public class NodeDownNotification : NotificationBase
    {

        #region Declarations

        protected ServiceNode m_node;

        #endregion

        #region Constructors

        public NodeDownNotification(ServiceNode node)
        {
            m_node = node;
        }

        #endregion

        #region NotificationBase implenentation

        protected override string GetSubject()
        {
            return $"{m_node.Uri.AbsoluteUri} is down";
        }

        protected override string GetBody()
        {
            return $"Hello!\r\n\r\nWe hereby inform you that node '{m_node.Uri.AbsoluteUri}' is offline.";
        }

        #endregion

    }
}
