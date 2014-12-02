using UnityEngine;
using System.Collections;

public class NoiseModule
{
    protected const int B = 256;
    protected int[] m_perm = new int[B + B];

    public NoiseModule(int seed)
    {
        UnityEngine.Random.seed = seed;

        int i, j, k;
        for (i = 0; i < B; i++)
        {
            m_perm[i] = i;
        }

        while (--i != 0)
        {
            k = m_perm[i];
            j = UnityEngine.Random.Range(0, B);
            m_perm[i] = m_perm[j];
            m_perm[j] = k;
        }

        for (i = 0; i < B; i++)
        {
            m_perm[B + i] = m_perm[i];
        }
    }

    protected float Noise1D(float x)
    {
        //returns a noise value between -0.5 and 0.5
        int ix0, ix1;
        float fx0, fx1;
        float s, n0, n1;

        ix0 = (int)Mathf.Floor(x); 	// Integer part of x
        fx0 = x - ix0;       	// Fractional part of x
        fx1 = fx0 - 1.0f;
        ix1 = (ix0 + 1) & 0xff;
        ix0 = ix0 & 0xff;    	// Wrap to 0..255

        s = NoiseUtil.FADE(fx0);

        n0 = NoiseUtil.GRAD1(m_perm[ix0], fx0);
        n1 = NoiseUtil.GRAD1(m_perm[ix1], fx1);
        return 0.188f * NoiseUtil.LERP(s, n0, n1);
    }

    protected float Noise2D(float x, float y)
    {
        //returns a noise value between -0.75 and 0.75
        int ix0, iy0, ix1, iy1;
        float fx0, fy0, fx1, fy1, s, t, nx0, nx1, n0, n1;

        ix0 = (int)Mathf.Floor(x); 	// Integer part of x
        iy0 = (int)Mathf.Floor(y); 	// Integer part of y
        fx0 = x - ix0;        	// Fractional part of x
        fy0 = y - iy0;        	// Fractional part of y
        fx1 = fx0 - 1.0f;
        fy1 = fy0 - 1.0f;
        ix1 = (ix0 + 1) & 0xff; // Wrap to 0..255
        iy1 = (iy0 + 1) & 0xff;
        ix0 = ix0 & 0xff;
        iy0 = iy0 & 0xff;

        t = NoiseUtil.FADE(fy0);
        s = NoiseUtil.FADE(fx0);

        nx0 = NoiseUtil.GRAD2(m_perm[ix0 + m_perm[iy0]], fx0, fy0);
        nx1 = NoiseUtil.GRAD2(m_perm[ix0 + m_perm[iy1]], fx0, fy1);

        n0 = NoiseUtil.LERP(t, nx0, nx1);

        nx0 = NoiseUtil.GRAD2(m_perm[ix1 + m_perm[iy0]], fx1, fy0);
        nx1 = NoiseUtil.GRAD2(m_perm[ix1 + m_perm[iy1]], fx1, fy1);

        n1 = NoiseUtil.LERP(t, nx0, nx1);

        return 0.507f * NoiseUtil.LERP(s, n0, n1);
    }

    protected float Noise3D(float x, float y, float z)
    {
        //returns a noise value between -1.5 and 1.5
        int ix0, iy0, ix1, iy1, iz0, iz1;
        float fx0, fy0, fz0, fx1, fy1, fz1;
        float s, t, r;
        float nxy0, nxy1, nx0, nx1, n0, n1;

        ix0 = (int)Mathf.Floor(x);  // Integer part of x
        iy0 = (int)Mathf.Floor(y);  // Integer part of y
        iz0 = (int)Mathf.Floor(z);  // Integer part of z
        fx0 = x - ix0;              // Fractional part of x
        fy0 = y - iy0;              // Fractional part of y
        fz0 = z - iz0;              // Fractional part of z
        fx1 = fx0 - 1.0f;
        fy1 = fy0 - 1.0f;
        fz1 = fz0 - 1.0f;
        ix1 = (ix0 + 1) & 0xff; // Wrap to 0..255
        iy1 = (iy0 + 1) & 0xff;
        iz1 = (iz0 + 1) & 0xff;
        ix0 = ix0 & 0xff;
        iy0 = iy0 & 0xff;
        iz0 = iz0 & 0xff;

        r = NoiseUtil.FADE(fz0);
        t = NoiseUtil.FADE(fy0);
        s = NoiseUtil.FADE(fx0);

        nxy0 = NoiseUtil.GRAD3(m_perm[ix0 + m_perm[iy0 + m_perm[iz0]]], fx0, fy0, fz0);
        nxy1 = NoiseUtil.GRAD3(m_perm[ix0 + m_perm[iy0 + m_perm[iz1]]], fx0, fy0, fz1);
        nx0 = NoiseUtil.LERP(r, nxy0, nxy1);

        nxy0 = NoiseUtil.GRAD3(m_perm[ix0 + m_perm[iy1 + m_perm[iz0]]], fx0, fy1, fz0);
        nxy1 = NoiseUtil.GRAD3(m_perm[ix0 + m_perm[iy1 + m_perm[iz1]]], fx0, fy1, fz1);
        nx1 = NoiseUtil.LERP(r, nxy0, nxy1);

        n0 = NoiseUtil.LERP(t, nx0, nx1);

        nxy0 = NoiseUtil.GRAD3(m_perm[ix1 + m_perm[iy0 + m_perm[iz0]]], fx1, fy0, fz0);
        nxy1 = NoiseUtil.GRAD3(m_perm[ix1 + m_perm[iy0 + m_perm[iz1]]], fx1, fy0, fz1);
        nx0 = NoiseUtil.LERP(r, nxy0, nxy1);

        nxy0 = NoiseUtil.GRAD3(m_perm[ix1 + m_perm[iy1 + m_perm[iz0]]], fx1, fy1, fz0);
        nxy1 = NoiseUtil.GRAD3(m_perm[ix1 + m_perm[iy1 + m_perm[iz1]]], fx1, fy1, fz1);
        nx1 = NoiseUtil.LERP(r, nxy0, nxy1);

        n1 = NoiseUtil.LERP(t, nx0, nx1);

        return 0.936f * NoiseUtil.LERP(s, n0, n1);
    }

    public virtual float FractalNoise2D(float x, float y, int octNum, float frq, float amp)
    {
        return 1;
    }
}
