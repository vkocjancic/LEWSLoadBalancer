using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.LoadBalancer.Diagnostics
{
    public interface ICheckAvailability
    {

        bool IsAvailable(string host, int port);

    }
}
