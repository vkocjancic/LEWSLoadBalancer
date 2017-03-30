using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Service.LoadBalancer.Proxy
{
    [ServiceContract]
    interface ISimulationServiceProxy
    {

        [OperationContract]
        JobStatus StartJob(SimulationData data);

        [OperationContract]
        JobStatus QueryJob(string jobId);

    }
}
