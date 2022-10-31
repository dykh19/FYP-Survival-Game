using UnityEngine;

[CreateAssetMenu(fileName = "Jump Height", menuName = "Skills/Jump Height")]
public class JumpHeight : Skill
{
    public float increasePerLevel = 0.5f;

    private PlayerCharacterController playerMovement;
    private string description = ".";

    public override string Description =>
        string.Format("Increases the Player's Jump Height{0}", description);

    public override void OnActivate()
    {
        var player = GameObject.Find("Player");
        playerMovement = player.GetComponent<PlayerCharacterController>();
    }

    public override void OnLevelUp(int level)
    {
        playerMovement.JumpForce += increasePerLevel;
        description = string.Format(" by {0:P1}.", increasePerLevel * level);
    }

    public override void Update() {}
}
