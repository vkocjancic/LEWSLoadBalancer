using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.LoadBalancer.Threading
{
    public interface IThreadRunner
    {

        void Run();
        void Stop();

    }
}
