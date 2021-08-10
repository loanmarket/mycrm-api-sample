using Azure.Core;
using Azure.Core.Pipeline;

namespace MyCrmSampleClient
{
    internal class MyCrmApiClientOptionsPolicy : HttpPipelineSynchronousPolicy
    {
        public MyCrmApiClientOptions Options { get; }

        public MyCrmApiClientOptionsPolicy(MyCrmApiClientOptions options)
        {
            Options = options;
        }

        public override void OnSendingRequest(HttpMessage message)
        {
            // message.Request.Headers.SetValue("Accept", "application/vnd.api+json");
            // message.Request.Headers.SetValue("Content-Type", "application/vnd.api+json");

            if (Options != null && Options.AdviserContactId > 0)
            {
                message.Request.Headers.SetValue("UserId", Options.AdviserContactId.ToString());
            }

            base.OnSendingRequest(message);
        }
    }
}