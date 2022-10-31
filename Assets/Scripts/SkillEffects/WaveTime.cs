using UnityEngine;

[CreateAssetMenu(fileName = "Wave Time", menuName = "Skills/Wave Time")]
public class WaveTime : Skill
{
    [Min(0)] public int increasePerLevel = 10;

    private string description = ".";

    public override string Description => string.Format("Increases Time between waves{0}", description);

    public override void OnActivate() {}

    public override void OnLevelUp(int level)
    {
        GameManager.Instance.TimeToNextWave += increasePerLevel;
        description = string.Format(" by {0}.", increasePerLevel * level);
    }

    public override void Update() {}
}
