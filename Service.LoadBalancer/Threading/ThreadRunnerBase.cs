using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Service.LoadBalancer.Threading
{
    public abstract class ThreadRunnerBase : IThreadRunner, IDisposable
    {

        #region Declarations

        private bool m_fDisposed = false;
        protected Thread m_tRunner = null;
        protected bool m_fRunThread = false;

        #endregion

        #region Constructors

        public ThreadRunnerBase()
        {
            m_tRunner = new Thread(ExecuteThread);
        }

        #endregion

        #region IThreadRunner implementation

        public void Run()
        {
            m_fRunThread = true;
            m_tRunner.Start();
        }

        public void Stop()
        {
            if (m_tRunner.IsAlive)
            {
                m_fRunThread = false;
                m_tRunner.Join(3000);
            }
        }

        #endregion

        #region Abstract methods

        protected abstract void ExecuteThread();

        #endregion

        #region IDisposable implementation

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (m_fDisposed)
                return;

            if (disposing)
            {
                Stop();
                // Free any other managed objects here.
                //
            }

            // Free any unmanaged objects here.
            //
            m_fDisposed = true;
        }

        #endregion

    }
}
