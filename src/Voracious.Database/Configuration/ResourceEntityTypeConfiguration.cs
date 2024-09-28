using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Voracious.RDF.Model;

namespace Voracious.Database.Configuration;

public class ResourceEntityTypeConfiguration : IEntityTypeConfiguration<Resource>
{
    public void Configure(EntityTypeBuilder<Resource> builder)
    {
        builder
            .HasKey(b => b.About);
            
        builder
        .ToTable("Resources");

        builder
            .Navigation(r => r.Creators)
            .AutoInclude();

        //builder
        //    .Navigation(r => r.Files)
        //    .AutoInclude();

        builder
            .Property(b => b.About)
            .IsRequired()
            .HasComment("An unambiguous reference to the resource within a given context. Recommended practice is to identify the resource by means of a string conforming to an identification system");

        builder
            .Property(b => b.Publisher)
            .HasComment("Book Source - usually Gutenberg");

        builder
            .Property(b => b.BookType)
            .HasComment("Most are Text. Human genome project e.g 3501 is Dataset.");

        builder
            .Property(b => b.Description)
            .HasComment("An account of the resource. Description may include but is not limited to: an abstract, a table of contents, a graphical representation, or a free-text account of the resource.");

        builder
            .Property(b => b.Imprint)
            .HasComment("Information relating to the publication, printing, distribution, issue, release, or production of a work.");

        builder
            .Property(b => b.Issued)
            .HasComment("A related resource of which the described resource is a version, edition, or adaptation. Changes in version imply substantive changes in content rather than differences in format. dcterms:issued Date of formal issuance (e.g., publication) of the resource.");

        builder
            .Property(b => b.Title)
            .HasComment("A name given to the resource.");

        builder
            .Property(b => b.TitleAlternative)
            .HasComment("An alternative name for the resource.");

        builder
            .Property(b => b.Language)
            .HasComment("A language of the resource.");

        builder
            .Property(b => b.LCSH)
            .HasComment("The set of labeled concepts specified by the Library of Congress Subject Headings.");

        builder
            .Property(b => b.LCCN)
            .HasComment("Unique number assigned to a record by the Library of Congress (LC) or a cooperative cataloging partner contributing authority records to the Name Authority Cooperative Program (NACO) database. The field is also assigned to records created by LC for the Library of Congress Subject Headings (LCSH).");

        builder
            .Property(b => b.PGEditionInfo)
            .HasComment("PG Edition Info.");

        builder
            .Property(b => b.PGProducedBy)
            .HasComment("PG Edition Info.");

        builder
            .Property(b => b.CreationProductionCreditsNote)
            .HasComment("Credits for persons or organizations, other than members of the cast, who have participated in the creation and/or production of the work. The introductory term Credits: is usually generated as a display constant.");

        builder
            .Property(b => b.BookSeries)
            .HasComment("Marc440 - Series statement consisting of a series title alone");

        builder
            .Property(b => b.LCC)
            .HasComment("The Library of Congress call number scheme is a standard used in academic libraries nationwide.");
    }
}