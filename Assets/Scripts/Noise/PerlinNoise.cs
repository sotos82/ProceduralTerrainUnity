using UnityEngine;
using System.Collections;

public class PerlinNoise : NoiseModule
{
	public PerlinNoise(int seed) : base(seed)
	{
	}

	
	public float FractalNoise1D(float x, int octNum, float frq, float amp)
	{
		float gain = 1.0f;
		float sum = 0.0f;
	
		for(int i = 0; i < octNum; i++)
		{
			sum +=  Noise1D(x*gain/frq) * amp/gain;
			gain *= 2.0f;
		}
		return sum;
	}
	
	public override float FractalNoise2D(float x, float y, int octNum, float frq, float amp)
	{
		float gain = 1.0f;
		float sum = 0.0f;
		
		for(int i = 0; i < octNum; i++)
		{
			sum += Noise2D(x*gain/frq, y*gain/frq) * amp/gain;
			gain *= 2.0f;
		}
		return sum;
	}
	
	public float FractalNoise3D(float x, float y, float z, int octNum, float frq, float amp)
	{
		float gain = 1.0f;
		float sum = 0.0f;
	
		for(int i = 0; i < octNum; i++)
		{
			sum +=  Noise3D(x*gain/frq, y*gain/frq, z*gain/frq) * amp/gain;
			gain *= 2.0f;
		}
		return sum;
	}
}













