using System.Collections.Generic;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Dapper;
using DapperIssue1570.Entities;
using Npgsql;

namespace DapperIssue1570
{
    [MemoryDiagnoser]
    public class Bench
    {
        private const string ConnectionString = "Host=localhost;Port=5432;Database=vellum;Username=root;Password=";

        private const long ProfileIdExists = 2;

        private const long ProfileIdNotExists = 7;

        [Benchmark]
        public async Task<Profile?> SAMPLE_CODE_PROFILE_EXISTS()
        {
            await using NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);

            const string sql = @"
                SELECT id as Id, role_id as Role, username as Username, email as Email, full_name as FullName, hash as hash, salt as salt, is_email_verified as IsEmailVerified, utc_register_date as UtcRegisterDate, utc_last_login_date as UtcLastLoginDate, email_verify_token as EmailVerifyToken, version as Version FROM profile WHERE id = @id LIMIT 1; 
                SELECT id as Id, token as Token, utc_expire_date as UtcExpireDate FROM refresh_token WHERE profile_id = @id;";

            using var multi =
                await connection.QueryMultipleAsync(sql, new {id = ProfileIdExists});

            Profile? profile = await multi.ReadSingleOrDefaultAsync<Profile?>();

            if (profile == null)
            {
                return null;
            }

            return profile with { RefreshTokens = await multi.ReadAsync<RefreshToken>()};
        }

        [Benchmark]
        public async Task<Profile?> SAMPLE_CODE_PROFILE_NOT_EXISTS()
        {
            await using NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);

            const string sql = @"
                SELECT id as Id, role_id as Role, username as Username, email as Email, full_name as FullName, hash as hash, salt as salt, is_email_verified as IsEmailVerified, utc_register_date as UtcRegisterDate, utc_last_login_date as UtcLastLoginDate, email_verify_token as EmailVerifyToken, version as Version FROM profile WHERE id = @id LIMIT 1; 
                SELECT id as Id, token as Token, utc_expire_date as UtcExpireDate FROM refresh_token WHERE profile_id = @id;";

            using var multi =
                await connection.QueryMultipleAsync(sql, new {id = ProfileIdNotExists});

            Profile? profile = await multi.ReadSingleOrDefaultAsync<Profile?>();

            if (profile == null)
            {
                return null;
            }

            return profile with { RefreshTokens = await multi.ReadAsync<RefreshToken>()};
        }

        [Benchmark]
        public async Task<Profile?> SOLVED_PROFILE_EXISTS()
        {
            await using NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);

            const string sql = @"
                SELECT id as Id, role_id as Role, username as Username, email as Email, full_name as FullName, hash as hash, salt as salt, is_email_verified as IsEmailVerified, utc_register_date as UtcRegisterDate, utc_last_login_date as UtcLastLoginDate, email_verify_token as EmailVerifyToken, version as Version FROM profile WHERE id = @id LIMIT 1; 
                SELECT id as Id, token as Token, utc_expire_date as UtcExpireDate FROM refresh_token WHERE profile_id = @id;";

            using var multi =
                await connection.QueryMultipleAsync(sql, new {id = ProfileIdExists});

            Profile? profile = await multi.ReadSingleOrDefaultAsync<Profile?>();

            IEnumerable<RefreshToken> refreshTokens = await multi.ReadAsync<RefreshToken>();

            if (profile == null)
            {
                return null;
            }

            return profile with { RefreshTokens = refreshTokens};
        }
        
        
        [Benchmark]
        public async Task<Profile?> SOLVED_PROFILE_NOT_EXISTS()
        {
            await using NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);

            const string sql = @"
                SELECT id as Id, role_id as Role, username as Username, email as Email, full_name as FullName, hash as hash, salt as salt, is_email_verified as IsEmailVerified, utc_register_date as UtcRegisterDate, utc_last_login_date as UtcLastLoginDate, email_verify_token as EmailVerifyToken, version as Version FROM profile WHERE id = @id LIMIT 1; 
                SELECT id as Id, token as Token, utc_expire_date as UtcExpireDate FROM refresh_token WHERE profile_id = @id;";

            using var multi =
                await connection.QueryMultipleAsync(sql, new {id = ProfileIdNotExists});

            Profile? profile = await multi.ReadSingleOrDefaultAsync<Profile?>();

            IEnumerable<RefreshToken> refreshTokens = await multi.ReadAsync<RefreshToken>();

            if (profile == null)
            {
                return null;
            }

            return profile with { RefreshTokens = refreshTokens};
        }
    }
}