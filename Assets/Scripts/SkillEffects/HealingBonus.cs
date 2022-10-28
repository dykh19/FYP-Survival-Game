using UnityEngine;

[CreateAssetMenu(fileName = "Healing Bonus", menuName = "Skills/Healing Bonus")]
public class HealingBonus : Skill
{
    private readonly float[] bonusPerLevel = { 0.1f, 0.2f, 0.3f, 0.4f, 0.5f };
    private Health playerHealth;
    private string description = ".";

    public override string Description =>
        string.Format("Boosts Healing Amount{0}", description);

    public override void OnActivate()
    {
        var player = GameObject.Find("Player");
        playerHealth = player.GetComponent<Health>();
    }

    public override void OnLevelUp(int level)
    {
        int i = level - 1;
        playerHealth.HealingBonus = bonusPerLevel[i];
        description = string.Format(" by {0:P1}", bonusPerLevel[i]);
    }

    public override void Update() {}
}
