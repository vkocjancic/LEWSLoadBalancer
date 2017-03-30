using Mock.Service.SimulationRunner.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.Service.SimulationRunner.Job
{
    public class SimulationJob
    {

        #region Properties

        public string JobId { get; set; }
        public SimulationData Data { get; set; }

        #endregion

        #region Constructors

        public SimulationJob()
        {
            JobId = Guid.NewGuid().ToString();
        }

        public SimulationJob(SimulationData data) : this() { Data = data; }

        #endregion

    }
}
