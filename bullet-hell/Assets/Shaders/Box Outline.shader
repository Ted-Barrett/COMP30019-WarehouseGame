Shader "Custom/Box Outline"
{
    Properties
    {
        _OutlineThickness ("Outline Thickness", float) = 10
        _InnerOutlineColour ("Inner Outline Colour", color) = (0, 0, 0, 1)
        _OuterOutlineColour1 ("Outer Outline Colour 1", color) = (0, 0, 0, 1)
        _OuterOutlineColour2 ("Outer Outline Colour 2", color) = (0, 0, 0, 1)
        _OuterColourFreq ("Outer Colour Frequency", float) = 1
    }
    SubShader
    {
        Tags 
        { 
            "RenderType" = "Transparent"
            "IgnoreProjector" = "True"
            "Queue" = "Overlay" 
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
            };

            struct v2g
            {
                float4 position : POSITION;
                float3 smoothNormal : TEXCOORD3;
            };

            struct g2f
            {
                float4 position : SV_POSITION;
                float4 colour : COLOR;
            };

            float _OutlineThickness;
            float4 _InnerOutlineColour;
            float4 _OuterOutlineColour1;
            float4 _OuterOutlineColour2;
            float _OuterColourFreq;

            v2g vert(appdata i)
            {
                v2g o;
                o.position = i.vertex;
                o.smoothNormal = i.smoothNormal;
                return o;
            }

            [maxvertexcount(6)]
            void geo(line v2g i[2], inout TriangleStream<g2f> stream)
            {
                g2f o0, o1, o2;
                    
                float4 outerColourVector = _OuterOutlineColour2 - _OuterOutlineColour1;
                float4 outerColour = _OuterOutlineColour1 + outerColourVector
                                     * ((sin(2 * 3.14159 * _OuterColourFreq * _Time.y) + 1) / 2.0); 

                o0.position = UnityObjectToClipPos(i[0].position);
                o1.position = UnityObjectToClipPos(i[0].position + i[0].smoothNormal * _OutlineThickness * 0.001);
                o2.position = UnityObjectToClipPos(i[1].position + i[1].smoothNormal * _OutlineThickness * 0.001);

                o0.colour = _InnerOutlineColour;
                o1.colour = outerColour;
                o2.colour = outerColour;

                stream.Append(o0);
                stream.Append(o1);
                stream.Append(o2);
                stream.RestartStrip();

                o1.position = UnityObjectToClipPos(i[1].position);
                o1.colour = _InnerOutlineColour;

                stream.Append(o0);
                stream.Append(o2);
                stream.Append(o1);
            }

            float4 frag(g2f i) : SV_TARGET
            {
                return i.colour;
            }

            ENDCG
        }
    }
}
