using UnityEngine;

[CreateAssetMenu(fileName = "Health", menuName = "Skills/Health")]
public class HealthBoost : Skill
{
    [Min(0)] public int increasePerLevel = 50;

    private Health playerHealth;
    private string description = ".";

    public override string Description =>
        string.Format("Increases the player health{0}", description);

    public override void OnActivate()
    {
        var player = GameObject.Find("Player");
        playerHealth = player.GetComponent<Health>();
    }

    public override void OnLevelUp(int level)
    {
        playerHealth.MaxHealth += increasePerLevel;
        description = string.Format(" by {0}.", increasePerLevel * level);
    }

    public override void Update() {}
}
