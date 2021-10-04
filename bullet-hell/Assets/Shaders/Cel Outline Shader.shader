
Shader "Custom/Cel Outline Shader"
{
    Properties
    {
        [MaterialToggle] _UseMatColour ("Use Material Colour", float) = 0
        _MatColour ("Material Colour", color) = (0,0,0,0)
        _OutlineThickness ("Outline Offset", float) = 10
        _MainTex ("Albedo", 2D) = "white" {}
        _NumColourLevels ("Colour levels", range(2, 50)) = 3
        _MinColourFactor ("Min colour factor", float) = 0.2
        _OutlineColour ("Outline Colour", color) = (0,0,0,1)
        [MaterialToggle] _ShadeFlat ("Shade flat", float) = 1
    }

    SubShader
    {
        Tags
        { 
            "RenderType" = "Opaque"
            "LightMode" = "ForwardBase" 
        }

        Pass
        {
            Cull Front
            Offset -1, -1

            CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			
            float _OutlineThickness;
            float4 _OutlineColour;

            struct appdata
            {
                float4 position : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 position : SV_POSITION;
            };

            v2f vert(appdata i)
            {
                v2f o;
                i.position.xyz += normalize(i.normal) * _OutlineThickness * 0.001;
                o.position = UnityObjectToClipPos(i.position);
                return o;
            }

            
            float4 frag(v2f i) : SV_Target
            {
                return _OutlineColour;
            }

			ENDCG
        }

        Pass
        {
            Cull Back

            CGPROGRAM
			#pragma vertex vert
            #pragma fragment frag
			#include "UnityStandardBRDF.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdbase

            sampler2D _MainTex;
            float _NumColourLevels;
            float _MinColourFactor;
            float _ShadeFlat;
            float _UseMatColour;
            float4 _MatColour;

            struct appdata 
            {
                float4 position : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                SHADOW_COORDS(2)
                float2 uv : TEXCOORD0;
                float4 worldPos : TEXCOORD1;
                float3 worldNormal : NORMAL;
            };

            v2f vert(appdata i)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(i.position);
                o.worldPos = mul(unity_ObjectToWorld, i.position);
                o.worldNormal = UnityObjectToWorldNormal(i.normal);
                o.uv = i.uv;
                TRANSFER_SHADOW(o)
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                float3 worldNormal;
                if(_ShadeFlat == 1)
                {
                    worldNormal = cross(ddy(i.worldPos), ddx(i.worldPos));
                }
                else
                {
                    worldNormal = i.worldNormal;
                }

                float shadow = SHADOW_ATTENUATION(i);
                float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
                float intensity = DotClamped(lightDir, normalize(worldNormal)) * shadow;
				
                float4 col;
                if(_UseMatColour) {
                    col = _MatColour;
                } 
                else {
                    col = tex2D(_MainTex, i.uv);
                }
                
				float colourLevel = round(intensity * _NumColourLevels);
                return col * (colourLevel/_NumColourLevels * (1 - _MinColourFactor) + _MinColourFactor);
            }

			ENDCG
        }

        UsePass "Standard/ShadowCaster"
    }
}
