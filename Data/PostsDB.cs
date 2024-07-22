using MiApi.Models;
using Microsoft.EntityFrameworkCore;

namespace MiApi.Data
{
    public class PostsDB : DbContext
    {
        public PostsDB(DbContextOptions<PostsDB> options): base(options)
        {
            
        }
        public DbSet<Post> Posts => Set<Post>();
    }
}
