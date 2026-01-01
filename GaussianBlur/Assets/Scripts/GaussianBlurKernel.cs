using UnityEngine;

public static class GaussianBlurKernel
{
    public static void Generate(
        float sigma,
        out float[] weights,
        out float[] offsets)
    {
        int radius = Mathf.CeilToInt(3f * sigma);

        float[] w = new float[radius + 1];
        float sum = 0f;
        float twoSigmaSq = 2f * sigma * sigma;

        // Center
        w[0] = 1f;
        sum += w[0];

        // Positive side
        for (int i = 1; i <= radius; i++)
        {
            float v = Mathf.Exp(-(i * i) / twoSigmaSq);
            w[i] = v;
            sum += 2f * v;
        }

        // Normalize
        for (int i = 0; i <= radius; i++)
            w[i] /= sum;

        // ---- OFFSET PACKING ----
        // center + pairs (i, i+1)
        int packedCount = 1 + (radius + 1) / 2;
        weights = new float[packedCount];
        offsets = new float[packedCount];

        // Center tap
        weights[0] = w[0];
        offsets[0] = 0f;

        int index = 1;
        for (int i = 1; i <= radius; i += 2)
        {
            float w0 = w[i];
            float w1 = (i + 1 <= radius) ? w[i + 1] : 0f;

            float combined = w0 + w1;
            weights[index] = combined;

            offsets[index] = i + (w1 / combined);

            index++;
        }
    }
}