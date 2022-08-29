using Microsoft.EntityFrameworkCore;
using ResourceMagnat.Models;

namespace ResourceMagnat.Data;

public class SessionDbContext: DbContext
{
    public SessionDbContext(DbContextOptions<SessionDbContext> options) : base(options)
    {

    }

    public DbSet<Session> Sessions { get; set; }
}