using UnityEngine;

[CreateAssetMenu(fileName = "Jump Height", menuName = "Skills/Jump Height")]
public class JumpHeight : Skill
{
    private PlayerCharacterController playerMovement;
    private const int increasePerLevel = 2;

    public override string Description => "Increases the Player's Jump Height.";

    public override void OnActivate()
    {
        var player = GameObject.Find("Player");
        playerMovement = player.GetComponent<PlayerCharacterController>();
    }

    public override void OnLevelUp(int level)
    {
        playerMovement.JumpForce += increasePerLevel;
    }

    public override void Update() {}
}
