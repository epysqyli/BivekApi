using Api.Data;
using Api.Interfaces;

namespace Api.Models
{
    public class TagRepository : GenericRepository<Tag>, ITagRepository
    {
        public TagRepository(ApiDbContext context) : base(context)
        { }
    }
}