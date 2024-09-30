//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;

//using Voracious.Core.Model;
//using Voracious.Core.;

//using static System.Net.Mime.MediaTypeNames;
//namespace Voracious.Database.Configuration;

//public class UserNoteEntityTypeConfiguration : IEntityTypeConfiguration<UserNote>
//{
//    public void Configure(EntityTypeBuilder<UserNote> builder)
//    {
//        builder
//            .HasKey(u => u.Id);
            
//        builder
//            .ToTable("UserNote");

//        builder
//            .Property(u => u.Id)
//            .IsRequired()
//            .HasComment("An unambiguous reference to the resource within a given context. Recommended practice is to identify the resource by means of a string conforming to an identification system");

//        builder
//            .Property(u => u.CreateDate)
//            .IsRequired()
//            .HasComment("The create date for the book.");

//        builder
//            .Property(u => u.MostRecentModificationDate)
//            .IsRequired()
//            .HasComment("The most recent modification date for the book.");

//        builder
//            .Property(u => u.Location)
//            .IsRequired()
//            .HasComment("The location of the note in the book.");

//        builder
//            .Property(u => u.Text)
//            .IsRequired()
//            .HasComment("The text of the note.");

//        builder
//            .Property(u => u.Tags)
//            .IsRequired()
//            .HasComment("The tags of the note.");

//        builder
//            .Property(u => u.Icon)
//            .IsRequired()
//            .HasComment("The icon for the note.");

//        builder
//            .Property(u => u.BackgroundColor)
//            .IsRequired()
//            .HasComment("The background for the note.");

//        builder
//            .Property(u => u.ForegroundColor)
//            .IsRequired()
//            .HasComment("The foreground for the note.");

//        builder
//            .Property(u => u.SelectedText)
//            .IsRequired()
//            .HasComment("The selected text for the note.");
//    }
//}