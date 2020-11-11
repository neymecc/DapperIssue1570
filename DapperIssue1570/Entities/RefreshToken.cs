using System;

namespace DapperIssue1570.Entities
{
    public record RefreshToken
    {
        public long Id { get; init; }
        
        public string Token { get; init; }
        
        public DateTime UtcExpireDate { get; init; }
    }
}

