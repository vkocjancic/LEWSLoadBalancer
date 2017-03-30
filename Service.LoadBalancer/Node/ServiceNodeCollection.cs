using System;
using System.Collections.Concurrent;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.LoadBalancer.Node
{
    public class ServiceNodeCollection : ConcurrentDictionary<int, ServiceNode>
    {

        #region Factory methods

        public static ServiceNodeCollection GetAll()
        {
            var collection = new ServiceNodeCollection();
            collection.TryAdd(0, new Node.ServiceNode(Properties.Settings.Default.S1));
            collection.TryAdd(1, new Node.ServiceNode(Properties.Settings.Default.S2));
            return collection;
        }

        #endregion

    }
}
