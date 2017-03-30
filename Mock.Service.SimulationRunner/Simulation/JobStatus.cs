using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mock.Service.SimulationRunner.Simulation
{
    [DataContract]
    public class JobStatus
    {
        #region Properties

        [DataMember]
        public string JobId { get; set; }

        [DataMember]
        public JobExecutionStatus Status { get; set; }

        [DataMember]
        public string Error { get; set; }

        [DataMember]
        public SimulationResult Result { get; set; }

        #endregion

        #region Constructors

        public JobStatus(JobExecutionStatus status)
        {
            Status = status;
        }

        #endregion
    }

    [DataContract]
    public enum JobExecutionStatus
    {
        Error = 0,
        Pending,
        InProgress,
        Done
    }
}
