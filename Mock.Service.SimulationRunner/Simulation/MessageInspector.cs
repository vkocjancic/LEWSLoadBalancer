using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;

namespace Mock.Service.SimulationRunner.Simulation
{
    public class MessageInspector : IDispatchMessageInspector
    {
        #region Declarations

        private ServiceEndpoint m_serviceEndpoint; 
        
        #endregion

        #region Constructors

        public MessageInspector(ServiceEndpoint serviceEndpoint)
        {
            m_serviceEndpoint = serviceEndpoint;
        } 

        #endregion

        public object AfterReceiveRequest(ref Message request,
                                              IClientChannel channel,
                                              InstanceContext instanceContext)
        {
            StateMessage stateMsg = null;
            HttpRequestMessageProperty requestProperty = null;
            if (request.Properties.ContainsKey(HttpRequestMessageProperty.Name))
            {
                requestProperty = request.Properties[HttpRequestMessageProperty.Name]
                                  as HttpRequestMessageProperty;
            }

            if (requestProperty != null)
            {
                var origin = requestProperty.Headers["Origin"];
                if (!string.IsNullOrEmpty(origin))
                {
                    stateMsg = new StateMessage();
                    // if a cors options request (preflight) is detected, 
                    // we create our own reply message and don't invoke any 
                    // operation at all.
                    if (requestProperty.Method == "OPTIONS")
                    {
                        stateMsg.Message = Message.CreateMessage(request.Version, null);
                    }
                    request.Properties.Add("CrossOriginResourceSharingState", stateMsg);
                }
            }

            return stateMsg;
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            var stateMsg = correlationState as StateMessage;

            if (stateMsg != null)
            {
                if (stateMsg.Message != null)
                {
                    reply = stateMsg.Message;
                }

                HttpResponseMessageProperty responseProperty = null;

                if (reply.Properties.ContainsKey(HttpResponseMessageProperty.Name))
                {
                    responseProperty = reply.Properties[HttpResponseMessageProperty.Name]
                                       as HttpResponseMessageProperty;
                }

                if (responseProperty == null)
                {
                    responseProperty = new HttpResponseMessageProperty();
                    reply.Properties.Add(HttpResponseMessageProperty.Name,
                                         responseProperty);
                }

                // Access-Control-Allow-Origin should be added for all cors responses
                responseProperty.Headers.Set("Access-Control-Allow-Origin", "*");

                if (stateMsg.Message != null)
                {
                    // the following headers should only be added for OPTIONS requests
                    responseProperty.Headers.Set("Access-Control-Allow-Methods",
                                                 "POST, OPTIONS, GET");
                    responseProperty.Headers.Set("Access-Control-Allow-Headers",
                              "Content-Type, Accept, Authorization, x-requested-with");
                }
            }
        }
    }

    class StateMessage
    {
        public Message Message;
    }
}
