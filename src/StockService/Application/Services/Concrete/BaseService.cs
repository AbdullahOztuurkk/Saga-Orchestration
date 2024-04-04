using CoreLib.DataAccess.Abstract;
using MapsterMapper;

namespace Application.Services.Concrete;
public class BaseService
{
    public IUnitOfWork Db { get; set; }
    public IMapper Mapper { get; set; }
}
