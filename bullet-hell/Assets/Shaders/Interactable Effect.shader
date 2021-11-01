// Creates a moving/flashing highlight effect using a moving texture

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
        // Setting up for transparency
        Tags { "RenderType"="Transparent" }

        Pass
        {
            ZWrite Off
            // Use additive blending for a strong effect
            Blend One One

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
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

            // Convert to clip space and to view space
            v2f vert (appdata v)
            {
                v2f o;
                o.position = UnityObjectToClipPos(v.vertex);
                o.viewPos = UnityObjectToViewPos(v.vertex).xy;
                return o;
            }

            // Work out the time varying colour for each pixel
            float4 frag (v2f i) : SV_Target
            {
                // Sample the texture using _MainTex_ST to account for tiling and offset
                // Sample using the view pos and pan the texure with time
                float4 col = tex2D(_MainTex, i.viewPos * _MainTex_ST + _Speed * _Time.y);

                // Use the red value from the texture to interpolate between the base colour
                // and shimmer colour
                col = col.r * (_ShimmerColour - _BaseColour) + _BaseColour;

                // Vary the colour intensity with time using sin
                col = col * (sin(_Time.y * _Tau * _FlashFreq) + 1) / 2.0;
                return col;
            }
            ENDCG
        }
    }
}
