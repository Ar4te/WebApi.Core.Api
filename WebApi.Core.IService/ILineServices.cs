using WebApi.Core.Common.Global;
using WebApi.Core.IService.Base;
using WebApi.Core.Model.Models;

namespace WebApi.Core.IService;

/// <summary>
/// ILineService
/// </summary>	
public interface ILineService : IBaseService<Line>
{
    Task<MessageModel<List<Line>>> GetAllLine();
}