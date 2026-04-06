Shader "Custom/FlashingVisionCone"
{
    Properties
    {
        [MainColor] _BaseColor("Base Color", Color) = (0, 0.53, 1, 0.5)
        _DetectionColor("Detection Color", Color) = (1, 0.3, 0, 0.5)
        _MinAlpha("Minimum Alpha", Range(0, 1)) = 0.25
        _MaxAlpha("Maximum Alpha", Range(0, 1)) = 0.5
        _Detection("_Detection", Range(0, 1)) = 0
        _PulseSpeed("Pulse Speed", Range(0, 8)) = 8
    }

    SubShader
    {
        Tags { "RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline" "Queue" = "Transparent" }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
            };


            CBUFFER_START(UnityPerMaterial)
                half4 _BaseColor;
                half4 _DetectionColor;
                float _MinAlpha;
                float _MaxAlpha;
                float _Detection;
                float _PulseSpeed;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                if (_Detection == 0)
                {
                    return _BaseColor;
                }

                float pulse = (sin(_Time.y * _PulseSpeed) + 1.0) * 0.5;
                float d = saturate(_Detection * 2.0);
                float alpha = lerp(_MinAlpha, _MaxAlpha, d) * pulse;
                
                return float4(_DetectionColor.rgb, alpha);
            }
            ENDHLSL
        }
    }
}
