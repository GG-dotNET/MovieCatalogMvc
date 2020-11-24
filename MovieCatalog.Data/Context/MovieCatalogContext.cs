using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieCatalog.Model.Models;

namespace MovieCatalog.Data.Context
{
    public class MovieCatalogContext : IdentityDbContext<IdentityUser>
    {
        public MovieCatalogContext(DbContextOptions<MovieCatalogContext> options) : base (options)
        {

        }

        public DbSet<Film> Films { get; set; }
    }
}
