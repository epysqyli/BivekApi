using Api.Data;
using Api.Interfaces;
using Api.Models.Entities;
using Api.Models.Dtos;

namespace Api.Models.Repositories
{
    public class TagRepository : GenericRepository<Tag>, ITagRepository
    {
        public TagRepository(ApiDbContext context) : base(context)
        { }

        public IEnumerable<ITagDto> GetAllDtos()
        {
            return _context.Tags.Select(t => t.Id).ToList().Select(id => GetDto(id));
        }

        public ITagDto GetDto(int id)
        {
            Tag tag = _context.Tags.Find(id);
            if (tag != null)
                return new TagDto { Id = tag.Id, Name = tag.Name };

            return null;
        }
    }
}