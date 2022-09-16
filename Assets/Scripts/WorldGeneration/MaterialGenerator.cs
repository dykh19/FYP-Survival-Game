using System.Linq;
using UnityEngine;
using UnityEditor;

// Written by Nicholas Sebastian Hendrata on 12/08/2022.

[System.Serializable]
public class MaterialGenerator
{
    public static MaterialGenerator Main { get; private set; }

    public TerrainType[] terrainTypes;

    private int width;
    private int height;
    private float[,] heightMap;
    private bool enableCustomShader;

    public MaterialGenerator()
    {
        Main = this;
    }

    public Material CreateMaterial(float[,] heightMap)
    {
        this.heightMap = heightMap;

        width = heightMap.GetLength(0);
        height = heightMap.GetLength(1);

        enableCustomShader = WorldGenerator.Main.displayMode == ShaderMode.CustomTerrainShader;

        return CreateMaterial();
    }

    private Material CreateMaterial()
    {
        if (enableCustomShader)
        {
            var shader = Shader.Find("Custom/TerrainShader");
            var material = new Material(shader);

            var sortedTerrainTypes = terrainTypes.OrderBy(terrainType => terrainType.height);
            var terrainColours = sortedTerrainTypes.Select(terrainType => terrainType.colour).ToArray();
            var terrainHeights = sortedTerrainTypes.Select(terrainType => terrainType.height).ToArray();
            var terrainBlends = sortedTerrainTypes.Select(terrainType => terrainType.blend).ToArray();

            material.SetFloat("minHeight", MeshGenerator.Main.NormalToWorldHeight(0));
            material.SetFloat("maxHeight", MeshGenerator.Main.NormalToWorldHeight(1));

            material.SetInteger("colourCount", terrainTypes.Length);
            material.SetColorArray("colours", terrainColours);
            material.SetFloatArray("startHeights", terrainHeights);
            material.SetFloatArray("blends", terrainBlends);

            return material;
        }
        else
        {
            var colourMap = CreateColourMap();
            var texture = CreateTexture(colourMap);

            var shader = Shader.Find("Unlit/Texture");
            var material = new Material(shader)
            {
                mainTexture = texture
            };

            return material;
        }
    }

    private Color[] CreateColourMap()
    {
        var colourMap = new Color[width * height];

        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
            {
                var i = (y * width) + x;
                var currentHeight = heightMap[x, y];

                colourMap[i] = Color.Lerp(Color.black, Color.white, currentHeight);
            }

        return colourMap;
    }

    private Texture2D CreateTexture(Color[] colourMap)
    {
        var texture = new Texture2D(width, height)
        {
            filterMode = FilterMode.Point,
            wrapMode = TextureWrapMode.Clamp
        };

        texture.SetPixels(colourMap);
        texture.Apply();

        return texture;
    }
}

[System.Serializable]
public struct TerrainType
{
    public string name;
    [Range(0, 1)]
    public float height;
    public Color colour;
    [Range(0.00001f, 0.04f)]
    public float blend;
}
