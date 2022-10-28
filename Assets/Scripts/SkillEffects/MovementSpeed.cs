using UnityEngine;

[CreateAssetMenu(fileName = "Movement Speed", menuName = "Skills/Movement Speed")]
public class MovementSpeed : Skill
{
    private PlayerCharacterController playerMovement;
    private const float increasePerLevel = 0.3f;

    public override string Description => "Increases the Player's Movement Speed.";

    public override void OnActivate()
    {
        var player = GameObject.Find("Player");
        playerMovement = player.GetComponent<PlayerCharacterController>();
    }

    public override void OnLevelUp(int level)
    {
        playerMovement.NormalSpeedModifier += increasePerLevel;
        playerMovement.SprintSpeedModifier += increasePerLevel;
    }

    public override void Update() {}
}
