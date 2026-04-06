Shader "Custom/FresnelWall"
{
    Properties
    {
        _BaseColor("Base Color", Color) = (0, 0.1, 0.3, 1)
        _GlowColor("Glow Color", Color) = (0, 0.8, 1, 1)
        _FresnelPower("FresnelPower", Range(1, 8)) = 5
        _GlowIntensity("Glow Intensity", Range(0, 5)) = 1.5
        _PulseSpeed("Pulse Speed", Range(0, 5)) = 2
        [MainTexture] _BaseMap ("Sprite Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline" "Queue" = "Transparent"}
        LOD 100

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
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION; 
                float2 uv : TEXCOORD0;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            CBUFFER_START(UnityPerMaterial)
                half4 _BaseColor;
                half4 _GlowColor;
                float _FresnelPower;
                float _GlowIntensity;
                float _PulseSpeed;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;

                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float4 tex = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv);

                float2 d = abs(IN.uv * 2.0 - 1.0);
                float edge = max(d.x, d.y);
                float fresnel = pow(edge, _FresnelPower);
                float pulse = sin(_Time.y * _PulseSpeed) * 0.5 + 0.5;
                float3 glow = _GlowColor.rgb * fresnel * _GlowIntensity * pulse;
                float3 baseColor = tex.rgb * _BaseColor.rgb;
                float3 finalColor = baseColor + glow;

                return half4(finalColor, 1.0);
            }
            ENDHLSL
        }
    }
}
