using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Voracious.Core.Model;

namespace Voracious.Database.Configuration;

public class PersonEntityTypeConfiguration : IEntityTypeConfiguration<PersonModel>
{
    public void Configure(EntityTypeBuilder<PersonModel> builder)
    {
        builder
            .HasKey(p => p.About);

        builder
            .ToTable("Person");

        builder
            .Property(p => p.About)
            .IsRequired()
            .HasComment("An unambiguous reference to the resource within a given context. Recommended practice is to identify the resource by means of a string conforming to an identification system");

        builder
            .Property(p => p.Relator)
            .HasComment("The nature or genre of the content of the resource. Type includes terms describing general categories, functions, genres, or aggregation levels for content.");

        builder
            .Property(p => p.Aliases)
            .HasComment("Alias for the contributor");

        builder
            .Property(p => p.DeathDate)
            .HasComment("The year of death of the contributor");

        builder
            .Property(p => p.BirthDate)
            .HasComment("The year of birth of the contributor");

        builder
            .Property(p => p.FileAs)
            .HasComment("The name the contributor should be filed nder");

        builder
            .Property(p => p.Webpage)
            .HasComment("The web page for the contributor");

        builder
            .Property(p => p.Name)
            .HasComment("The name of the contributor");
    }
}