using Api.Data;
using Api.Interfaces;

namespace Api.Models
{
    public class TagRepository : GenericRepository<Tag>, ITagRepository
    {
        public TagRepository(ApiDbContext context) : base(context)
        { }

        public IEnumerable<ITagDto> GetAllDtos()
        {
            IEnumerable<int> tagIds = _context.Tags.Select(t => t.Id).ToList();
            List<ITagDto> tagDtos = new List<ITagDto>();
            foreach (int id in tagIds)
                tagDtos.Add(GetTagDto(id));

            return tagDtos;
        }

        public ITagDto GetTagDto(int id)
        {
            Tag tag = _context.Tags.Find(id);
            if (tag != null)
                return new TagDto { Id = tag.Id, Name = tag.Name };

            return null;
        }
    }
}