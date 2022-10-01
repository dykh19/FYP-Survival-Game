using System.Reflection;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WorldGenerator))]
public class WorldGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox(
            "Instructions:\n" +
            "1) Create a Terrain Preset by Right Click > Create > Terrain Configuration.\n" +
            "2) Set all the map, texture, mesh and spawn settings.\n" +
            "3) Drag the Terrain Preset you just made into the Terrain Preset field.",
            MessageType.None
        );

        GUILayout.Space(10);

        var worldGenerator = (WorldGenerator)target;
        var update = DrawDefaultInspector();

        if (update && worldGenerator.autoUpdate)
            //worldGenerator.CreateWorld();

        GUILayout.Space(4);

        if (GUILayout.Button("Generate World"))
            //worldGenerator.CreateWorld();

        if (GUILayout.Button("Generate Empty World"))
            //worldGenerator.CreateTerrain();

        if (GUILayout.Button("Destroy World"))
            //worldGenerator.Reset();

        GUILayout.Space(10);
        EditorGUILayout.LabelField("Debug Controls", EditorStyles.boldLabel);

        if (GUILayout.Button("Fix NullReferenceException"))
        {
            //worldGenerator.SetInstance();
            ClearLog();
        }
    }

    private static void ClearLog()
    {
        var assembly = Assembly.GetAssembly(typeof(Editor));
        var type = assembly.GetType("UnityEditor.LogEntries");
        var method = type.GetMethod("Clear");

        method.Invoke(new object(), null);
    }
}
