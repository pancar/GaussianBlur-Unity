using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;

public class GaussianBlurWindow : EditorWindow
{
    float sigma = 2.0f;
    string fileName = "GaussianBlur.hlsl";

    [MenuItem("Tools/Gaussian Blur")]
    static void Open()
    {
        GetWindow<GaussianBlurWindow>("Gaussian Blur");
    }

    void OnGUI()
    {
        sigma = EditorGUILayout.FloatField("Sigma", sigma);
        fileName = EditorGUILayout.TextField("File Name", fileName);

        GUILayout.Space(10);

        if (GUILayout.Button("Generate HLSL Include"))
            Generate();
    }

    void Generate()
    {
        // --- Generate kernel ---
        GaussianBlurKernel.Generate(
            sigma,
            out float[] weights,
            out float[] offsets
        );

        // BinomialKernel.Generate(
        //     (int)sigma,
        //     out float[] weights,
        //     out float[] offsets
        // );
        
        // --- Build HLSL ---
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("// Auto-generated Gaussian Blur (offset-packed)");
        sb.AppendLine("// sigma = " + sigma);
        sb.AppendLine();
        sb.AppendLine("#ifndef GAUSSIAN_BLUR_INCLUDED");
        sb.AppendLine("#define GAUSSIAN_BLUR_INCLUDED");
        sb.AppendLine();

        sb.AppendLine($"static const int GAUSSIAN_TAP_COUNT = {weights.Length};");
        sb.AppendLine($"static const float GAUSSIAN_SIGMA = {sigma:0.000000f};");
        sb.AppendLine();

        sb.AppendLine("static const float GaussianWeights[GAUSSIAN_TAP_COUNT] = {");
        for (int i = 0; i < weights.Length; i++)
            sb.AppendLine($"    {weights[i]:0.000000f},");
        sb.AppendLine("};");
        sb.AppendLine();

        sb.AppendLine("static const float GaussianOffsets[GAUSSIAN_TAP_COUNT] = {");
        for (int i = 0; i < offsets.Length; i++)
            sb.AppendLine($"    {offsets[i]:0.000000f},");
        sb.AppendLine("};");
        sb.AppendLine();

        sb.AppendLine("#endif");

        // --- FOLDER CREATION ---
        const string shadersPath = "Assets/Shaders";
        const string includesPath = "Assets/Shaders/Includes";

        if (!AssetDatabase.IsValidFolder(shadersPath))
            AssetDatabase.CreateFolder("Assets", "Shaders");

        if (!AssetDatabase.IsValidFolder(includesPath))
            AssetDatabase.CreateFolder(shadersPath, "Includes");

        string assetPath = $"{includesPath}/{fileName}";
        File.WriteAllText(assetPath, sb.ToString());

        AssetDatabase.Refresh();
    }
}
