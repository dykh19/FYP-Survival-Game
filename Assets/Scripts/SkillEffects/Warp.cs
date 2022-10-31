using UnityEngine;

[CreateAssetMenu(fileName = "Warp", menuName = "Skills/Warp")]
public class Warp : Skill
{
    private readonly Vector3 spawnPoint = new (16.84f, 11.3f, 5.7f);
    private GameObject player;

    public override string Description => "Allows the Player to Warp home with the 'C' key.";

    public override void OnActivate()
    {
        player = GameObject.Find("Player");
    }

    public override void OnLevelUp(int level) {}

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            player.transform.position = spawnPoint;
            // TODO: Play sound effect.
        }
    }
}
