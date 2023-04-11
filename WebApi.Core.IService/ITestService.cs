using WebApi.Core.Model;

namespace WebApi.Core.IService;

public interface ITestService
{
    Task<Test> CreateTest();
}
