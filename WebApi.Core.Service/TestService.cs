using WebApi.Core.IRepository;
using WebApi.Core.IService;
using WebApi.Core.Repository;

namespace WebApi.Core.Service
{
    public class TestService : ITestService
    {
        private readonly ITestRepository _test;
        public TestService(ITestRepository test)
        {
            _test = test;
        }

        public string payType()
        {
            return "wechat";
        }

        public int sum(int x, int y)
        {
            return _test.sum(x, y);
        }
    }
}
