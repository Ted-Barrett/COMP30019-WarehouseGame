Shader "Custom/grass"
{
    Properties
    {
        _GrassBaseColour ("Grass Base Colour", color) = (0, 0.8, 0, 1)
        _GrassTipColour ("Grass Tip Colour", color) = (0.2, 0.9, 0, 1)
        _GroundTexture ("Ground Texture", 2D) = "white" {}
        _CloseBladeDensity ("Close Grass Density", range(0, 18)) = 10
        _FarBladeDensity ("Far Grass Density", range(0, 18)) = 8
        _BladeCullingMult ("Blade Culling Multiplier", range(0.001, 0.5)) = 0.001
        _BladeCullingDist ("Blade Culling Dist", float) = 0.5
        _MinGrassHeight ("Min Grass Height", range(0.05, 2)) = 0.5
        _MaxGrassHeight ("Max Grass Height", range(0.05, 3)) = 1
        _MaxNormalVariation ("Normal Variation", range(0.0, 0.5)) = 0.1
        _ColourVariation ("Colour Variation", range(0.0, 0.5)) = 0.1
        _BladeWidth ("Blade Width", range(0.001, 0.1)) = 0.01
        _WindDir ("Wind Direction", vector) = (1, 0, 1, 0)
        _WindStrength ("Wind Strength", range(0.0, 0.1)) = 0.05
        _WindTexture ("Wind Texture", 2D) = "white" {}
        _WindSpeed ("Wind Speed", range(0.0, 1.0)) = 0.5
    }

    SubShader
    {
        Tags 
        { 
            "RenderType" = "Opaque" 
            "LightMode" = "ForwardBase"
            "Queue" = "Geometry"
        }

        Pass
        {
            Cull Off
            CGPROGRAM
            #pragma vertex vert
            #pragma geometry geo
            #pragma fragment frag
            #pragma multi_compile_fwdbase

            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "UnityStandardBRDF.cginc"

            float rand(float2 uv)
            {
                
                float3 vec  = frac(float3(uv, uv.x) * frac(sin(dot(uv, float2(12.9898,78.233)))*43758.5453123));
                vec += dot(vec, vec.yzx + 33.33);
                return frac((vec.x + vec.y) * vec.z);
            }

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2g
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
                float4 worldPos : TEXCOORD1;
            };

            struct g2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 colour : TEXCOORD1;
                float3 worldNormal : TEXCOORD2;
                SHADOW_COORDS(3)
            };

            float4 _GrassBaseColour;
            float4 _GrassTipColour;
            float4 _FloorColour;
            float _CloseBladeDensity;
            float _FarBladeDensity;
            float _MinGrassHeight;
            float _MaxGrassHeight;
            float _MaxNormalVariation;
            float _BladeWidth;
            float _ColourVariation;
            float4 _WindDir;
            float _WindStrength;
            sampler2D _WindTexture;
            float4 _WindTexture_ST;
            float _WindSpeed;
            sampler2D _GroundTexture;
            float4 _GroundTexture_ST;
            float _BladeCullingMult;
            float _BladeCullingDist;

            v2g vert (appdata i)
            {
                v2g o;
                o.pos = i.vertex;
                o.worldPos = mul(unity_ObjectToWorld, i.vertex);
                o.uv = TRANSFORM_TEX(i.uv, _GroundTexture);
                return o;
            }

            [maxvertexcount(57)]
            void geo(triangle v2g i[3], inout TriangleStream<g2f> stream)
            {
                g2f o0, o1, o2;
                float b1, b2, heightFactor, xVar, zVar, temp, rTipVar, gTipVar, bTipVar;
                float rBaseVar, gBaseVar, bBaseVar;
                float heightDiff = _MaxGrassHeight - _MinGrassHeight;

                float3 widthDir, bladeDir;
                float3 e1 = i[1].worldPos - i[0].worldPos;
                float3 e2 = i[2].worldPos - i[0].worldPos;
                float3 worldNormal = normalize(cross(e1, e2));

                uint bladeDensity = round(lerp(_CloseBladeDensity, 
                                               _FarBladeDensity, 
                                               saturate((length(_WorldSpaceCameraPos - i[0].worldPos)
                                                        - _BladeCullingDist) * _BladeCullingMult)));

                float3 centre;
                o0.uv = float2(0, 0);
                o1.uv = float2(0, 0);
                o2.uv = float2(0, 0);

                for(uint k = 0; k < bladeDensity; k++)
                {
                    b1 = rand(i[k % 3].pos.xz * 8734 + k * 0.237);
                    b2 = rand(i[(k + 1) % 3].pos.xz * 23691 + k * 0.345);
                    heightFactor = rand(i[1].pos.xz + k * 0.127);

                    xVar = (rand(i[2].pos.xz + k * 0.934) * 2 - 1) *  _MaxNormalVariation;
                    zVar = (rand(i[1].pos.xz + k * 0.654) * 2 - 1) * _MaxNormalVariation;

                    rTipVar = (rand(i[0].pos.xz * 38 + k * 0.943) * 2 - 1) * _ColourVariation;
                    gTipVar = (rand(i[1].pos.xz * 7324 + k * 0.624) * 2 - 1) * _ColourVariation;
                    bTipVar = (rand(i[k % 3].pos.xz * 236 + k * 0.4618) * 2 - 1) * _ColourVariation;
                    rBaseVar = (rand(i[2].pos.xz * 23 + k * 0.284) * 2 - 1) * _ColourVariation;
                    gBaseVar = (rand(i[1].pos.xz * 9345 + k * 0.278) * 2 - 1) * _ColourVariation;
                    bBaseVar = (rand(i[k % 3].pos.xz * 2731 + k * 0.574) * 2 - 1) * _ColourVariation;

                    temp = sqrt(b1);

                    centre = (1 - temp) * i[0].worldPos
                             + temp * (1 - b2) * i[1].worldPos
                             + temp * b2 * i[2].worldPos;

                    widthDir = normalize(cross(_WorldSpaceCameraPos.xyz - centre, worldNormal)) 
                                                + 10 * float3(bTipVar, b1, bBaseVar);

                    bladeDir = normalize(float3(worldNormal.x + xVar,
                                                worldNormal.y,
                                                worldNormal.z + zVar));

                    o0.pos = mul(UNITY_MATRIX_VP, 
                                     float4(centre - widthDir * _BladeWidth * 0.5, 1.0));
                    
                    o1.pos = float4(centre + bladeDir * (_MinGrassHeight + heightDiff * heightFactor), 1.0);

                    float2 wind = (tex2Dlod(_WindTexture, float4(o1.pos.xz * _WindTexture_ST.xy -
                                                                    normalize(_WindDir.xz) * _Time.y 
                                                                    * _WindSpeed, 0, 0)) * 2 - 1).rg;

                    
                    o1.pos.xyz += normalize(float3(wind.r, 0, wind.g)) * _WindStrength;

                    o1.pos = mul(UNITY_MATRIX_VP, o1.pos);
                    
                    o2.pos = mul(UNITY_MATRIX_VP, 
                                     float4(centre + widthDir * _BladeWidth * 0.5, 1.0));
                    
                    o0.worldNormal = normalize(cross(o2.pos.xyz - o0.pos.xyz,
                                                     o1.pos.xyz - o0.pos.xyz));
                    o1.worldNormal = o0.worldNormal;
                    o2.worldNormal = o0.worldNormal;
                    o0.colour = _GrassBaseColour + float4(rBaseVar, gBaseVar, bBaseVar, 0);
                    o1.colour = _GrassTipColour + float4(rTipVar, gTipVar, bTipVar, 0);
                    o2.colour = o0.colour;

                    TRANSFER_SHADOW(o0)
                    TRANSFER_SHADOW(o1)
                    TRANSFER_SHADOW(o2)
                    stream.Append(o0);
                    stream.Append(o1);
                    stream.Append(o2);
                    stream.RestartStrip();
                }

                o0.uv = i[0].uv;
                o0.colour = float4(1, 1, 1, 0);
                o0.pos = UnityObjectToClipPos(i[0].pos);
                TRANSFER_SHADOW(o0)
                o0.worldNormal = worldNormal;
                stream.Append(o0);
                o0.uv = i[1].uv;
                o0.pos = UnityObjectToClipPos(i[1].pos);
                TRANSFER_SHADOW(o0)
                stream.Append(o0);
                o0.uv = i[2].uv;
                o0.pos = UnityObjectToClipPos(i[2].pos);
                TRANSFER_SHADOW(o0)
                stream.Append(o0);
            }

            float4 frag (g2f i) : SV_Target
            {
                if(i.colour.a == 0) {
                    i.colour = tex2D(_GroundTexture, i.uv);
                }
                float intensity = abs(dot(_WorldSpaceLightPos0, i.worldNormal));
                float shadow = SHADOW_ATTENUATION(i);
                return 0.3 * i.colour * intensity * shadow * _LightColor0 + 0.5 * i.colour * shadow + 0.2 * i.colour; 
            }

            ENDCG
        }

        UsePass "Standard/ShadowCaster"
    }
}
