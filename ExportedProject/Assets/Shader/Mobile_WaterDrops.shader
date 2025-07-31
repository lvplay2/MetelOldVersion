Shader "Mobile/WaterDrops" {
    Properties {
        _Texture("Base Texture", 2D) = "black" {}
        _Reflection("Reflection Cube", Cube) = "white" {}
        _WaterNormal("Water Normal", 2D) = "bump" {}
        _Drops("Drops Normal", 2D) = "bump" {}
        _Columns("Columns", Float) = 4
        _Rows("Rows", Float) = 4
        _AnimationSpeed("Animation Speed", Range(0, 50)) = 10
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass {
            Tags { "LightMode"="ForwardBase" }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _Texture;
            sampler2D _WaterNormal;
            sampler2D _Drops;
            samplerCUBE _Reflection;

            float4 _WaterNormal_ST;
            float _Columns;
            float _Rows;
            float _AnimationSpeed;

            struct appdata {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float4 pos : SV_POSITION;
                float3 worldPos : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
                float3 worldTangent : TEXCOORD2;
                float3 worldBinormal : TEXCOORD3;
                float2 uv : TEXCOORD4;
            };

            v2f vert(appdata v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

                float3 worldNormal = UnityObjectToWorldNormal(v.normal);
                float3 worldTangent = UnityObjectToWorldDir(v.tangent.xyz);
                float3 worldBinormal = cross(worldNormal, worldTangent) * v.tangent.w;

                o.worldNormal = normalize(worldNormal);
                o.worldTangent = normalize(worldTangent);
                o.worldBinormal = normalize(worldBinormal);
                o.uv = v.uv;
                return o;
            }

            float2 GetDropsUV(float2 uv, float time) {
                float totalFrames = _Columns * _Rows;
                float frame = floor(fmod(time * _AnimationSpeed, totalFrames));
                float2 cellSize = float2(1.0 / _Columns, 1.0 / _Rows);

                float col = fmod(frame, _Columns);
                float row = _Rows - 1 - floor(frame / _Columns);

                float2 uvTile = frac(uv) * cellSize + float2(col, row) * cellSize;
                return uvTile;
            }

            fixed4 frag(v2f i) : SV_Target {
                float time = _Time.y;
                float2 uv1 = i.uv * _WaterNormal_ST.xy + _WaterNormal_ST.zw + time * float2(-0.03, 0);
                float2 uv2 = i.uv * _WaterNormal_ST.xy + _WaterNormal_ST.zw + time * float2(0.04, 0.04);

                float3 n1 = UnpackNormal(tex2D(_WaterNormal, uv1));
                float3 n2 = UnpackNormal(tex2D(_WaterNormal, uv2));
                float3 waterNormal = normalize(float3(n1.xy + n2.xy, n1.z * n2.z));

                float2 dropUV = GetDropsUV(i.uv, time);
                float3 dropNormal = UnpackNormal(tex2D(_Drops, dropUV));

                float3 finalNormal = normalize(waterNormal + dropNormal);

                float3x3 tbn = float3x3(i.worldTangent, i.worldBinormal, i.worldNormal);
                float3 worldNormal = normalize(mul(finalNormal, tbn));

                float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);
                float3 reflDir = reflect(-viewDir, worldNormal);
                fixed4 refl = texCUBE(_Reflection, reflDir);

                fixed4 baseCol = tex2D(_Texture, i.uv);

                fixed4 finalColor = lerp(refl, baseCol, baseCol.a);
                return finalColor;
            }
            ENDCG
        }
    }
}
