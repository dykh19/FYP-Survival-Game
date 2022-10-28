using UnityEngine;

[CreateAssetMenu(fileName = "Ranged Damage", menuName = "Skills/Ranged Damage")]
public class RangedDamage : Skill
{
    private readonly float[] damagePerLevel = { 0.2f, 0.4f, 0.6f, 0.8f, 1.0f };
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
        int i = level - 1;
        description = string.Format(" by {0:P1}.", damagePerLevel[i]);

        foreach (var rangedWeapon in rangedWeapons)
        {
            rangedWeapon.damageModifier = damagePerLevel[i];
        }
    }

    public override void Update() { }
}
