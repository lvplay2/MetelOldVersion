Shader "Mobile/VegetationFade"
{
    Properties
    {
        _Albedo("Albedo", 2D) = "white" {}
        _LightMapUV1("LightMap (UV1)", 2D) = "white" {}
        _Turbulence("Turbulence", Float) = 1
        _SpeedTurbulence("Speed Turbulence", Float) = 1
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" }
        LOD 200

        Pass
        {
            Tags { "LightMode"="ForwardBase" }
            Cull Off
            Blend SrcAlpha OneMinusSrcAlpha

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
                float2 uv     : TEXCOORD0;
                float2 uv2    : TEXCOORD1;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv  : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
                float4 color : COLOR;
            };

            v2f vert(appdata v)
            {
                v2f o;

                float wave = sin(_Time.y * _SpeedTurbulence + v.vertex.x * 0.1) 
                             * v.color.x * _Turbulence;
                
                float3 displaced = v.vertex.xyz + v.normal * wave;

                o.pos = UnityObjectToClipPos(float4(displaced, 1.0));
                o.uv  = TRANSFORM_TEX(v.uv, _Albedo);
                o.uv2 = TRANSFORM_TEX(v.uv2, _LightMapUV1);
                o.color = v.color;

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 albedo = tex2D(_Albedo, i.uv);
                fixed4 lightmap = tex2D(_LightMapUV1, i.uv2);

                fixed3 finalColor = albedo.rgb * lightmap.r;
                return fixed4(finalColor, albedo.a);
            }
            ENDCG
        }
    }
}
