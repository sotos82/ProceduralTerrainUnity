using UnityEngine;
using System.Collections;

public class TreePatch : IPatch
{
    private Terrain terrain;
    private int globalTileX, globalTileZ, h0, h1;
    private TreeInstance[] tr;

    private NoiseModule m_treeNoise = new RidgedNoise(1);

    public TreePatch(int globTileX_i, int globTileZ_i, Terrain terrain_i, int h0_i, int h1_i)
    {
        terrain = terrain_i;
        h0 = h0_i;
        h1 = h1_i;
        globalTileX = globTileX_i;
        globalTileZ = globTileZ_i;
    }
    
    public void ExecutePatch()
    {
        FillTreePatch();
        if (h1 == InfiniteTerrain.numOfTreesPerTerrain)
        {
            terrain.terrainData.treeInstances = tr;
        }
    }

    private void FillTreePatch()
    {
        tr = new TreeInstance[InfiniteTerrain.numOfTreesPerTerrain];
        float bushHeight = 80;

        for (int k = 0; k < InfiniteTerrain.numOfTreesPerTerrain; k++)
        {
            float x = Random.value;
            float y = Random.value;
            float angle = terrain.terrainData.GetSteepness(x, y);
            float ht = terrain.terrainData.GetInterpolatedHeight(x, y);
            if (ht > InfiniteTerrain.waterHeight * 1.1f)
            {
                float noise = m_treeNoise.FractalNoise2D(x, y, 2, 100, 0.4f);
                if (ht < bushHeight)
                {
                    if (noise > 0)
                    {
                        tr[k].position = new Vector3(x, ht / InfiniteTerrain.m_terrainHeight, y);
                        tr[k].prototypeIndex = 3;
                        tr[k].widthScale = Random.Range(8f, 9f);
                        tr[k].heightScale = Random.Range(8f, 9f);
                        tr[k].color = Color.white;
                        tr[k].lightmapColor = Color.white;
                    }
                }
                else if (ht > bushHeight && angle < 35)
                {
                    if (noise > 0)
                    {
                        tr[k].position = new Vector3(x, ht / InfiniteTerrain.m_terrainHeight, y);
                        tr[k].prototypeIndex = Random.Range(0, 2);
                        tr[k].widthScale = Random.Range(2f, 2.5f);
                        tr[k].heightScale = Random.Range(2f, 2.5f);
                        tr[k].color = Color.white;
                        tr[k].lightmapColor = Color.white;
                    }
                }
            }
            else
            {
                tr[k].widthScale = 0;
                tr[k].heightScale = 0;
            }
        }

    }
}
