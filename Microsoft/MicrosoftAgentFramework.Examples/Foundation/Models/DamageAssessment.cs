namespace MicrosoftAgentFramework.Examples.Foundation.Models;

public class DamageAssessment
{
    public bool IsDamaged { get; set; }

    public DamageAssessmentArea AreaOfDamage { get; set; } = DamageAssessmentArea.NoDamage;

    public string Description { get; set; } = string.Empty;
}
