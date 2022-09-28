using UnityEngine;

// Written by Nicholas Sebastian Hendrata on 13/08/2022.

[System.Serializable]
public class MeshGenerator
{
    public static MeshGenerator Main { get; private set; }

    [Range(1, 200)] public float heightMultiplier = 20f;
    public AnimationCurve multiplierCurve;

    private int width;
    private int height;
    private bool enableHeight;
    private float[,] heightMap;

    public MeshGenerator()
    {
        Main = this;
    }

    public MeshData CreateMeshData(float[,] heightMap)
    {
        this.heightMap = heightMap;

        width = heightMap.GetLength(0);
        height = heightMap.GetLength(1);

        enableHeight = GameManager.Instance.WorldGen.GetComponent<WorldGenerator>().displayMode != ShaderMode.NoiseMap;

        var meshData = new MeshData(width, height);
        int vertexIndex = 0;

        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
            {
                // Load vertices.
                var vertexPosition = HeightMapToWorld(x, y);
                meshData.vertices[vertexIndex] = vertexPosition;

                // Load triangles.
                if (x < (width - 1) && y < (height - 1))
                    meshData.CreateSquare(vertexIndex);

                // Load UVs.
                var uvPosition = NormalizeCoordinate(x, y);
                meshData.uvs[vertexIndex] = uvPosition;

                vertexIndex++;
            }

        return meshData;
    }

    public float NormalToWorldHeight(float normalHeight)
    {
        return multiplierCurve.Evaluate(normalHeight) * heightMultiplier;
    }

    private Vector3 HeightMapToWorld(int x, int y)
    {
        var centerX = ((width - 1) / -2f) + x;
        var centerZ = ((height - 1) / 2f) - y;

        if (enableHeight)
        {
            var currentHeight = heightMap[x, y];
            var finalHeight = NormalToWorldHeight(currentHeight);

            return new Vector3(centerX, finalHeight, centerZ);
        }
        else
            return new Vector3(centerX, 0, centerZ);
    }

    private Vector2 NormalizeCoordinate(int x, int y)
    {
        var uvX = x / (float)width;
        var uvY = y / (float)height;

        return new Vector2(uvX, uvY);
    }
}

public class MeshData
{
    public readonly int width;
    public readonly int height;

    public Vector3[] vertices;
    public Vector2[] uvs;
    public int[] triangles;

    private const int verticesInQuad = 6;
    private int triangleIndex = 0;

    public MeshData(int width, int height)
    {
        this.width = width;
        this.height = height;

        var vertexCount = width * height;
        var edgeCount = (width - 1) * (height - 1) * verticesInQuad;

        vertices = new Vector3[vertexCount];
        uvs = new Vector2[vertexCount];
        triangles = new int[edgeCount];
    }

    public Mesh CreateMesh()
    {
        var mesh = new Mesh
        {
            vertices = vertices,
            triangles = triangles,
            uv = uvs
        };

        mesh.RecalculateNormals();
        mesh.RecalculateTangents();

        return mesh;
    }

    public void CreateTriangle(int a, int b, int c)
    {
        triangles[triangleIndex] = a;
        triangles[triangleIndex + 1] = b;
        triangles[triangleIndex + 2] = c;

        triangleIndex += 3;
    }

    public void CreateSquare(int topLeftIndex)
    {
        var topRightIndex = topLeftIndex + 1;
        var bottomLeftIndex = topLeftIndex + width;
        var bottomRightIndex = bottomLeftIndex + 1;

        CreateTriangle(topLeftIndex, bottomRightIndex, bottomLeftIndex);
        CreateTriangle(bottomRightIndex, topLeftIndex, topRightIndex);
    }
}
