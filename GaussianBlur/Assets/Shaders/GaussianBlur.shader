Shader "Hidden/GaussianBlur"
{
    HLSLINCLUDE
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
    #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"
    #include "Assets/Shaders/Includes/GaussianBlur.hlsl"

    float _BlurScale;

    float4 VerticalBlurPass(Varyings input) : SV_Target0
    {
        UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

        float2 uv = input.texcoord;
        float2 dir = float2(0, 1);
        float4 color = 0;

        // Center sample
        color += SAMPLE_TEXTURE2D_X_LOD(_BlitTexture, sampler_LinearRepeat, uv, _BlitMipLevel) * GaussianWeights[0];
        // Packed samples
        [unroll]
        for (int k = 1; k < GAUSSIAN_TAP_COUNT; k++)
        {
            float2 offset = GaussianOffsets[k] * _BlitTexture_TexelSize * dir;

            float4 s1 = SAMPLE_TEXTURE2D_X_LOD(_BlitTexture, sampler_LinearRepeat, uv + offset, _BlitMipLevel);
            float4 s2 = SAMPLE_TEXTURE2D_X_LOD(_BlitTexture, sampler_LinearRepeat, uv - offset, _BlitMipLevel);
            color += (s1 + s2) * GaussianWeights[k];
        }
        return color;
    }

    float4 HorizontalBlurPass(Varyings input) : SV_Target0
    {
        UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

        float2 uv = input.texcoord;
        float2 dir = float2(1, 0);
        float4 color = 0;

        // Center sample
        color += SAMPLE_TEXTURE2D_X_LOD(_BlitTexture, sampler_LinearRepeat, uv, _BlitMipLevel) * GaussianWeights[0];
        // Packed samples
        [unroll]
        for (int k = 1; k < GAUSSIAN_TAP_COUNT; k++)
        {
            float2 offset = GaussianOffsets[k] * _BlitTexture_TexelSize * dir;

            float4 s1 = SAMPLE_TEXTURE2D_X_LOD(_BlitTexture, sampler_LinearRepeat, uv + offset, _BlitMipLevel);
            float4 s2 = SAMPLE_TEXTURE2D_X_LOD(_BlitTexture, sampler_LinearRepeat, uv - offset, _BlitMipLevel);
            color += (s1 + s2) * GaussianWeights[k];
        }
        return color;
    }
    ENDHLSL

    SubShader
    {
        Tags
        {
            "RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline"
        }
        LOD 100
        ZWrite Off Cull Off
        Pass
        {
            Name "BlurVerticalPass"

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment VerticalBlurPass
            ENDHLSL
        }

        Pass
        {
            Name "BlurHorizontalPass"

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment HorizontalBlurPass
            ENDHLSL
        }
    }
}