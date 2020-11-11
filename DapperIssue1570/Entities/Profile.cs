using System;
using System.Collections.Generic;

namespace DapperIssue1570.Entities
{
    public record Profile
    {
        public long Id { get; init; }

        public Role Role { get; init; }
        
        public string Username { get; init; }
        
        public string Email { get; init; }
        
        public string? FullName { get; init; }
        
        public string Hash { get; init; }
        
        public string Salt { get; init; }
        
        public bool IsEmailVerified { get; init; }
        
        public DateTime UtcRegisterDate { get; init; }
        
        public DateTime? UtcLastLoginDate { get; init; }
        
        public string? EmailVerifyToken { get; init; }
        
        public long Version { get; init; }
        
        public IEnumerable<RefreshToken>? RefreshTokens { get; init; }
    }
}