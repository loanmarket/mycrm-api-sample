using System;

namespace MyCrmSampleClient.Auth
{
    public sealed class AuthResult
    {
        public AuthResult(bool success, string token, DateTime expiresAt)
        {
            Success = success;
            Token = token;
            ExpiresAt = expiresAt;
        }

        public bool Success { get; }
        public string Token { get; }
        public DateTime ExpiresAt { get; }
    }
}