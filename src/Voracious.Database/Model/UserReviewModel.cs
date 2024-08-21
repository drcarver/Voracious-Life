namespace Voracious.Database.Model;

public class UserReviewModel : IUserReview
{
    public int Id { get; set; }
    public string BookId { get; set; }
    public DateTimeOffset CreateDate { get; set; }
    public DateTimeOffset MostRecentModificationDate { get; set; }
    public double NStars { get; set; }
    public string Review { get; set; }
    public string Tags { get; set; }
}
