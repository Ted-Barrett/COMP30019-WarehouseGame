Shader "Custom/Interactable Effect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ShimmerColour ("Shimmer Colour", color) = (0.8, 0.8, 0, 0.5)
        _BaseColour ("Base Colour", color) = (0.8, 0.8, 0, 0.1)
        _Speed ("Speed" , float) = 0.1
        _FlashFreq ("Flash Frequency", float) = 2

        [HideInInspector] _Tau ("Tau", float) = 6.28319
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }

        Pass
        {
            ZWrite Off
            Blend One One

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                // float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                // float2 uv : TEXCOORD0;
                float2 viewPos : TEXCOORD1;
                float4 position : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _ShimmerColour;
            float4 _BaseColour;
            float _Speed;
            float _FlashFreq;
            float _Tau;

            v2f vert (appdata v)
            {
                v2f o;
                o.position = UnityObjectToClipPos(v.vertex);
                o.viewPos = UnityObjectToViewPos(v.vertex).xy;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float4 col = tex2D(_MainTex, i.viewPos * _MainTex_ST + _Speed * _Time.y);
                col = col.r * (_ShimmerColour - _BaseColour) + _BaseColour;
                col = col * (sin(_Time.y * _Tau * _FlashFreq) + 1) / 2.0;
                return col;
            }
            ENDCG
        }
    }
}
