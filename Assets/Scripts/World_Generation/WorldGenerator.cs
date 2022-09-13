using Unity.AI.Navigation;
using UnityEngine;

// Written by Nicholas Sebastian Hendrata on 12/08/2022.

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class WorldGenerator : MonoBehaviour
{
    public static WorldGenerator Main { get; private set; }

    public TerrainPreset terrainPreset;
    public ShaderMode displayMode = ShaderMode.CustomTerrainShader;

    [Header("World Generator Settings")]
    public bool randomSeed = true;
    public int seed;
    public Vector2 offset;

    [Header("Center Area Settings")]
    public bool flattenCenter = true;
    [Range(0, 1)] public float groundLevel;
    [Range(1, 10)] public float radius = 10;
    [Range(0, 10)] public float radialSmoothing = 5;

    [Header("Editor Controls")]
    public bool autoUpdate = false;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;

    public NavMeshSurface NavMesh;

    void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();
        NavMesh = GetComponent<NavMeshSurface>();
        SetInstance();
    }

    public void SetInstance()
    {
        Main = this;
    }

    public void CreateWorld()
    {
        var meshData = CreateTerrain();

        if (displayMode == ShaderMode.CustomTerrainShader)
            terrainPreset.prefabSpawner.SpawnStuff(meshData, transform);

        NavMesh.BuildNavMesh();
    }

    public MeshData CreateTerrain()
    {
        var (mapGenerator, materialGenerator, meshGenerator) = terrainPreset;

        if (randomSeed)
            seed = Random.Range(int.MinValue, int.MaxValue);

        SetInstance();

        // Create the Terrain.
        var noiseMap = mapGenerator.CreateMap(seed, offset);
        var material = materialGenerator.CreateMaterial(noiseMap);
        var meshData = meshGenerator.CreateMeshData(noiseMap);
        var mesh = meshData.CreateMesh();

        meshFilter.sharedMesh = mesh;
        meshCollider.sharedMesh = mesh;
        meshRenderer.sharedMaterial = material;

        ClearStuff();

        return meshData;
    }

    public void Reset()
    {
        ClearStuff();
        meshFilter.sharedMesh = null;
        meshCollider.sharedMesh = null;
        meshRenderer.sharedMaterial = null;
    }

    private void ClearStuff()
    {
        while (transform.childCount > 0)
            DestroyImmediate(transform.GetChild(0).gameObject);
    }
}

public enum ShaderMode
{
    CustomTerrainShader,
    HeightMap,
    NoiseMap
}
