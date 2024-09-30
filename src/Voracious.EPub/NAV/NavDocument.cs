namespace Voracious.EPub.NAV;

public class NavDocument
{
    public NavHead Head { get; set; } = new NavHead();
    public NavBody Body { get; set; } = new NavBody();
}
