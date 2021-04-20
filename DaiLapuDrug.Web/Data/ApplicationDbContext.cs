using DaiLapuDrug.Web.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace DaiLapuDrug.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext([NotNull] DbContextOptions options) : base(options)
        {
        }

        public DbSet<Pet> Pets { get; set; }

        public DbSet<Article> Articles { get; set; }

        public DbSet<FileAttachment> FileAttachments { get; set; }

        public DbSet<Option> Options { get; set; }

        public DbSet<PetOption> PetOptions { get; set; }

        public DbSet<PetFileAttachment> PetFileAttachments { get; set; }
    }
}
