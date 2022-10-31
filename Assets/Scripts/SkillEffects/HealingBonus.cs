using UnityEngine;

[CreateAssetMenu(fileName = "Healing Bonus", menuName = "Skills/Healing Bonus")]
public class HealingBonus : Skill
{
    [Min(0)] public float increasePerLevel = 0.1f;

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
        playerHealth.HealingBonus += increasePerLevel;
        description = string.Format(" by {0:P1}.", increasePerLevel * level);
    }

    public override void Update() {}
}
