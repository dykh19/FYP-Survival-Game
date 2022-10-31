using UnityEngine;

[CreateAssetMenu(fileName = "Resource Gathering", menuName = "Skills/Resource Gathering")]
public class ResourceGathering : Skill
{
    private string description = ".";
    public override string Description =>
        string.Format("Increases the number of items dropped by resources{0}", description);

    public override void OnActivate() {}

    public override void OnLevelUp(int level)
    {
        Breakable.dropBonus = level;
        description = string.Format(" by {0}.", level);
    }

    public override void Update() {}
}
