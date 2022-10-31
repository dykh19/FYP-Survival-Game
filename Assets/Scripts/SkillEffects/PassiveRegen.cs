using UnityEngine;

[CreateAssetMenu(fileName = "Passive Regeneration", menuName = "Skills/Passive Regeneration")]
public class PassiveRegen : Skill
{
    [Min(0)] public int increasePerLevel = 2;

    private Health playerHealth;
    private string description = " ";

    public override string Description =>
        string.Format("Player regenerates{0}Health every second.", description);

    public override void OnActivate()
    {
        var player = GameObject.Find("Player");
        playerHealth = player.GetComponent<Health>();
    }

    public override void OnLevelUp(int level)
    {
        playerHealth.HealthRegenValue = increasePerLevel * level;
        description = string.Format(" {0} ", increasePerLevel * level);
    }

    public override void Update() {}
}
