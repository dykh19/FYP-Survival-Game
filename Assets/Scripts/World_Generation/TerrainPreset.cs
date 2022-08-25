using UnityEngine;

// Written by Nicholas Sebastian Hendrata on 13/08/2022.

[CreateAssetMenu(fileName = "New Terrain Configuration", menuName = "Terrain Configuration")]
public class TerrainPreset : ScriptableObject
{
    public MapGenerator mapGenerator;
    public MaterialGenerator materialGenerator;
    public MeshGenerator meshGenerator;
    public PrefabSpawner prefabSpawner;

    // For editor purposes.
    [HideInInspector] public string editorNotes;

    // Deconstructor.
    public void Deconstruct(
        out MapGenerator mapGenerator,
        out MaterialGenerator materialGenerator,
        out MeshGenerator meshGenerator)
    {
        mapGenerator = this.mapGenerator;
        materialGenerator = this.materialGenerator;
        meshGenerator = this.meshGenerator;
    }
}
