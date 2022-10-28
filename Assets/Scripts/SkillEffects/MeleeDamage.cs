using UnityEngine;

[CreateAssetMenu(fileName = "Melee Damage", menuName = "Skills/Melee Damage")]
public class MeleeDamage : Skill
{
    private readonly float[] damagePerLevel = { 0.2f, 0.4f, 0.6f, 0.8f, 1.0f };
    private MeleeWeaponController[] meleeWeapons;
    private string description = ".";

    public override string Description =>
        string.Format("Increases Melee Weapon Damage{0}", description);

    public override void OnActivate()
    {
        meleeWeapons = FindObjectsOfType<MeleeWeaponController>(true);
    }

    public override void OnLevelUp(int level)
    {
        int i = level - 1;
        description = string.Format(" by {0:P1}.", damagePerLevel[i]);

        foreach (var meleeWeapon in meleeWeapons)
        {
            meleeWeapon.damageModifier = damagePerLevel[i];
        }
    }

    public override void Update() {}
}
