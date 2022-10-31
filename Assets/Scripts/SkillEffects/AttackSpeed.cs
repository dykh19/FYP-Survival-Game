using UnityEngine;

[CreateAssetMenu(fileName = "Attack Speed", menuName = "Skills/Attack Speed")]
public class AttackSpeed : Skill
{
    [Min(0)] public float increasePerLevel = 0.2f;

    private MeleeWeaponController[] meleeWeapons;
    private RangedWeaponController[] rangedWeapons;
    private string description = ".";

    public override string Description =>
        string.Format("Increases Weapon Attack Speed{0}", description);

    public override void OnActivate()
    {
        meleeWeapons = FindObjectsOfType<MeleeWeaponController>(true);
        rangedWeapons = FindObjectsOfType<RangedWeaponController>(true);
    }

    public override void OnLevelUp(int level)
    {
        description = string.Format(" by {0:P1}.", increasePerLevel * level);

        foreach (var meleeWeapon in meleeWeapons)
        {
            meleeWeapon.attackSpeed = 1 + (increasePerLevel * level);
        }

        foreach (var rangedWeapon in rangedWeapons)
        {
            rangedWeapon.delayBetweenShots = 0.5f - (increasePerLevel * level / 4);
        }
    }

    public override void Update() { }
}
