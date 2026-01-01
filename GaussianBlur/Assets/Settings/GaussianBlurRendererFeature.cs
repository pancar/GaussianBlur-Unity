using UnityEngine;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.RenderGraphModule.Util;

public class GaussianBlurPass : ScriptableRenderPass
{
    const string m_PassName = "GaussianBlurPass";

    Material m_BlitMaterial;

    public void Setup(Material mat)
    {
        m_BlitMaterial = mat;
        requiresIntermediateTexture = true;
    }

    public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
    {
        var resourceData = frameData.Get<UniversalResourceData>();
        var source = resourceData.activeColorTexture;

        var destinationDesc = renderGraph.GetTextureDesc(source);
        destinationDesc.name = $"CameraColor-{m_PassName}";
        destinationDesc.clearBuffer = false;

        TextureHandle destination = renderGraph.CreateTexture(destinationDesc);

        RenderGraphUtils.BlitMaterialParameters verticalParams =
            new(source, destination, m_BlitMaterial, 0);
        renderGraph.AddBlitPass(verticalParams, passName: m_PassName + "Vertical");

        RenderGraphUtils.BlitMaterialParameters horizontalParams =
            new(destination, source, m_BlitMaterial, 1);
        renderGraph.AddBlitPass(horizontalParams, passName: m_PassName + "Horizontal");

    }
}

public class GaussianBlurRendererFeature : ScriptableRendererFeature
{
    public Material material;
    public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
    GaussianBlurPass m_Pass;

    public override void Create()
    {
        m_Pass = new GaussianBlurPass();
        m_Pass.renderPassEvent = renderPassEvent;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (material == null)
        {
            Debug.LogWarning(this.name + " material is null and will be skipped.");
            return;
        }

        m_Pass.Setup(material);
        renderer.EnqueuePass(m_Pass);
    }
}