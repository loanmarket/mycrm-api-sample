using System;
using Azure.Core;
using Azure.Core.Pipeline;

namespace MyCrmSampleClient
{
    public class MyCrmApiClientOptions : ClientOptions
    {
        public int AdviserContactId { get; }
        public Uri Endpoint { get; }

        public MyCrmApiClientOptions(int adviserContactId, Uri endpoint = null)
        {
            AdviserContactId = adviserContactId;
            Endpoint = endpoint ?? new Uri("https://api.mycrm.finance");
        }

        internal HttpPipeline Build(MyCrmApiClientCredential credential, MyCrmApiClientOptions options) =>
            HttpPipelineBuilder.Build(
                this,
                new HttpPipelinePolicy[]
                    {new MyCrmApiClientCredentialPolicy(credential), new MyCrmApiClientOptionsPolicy(options)},
                Array.Empty<HttpPipelinePolicy>(),
                null);
    }
}