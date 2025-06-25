Shader "CustomRenderTexture/FunctionShader"
{
    Properties
    {
        _Color1 ("Color 1", Color) = (1, 0, 0, 1)
        _Color2 ("Color 2", Color) = (0, 0, 1, 1)
        _MainTex("InputTex", 2D) = "white" {}
     }

     SubShader
     {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        sampler2D _MainTex;
        fixed4 _Color1;
        fixed4 _Color2;

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Calculate triangle color based on vertex position or other factors
            fixed3 barycentric = float3(1.0 - IN.uv_MainTex.x - IN.uv_MainTex.y, IN.uv_MainTex.x, IN.uv_MainTex.y);
            fixed4 color = lerp(_Color1, _Color2, barycentric.x);

            o.Albedo = color.rgb;
            // Set other shader properties as needed
        }
        ENDCG
    }
    Fallback "Diffuse"
}
