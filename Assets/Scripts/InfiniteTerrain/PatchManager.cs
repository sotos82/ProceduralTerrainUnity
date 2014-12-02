using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public static class PatchManager
{
    private static int terrainPatchRes = 96;
    private static int splatDetailPatchRes = 32;
    private static int treePatchRes = 8;

    private class TerrainInfo
    {
        public TerrainInfo(int globX, int globZ, Terrain ter, Vector3 newPos)
        {
            newPosition = newPos;
            globalX = globX;
            globalZ = globZ;
            terrain = ter;
        }

        public Vector3 newPosition;
        public int globalX;
        public int globalZ;
        public Terrain terrain;
    }

    public static Queue<IPatch> patchQueue = new Queue<IPatch>();
    private static List<TerrainInfo> patchList = new List<TerrainInfo>();

    public static void AddTerrainInfo(int globX, int globZ, Terrain terrain, Vector3 pos)
    {
        patchList.Add(new TerrainInfo(globX, globZ, terrain, pos));
    }

    public static void MakePatches()
    {
        foreach (TerrainInfo tI in patchList)
        {
            for (int i = 0; i < terrainPatchRes; i++)
                patchQueue.Enqueue(new TerrainPatch(tI.globalX, tI.globalZ, tI.terrain, InfiniteTerrain.m_heightMapSize * i / terrainPatchRes, InfiniteTerrain.m_heightMapSize * (i + 1) / terrainPatchRes, tI.newPosition));
        }
        foreach (TerrainInfo tI in patchList)
        {
            for (int i = 0; i < splatDetailPatchRes; i++)
                patchQueue.Enqueue(new SplatDetailPatch(tI.globalX, tI.globalZ, tI.terrain, InfiniteTerrain.m_alphaMapSize * i / splatDetailPatchRes, InfiniteTerrain.m_alphaMapSize * (i + 1) / splatDetailPatchRes));
        }
        foreach (TerrainInfo tI in patchList)
            for (int i = 0; i < treePatchRes; i++)
                patchQueue.Enqueue(new TreePatch(tI.globalX, tI.globalZ, tI.terrain, InfiniteTerrain.numOfTreesPerTerrain * i / treePatchRes, InfiniteTerrain.numOfTreesPerTerrain * (i + 1) / treePatchRes));
        patchList.Clear();
    }
}
