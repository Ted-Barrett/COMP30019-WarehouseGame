Shader "Custom/Mask"
{
    SubShader
    {
        Tags 
        { 
            "RenderType" = "Transparent" 
            "Queue" = "Geometry-10"
        }


        Pass
        {
            Cull Off
            ZTest Always
            ZWrite Off
            ColorMask 0

            Stencil
            {
                Ref 2
                Pass Replace
            }

        }
    }
}
