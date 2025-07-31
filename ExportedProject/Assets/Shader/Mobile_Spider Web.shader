Shader "Mobile/Spider Web"
{
    Properties
    {
        _Alpha ("Alpha", 2D) = "white" {}
        _Turbulence ("Turbulence", Range(0,5)) = 1
        _SpeedTurbulence ("Speed Turbulence", Range(0,5)) = 1
        _Light ("Light", Range(0,2)) = 1
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _Alpha;
            float4 _Alpha_ST;

            float _Turbulence;
            float _SpeedTurbulence;
            float _Light;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                float turbulence = sin((_Time.y * _SpeedTurbulence + dot(v.normal, float3(1,1,1))) * 10.0) 
                                   * _Turbulence * v.color.r * 0.01;
                float3 displaced = v.vertex.xyz + v.normal * turbulence;

                o.vertex = UnityObjectToClipPos(float4(displaced, 1.0));
                o.uv = TRANSFORM_TEX(v.uv, _Alpha);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed alpha = tex2D(_Alpha, i.uv).r;
                return fixed4(_Light*0.1, _Light*0.1, _Light*0.1, alpha);
            }
            ENDCG
        }
    }
}
