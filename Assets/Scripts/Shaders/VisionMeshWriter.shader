Shader "Custom/VisionMeshWriter"
{
    Properties { }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "Queue" = "Geometry-10"}

        Pass
        {
            ColorMask 0
            ZWrite Off
            Cull Off

            Stencil
            {
                Ref 1
                Comp Always
                Pass Replace
            }           
        }        
    }
}
