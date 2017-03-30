using Mock.Service.SimulationRunner.Simulation;
using System;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceProcess;
using System.Threading;

namespace Mock.Service.SimulationRunner
{
    public partial class SimulationRunnerService : ServiceBase
    {

        #region Fields

        WebServiceHost m_hostSvc = null;

        #endregion

        public SimulationRunnerService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            if (null != m_hostSvc)
            {
                m_hostSvc.Close();
            }
            m_hostSvc = new WebServiceHost(typeof(SimulationService), new Uri(Properties.Settings.Default.UriEndpoint));
            var endpoint = m_hostSvc.AddServiceEndpoint(typeof(ISimulationService), new WebHttpBinding(), Properties.Settings.Default.UriEndpoint);
            endpoint.EndpointBehaviors.Add(new BehaviorAttribute());
            m_hostSvc.Open();
        }

        protected override void OnStop()
        {
            if (null != m_hostSvc)
            {
                m_hostSvc.Close();
            }
        }

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
