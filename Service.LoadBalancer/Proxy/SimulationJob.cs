using Service.LoadBalancer.Data;
using Service.LoadBalancer.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Service.LoadBalancer.Proxy
{
    public class SimulationJob
    {

        #region Properties

        public string Id { get; set; }
        public Uri NodeUri { get; set; }
        public JobStatus Status { get; set; }

        #endregion

        #region Constructors

        public SimulationJob() { }

        public SimulationJob(ServiceNode node)
        {
            NodeUri = node.Uri;
        }

        #endregion

        #region Public methods

        public void Execute(SimulationData data)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = NodeUri;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.PostAsJsonAsync(NodeUri.AbsolutePath + "/start", data).Result;
                if (response.IsSuccessStatusCode)
                {
                    Status = response.Content.ReadAsAsync<JobStatus>().Result;
                    Status.Node = NodeUri.ToString();
                }
                else
                {
                    Status = new JobStatus()
                    {
                        Status = JobExecutionStatus.Error,
                        Error = "Invalid response status: " + response.StatusCode.ToString()
                    };
                }
            }
        }

        public void Query()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = NodeUri;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.GetAsync(NodeUri.AbsolutePath + "/query/" + Status.JobId).Result;
                if (response.IsSuccessStatusCode)
                {
                    Status = response.Content.ReadAsAsync<JobStatus>().Result;
                }
                else
                {
                    Status = new JobStatus()
                    {
                        Status = JobExecutionStatus.Error,
                        Error = "Invalid repsonse status: " + response.StatusCode.ToString()
                    };
                }
            }
        }

        public void Remove(JobRepository repository)
        {
            repository.Remove(this);
        }

        public void Save(JobRepository repository)
        {
            Id = Status.JobId;
            repository.InsertOrUpdate(this);
        }

        #endregion

    }
}
