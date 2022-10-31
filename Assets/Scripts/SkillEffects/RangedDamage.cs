using UnityEngine;

[CreateAssetMenu(fileName = "Ranged Damage", menuName = "Skills/Ranged Damage")]
public class RangedDamage : Skill
{
    [Min(0)] public float increasePerLevel = 0.2f;

    private RangedWeaponController[] rangedWeapons;
    private string description = ".";

    public override string Description =>
        string.Format("Increases Ranged Weapon Damage{0}", description);

    public override void OnActivate()
    {
        rangedWeapons = FindObjectsOfType<RangedWeaponController>(true);
    }

    public override void OnLevelUp(int level)
    {
        description = string.Format(" by {0:P1}.", increasePerLevel * level);

        foreach (var rangedWeapon in rangedWeapons)
        {
            rangedWeapon.damageModifier = increasePerLevel * level;
        }
    }

    public override void Update() { }
}
