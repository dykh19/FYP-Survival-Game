using UnityEngine;

[CreateAssetMenu(fileName = "Attack Speed", menuName = "Skills/Attack Speed")]
public class AttackSpeed : Skill
{
    private readonly float[] speedPerLevel = { 0.2f, 0.4f, 0.6f, 0.8f, 1.0f };
    private MeleeWeaponController[] meleeWeapons;
    private RangedWeaponController[] rangedWeapons;
    private string description = ".";

    public override string Description =>
        string.Format("Increases Weapon Attack Speed{0}", description);

    public override void OnActivate()
    {
        meleeWeapons = FindObjectsOfType<MeleeWeaponController>(true);
    }

    public override void OnLevelUp(int level)
    {
        int i = level - 1;
        description = string.Format(" by {0:P1}.", speedPerLevel[i]);

        foreach (var meleeWeapon in meleeWeapons)
        {
            meleeWeapon.attackSpeed = 1 + speedPerLevel[i];
        }

        foreach (var rangedWeapon in rangedWeapons)
        {
            rangedWeapon.delayBetweenShots = 0.5f - (speedPerLevel[i] / 4);
        }
    }

    public override void Update() { }
}
