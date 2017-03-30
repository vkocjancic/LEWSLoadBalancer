using Service.LoadBalancer.Data;
using Service.LoadBalancer.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Service.LoadBalancer.Proxy
{
    public class SimulationServiceProxy : ISimulationServiceProxy
    {

        #region ISimulationServiceProxy implementation

        [WebGet(UriTemplate = "query/{jobId}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public JobStatus QueryJob(string jobId)
        {
            var repository = new JobRepository();
            var job = repository.GetById(jobId);
            if (null == job)
            {
                return new Proxy.JobStatus()
                {
                    Status = JobExecutionStatus.Error,
                    Error = "Job not found"
                };
            }
            job.Query();
            if (JobExecutionStatus.Done == job.Status.Status)
            {
                job.Remove(repository);
            }
            return job.Status;
        }

        [WebInvoke(Method = "POST", UriTemplate = "start", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public JobStatus StartJob([FromBody] SimulationData data)
        {
            ServiceNode node = null;
            if ((LoadBalancerService.Nodes.TryGetValue(0, out node) && (ServiceNodeStatus.Up == node.Status))
                || (LoadBalancerService.Nodes.TryGetValue(1, out node) && (ServiceNodeStatus.Up == node.Status)))
            {
                try
                {
                    var job = new SimulationJob(node);
                    job.Execute(data);
                    if (JobExecutionStatus.Error != job.Status.Status)
                    {
                        job.Save(new JobRepository());
                    }
                    return job.Status;
                }
                catch (Exception ex)
                {
                    return new Proxy.JobStatus()
                    {
                        Status = JobExecutionStatus.Error,
                        Error = "Exception: " + ex.Message
                    };
                }
            }
            else
            {
                return new Proxy.JobStatus()
                {
                    Status = JobExecutionStatus.Error,
                    Error = "No active nodes found"
                };
            }
        } 

        #endregion

    }
}
