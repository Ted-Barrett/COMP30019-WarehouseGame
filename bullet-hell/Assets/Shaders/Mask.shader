Shader "Custom/Mask"
{
    SubShader
    {
        Tags 
        { 
            "RenderType" = "Transparent" 
            // Set the draw order to before the outline
            "Queue" = "Geometry-10"
        }


        Pass
        {
            Cull Off
            ZTest Always
            ZWrite Off
            ColorMask 0

            // Always write a value of 2 to the stencil buffer 
            // which is used as a mask for the box outline
            Stencil
            {
                Ref 2
                Pass Replace
            }

        }
    }
}
