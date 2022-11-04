using UnityEngine;

[CreateAssetMenu(fileName = "Movement Speed", menuName = "Skills/Movement Speed")]
public class MovementSpeed : Skill
{
    [Min(0)] public float increasePerLevel = 0.05f;

    private PlayerCharacterController playerMovement;
    private string description = ".";

    public override string Description =>
        string.Format("Increases the Player's Movement Speed{0}", description);

    public override void OnActivate()
    {
        var player = GameObject.Find("Player");
        playerMovement = player.GetComponent<PlayerCharacterController>();
    }

    public override void OnLevelUp(int level)
    {
        playerMovement.NormalSpeedModifier += increasePerLevel;
        playerMovement.SprintSpeedModifier += increasePerLevel;

        description = string.Format(" by {0:P1}.", increasePerLevel * level);
    }

    public override void Update() {}
}
