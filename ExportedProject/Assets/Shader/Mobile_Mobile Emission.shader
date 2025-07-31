Shader "Mobile/Mobile Emission"
{
    Properties
    {
        _Diffuse("Diffuse", 2D) = "white" {}
        _Emission("Emission", 2D) = "black" {}
        [Toggle] _On("On", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" "Queue" = "Geometry" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _Diffuse;
            sampler2D _Emission;
            float _On;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 diff = tex2D(_Diffuse, i.uv);
                fixed4 emis = tex2D(_Emission, i.uv);

                if (_On < 0.5) 
                    return diff;

                return diff + emis;
            }
            ENDCG
        }
    }
}
