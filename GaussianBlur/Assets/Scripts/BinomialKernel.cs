using UnityEngine;

public static class BinomialKernel
{
  
    public static void Generate(int n, out float[] weights, out float[] offsets)
    {
        int taps = n + 1;   
        int radius = n / 2;

        //1. Binomial coefficients
        float[] c = new float[taps];
        c[0] = 1f; 

        for (int k = 1; k <= n; k++)
        {
            // Multiplicative formula
            // C(n, k) = C(n, k-1) * (n-(k-1))/k
            //https://wikimedia.org/api/rest_v1/media/math/render/svg/6f812bcef621eaaf7a433f05e4bf4d945bc58cd5
            c[k] = c[k - 1] * (n - (k - 1)) / k;
        }

        //2. Normalize
        float sum = 0f;
        for (int k = 0; k <= n; k++)
            sum += c[k];

        for (int k = 0; k <= n; k++)
            c[k] /= sum;

        // --- 3. Offset-packing (linear sampling) ---
        int packedCount = 1 + (radius + 1) / 2;
        weights = new float[packedCount];
        offsets = new float[packedCount];

        // Center tap
        weights[0] = c[radius];
        offsets[0] = 0f;

        int idx = 1;
        for (int k = 1; k <= radius; k += 2)
        {
            float w0 = c[radius + k];
            float w1 = (radius + k + 1 < taps) ? c[radius + k + 1] : 0f;

            float combined = w0 + w1;
            weights[idx] = combined;
            offsets[idx] = k + (w1 / combined);
            idx++;
        }
    }
}