using Api.Data;
using Api.Interfaces;

namespace Api.Models
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        public CommentRepository(ApiDbContext context) : base(context)
        { }
    }
}