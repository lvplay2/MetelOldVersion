Shader "Mobile/Fresnel" {
    Properties {
        _FlakesColor2 ("Flakes Color 2", Color) = (1,0,0,1)
        _Diffuse ("Diffuse", 2D) = "white" {}
        _FresnelPower ("Fresnel Power", Range(0.1,5)) = 1
        _Brightness ("Brightness", Range(0, 3)) = 1
        _Contrast ("Contrast", Range(0, 3)) = 1.5
    }

    SubShader {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }
        LOD 100

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _Diffuse;
            float4 _FlakesColor2;
            float _FresnelPower;
            float _Brightness;
            float _Contrast;

            struct appdata {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
                float3 viewDir : TEXCOORD2;
                float4 pos : SV_POSITION;
            };

            v2f vert (appdata v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.viewDir = normalize(_WorldSpaceCameraPos - mul(unity_ObjectToWorld, v.vertex).xyz);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                float fresnel = pow(1.0 - saturate(dot(i.viewDir, normalize(i.worldNormal))), _FresnelPower);
                fixed4 tex = tex2D(_Diffuse, i.uv);
                fixed4 color = tex + _FlakesColor2 * fresnel;

                color.rgb += (_Brightness - 1.0);

                color.rgb = (color.rgb - 0.5) * _Contrast + 0.5;

                color.rgb = saturate(color.rgb);
                
                return color;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
