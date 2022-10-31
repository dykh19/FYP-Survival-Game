using UnityEngine;

[CreateAssetMenu(fileName = "Double Jump", menuName = "Skills/Double Jump")]
public class DoubleJump : Skill
{
    private PlayerCharacterController playerMovement;
    private bool hasDoubleJumped = false;

    public override string Description => "Allows the Player to Double Jump.";

    public override void OnActivate()
    {
        var player = GameObject.Find("Player");
        playerMovement = player.GetComponent<PlayerCharacterController>();
    }

    public override void OnLevelUp(int level) {}

    public override void Update()
    {
        if (playerMovement.IsGrounded)
        {
            hasDoubleJumped = false;
        }
        else if (!hasDoubleJumped && Input.GetKeyDown(KeyCode.Space))
        {
            var velocity = playerMovement.CharacterVelocity;
            var y = Mathf.Max(0, velocity.y) + playerMovement.JumpForce;

            playerMovement.CharacterVelocity = new Vector3(velocity.x, y, velocity.z);
            hasDoubleJumped = true;
        }
    }
}
