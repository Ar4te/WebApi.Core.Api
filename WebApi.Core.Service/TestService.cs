using WebApi.Core.IService;
using WebApi.Core.Model.Models;
using WebApi.Core.Repository.Base;

namespace WebApi.Core.Service;

public class TestService : ITestService
{
    private readonly IBaseRepository<Test> _dal;
    public TestService(IBaseRepository<Test> dal)
    {
        _dal = dal;
    }
    public async Task<Test> CreateTest()
    {
        var res = await _dal.CreateNew(new Test { TestName = "arete234" });
        return res;
    }
}
