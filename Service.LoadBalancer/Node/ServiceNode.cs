using Service.LoadBalancer.Diagnostics;
using Service.LoadBalancer.Notification;
using System;

namespace Service.LoadBalancer.Node
{
    public class ServiceNode
    {

        #region Fields

        protected bool m_bSendNotificationOnError = true;

        #endregion

        #region Properties

        public Uri Uri { get; protected set; }

        protected volatile ServiceNodeStatus m_status;
        public ServiceNodeStatus Status {
            get { return m_status; }
            set { m_status = value; }
        }

        #endregion

        #region Constructors

        public ServiceNode(string propertyValue)
        {
            Uri = new Uri(propertyValue);
            Status = ServiceNodeStatus.Up;
        }

        #endregion

        #region Public methods

        public void CheckAvailability(ICheckAvailability availChecker)
        {
            if (availChecker.IsAvailable(Uri.Host, Uri.Port))
            {
                Status = ServiceNodeStatus.Up;
                m_bSendNotificationOnError = true;
            }
            else
            {
                Status = ServiceNodeStatus.Down;
            }
        }

        public void HandleNotifications(INotify notification)
        {
            if ((Status == ServiceNodeStatus.Down) && (m_bSendNotificationOnError))
            {
                notification.Send();
                m_bSendNotificationOnError = false;
            }
        }

        #endregion

    }

    public enum ServiceNodeStatus
    {
        Down = 0,
        Up
    }

}