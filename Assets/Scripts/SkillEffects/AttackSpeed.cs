using UnityEngine;

[CreateAssetMenu(fileName = "Attack Speed", menuName = "Skills/Attack Speed")]
public class AttackSpeed : Skill
{
    [Min(0)] public float increasePerLevel = 0.03f;

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
        description = string.Format(" by {0:P0}.", increasePerLevel * level);
        foreach (var meleeWeapon in meleeWeapons)
        {
            if (level == 1)
                meleeWeapon.attackSpeed = meleeWeapon.attackSpeed - increasePerLevel;
            else
            {
                meleeWeapon.attackSpeed = meleeWeapon.attackSpeed + (increasePerLevel * (level - 1));
                meleeWeapon.attackSpeed = meleeWeapon.attackSpeed - (increasePerLevel * level);
            }
        }

        foreach (var rangedWeapon in rangedWeapons)
        {
            if (level == 1)
                rangedWeapon.delayBetweenShots = rangedWeapon.delayBetweenShots - increasePerLevel;
            else
            {
                rangedWeapon.delayBetweenShots = rangedWeapon.delayBetweenShots + (increasePerLevel * (level - 1));
                rangedWeapon.delayBetweenShots = rangedWeapon.delayBetweenShots - (increasePerLevel * level);
            }
        }
    }

    public override void Update() { }
}
