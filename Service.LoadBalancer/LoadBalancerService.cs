using Service.LoadBalancer.Diagnostics;
using Service.LoadBalancer.Node;
using Service.LoadBalancer.Proxy;
using System;
using System.Collections.Concurrent;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceProcess;
using System.Threading;

namespace Service.LoadBalancer
{
    public partial class LoadBalancerService : ServiceBase
    {

        #region Static fields

        public static ConcurrentDictionary<int, ServiceNode> Nodes = ServiceNodeCollection.GetAll();

        #endregion

        #region Declarations

        private DiagnosticsRunner m_runrDiagnostics = null;
        private WebServiceHost m_hostSvc = null;

        #endregion

        #region Constructors

        public LoadBalancerService()
        {
            InitializeComponent();
        } 

        #endregion

        #region ServiceBase implementation

        protected override void OnStart(string[] args)
        {
            m_runrDiagnostics = new DiagnosticsRunner();
            m_runrDiagnostics.Run();
            if (null != m_hostSvc)
            {
                m_hostSvc.Close();
            }
            m_hostSvc = new WebServiceHost(typeof(SimulationServiceProxy), new Uri(Properties.Settings.Default.UriEndpoint));
            var endpoint = m_hostSvc.AddServiceEndpoint(typeof(ISimulationServiceProxy), new WebHttpBinding(), Properties.Settings.Default.UriEndpoint);
            endpoint.EndpointBehaviors.Add(new BehaviorAttribute());
            m_hostSvc.Open();
        }

        protected override void OnStop()
        {
            if (null != m_hostSvc)
            {
                m_hostSvc.Close();
            }
            if (null != m_runrDiagnostics)
            {
                m_runrDiagnostics.Stop();
            }
        } 

        #endregion

        #region Public methods

        public void OnInteractiveStart()
        {
            OnStart(null);
            Thread.Sleep(5 * 60 * 1000);
            OnStop();
        }

        #endregion

    }

}
