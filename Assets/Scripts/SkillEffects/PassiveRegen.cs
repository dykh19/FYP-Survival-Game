using UnityEngine;

[CreateAssetMenu(fileName = "Passive Regeneration", menuName = "Skills/Passive Regeneration")]
public class PassiveRegen : Skill
{
    private readonly int[] healPerLevel = { 2, 4, 6, 8, 10 };
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
        int i = level - 1;
        playerHealth.HealthRegenValue = healPerLevel[i];
        description = string.Format(" {0} ", healPerLevel[i]);
    }

    public override void Update() {}
}
