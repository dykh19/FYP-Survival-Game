using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

// Written by Nicholas Sebastian Hendrata on 16/08/2022.

[CustomEditor(typeof(TerrainPreset))]
public class TerrainPresetEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var terrainPreset = target as TerrainPreset;

        // Notes.
        EditorGUILayout.HelpBox(
            "Sometimes you might get a NullReferenceException when trying to " +
            "generate a map from this TerrainPreset Editor. I don't know why this happens. " +
            "I added a button on the World Generator inspector to fix this error though.",
            MessageType.None
        );

        GUILayout.Space(4);

        // Notes.
        EditorGUILayout.HelpBox(
            "Make sure all the prefabs used in the resource spawner already have colliders " +
            "attached because the scripts will not automatically attach one for you.",
            MessageType.None
        );

        GUILayout.Space(10);

        // Default content.
        DrawDefaultInspector();

        GUILayout.Space(4);

        // Buttons.
        if (GUILayout.Button("Generate World"))
            //GameManager.Instance.WorldGen.GetComponent<WorldGenerator>().CreateWorld();

        if (GUILayout.Button("Generate Empty World"))
            //GameManager.Instance.WorldGen.GetComponent<WorldGenerator>().CreateTerrain();

        if (GUILayout.Button("Destroy World"))
            //GameManager.Instance.WorldGen.GetComponent<WorldGenerator>().Reset();

        GUILayout.Space(10);

        // Header.
        EditorGUILayout.LabelField("Editor Tools", EditorStyles.boldLabel);

        // Button.
        if (GUILayout.Button("Generate Data Table"))
        {
            if (terrainPreset.editorNotes.Length > 0)
                terrainPreset.editorNotes = "";
            try
            {
                SetLayerTable();
                SetSpawnTable();
            }
            catch { throw; }
            finally
            {
                terrainPreset.editorNotes += string.Format(
                    "\n\nLast updated on {0:d} at {0:t}", DateTime.Now);
            }
        }

        // Output notes.
        if (terrainPreset.editorNotes.Length > 0)
        {
            GUILayout.Space(4);
            EditorGUILayout.HelpBox(terrainPreset.editorNotes, MessageType.None);
        }
    }

    private void SetLayerTable()
    {
        var terrainPreset = target as TerrainPreset;

        // Table header.
        terrainPreset.editorNotes += "Name\t\tStart Height";

        // Table body displaying every terrain height in world space.
        foreach (var terrainType in MaterialGenerator.Main.terrainTypes)
            terrainPreset.editorNotes += string.Format("\n{0}\t\t{1:F1}",
                terrainType.name,
                MeshGenerator.Main.NormalToWorldHeight(terrainType.height));
    }

    private void SetSpawnTable()
    {
        var terrainPreset = target as TerrainPreset;

        // Enable normal rendering mode.
        GameManager.Instance.WorldGen.GetComponent<WorldGenerator>().displayMode = ShaderMode.CustomTerrainShader;
        GameManager.Instance.WorldGen.GetComponent<WorldGenerator>().randomSeed = true;

        // Get all the necessary data.
        var iterCount = 10;
        var spawnData = CalculateSpawns(iterCount).OrderByDescending(obj => obj.Value);
        var averageCount0 = spawnData.ElementAt(0).Value / iterCount;

        // Table header.
        terrainPreset.editorNotes += "\n\nName\t\tAvg\tHeight\t\tR.Rate";

        // Table body displaying data of every resource spawned.
        foreach (var obj in spawnData)
        {
            var objRef = PrefabSpawner.Main.spawnables.First(s => s.name == obj.Key);
            var spawnHeight = MeshGenerator.Main.NormalToWorldHeight(objRef.spawnHeight);

            var averageCount = obj.Value / iterCount;
            var minHeight = spawnHeight - objRef.spawnRange;
            var maxHeight = spawnHeight + objRef.spawnRange;
            var relativeRate = ((float)averageCount / averageCount0) * 100;

            terrainPreset.editorNotes += string.Format("\n{0}{5}{1}\t{2:F1} - {3:F1}\t{4:F1}%",
                obj.Key, averageCount, minHeight, maxHeight, relativeRate,
                (obj.Key.Length > 8) ? "\t" : "\t\t");
        }
    }

    private static Dictionary<string, int> CalculateSpawns(int iterCount = 10)
    {
        var counter = new Dictionary<string, int>();

        for (int i = 0; i < iterCount; i++)
        {
            // Generate world.
            GameManager.Instance.WorldGen.GetComponent<WorldGenerator>().CreateWorld(false);

            // For each object spawned by the world:
            for (int j = 0; j < GameManager.Instance.WorldGen.GetComponent<WorldGenerator>().transform.childCount; j++)
            {
                var spawnedObject = GameManager.Instance.WorldGen.GetComponent<WorldGenerator>().transform.GetChild(j);

                // Find the corresponding spawn data then increment.
                foreach (var spawnable in PrefabSpawner.Main.spawnables)
                    if (spawnedObject.name == spawnable.name)
                    {
                        if (counter.ContainsKey(spawnable.name))
                            counter[spawnable.name]++;
                        else
                            counter.TryAdd(spawnable.name, 1);
                    }
            }
        }

        return counter;
    }
}
