using WebApi.Core.Model.Models;

namespace WebApi.Core.IService;

public interface ITestService
{
    Task<Test> CreateTest();
}
