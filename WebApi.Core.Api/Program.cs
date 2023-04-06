using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using WebApi.Core.Common.Extension;
using WebApi.Core.Common.Helper;

namespace WebApi.Core.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureContainer<ContainerBuilder>(builder =>
                {
                    builder.RegisterModule(new AutofacModuleRegister());
                });

            builder.Services.AddSingleton(new AppSettings(builder.Configuration));

            #region
            //builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o =>
            //{
            //    //o.TokenValidationParameters = new TokenValidationParameters
            //    //{
            //    //    ValidateIssuer = true,
            //    //    ValidateAudience = true,
            //    //    ValidateLifetime = true,
            //    //    ValidateIssuerSigningKey = true,
            //    //    ValidAudience = "WebApi.Core.Api.com",
            //    //    ValidIssuer = "WebApi.Core.Api.com",
            //    //    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSettings.app("JWTSettings", "SecretKey")))
            //    //};
            //    o.Authority = "WebApi.Core.Api.com";
            //    o.Audience = AppSettings.app("JWTSettings", "Audience");
            //});
            //builder.Services.AddAuthorization();
            #endregion
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApi.Core.Api", Version = "v1" });
                #region 
                //s.OperationFilter<AddResponseHeadersFilter>();
                //s.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                //s.OperationFilter<SecurityRequirementsOperationFilter>();
                //s.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                //{
                //    Description = "������token,��ʽΪ Bearer {token} (��ע���м�����пո�) ",
                //    Name = "Authorization",
                //    In = ParameterLocation.Header,
                //    Type = SecuritySchemeType.ApiKey,
                //    BearerFormat = "JWT",
                //    Scheme = "Bearer"
                //});

                //s.AddSecurityRequirement(new OpenApiSecurityRequirement
                //{
                //    {
                //        new OpenApiSecurityScheme
                //        {
                //            Reference = new OpenApiReference
                //            {
                //                Type = ReferenceType.SecurityScheme,
                //                Id = "Bearer",
                //            }
                //        },
                //        Array.Empty<string>()
                //    }
                //});
                #endregion
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}