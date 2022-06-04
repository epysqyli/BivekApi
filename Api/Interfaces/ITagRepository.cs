using Api.Models;

namespace Api.Interfaces
{
    public interface ITagRepository : IGenericRepository<Tag>
    {
        IEnumerable<ITagDto> GetAllDtos();
        ITagDto GetTagDto(int id);
    }
}