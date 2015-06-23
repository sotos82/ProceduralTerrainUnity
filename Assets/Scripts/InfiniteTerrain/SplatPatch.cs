using UnityEngine;
using System.Collections;

public class SplatDetailPatch : IPatch  //To save some calls I have merged the splat & details patches
{
    private Terrain terrain;
    private int globalTileX, globalTileZ, h0, h1;

    private NoiseModule m_detailNoise = new PerlinNoise(1);
    private NoiseModule m_SplatNoise = new PerlinNoise(1);

    public SplatDetailPatch(int globTileX_i, int globTileZ_i, Terrain terrain_i, int h0_i, int h1_i)
    {
        terrain = terrain_i;
        h0 = h0_i;
        h1 = h1_i;
        globalTileX = globTileX_i;
        globalTileZ = globTileZ_i;
    }

    public void ExecutePatch()
    {
        FillSplatDetailPatch();
        if (h1 == InfiniteTerrain.m_alphaMapSize)
        {
            terrain.terrainData.SetAlphamaps(0, 0, InfiniteTerrain.m_alphaMap);

            terrain.terrainData.SetDetailLayer(0, 0, 0, InfiniteTerrain.detailMap0);
            terrain.terrainData.SetDetailLayer(0, 0, 1, InfiniteTerrain.detailMap1);
        }
    }

    void FillSplatDetailPatch()
    {
        float heightThres = 700;
        float sandHeight = 80;
        float ratio = (float)InfiniteLandscape.m_landScapeSize / (float)InfiniteTerrain.m_heightMapSize;

        for (int x = h0; x < h1; x++)
        {
            

            for (int z = 0; z < InfiniteTerrain.m_alphaMapSize; z++)
            {
                float worldPosX = (x + globalTileX * (InfiniteTerrain.m_heightMapSize - 1)) * ratio;
                float worldPosZ = (z + globalTileZ * (InfiniteTerrain.m_heightMapSize - 1)) * ratio;

                InfiniteTerrain.detailMap0[z, x] = 0;
                InfiniteTerrain.detailMap1[z, x] = 0;

                float normX = x * 1.0f / (InfiniteTerrain.m_alphaMapSize - 1);
                float normZ = z * 1.0f / (InfiniteTerrain.m_alphaMapSize - 1);

                float angle = terrain.terrainData.GetSteepness(normX, normZ);
                float height = terrain.terrainData.GetInterpolatedHeight(normX, normZ);
                float frac = angle / 90.0f;

                if (height < heightThres)
                {
                    //details
                    if (frac < 0.6f && height > 1.1f * InfiniteLandscape.waterHeight)
                    {
                        float noise = m_detailNoise.FractalNoise2D(worldPosX, worldPosZ, 2, 100, 1.0f);

                        if (noise > 0.0f)
                        {
                            float rnd = Random.value;
                            if (rnd < 0.33f)
                                InfiniteTerrain.detailMap0[z, x] = 1;
                            else if (rnd < 0.66f)
                                InfiniteTerrain.detailMap1[z, x] = 1;
                        }
                    }
                    /******************************************/

                    float c = Mathf.Clamp01(Mathf.Pow(height / sandHeight, 3));

                    InfiniteTerrain.m_alphaMap[z, x, 0] = frac;
                    InfiniteTerrain.m_alphaMap[z, x, 1] = (1 - frac) * c;
                    InfiniteTerrain.m_alphaMap[z, x, 2] = 0;
                    InfiniteTerrain.m_alphaMap[z, x, 3] = 1 - frac - (1 - frac) * c;
                }
                else
                {
                    InfiniteTerrain.m_alphaMap[z, x, 0] = 1 - height / 1500;
                    InfiniteTerrain.m_alphaMap[z, x, 1] = 0;
                    InfiniteTerrain.m_alphaMap[z, x, 2] = height / 1500;
                    InfiniteTerrain.m_alphaMap[z, x, 3] = 0;
                }
            }
        }
    }
}