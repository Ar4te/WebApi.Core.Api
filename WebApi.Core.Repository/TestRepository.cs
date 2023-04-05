using WebApi.Core.IRepository;

namespace WebApi.Core.Repository
{
    public class TestRepository : ITestRepository
    {
        public int sum(int x, int y)
        {
            return x + y;
        }
    }
}
