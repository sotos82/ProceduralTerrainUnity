using UnityEngine;
using System.Collections;

public class RidgedNoise : NoiseModule
{
    public override float FractalNoise2D(float x, float y, int octNum, float frq, float amp)
    {
        float gain = 1.0f;
        float signal = 0.0f;
        float value = 0.0f;
        float weight = 1.0f;
        float offset = 1.0f;
       
        for (int i = 0; i < octNum; i++)
        {
            signal = Noise2D(x * gain / frq, y * gain / frq) / gain;
            signal = Mathf.Abs(signal);
            signal = offset - signal;
            signal *= signal;
            signal *= weight;
            weight = signal * gain;
            if (weight > 1.0) { weight = 1.0f; }
            if (weight < 0.0) { weight = 0.0f; }
            
            value += (signal * m_weights[i]);
            x *= m_lacunarity;
            y *= m_lacunarity;
    }
        return ((value * 1.25f) - 1.0f) * amp;
}

    public RidgedNoise(int seed) : base(seed)
	{
        UpdateWeights();	
	}

    private float[] m_weights = new float[NoiseUtil.OctavesMaximum];
    private float m_lacunarity = 2.0f;

    private void UpdateWeights()
    {
        float f = 1.0f;
        for (int i = 0; i < NoiseUtil.OctavesMaximum; i++)
        {
            m_weights[i] = Mathf.Pow(f, -1.0f);
            f *= m_lacunarity;
        }
    }
}
