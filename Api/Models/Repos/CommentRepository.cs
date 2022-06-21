using Api.Data;
using Api.Interfaces;
using Api.Models.Entities;

namespace Api.Models.Repositories
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        public CommentRepository(ApiDbContext context) : base(context)
        { }
    }
}