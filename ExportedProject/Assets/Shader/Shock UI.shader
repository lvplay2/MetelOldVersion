Shader "Custom/Shock UI"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint Color", Color) = (1,1,1,1)
        _AnimationTexture ("Animation Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" "CanUseSpriteAtlas"="True" }

        Cull Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _AnimationTexture;
            float4 _Color;

            struct appdata
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.color = v.color * _Color;
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = frac(i.uv);
                
                float animSpeed = 10.0;
                float totalFrames = 8.0;

                float frameIndex = floor(frac((_Time.y * animSpeed + 1.0) / totalFrames) * totalFrames + 0.5);

                frameIndex = clamp(frameIndex, 0.0, totalFrames - 1.0);

                float col = frameIndex % 4.0;
                float row = floor(frameIndex / 4.0);

                float2 frameUV = uv * float2(1.0 / 4.0, 1.0 / 2.0) + float2(col / 4.0, 1.0 - (row + 1.0) / 2.0);

                fixed4 texColor = tex2D(_AnimationTexture, frameUV);

                return texColor * i.color;
            }
            ENDCG
        }
    }
    FallBack "UI/Default"
}
