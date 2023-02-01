using Microsoft.Extensions.DependencyInjection;
using BookStore.Data.Contracts;
using BookStore.Data.Repositories;
using BookStore.Services;
using BookStore.Common.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using BookStore.Entities;
using System.Security.Claims;
using BookStore.Common.Utilities;

namespace BookStore.WebFramework.Configuration;

public static class ServiceCollectionExtensions
{
    public static void AddMiniMvc(this IServiceCollection services)
    {
        //Repo 
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUserRepository, UserRepository>();

        //Services
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IBookService, BookService>();
        services.AddScoped<IOrderService, OrderService>();
    }
    public static void AddJwtAuthentication(this IServiceCollection services, JwtSettings jwtSettings)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

        }).AddJwtBearer(options =>
        {
            var secretkey = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);
            var encryptkey = Encoding.UTF8.GetBytes(jwtSettings.Encryptkey);

            var validationParameters = new TokenValidationParameters
            {
                ClockSkew = TimeSpan.Zero,
                RequireSignedTokens = true,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretkey),

                RequireExpirationTime = true,
                ValidateLifetime = true,

                ValidateAudience = true,
                ValidAudience = jwtSettings.Audience,

                ValidateIssuer = true,
                ValidIssuer = jwtSettings.Issuer,

                TokenDecryptionKey = new SymmetricSecurityKey(encryptkey),
            };

            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = validationParameters;
            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    //if (context.Exception != null)
                    //    throw new UnauthorizedAccessException("Authentication failed.");

                    return Task.CompletedTask;
                },
                OnTokenValidated = async context =>
                {
                    var signInManager = context.HttpContext.RequestServices.GetRequiredService<SignInManager<User>>();
                    var userManager = context.HttpContext.RequestServices.GetRequiredService<UserManager<User>>();
                    var userRepository = context.HttpContext.RequestServices.GetRequiredService<IUserRepository>();

                    var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
                    if (claimsIdentity.Claims?.Any() != true)
                        context.Fail("This token has no claims.");

                    var securityStamp = claimsIdentity.FindFirstValue(new ClaimsIdentityOptions().SecurityStampClaimType);
                    if (!securityStamp.HasValue())
                        context.Fail("This token has no secuirty stamp");

                    //Find user and token from database and perform your custom validation
                    var userId = claimsIdentity.GetUserId<int>();
                    var user = await userManager.FindByIdAsync(userId.ToString());

                    var validatedUser = await signInManager.ValidateSecurityStampAsync(context.Principal);
                    if (validatedUser == null)
                        context.Fail("Token secuirty stamp is not valid.");

                    if (!user.IsActive)
                        context.Fail("User is not active.");

                    await userRepository.UpdateLastLoginDateAsync(user, context.HttpContext.RequestAborted);
                },
                OnChallenge = context =>
                {
                    //if (context.AuthenticateFailure != null)
                    //    throw new UnauthorizedAccessException("Authenticate failure.");
                    //throw new UnauthorizedAccessException("You are unauthorized to access this resource.");

                    return Task.CompletedTask;
                }
            };
        });
    }
}

