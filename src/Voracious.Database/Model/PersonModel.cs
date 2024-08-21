using Voracious.Core.Enum;
using Voracious.Core.Interface;

namespace Voracious.Database.Model;

public class PersonModel : IPerson
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Aliases { get; set; }
    public int BirthDate { get; set; }
    public int DeathDate { get; set; }
    public string Webpage { get; set; }
    public RelatorEnum PersonType { get; set; }
}
