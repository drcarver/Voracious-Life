using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Voracious.RDF.Model;

namespace Voracious.Database.Configuration;

public class CreatorEntityTypeConfiguration : IEntityTypeConfiguration<Creator>
{
    public void Configure(EntityTypeBuilder<Creator> builder)
    {
        builder
            .HasKey(c => c.About);

        builder
            .ToTable("Creators");

        builder
            .Navigation(r => r.Resources);
            //.AutoInclude();

        builder
            .Property(c => c.About)
            .IsRequired()
            .HasComment("An unambiguous reference to the resource within a given context. Recommended practice is to identify the resource by means of a string conforming to an identification system");

        builder
            .Property(c => c.Role)
            .HasComment("The nature or genre of the content of the resource. Type includes terms describing general categories, functions, genres, or aggregation levels for content.");

        builder
            .Property(c => c.Aliases)
            .HasComment("Alias for the contributor");

        builder
            .Property(c => c.DeathDate)
            .HasComment("The year of death of the contributor");

        builder
            .Property(c => c.BirthDate)
            .HasComment("The year of birth of the contributor");

        builder
            .Property(c => c.FileAs)
            .HasComment("The name the contributor should be filed nder");

        builder
            .Property(c => c.Webpage)
            .HasComment("The web page for the contributor");

        builder
            .Property(c => c.Name)
            .HasComment("The name of the contributor");
    }
}