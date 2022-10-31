using UnityEngine;

// Written by Nicholas Sebastian Hendrata on 12/08/2022.

// NOTE: The max noise height value might not be correct
//       because the fallout map is applied after the noise map has been generated,
//       which means that what might have been the highest point in the map
//       might have already been eliminated by the fallout map.

[System.Serializable]
public class MapGenerator
{
    [Range(1, 255)] public int width = 100;
    [Range(1, 255)] public int height = 100;

    [Header("Noise Settings")]
    [Min(0.1f)]     public float noiseScale = 30;
    [Min(1)]        public int octaves = 4;
    [Range(0, 1)]   public float persistance = 0.4f;
    [Min(1)]        public float lacunarity = 2;

    [Header("Falloff Settings")]
    public bool enableFalloff = true;
    public AnimationCurve falloffCurve;

    private int seed;
    private Vector2 offset;
    private readonly Vector2[] octaveOffsets;
    private float maxNoiseHeight = float.MinValue;
    private float minNoiseHeight = float.MaxValue;

    public static MapGenerator Main { get; private set; }

    private MapGenerator()
    {
        octaveOffsets = new Vector2[octaves];
        Main = this;
    }

    public float[,] CreateMap(int seed, Vector2 offset)
    {
        this.seed = seed;
        this.offset = offset;

        var noiseMap = new float[width, height];

        GenerateOctaveOffsets(-100000, 100000);
        GenerateNoiseMap(ref noiseMap);
        ProcessMap(ref noiseMap);

        return noiseMap;
    }

    private void GenerateNoiseMap(ref float[,] noiseMap)
    {
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
            {
                var noiseHeight = GenerateNoiseHeight(x, y);
                noiseMap[x, y] = noiseHeight;

                SetMinMaxNoiseHeights(noiseHeight);
            }
    }

    private float[,] GenerateFalloffMap()
    {
        var falloffMap = new float[width, height];

        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
            {
                var edgeX = Mathf.Abs(x / (float)width * 2 - 1);
                var edgeY = Mathf.Abs(y / (float)height * 2 - 1);
                var value = Mathf.Max(edgeX, edgeY);

                falloffMap[x, y] = falloffCurve.Evaluate(value);
            }

        return falloffMap;
    }

    private float FlattenCenter(float value, int x, int y)
    {
        var centerX = width / 2;
        var centerY = height / 2;
        var distanceX = centerX - x;
        var distanceY = centerY - y;

        var distanceSquared = (distanceX * distanceX) + (distanceY * distanceY);
        var radiusSquared = GameManager.Instance.WorldGen.GetComponent<WorldGenerator>().radius * GameManager.Instance.WorldGen.GetComponent<WorldGenerator>().radius;
        var outerRadius = GameManager.Instance.WorldGen.GetComponent<WorldGenerator>().radius + GameManager.Instance.WorldGen.GetComponent<WorldGenerator>().radialSmoothing;
        var oRadiusSquared = outerRadius * outerRadius;

        if (distanceSquared >= oRadiusSquared) return value;
        if (distanceSquared <= radiusSquared) return GameManager.Instance.WorldGen.GetComponent<WorldGenerator>().groundLevel;

        var distance = Mathf.Sqrt(distanceSquared);
        var smoothing = Mathf.InverseLerp(GameManager.Instance.WorldGen.GetComponent<WorldGenerator>().radius, outerRadius, distance);

        return Mathf.Lerp(GameManager.Instance.WorldGen.GetComponent<WorldGenerator>().groundLevel, value, smoothing);
    }

    private void GenerateOctaveOffsets(int min, int max)
    {
        var rng = new System.Random(seed);

        for (int i = 0; i < octaves; i++)
        {
            float x = rng.Next(min, max);
            float y = rng.Next(min, max);

            octaveOffsets[i] = new Vector2(x, y);
        }
    }

    private float GenerateNoiseHeight(int x, int y)
    {
        float noiseHeight = 0;
        float amplitude = 1;
        float frequency = 1;

        for (int i = 0; i < octaves; i++)
        {
            // Center around the middle of the map rather than the top-right.
            var centerX = x - (width / 2f);
            var centerY = y - (height / 2f);

            // Add the random offsets generated earlier.
            var totalOffset = offset + octaveOffsets[i];

            // Calculate the sampling coordinate for the perlin noise.
            var sampleX = centerX / noiseScale * frequency + totalOffset.x;
            var sampleY = centerY / noiseScale * frequency + totalOffset.y;

            // Calculate the perlin noise and denormalize it to (-1 to 1).
            var perlinValueAbs = Mathf.PerlinNoise(sampleX, sampleY);
            var perlinValue = (perlinValueAbs * 2) - 1;

            noiseHeight += perlinValue * amplitude;
            amplitude *= persistance;
            frequency *= lacunarity;
        }

        return noiseHeight;
    }

    private void ProcessMap(ref float[,] noiseMap)
    {
        var falloffMap = enableFalloff ? GenerateFalloffMap() : null;

        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
            {
                var height = NormalizeNoiseHeight(noiseMap[x, y]);

                if (GameManager.Instance.WorldGen.GetComponent<WorldGenerator>().flattenCenter)
                    height = FlattenCenter(height, x, y);

                if (enableFalloff)
                    height = Mathf.Clamp01(height - falloffMap[x, y]);

                noiseMap[x, y] = height;
            }
    }

    private float NormalizeNoiseHeight(float noiseHeight)
    {
        return Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseHeight);
    }

    private void SetMinMaxNoiseHeights(float noiseHeight)
    {
        if (noiseHeight > maxNoiseHeight)
            maxNoiseHeight = noiseHeight;

        if (noiseHeight < minNoiseHeight)
            minNoiseHeight = noiseHeight;
    }
}
