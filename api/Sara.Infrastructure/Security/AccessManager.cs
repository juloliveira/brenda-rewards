using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
//using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Sara.Core;

namespace Sara.Infrastructure.Security
{
    public class AccessManager
    {
        private readonly UserManager<SaraUser> _userManager;
        private readonly SignInManager<SaraUser> _signInManager;
        private readonly IDistributedCache _cache;
        private readonly TokenConfiguration _settings;

        public AccessManager(
            UserManager<SaraUser> userManager,
            SignInManager<SaraUser> signInManager,
            IDistributedCache cache,
            IOptions<TokenConfiguration> setting)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _cache = cache;
            _settings = setting.Value;
        }

        public async Task<bool> ValidateCredentials(AccessCredentials credentials)
        {
            bool validCredentials = false;
            if (credentials != null && !String.IsNullOrWhiteSpace(credentials.Username))
            {
                if (credentials.GrantType == "password")
                {
                    // Verifica a existência do usuário nas tabelas do
                    // ASP.NET Core Identity
                    var userIdentity = await _userManager.FindByNameAsync(credentials.Username);
                    if (userIdentity != null)
                    {
                        // Efetua o login com base no Id do usuário e sua senha
                        var signinResult = await _signInManager
                            .CheckPasswordSignInAsync(userIdentity, credentials.Password, false);
                        if (signinResult.Succeeded)
                        {
                            // Verifica se o usuário em questão possui
                            // a role Acesso-APIProdutos
                            validCredentials = await _userManager.IsInRoleAsync(userIdentity, "Default");
                            credentials.UserId = userIdentity.Id;
                        }
                    }
                }
                else if (credentials.GrantType == "refresh_token")
                {
                    if (!string.IsNullOrWhiteSpace(credentials.RefreshToken))
                    {
                        RefreshTokenData refreshTokenBase = null;

                        string strTokenArmazenado = await _cache.GetStringAsync(credentials.RefreshToken);
                        if (!string.IsNullOrWhiteSpace(strTokenArmazenado))
                        {
                            refreshTokenBase = JsonConvert
                                .DeserializeObject<RefreshTokenData>(strTokenArmazenado);
                        }

                        validCredentials = (refreshTokenBase != null &&
                            credentials.Username == refreshTokenBase.Username &&
                            credentials.RefreshToken == refreshTokenBase.RefreshToken);

                        // Elimina o token de refresh já que um novo será gerado
                        if (validCredentials)
                        {
                            credentials.UserId = refreshTokenBase.UserId;
                            await _cache.RemoveAsync(credentials.RefreshToken);
                        }
                    }
                }
            }

            return validCredentials;
        }

        public async Task<Token> GenerateToken(AccessCredentials credentials)
        {
            var identity = new ClaimsIdentity(
                new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim(JwtRegisteredClaimNames.UniqueName, credentials.Username),
                        new Claim(ClaimTypes.NameIdentifier, credentials.UserId.ToString())
                }
            );

#if DEBUG
            DateTime createdAt = DateTime.UtcNow;
            DateTime expirationAt = createdAt +
                TimeSpan.FromSeconds(99999);
#else
            DateTime createdAt = DateTime.UtcNow;
            DateTime expirationAt = createdAt +
                TimeSpan.FromSeconds(_settings.AccessTokenLifetimeSeconds);
#endif

            var privateRsa = RSA.Create();
            var xml = System.IO.File.ReadAllText(_settings.PrivateKey);
            privateRsa.FromXmlString(xml);
            var privateKey = new RsaSecurityKey(privateRsa);
            SigningCredentials signingCredentials = new SigningCredentials(privateKey, SecurityAlgorithms.RsaSha256);

            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _settings.Issuer,
                Audience = _settings.Audience,
                SigningCredentials = signingCredentials,
                Subject = identity,
                NotBefore = createdAt,
                Expires = expirationAt
            });
            var token = handler.WriteToken(securityToken);

            var tokenResult = new Token()
            {
                Created = createdAt.ToString("yyyy-MM-dd HH:mm:ss"),
                Expiration = expirationAt.ToString("yyyy-MM-dd HH:mm:ss"),
                AccessToken = token,
                RefreshToken = Guid.NewGuid().ToString("N"),
                UserId = credentials.UserId
            };

            var refreshTokenData = new RefreshTokenData(credentials.UserId, credentials.Username, tokenResult.RefreshToken);
            var finalExpiration = TimeSpan.FromDays(_settings.RefreshTokenLifetimeDays);
            var cacheOptions = new DistributedCacheEntryOptions();
            cacheOptions.SetAbsoluteExpiration(finalExpiration);
            await _cache.SetStringAsync(tokenResult.RefreshToken,
                JsonConvert.SerializeObject(refreshTokenData),
                cacheOptions);

            return tokenResult;
        }
    }
}
