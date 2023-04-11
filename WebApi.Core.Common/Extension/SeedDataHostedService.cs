using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebApi.Core.Common.DB;
using WebApi.Core.Common.Helper;

namespace WebApi.Core.Common.Extension;

public sealed class SeedDataHostedService : IHostedService
{
    private readonly CustomContext _myContext;
    private readonly ILogger<SeedDataHostedService> _logger;
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly string _webRootPath;

    public SeedDataHostedService(
        CustomContext myContext,
        IWebHostEnvironment webHostEnvironment,
        IHostApplicationLifetime hostApplicationLifetime,
        ILogger<SeedDataHostedService> logger)
    {
        _myContext = myContext;
        _logger = logger;
        _hostApplicationLifetime = hostApplicationLifetime;
        _webRootPath = webHostEnvironment.WebRootPath;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Start Initialization Db Seed Service!");
        await DoWork();
    }

    private async Task DoWork()
    {
        try
        {
            if (AppSettings.app("SeedDb", "SeedDBEnabled").ToUpper() == "TRUE" || AppSettings.app("SeedDb", "SeedDBDataEnabled").ToUpper() == "TRUE")
            {
                await DBSeed.SeedAsync(_myContext, _webRootPath);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occured seeding the Database.");
            throw;
        }
        //try
        //{
        //    if (!AppSettings.app("AppSettings", "CheckDbModel").ObjToBool())
        //    {
        //        if (DBSeed.CheckDbModel(_myContext, out string msg))
        //        {
        //            _logger.LogError(msg);
        //            Console.WriteLine(msg);
        //            _hostApplicationLifetime.StopApplication();
        //        }
        //    }
        //}
        //catch (Exception ex)
        //{
        //    _logger.LogError(ex, "Error occured seeding the Database.");
        //    throw;
        //}
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stop Initialization Db Seed Service!");
        return Task.CompletedTask;
    }
}
