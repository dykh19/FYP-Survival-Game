using UnityEngine;
using UnityEditor;

// Written by Nicholas Sebastian Hendrata on 15/08/2022.

public class GameManagerJoseph : MonoBehaviour
{
    public static GameManagerJoseph Main { get; private set; }

    public Vector2 spawnPoint;
    public GameItem[] startingItems; // For testing.

    [HideInInspector] public bool isPlaying = true;
    [HideInInspector] public PlayerStatus playerStatus;
    [HideInInspector] public Inventory playerInventory;
    public UserInterface[] userInterfaces;

    void Awake()
    {
        Main = this;
        playerInventory = new Inventory();
    }

    void Start()
    {
        WorldGenerator.Main.CreateWorld();
        playerStatus = SpawnPlayer();

        Cursor.lockState = CursorLockMode.Locked;

        foreach (var item in startingItems) // For testing.
            playerInventory.AddItem(item);
    }

    private PlayerStatus SpawnPlayer()
    {
        var rayOrigin = new Vector3(spawnPoint.x, 100f, spawnPoint.y);
        var ray = new Ray(rayOrigin, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            var player = new GameObject("Player",
                typeof(PlayerMovement),
                typeof(PlayerLook),
                typeof(PlayerOpenUI),
                typeof(PlayerStatus),
                typeof(PlayerStatus));

            player.transform.position = hit.point + hit.normal;
            player.transform.forward = Vector3.back;

            return player.GetComponent<PlayerStatus>();
        }

        return null;
    }
}

[CustomEditor(typeof(GameManagerJoseph))]
public class GameManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox(
            "Survival Game Final Year Project members:\n" +
            "Damian, Eddie, Joseph, Nicholas, Sean",
            MessageType.None
        );

        GUILayout.Space(10);
        DrawDefaultInspector();
    }
}
