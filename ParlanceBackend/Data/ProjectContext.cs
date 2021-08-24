using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ParlanceBackend.Models;

namespace ParlanceBackend.Data
{
    public class ProjectContext : DbContext
    {
        public ProjectContext (DbContextOptions<ProjectContext> options)
            : base(options)
        {
        }

        public DbSet<ProjectPrivate> Projects { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<AllowedLanguages> AllowedLanguages { get; set; }
        public DbSet<Superuser> Superusers { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Language>().HasData(CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                .Distinct()
                .Where(
                    culture =>
                    {
                        try
                        {
                            new RegionInfo(culture.LCID);
                            return true;
                        }
                        catch (CultureNotFoundException e)
                        {
                            return false;
                        }
                    })
                .Select(culture => (culture, new RegionInfo(culture.LCID)))
                .Select(culture => new Language()
            {
                Identifier = $"{culture.culture.TwoLetterISOLanguageName}-{culture.Item2.TwoLetterISORegionName}",
                Name = culture.culture.DisplayName
            }).Distinct());
        }
    }
}
