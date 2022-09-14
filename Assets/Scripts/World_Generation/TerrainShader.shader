Shader "Custom/TerrainShader"
{
    // Written by Nicholas Sebastian Hendrata on 14/08/2022.

    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        // Define inputs per pixel.
        struct Input
        {
            float3 worldPos;
        };

        // Variables.
        const static int maxColourCount = 8;

        uniform int colourCount;
        uniform float3 colours[maxColourCount];
        uniform float startHeights[maxColourCount];
        uniform float blends[maxColourCount];

        uniform float minHeight;
        uniform float maxHeight;

        // Returns a value between 0 to 1 of which value is relative to a and b.
        float inverseLerp(float a, float b, float value)
        {
            float ratio = (value - a) / (b - a);
            return saturate(ratio);
        }

        // Executes for every pixel rendered by this shader.
        void surf (Input input, inout SurfaceOutputStandard output)
        {
            float height = inverseLerp(minHeight, maxHeight, input.worldPos.y);

            for (int i = 0; i < colourCount; i++)
            {
                float difference = height - startHeights[i];
                float drawStrength = inverseLerp(-blends[i], blends[i], difference);

                float3 prevColour = output.Albedo * (1 - drawStrength);
                float3 nextColour = colours[i] * drawStrength;

                output.Albedo = prevColour + nextColour;
            }
        }
        ENDCG
    }

    FallBack "Diffuse"
}
