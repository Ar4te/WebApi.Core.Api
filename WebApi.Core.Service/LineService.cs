
using WebApi.Core.Common.Global;
using WebApi.Core.IService;
using WebApi.Core.Model.Models;
using WebApi.Core.Repository.Base;
using WebApi.Core.Service.Base;

namespace WebApi.Core.Service;

public class LineService : BaseService<Line>, ILineService
{
    private readonly IBaseRepository<Line> _dal;
    public LineService(IBaseRepository<Line> dal) : base(dal)
    {
        _dal = dal;
        base.BaseDal = dal;
    }

    public async Task<MessageModel<List<Line>>> GetAllLine()
    {
        var res = await _dal.Query();
        return MessageModel<List<Line>>.Success("", res);
    }
}