using Api.Models.Entities;

namespace Api.Interfaces
{
    public interface ITagRepository : IGenericRepository<Tag>, IDtoRepository<ITagDto>
    { }
}