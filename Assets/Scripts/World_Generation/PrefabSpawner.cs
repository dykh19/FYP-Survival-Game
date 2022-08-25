using UnityEngine;

// Written by Nicholas Sebastian Hendrata on 13/08/2022.

[System.Serializable]
public class PrefabSpawner
{
    public static PrefabSpawner Main { get; private set; }

    public bool enableWater = true;
    public bool expandOcean = false;

    [Range(0, 1)] public float waterLevel;
    public Material waterMaterial;
    public SpawnableResource[] spawnables;

    private int width;
    private int height;

    private MeshData mapMeshData;
    private Transform terrain;

    public PrefabSpawner()
    {
        Main = this;
    }

    public void SpawnStuff(MeshData mapMeshData, Transform terrain)
    {
        width = mapMeshData.width;
        height = mapMeshData.height;

        this.mapMeshData = mapMeshData;
        this.terrain = terrain;

        if (enableWater)
            SpawnWaterBodies();

        SpawnResources();
    }

    private void SpawnWaterBodies()
    {
        var waterHeight = MeshGenerator.Main.NormalToWorldHeight(waterLevel);
        var water = GameObject.CreatePrimitive(PrimitiveType.Plane);
        var waterScale = expandOcean ? 2 : 10;

        water.name = "Water";
        water.transform.position = new Vector3(0, waterHeight, 0);
        water.transform.localScale = new Vector3(width / waterScale, 1, height / waterScale);
        water.transform.SetParent(terrain);

        water.GetComponent<MeshRenderer>().material = waterMaterial;

        var waterCollider = water.GetComponent<MeshCollider>();
        GameObject.DestroyImmediate(waterCollider);
    }

    private void SpawnResources()
    {
        foreach (var point in mapMeshData.vertices)
        {
            if (WorldGenerator.Main.flattenCenter)
            {
                var distanceX = 0 - point.x;  // Assuming the center of the map is (0, y, 0).
                var distanceZ = 0 - point.z;
                var distanceSquared = (distanceX * distanceX) + (distanceZ * distanceZ);
                var radiusSquared = WorldGenerator.Main.radius * WorldGenerator.Main.radius;

                if (distanceSquared <= radiusSquared) continue;
            }

            foreach (var spawnable in spawnables)
            {
                var spawnHeight = MeshGenerator.Main.NormalToWorldHeight(spawnable.spawnHeight);
                var aboveLowest = point.y > spawnHeight - spawnable.spawnRange;
                var belowHighest = point.y < spawnHeight + spawnable.spawnRange;

                if (aboveLowest && belowHighest)
                {
                    var spawnChance = 1 - spawnable.rarity;
                    var random = Random.Range(0f, 100f);

                    if (spawnChance > random)
                    {
                        SpawnResource(spawnable, point);
                        break;
                    }
                }
            }
        }
    }

    private void SpawnResource(SpawnableResource spawnable, Vector3 position)
    {
        GameObject objectToSpawn;

        if (spawnable.model)
            objectToSpawn = Object.Instantiate(spawnable.model);
        else
            objectToSpawn = GameObject.CreatePrimitive(PrimitiveType.Cube);

        var surfaceDirection = GetSurfaceNormal(position);
        var scale = spawnable.modelScale + 1;

        var objectRenderer = objectToSpawn.GetComponentInChildren<Renderer>();
        var objectHeight = objectRenderer.localBounds.size.y * scale;
        var offsetAmount = (objectHeight / 2) + spawnable.verticalOffset;
        var positionOffset = surfaceDirection * offsetAmount;

        objectToSpawn.name = spawnable.name;
        objectToSpawn.transform.position = position + positionOffset;
        objectToSpawn.transform.up = surfaceDirection;
        objectToSpawn.transform.localScale = Vector3.one * scale;
        objectToSpawn.transform.SetParent(terrain);
    }

    private static Vector3 GetSurfaceNormal(Vector3 position)
    {
        var ray = new Ray(position + Vector3.up, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit))
            return hit.normal;

        return Vector3.up;
    }
}

[System.Serializable]
public struct SpawnableResource
{
    public string name;

    [Header("Object Settings")]
    public GameObject model;
    [Range(-0.9f, 1f)]  public float modelScale;
    [Range(-10, 10)]    public float verticalOffset;

    [Header("Spawn Settings")]
    [Range(0, 1)]   public float spawnHeight;
    [Range(1, 50)]  public int spawnRange;

    [Tooltip("0.0 rarity = 1% spawn chance per vertex.\n1.0 rarity = 0% spawn chance per vertex.")]
    [Range(0, 1)]   public float rarity;
}
