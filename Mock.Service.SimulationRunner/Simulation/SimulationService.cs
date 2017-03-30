using Mock.Service.SimulationRunner.Job;
using NLog;
using System;
using System.ServiceModel.Web;
using System.Web.Http;

namespace Mock.Service.SimulationRunner.Simulation
{

    public class SimulationService : ISimulationService
    {

        #region Declarations

        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static Random rnd = new Random();

        #endregion

        #region ISimulationService implementation

        [WebGet(UriTemplate="query/{jobId}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public JobStatus QueryJob(string jobId)
        {
            var status = new JobStatus(JobExecutionStatus.InProgress);
            status.JobId = jobId;
            if (rnd.Next(0, 63) > 55)
            {
                status.Result = new SimulationResult();
                status.Status = JobExecutionStatus.Done;
            }
            return status;
        }
       
        [WebInvoke(Method = "POST", UriTemplate = "start", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public JobStatus StartJob([FromBody] SimulationData data)
        {
            logger.Info($"StartJob invoked: {data?.ToString()}");
            var status = new JobStatus(JobExecutionStatus.Pending);
            try
            {
                logger.Debug("Creating new job");
                var job = new SimulationJob(data);
                logger.Info($"Job created '{job.JobId}'");
                status.JobId = job.JobId;
            }
            catch (Exception ex)
            {
                status.Status = JobExecutionStatus.Error;
                status.Error = ex.Message;
                logger.Error(ex);
            }
            logger.Info($"StartJob done {status.ToString()}");
            return status;
        }

        #endregion
    }

}
