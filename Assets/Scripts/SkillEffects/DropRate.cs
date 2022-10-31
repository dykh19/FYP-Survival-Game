using UnityEngine;

[CreateAssetMenu(fileName = "Drop Rate", menuName = "Skills/Drop Rate")]
public class DropRate : Skill
{
    private string description = ".";
    public override string Description =>
        string.Format("Increases the number of items dropped by enemies{0}", description);

    public override void OnActivate() {}

    public override void OnLevelUp(int level)
    {
        EnemyBehavior.dropBonus = level;
        description = string.Format(" by {0}.", level);
    }

    public override void Update() {}
}
