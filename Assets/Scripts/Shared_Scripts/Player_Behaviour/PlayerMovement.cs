using UnityEngine;

// Written by Nicholas Sebastian Hendrata on 14/08/2022.

// TODO: Footstep sounds based on terrain surface.
//       This can be done by checking the terrain height at the player's feet
//       then playing the corresponding sound set for that surface.

// TODO: Bug - Sometimes jump doesn't trigger when moving at the same time.

// TODO: Bug - Sometimes the player can super jump when jumping onto the
//       edge of an Ore crystal for some reason.

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 1.75f;
    public float runningSpeed = 3f;
    public float jumpStrength = 4f;
    public float gravity = 9.81f;
    public float runningEnergyConsumption = 0.2f;

    private CharacterController controller;
    private float verticalVelocity;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (GameManagerJoseph.Main.isPlaying)
        {
            Movement();
            JumpControl();
        }
    }

    void LateUpdate()
    {
        SimulateGravity();
    }

    private void Movement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (horizontal != 0 || vertical != 0)
        {
            var horizontalMovement = horizontal * transform.right;
            var verticalMovement = vertical * transform.forward;
            var movement = horizontalMovement + verticalMovement;

            var isRunning = Input.GetKey(KeyCode.LeftShift);
            var canRun = GameManagerJoseph.Main.playerStatus.playerEnergy > runningEnergyConsumption;

            if (isRunning && canRun)
            {
                controller.Move(movement * runningSpeed * Time.deltaTime);
                GameManagerJoseph.Main.playerStatus.playerEnergy -= runningEnergyConsumption;
            }
            else
            {
                controller.Move(movement * movementSpeed * Time.deltaTime);
            }
        }
    }

    private void JumpControl()
    {
        bool spacebar = Input.GetKey(KeyCode.Space);

        if (spacebar && controller.isGrounded)
            verticalVelocity += jumpStrength;
    }

    private void SimulateGravity()
    {
        if (controller.isGrounded && verticalVelocity < 0)
            verticalVelocity = 0;
        else
            verticalVelocity -= gravity * Time.deltaTime;

        controller.Move(verticalVelocity * Time.deltaTime * Vector3.up);
    }
}
