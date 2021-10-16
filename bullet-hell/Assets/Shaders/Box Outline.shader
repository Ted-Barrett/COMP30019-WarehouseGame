Shader "Custom/Box Outline"
{
    Properties
    {
        _OutlineThickness1 ("Outline Thickness 1", float) = 40
        _OutlineThickness2 ("Outline Thickness 2", float) = 50
        _InnerOutlineColour ("Inner Outline Colour", color) = (0, 0, 0, 1)
        _OuterOutlineColour1 ("Outer Outline Colour 1", color) = (0, 0, 0, 1)
        _OuterOutlineColour2 ("Outer Outline Colour 2", color) = (0, 0, 0, 1)
        _OuterColourFreq ("Outer Colour Frequency", float) = 1
        _CornerRadius ("Corner Radius", range(0, 1000)) = 1
        [IntRange] _CornerResolution ("Corner Resolution", range(0, 5)) = 1

        [HideInInspector] _DistMult ("Distance Multiplier", float) = 0.001
        [HideInInspector] _Tau ("Tau", float) = 6.28319

    }
    SubShader
    {
        Tags 
        { 
            "RenderType" = "Transparent"
            "IgnoreProjector" = "True"
            "Queue" = "Transparent" 
        }

        Stencil
        {
            Ref 2
            Comp NotEqual
        }

        Pass
        {
            Cull Off
            ZTest Always
            Zwrite Off       
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #include "UnityCG.cginc"

            #pragma vertex vert
            #pragma geometry geo
            #pragma fragment frag

            struct appdata
            {
                float4 vertex : POSITION;
                float3 smoothNormal : TEXCOORD3;
                float2 cull : TEXCOORD1;
            };

            struct v2g
            {
                float3 position : TEXCOORD0;
                float2 smoothNormal : TEXCOORD3;
                float2 cull : TEXCOORD1;
            };

            struct g2f
            {
                float4 position : SV_POSITION;
                float4 colour : COLOR;
            };

            float _OutlineThickness1;
            float _OutlineThickness2;
            float4 _InnerOutlineColour;
            float4 _OuterOutlineColour1;
            float4 _OuterOutlineColour2;
            float _OuterColourFreq;
            float _CornerRadius;
            uint _CornerResolution;
            float _DistMult;
            float _Tau;

            v2g vert(appdata i)
            {
                v2g o;
                o.position = UnityObjectToViewPos(i.vertex);
                o.smoothNormal = normalize(UnityObjectToViewPos(float4(i.smoothNormal, 0.0)).xy);
                o.cull = i.cull;
                return o;
            }

            [maxvertexcount(36)]
            void geo(line v2g i[2], inout TriangleStream<g2f> stream)
            {
                
                g2f o0, o1, o2, o3, o4, o5;

                float sinVal = (sin(_Tau * _OuterColourFreq * _Time.y) + 1) / 2.0;   
                float4 outerColourVector = _OuterOutlineColour2 - _OuterOutlineColour1;
                float4 outerColour = _OuterOutlineColour1 + outerColourVector
                                        * sinVal;

                
                float outlineThickness = _OutlineThickness1 
                                        + (_OutlineThickness2 - _OutlineThickness1) * sinVal;
                float cornerRad = clamp(_CornerRadius, 0.0001, outlineThickness);
                    
                float2 edge = normalize(i[1].position.xy - i[0].position.xy);
                float z0 = i[0].position.z;
                float z1 = i[1].position.z;

                float2 innerCorner0 = i[0].position.xy 
                                        + (outlineThickness - cornerRad) * _DistMult * i[0].smoothNormal;
                float2 innerCorner1 = i[1].position.xy 
                                        + (outlineThickness - cornerRad) * _DistMult * i[1].smoothNormal;

                float2 outerCorner0 = i[0].position.xy + outlineThickness * _DistMult * i[0].smoothNormal;
                float2 outerCorner1 = i[1].position.xy + outlineThickness * _DistMult * i[1].smoothNormal;

                float2 cornerDiff0 = outerCorner0 - innerCorner0;
                float2 cornerDiff1 = outerCorner1 - innerCorner1;

                float2 roundingDir0 = dot(cornerDiff0, edge) * -edge;
                float2 roundingDir1 = dot(cornerDiff1, edge) * -edge;
                    
                float2 roundingPos0 = outerCorner0;
                float2 roundingPos1 = outerCorner1;

                o0.position = mul(UNITY_MATRIX_P, float4(i[0].position, 1));
                o2.position = mul(UNITY_MATRIX_P, float4(roundingPos0, z0, 1));
                o0.colour = _InnerOutlineColour;
                o1.colour = outerColour;
                o2.colour = outerColour;

                o3.position = mul(UNITY_MATRIX_P, float4(i[1].position, 1));
                o5.position = mul(UNITY_MATRIX_P, float4(roundingPos1, z1, 1));
                o3.colour = _InnerOutlineColour;
                o4.colour = outerColour;
                o5.colour = outerColour;

                for(uint i = 1; i < _CornerResolution + 1; i++)
                {
                    outerCorner0 = roundingPos0;
                    outerCorner1 = roundingPos1;

                    roundingPos0 = innerCorner0 
                                    + normalize(cornerDiff0 + roundingDir0 * i / _CornerResolution) 
                                        * cornerRad * _DistMult;
                    roundingPos1 = innerCorner1 
                                    + normalize(cornerDiff1 + roundingDir1 * i / _CornerResolution) 
                                        * cornerRad * _DistMult;
                        
                    o1.position = mul(UNITY_MATRIX_P, float4(outerCorner0, z0, 1));
                    o2.position = mul(UNITY_MATRIX_P, float4(roundingPos0, z0, 1));
                    o4.position = mul(UNITY_MATRIX_P, float4(outerCorner1, z1, 1));
                    o5.position = mul(UNITY_MATRIX_P, float4(roundingPos1, z1, 1));
                    
                    stream.Append(o0);
                    stream.Append(o1);
                    stream.Append(o2);
                    stream.RestartStrip();

                    stream.Append(o3);
                    stream.Append(o4);
                    stream.Append(o5);
                    stream.RestartStrip();
                }

                stream.Append(o0);
                stream.Append(o2);
                stream.Append(o5);
                stream.RestartStrip();

                stream.Append(o0);
                stream.Append(o5);
                stream.Append(o3);
            }

            float4 frag(g2f i) : SV_TARGET
            {
                return i.colour;
            }

            ENDCG
        }
    }
}
