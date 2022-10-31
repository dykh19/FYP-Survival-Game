using UnityEngine;

[CreateAssetMenu(fileName = "Melee Damage", menuName = "Skills/Melee Damage")]
public class MeleeDamage : Skill
{
    [Min(0)] public float increasePerLevel = 0.2f;

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
        description = string.Format(" by {0:P1}.", increasePerLevel * level);

        foreach (var meleeWeapon in meleeWeapons)
        {
            meleeWeapon.damageModifier = increasePerLevel * level;
        }
    }

    public override void Update() {}
}
