using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollSys.Matm.Kitabxana.Presentation.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CollSys.Matm.Kitabxana.Presentation.Data
{
    public class UserContext : IdentityDbContext<UserModel>
    {
        public UserContext(DbContextOptions<UserContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);


            // Indexing and making values unique
            builder.Entity<UserModel>().HasIndex(c => c.NormalizedEmail).IsUnique();


            // Self Join
            builder.Entity<UserModel>().HasOne(c => c.Reference).WithMany().HasForeignKey(c => c.ReferenceId);

        }
    }
}
