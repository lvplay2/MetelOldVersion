Shader "Mobile/VegetationFadeCutScene"
{
    Properties
    {
        _Albedo ("Albedo", 2D) = "white" {}
        _LightMapUV1 ("LightMap (UV1)", 2D) = "white" {}
        _Turbulence ("Turbulence", Float) = 1
        _SpeedTurbulence ("Speed Turbulence", Float) = 1
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _Albedo;
            float4 _Albedo_ST;
            sampler2D _LightMapUV1;
            float4 _LightMapUV1_ST;

            float _Turbulence;
            float _SpeedTurbulence;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 color  : COLOR;
                float2 uv0    : TEXCOORD0;
                float2 uv1    : TEXCOORD1;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float4 color : COLOR;
            };

            v2f vert(appdata v)
            {
                v2f o;
                float3 offset = sin(_Time.y * _SpeedTurbulence + dot(v.normal, float3(1,1,1)))
                                * _Turbulence * 0.01 * v.color.x;
                float3 pos = v.vertex.xyz + offset;
                o.pos = UnityObjectToClipPos(float4(pos,1));
                o.uv0 = TRANSFORM_TEX(v.uv0, _Albedo);
                o.uv1 = TRANSFORM_TEX(v.uv1, _LightMapUV1);
                o.color = v.color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 albedo = tex2D(_Albedo, i.uv0);
                fixed4 lightmap = tex2D(_LightMapUV1, i.uv1);
                fixed4 finalColor = albedo * lightmap.r * 2.0 * UNITY_LIGHTMODEL_AMBIENT;
                finalColor.a = albedo.a;
                return finalColor;
            }
            ENDCG
        }
    }
}
