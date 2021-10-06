using MyCrmSampleClient.Auth;

namespace MyCrmSampleClient
{
    public class MyCrmApiClientCredential
    {
        public AuthResult Authorization { get; }

        public MyCrmApiClientCredential(AuthResult authorization)
        {
            Authorization = authorization;
        }
    }
}