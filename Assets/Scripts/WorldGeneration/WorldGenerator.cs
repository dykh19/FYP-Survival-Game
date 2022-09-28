using Unity.AI.Navigation;
using UnityEngine;
using System.Collections.Generic;

// Written by Nicholas Sebastian Hendrata on 12/08/2022.

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class WorldGenerator : MonoBehaviour
{
    //public static WorldGenerator Main { get; private set; }

    public TerrainPreset terrainPreset;
    public ShaderMode displayMode = ShaderMode.CustomTerrainShader;

    [Header("World Generator Settings")]
    public bool randomSeed = true;
    public int seed;
    public Vector2 offset;

    [Header("Center Area Settings")]
    public bool flattenCenter = true;
    [Range(0, 1)] public float groundLevel;
    [Range(1, 50)] public float radius = 10;
    [Range(0, 10)] public float radialSmoothing = 5;

    [Header("Editor Controls")]
    public bool autoUpdate = false;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;

    public NavMeshSurface NavMesh;

    public GameObject BasePrefab;

    public GameObject[] Spawnables;

    void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();
        NavMesh = GetComponent<NavMeshSurface>();
        //SetInstance();
    }

    /*public void SetInstance()
    {
        Main = this;
    }*/

    public void CreateWorld(bool LoadSavedData)
    {
        var meshData = CreateTerrain();

        if(!LoadSavedData)
        {
            if (displayMode == ShaderMode.CustomTerrainShader)
                terrainPreset.prefabSpawner.SpawnStuff(meshData, transform);
        }
        

        SpawnBase();
        NavMesh.BuildNavMesh();
    }

    public MeshData CreateTerrain()
    {
        var (mapGenerator, materialGenerator, meshGenerator) = terrainPreset;

        if (randomSeed)
            seed = Random.Range(int.MinValue, int.MaxValue);
        

        //SetInstance();

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

    public void SpawnBase()
    {
        Instantiate(BasePrefab, new Vector3(0, 7.92f, 0), Quaternion.identity, this.transform);
    }

    public void SaveWorldData(WorldGenData data)
    {
        data.randomSeed = false;
        data.seed = seed;
        data.offset = offset;
        data.flattenCenter = flattenCenter;
        data.groundLevel = groundLevel;
        data.radius = radius;
        data.radialSmoothing = radialSmoothing;

        data.worldObjects = new WorldGenData.WorldObject[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            data.worldObjects[i].pos = transform.GetChild(i).transform.position;
            data.worldObjects[i].rotation = transform.GetChild(i).transform.rotation;
            data.worldObjects[i].scale = transform.GetChild(i).transform.localScale;
            data.worldObjects[i].prefabName = transform.GetChild(i).transform.name;
        }
    }

    public void LoadWorldData(WorldGenData data)
    {
        randomSeed = data.randomSeed;
        seed = data.seed;
        offset = data.offset;
        flattenCenter = data.flattenCenter;
        groundLevel = data.groundLevel;
        radius = data.radius;
        radialSmoothing = data.radialSmoothing;
    }

    public void LoadWorldObjects(WorldGenData data)
    {
        for (int i = 0; i < data.worldObjects.Length; i++)
        {
            foreach (GameObject obj in Spawnables)
            {
                if (data.worldObjects[i].prefabName == obj.name)
                {
                    GameObject SpawnedObject = Instantiate(obj, data.worldObjects[i].pos, data.worldObjects[i].rotation, transform);
                    SpawnedObject.transform.localScale = data.worldObjects[i].scale;
                    SpawnedObject.transform.parent = transform;
                }
            }
        }
    }
}

public enum ShaderMode
{
    CustomTerrainShader,
    HeightMap,
    NoiseMap
}

[System.Serializable]
public class WorldGenData
{
    public bool randomSeed = true;
    public int seed;
    public Vector2 offset;
    public bool flattenCenter = true;
    public float groundLevel;
    public float radius;
    public float radialSmoothing;

    [System.Serializable]
    public struct WorldObject
    {
        public Vector3 pos;
        public Quaternion rotation;
        public Vector3 scale;
        public string prefabName;
    }

    public WorldObject[] worldObjects;
    
}
