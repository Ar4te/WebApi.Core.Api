using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using SqlSugar;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using WebApi.Core.Common.Extension;
using WebApi.Core.Common.Helper;
using WebApi.Core.Common.Helper.SnowFlake;

namespace WebApi.Core.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            #region AppSettings
            builder.Services.AddSingleton(new AppSettings(builder.Configuration));
            #endregion

            #region SnowFlake
            builder.Services.AddSingleton(new YittHelper());
            #endregion

            #region Serilog
            Log.Logger = new LoggerConfiguration()
               .ReadFrom.Configuration(builder.Configuration)
               .CreateLogger();
            builder.Services.AddLogging(logBuilder =>
            {
                logBuilder.ClearProviders();
                logBuilder.AddSerilog(Log.Logger, dispose: true);
            });
            #endregion

            #region Autofac
            builder.Host
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureContainer<ContainerBuilder>(builder =>
                {
                    builder.RegisterModule(new AutofacModuleRegister());
                })
                .UseSerilog();
            #endregion

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
                //    Description = "请输入token,格式为 Bearer {token} (请注意中间必须有空格) ",
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
            app.UseSerilogRequestLogging();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
        public class DDD
        {
            public string Name { get; set; }
            public object Args { get; set; }
        }
    }
}