using Azure.Core;
using Azure.Core.Pipeline;

namespace MyCrmSampleClient
{
    internal class MyCrmApiClientCredentialPolicy : HttpPipelineSynchronousPolicy
    {
        public MyCrmApiClientCredential Credential { get; }

        public MyCrmApiClientCredentialPolicy(MyCrmApiClientCredential credential)
        {
            Credential = credential;
        }

        public override void OnSendingRequest(HttpMessage message)
        {
            if (Credential.Authorization?.Success == true)
            {
                message.Request.Headers.SetValue("Authorization", $"Bearer {Credential.Authorization.Token}");
            }

            base.OnSendingRequest(message);
        }
    }
}