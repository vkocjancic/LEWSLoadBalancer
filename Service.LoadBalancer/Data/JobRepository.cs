using LiteDB;
using Service.LoadBalancer.Proxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.LoadBalancer.Data
{
    public class JobRepository
    {

        #region Declarations

        protected readonly string m_rosConnectionString = "jobcache.db";

        #endregion

        #region Public methods

        public SimulationJob GetById (string jobId)
        {
            using (var db = new LiteDatabase(m_rosConnectionString))
            {
                return db.GetCollection<SimulationJob>("jobs").FindById(jobId);
            }
        }

        public void InsertOrUpdate(SimulationJob job)
        {
            using (var db = new LiteDatabase(m_rosConnectionString))
            {
                db.GetCollection<SimulationJob>("jobs").Upsert(job);
            }
        }

        public void Remove(SimulationJob job)
        {
            using (var db = new LiteDatabase(m_rosConnectionString))
            {
                db.GetCollection<SimulationJob>("jobs").Delete(job.Id);
            }
        }

        #endregion

    }
}
