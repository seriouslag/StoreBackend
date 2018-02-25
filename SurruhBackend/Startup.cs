using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using SurruhBackend.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IO;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using Google.Apis.Auth.OAuth2;
using System.Security.Claims;
using System.Threading;

namespace SurruhBackend
{
    public class Startup
    {
        private static ServiceAccountCredential credential;
        private static IConfiguration Config;
        private static string[] scopes;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("firebase.json");
            Config = builder.Build();

            // Set scopes from config
            scopes = Config["Google:firebase_scopes"].Split(',');

            services.AddCors();
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = Config["Google:auth_url"];
                    options.IncludeErrorDetails = true;

                    // May not need SaveToken
                    options.SaveToken = true;

                    options.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = async context =>
                        {
                            await AddAuthClaims(context);
                        }
                    };
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        RequireSignedTokens = true,
                        RequireExpirationTime = true,
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidIssuer = Config["Google:valid_issuer"],
                        ValidateAudience = true,
                        ValidAudience = Config["Firebase:project_id"],
                        ValidateLifetime = true
                    };
                });
            services.AddMvc();

            services.AddDbContext<Context>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("SurruhBackendContext")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin();
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
            });
            app.UseAuthentication();
            app.UseMvc();
        }

        private async Task<bool> AddAuthClaims(TokenValidatedContext context)
        {
            // should never be null or not a JwtSecurityToken
            if (context.SecurityToken is JwtSecurityToken token)
            {
                // Get the usersId from JWT
                string userId = token.Payload["user_id"].ToString();

                if (context.Principal.Identity is ClaimsIdentity identity)
                {
                    // Check if credential or token is null or is the token is expired or expiring
                    if (credential == null || credential.Token == null || credential.Clock == null || credential.Token.IsExpired(credential.Clock))
                    {
                        // Get access token for database using private key

                        credential = new ServiceAccountCredential(
                            new ServiceAccountCredential.Initializer(Config["Firebase:client_email"])
                            {
                                Scopes = scopes
                            }.FromPrivateKey(Config["Firebase:private_key"])
                        );

                        // Retrieving Token from credentials async.
                        // Why do I assign to task?
                        // What does CancellationToken.None do
                        var task = await credential.RequestAccessTokenAsync(CancellationToken.None);
                    }
                    if (credential != null && credential.Token != null && credential.Clock != null && !credential.Token.IsExpired(credential.Clock))
                    {
                        // Get access token from creds
                        string accessToken = credential.Token.AccessToken;
                        // Add Auth Claims
                        await AddRoleClaims(identity, accessToken, userId);
                    }
                    else
                    {
                        // Should not get here
                        // creds are bad or token is expired even after a refresh check
                    }

                    // Add Auth role claim regardless of admin status
                    identity.AddClaim(new Claim(ClaimTypes.Role, "Auth"));
                    return true;
                }
            }
            return false;
        }

        private async Task<bool> AddRoleClaims(ClaimsIdentity claimsIdentity, string accessToken, string userId)
        {
            // Using HttpClient look for admin tag in database with the userId of the supplied JWT.
            using (HttpClient client = new HttpClient())
            {
                // Set header
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string requestUri = $"https://{Config["Firebase:project_id"]}.firebaseio.com/roles/{userId}.json?access_token={accessToken}";
                // get request to database
                try
                {
                    string role = await client.GetStringAsync(requestUri);

                    //format role
                    if (role.IndexOf("\"") > -1)
                    {
                        role = role.Replace("\"", "").Trim();
                        role = role.Substring(0, 1).ToUpper() + role.Substring(1);
                    }

                    // Add role claim if exists
                    if (role != null && role != "null")
                    {
                        claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role));
                        return true;
                    }
                } catch(Exception)
                {
                    return false;
                }
            }
            return false;
        }
    }
}
