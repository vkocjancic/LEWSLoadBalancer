using System.ServiceModel;

namespace Mock.Service.SimulationRunner.Simulation
{
    [ServiceContract]
    public interface ISimulationService
    {

        [OperationContract]
        JobStatus StartJob(SimulationData data);

        [OperationContract]
        JobStatus QueryJob(string jobId);

    }

}
