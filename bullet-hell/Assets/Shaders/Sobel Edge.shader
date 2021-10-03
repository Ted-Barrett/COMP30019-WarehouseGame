Shader "Custom/Sobel Edge"
{
    Properties
    {
        [HideInInspector] _MainTex ("Texture", 2D) = "white" {}

        _SobelDepthMultiplier ("Sobel Depth Multiplier", float) = 1
        _DepthOffsetMagnitude ("Depth Offset Magnitude", float) = 1
        _MaxSobelDepth ("Max Sobel Depth", float) = 1
        _SobelDepthExponent ("Sobel Depth Exponent", float) = 1
        _SobelDepthSnapThreshold ("Sobel Depth Snap Threshold", float) = 1
        _DepthEdgeCol ("Depth Edge Colour", color) = (0,0,0,1)

        _SobelColourMultiplier ("Sobel Colour Multiplier", float) = 1
        _ColourOffsetMagnitude ("Colour Offset Magnitude", float) = 1
        _MaxSobelColour ("Max Sobel Colour", float) = 1
        _SobelColourExponent ("Sobel Colour Exponent", float) = 1
        _SobelColourSnapThreshold ("Sobel Colour Snap Threshold", float) = 1
        _ColourEdgeCol ("Colour Edge Colour", color) = (0,0,0,1)

        _SobelNormalMultiplier ("Sobel Normal Multiplier", float) = 1
        _NormalOffsetMagnitude ("Normal Offset Magnitude", float) = 1
        _MaxSobelNormal ("Max Sobel Normal", float) = 1
        _SobelNormalExponent ("Sobel Normal Exponent", float) = 1
        _SobelNormalSnapThreshold ("Sobel Normal Snap Threshold", float) = 1
        _NormalEdgeCol ("Normal Edge Colour", color) = (0,0,0,1)
    }

    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 position : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 position : SV_POSITION;
            };

            // Relative offset to the point we are finding the sobel value of
            static const float2 pointOffsets[9] = 
            {
                float2(-1, 1), float2(0, 1), float2(1, 1),
                float2(-1, 0), float2(0, 0), float2(1, 0),
                float2(-1, -1), float2(0, -1), float2(1, -1)
            };

            // Convolution matrix in x direction (vertical edges)
            static const float xWeightMatrix[9] = 
            {
                1, 0, -1,
                2, 0, -2,
                1, 0, -1
            };

            // Convolution matrix in y direction (horizontal edges)
            static const float yWeightMatrix[9] =
            {
                1, 2, 1,
                0, 0, 0,
                -1, -2, -1
            };

            sampler2D _MainTex;
            sampler2D _CameraDepthNormalsTexture;

            float _DepthOffsetMagnitude;
            float _MaxSobelDepth;
            float _SobelDepthExponent;
            float _SobelDepthMultiplier;
            float _SobelDepthSnapThreshold;
            float4 _DepthEdgeCol;

            float _ColourOffsetMagnitude;
            float _MaxSobelColour;
            float _SobelColourExponent;
            float _SobelColourMultiplier;
            float _SobelColourSnapThreshold;
            float4 _ColourEdgeCol;

            float _NormalOffsetMagnitude;
            float _MaxSobelNormal;
            float _SobelNormalExponent;
            float _SobelNormalMultiplier;
            float _SobelNormalSnapThreshold;
            float4 _NormalEdgeCol;

            // Calculate the depth sobel value.
            // When this is high, essentially means that there is a large discrepancy
            // between the depths of this point on the screen and surrounding points.
            float depthSobel(float2 uv, float offsetMag)
            {
                float2 sobelVec = 0;

                [unroll] for(int i = 0; i < 9; i++)
                {
                    float depth = DecodeFloatRG(tex2D(_CameraDepthNormalsTexture,
                                                uv + pointOffsets[i] * offsetMag).zw);

                    sobelVec += depth * float2(xWeightMatrix[i], yWeightMatrix[i]);
                }

                return length(sobelVec);
            }

            // Calculate the colour sobel value.
            // When this is high, essentially means that there is a large discrepancy
            // between the colour of this point on the screen and surrounding points.
            float colourSobel(float2 uv, float offsetMag)
            {
                float2 rSobelVec = 0;
                float2 gSobelVec = 0;
                float2 bSobelVec = 0;

                [unroll] for(int i = 0; i < 9; i++)
                {
                    float3 rgb = tex2D(_MainTex, uv + pointOffsets[i] * offsetMag).rgb;

                    float2 xyWeight = float2(xWeightMatrix[i], yWeightMatrix[i]);

                    rSobelVec += rgb.r * xyWeight;
                    gSobelVec += rgb.g * xyWeight;
                    bSobelVec += rgb.b * xyWeight;
                }

                return max(length(rSobelVec), max(length(gSobelVec), length(bSobelVec)));
            }

            // Calculate the normal sobel value.
            // When this is high, essentially means that there is a large discrepancy
            // between the normal of this point on the screen and surrounding points.
            float normalSobel(float2 uv, float offsetMag)
            {
                float2 xSobelVec = 0;
                float2 ySobelVec = 0;
                float2 zSobelVec = 0;


                // float4 cameraDir = float4(uv * 2 - 1, UNITY_NEAR_CLIP_VALUE, 1);
                // cameraDir = mul(UNITY_MATRIX_I_VP, cameraDir);

                [unroll] for(int i = 0; i < 9; i++)
                {
                    float3 normal = DecodeViewNormalStereo(tex2D(_CameraDepthNormalsTexture,
                                                                 uv + pointOffsets[i] * offsetMag));
                    // normal *= DotClamped(normal, cameraDir.xyz);
                    float2 xyWeight = float2(xWeightMatrix[i], yWeightMatrix[i]);

                    xSobelVec += normal.x * xyWeight;
                    ySobelVec += normal.y * xyWeight;
                    zSobelVec += normal.z * xyWeight;
                }

                return max(length(xSobelVec), max(length(ySobelVec), length(zSobelVec)));
            }


            // Process a sobel value and relevant parameters to get the lerp factor.
            // Lerp factor is used to blend edges with rendered image
            float getSobelLerpFactor(float maxSobel, float sobelVal, float snapToMaxThreshold,
                                     float sobelExponent, float sobelMultiplier)
            {
                float lerpFactor = pow(sobelVal, sobelExponent) * sobelMultiplier;
                
                if(lerpFactor >= snapToMaxThreshold)
                {
                    lerpFactor = maxSobel;
                }

                return lerpFactor;
            }

            v2f vert (appdata i)
            {
                v2f o;
                o.position = UnityObjectToClipPos(i.position);
                o.uv = i.uv;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                // Getting lerp factor for depth edges
                float depthSobelVal = depthSobel(i.uv, 0.001 * _DepthOffsetMagnitude);
                float depthLerpFactor = getSobelLerpFactor(_MaxSobelDepth, depthSobelVal,
                                                           _SobelDepthSnapThreshold,
                                                           _SobelDepthExponent, _SobelDepthMultiplier);
                depthLerpFactor *= _DepthEdgeCol.a;

                // Getting lerp factor for colour edges
                float colourSobelVal = colourSobel(i.uv, 0.001 * _ColourOffsetMagnitude);
                float colourLerpFactor = getSobelLerpFactor(_MaxSobelColour, colourSobelVal,
                                                            _SobelColourSnapThreshold,
                                                            _SobelColourExponent, _SobelColourMultiplier);
                colourLerpFactor *= _ColourEdgeCol.a;

                // Getting lerp factor for normal edges
                float normalSobelVal = normalSobel(i.uv, 0.001 * _NormalOffsetMagnitude);
                float normalLerpFactor = getSobelLerpFactor(_MaxSobelNormal, normalSobelVal,
                                                            _SobelNormalSnapThreshold,
                                                            _SobelNormalExponent, _SobelNormalMultiplier);
                normalLerpFactor *= _NormalEdgeCol.a;

                float lerpFactor;
                float4 edgeCol;


                if((depthLerpFactor >= colourLerpFactor) && (depthLerpFactor >= normalLerpFactor))
                {
                    lerpFactor = depthLerpFactor;
                    edgeCol = _DepthEdgeCol;
                }
                else if(colourLerpFactor >= normalLerpFactor)
                {
                    lerpFactor = colourLerpFactor;
                    edgeCol = _ColourEdgeCol;
                }
                else
                {
                    lerpFactor = normalLerpFactor;
                    edgeCol = _NormalEdgeCol;
                }

                float4 col = tex2D(_MainTex, i.uv);
                col = lerp(col, edgeCol, lerpFactor);

                return col;
            }
            ENDCG
        }
    }
}
