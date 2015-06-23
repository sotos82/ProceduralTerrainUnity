using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InfiniteTerrain : InfiniteLandscape
{
    private IPatch patchToBeFilled = null;
    bool terrainIsFlushed = true;

    public const int m_heightMapSize = 513;
    public const float m_terrainHeight = 1500;
    public static float[,] m_terrainHeights = new float[m_heightMapSize, m_heightMapSize];

    public static Terrain[,] m_terrainGrid = new Terrain[dim, dim];

    //Trees
    public static int numOfTreePrototypes = 4;
    public static int numOfTreesPerTerrain = 2000;
    private GameObject[] trees = new GameObject[numOfTreePrototypes];
    TreePrototype[] m_treeProtoTypes = new TreePrototype[numOfTreePrototypes];
    public float m_treeDistance = 2000.0f;          //The distance at which trees will no longer be drawn
    public float m_treeBillboardDistance = 400.0f;  //The distance at which trees meshes will turn into tree billboards
    public float m_treeCrossFadeLength = 50.0f;     //As trees turn to billboards there transform is rotated to match the meshes, a higher number will make this transition smoother
    public int m_treeMaximumFullLODCount = 400;     //The maximum number of trees that will be drawn in a certain area. 

    //Splat
    public const int numOfSplatPrototypes = 4;
    public const int m_alphaMapSize = (InfiniteTerrain.m_heightMapSize - 1) / 2;
    public static float[, ,] m_alphaMap = new float[m_alphaMapSize, m_alphaMapSize, numOfSplatPrototypes];
    private Texture2D[] splat = new Texture2D[numOfSplatPrototypes];
    private SplatPrototype[] m_splatPrototypes = new SplatPrototype[numOfSplatPrototypes];
    //Details
    public const int numOfDetailPrototypes = 2;
    public const int m_detailMapSize = m_alphaMapSize;                 //Resolutions of detail (Grass) layers SHOULD BE EQUAL TO SPLAT RES
    public static int[,] detailMap0 = new int[m_detailMapSize, m_detailMapSize];
    public static int[,] detailMap1 = new int[m_detailMapSize, m_detailMapSize];
    public int m_detailObjectDistance = 500;                                //The distance at which details will no longer be drawn
    public float m_detailObjectDensity = 40.0f;                             //Creates more dense details within patch
    public int m_detailResolutionPerPatch = 32;                             //The size of detail patch. A higher number may reduce draw calls as details will be batch in larger patches
    public float m_wavingGrassStrength = 0.4f;
    public float m_wavingGrassAmount = 0.2f;
    public float m_wavingGrassSpeed = 0.4f;
    public Color m_wavingGrassTint = Color.white;
    public Color m_grassHealthyColor = Color.white;
    public Color m_grassDryColor = Color.white;

    private DetailPrototype[] m_detailProtoTypes = new DetailPrototype[numOfDetailPrototypes];
    private Texture2D[] detailTexture = new Texture2D[numOfDetailPrototypes];
    private GameObject[] detailMesh = new GameObject[numOfDetailPrototypes];
    private DetailRenderMode detailMode;

    private float Tile0 = 300;
    private float Tile1 = 5;
    private float Tile2 = 10;
    private float Tile3 = 5;

    void Awake()
    {
        splat[0] = Resources.Load("Textures/" + "rock_2048") as Texture2D;
        splat[1] = Resources.Load("Textures/" + "forst_1024") as Texture2D;
        splat[2] = Resources.Load("Textures/" + "snow_512") as Texture2D;
        splat[3] = Resources.Load("Textures/" + "GoodDirt") as Texture2D;

        detailTexture[0] = Resources.Load("Details/SimpleGrass/" + "Grass") as Texture2D;
        detailTexture[1] = Resources.Load("Details/SimpleGrass/" + "WhiteFlowers") as Texture2D;

        trees[0] = Resources.Load("Trees/" + "Tree4Master") as GameObject;
        trees[1] = Resources.Load("Trees/" + "Tree5") as GameObject;
        trees[2] = Resources.Load("Trees/" + "Tree4Master") as GameObject;
        trees[3] = Resources.Load("Trees/" + "Bush1") as GameObject;

        Vector2[] splatTileSize = new Vector2[4] { new Vector2(Tile0, Tile0), new Vector2(Tile1, Tile1), new Vector2(Tile2, Tile2), new Vector2(Tile3, Tile3) };
        for (int i = 0; i < numOfSplatPrototypes; i++)
            m_splatPrototypes[i] = new SplatPrototype();

        for (int i = 0; i < numOfSplatPrototypes; i++)
        {
            m_splatPrototypes[i].texture = splat[i];
            m_splatPrototypes[i].tileOffset = Vector2.zero;
            m_splatPrototypes[i].tileSize = splatTileSize[i];
            m_splatPrototypes[i].texture.Apply(true);
        }

        for (int i = 0; i < numOfDetailPrototypes; i++)
        {
            m_detailProtoTypes[i] = new DetailPrototype();
            m_detailProtoTypes[i].prototypeTexture = detailTexture[i];
            m_detailProtoTypes[i].renderMode = detailMode;
            m_detailProtoTypes[i].healthyColor = m_grassHealthyColor;
            m_detailProtoTypes[i].dryColor = m_grassDryColor;
            m_detailProtoTypes[i].maxHeight = 0.5f;
            m_detailProtoTypes[i].maxWidth = 0.2f;
        }
        for (int i = 0; i < numOfTreePrototypes; i++)
        {
            m_treeProtoTypes[i] = new TreePrototype();
            m_treeProtoTypes[i].prefab = trees[i];
        }
        for (int i = 0; i < dim; i++)
        {
            for (int j = 0; j < dim; j++)
            {
                TerrainData terrainData = new TerrainData();

                terrainData.wavingGrassStrength = m_wavingGrassStrength;
                terrainData.wavingGrassAmount = m_wavingGrassAmount;
                terrainData.wavingGrassSpeed = m_wavingGrassSpeed;
                terrainData.wavingGrassTint = m_wavingGrassTint;
                terrainData.heightmapResolution = m_heightMapSize;
                terrainData.size = new Vector3(m_landScapeSize, m_terrainHeight, m_landScapeSize);
                terrainData.alphamapResolution = m_alphaMapSize;
                terrainData.splatPrototypes = m_splatPrototypes;
                terrainData.treePrototypes = m_treeProtoTypes;
                terrainData.SetDetailResolution(m_detailMapSize, m_detailResolutionPerPatch);
                terrainData.detailPrototypes = m_detailProtoTypes;

                m_terrainGrid[i, j] = Terrain.CreateTerrainGameObject(terrainData).GetComponent<Terrain>();
            }
        }

        for (int i = 0; i < dim; i++)
        {
            for (int j = 0; j < dim; j++)
            {
                m_terrainGrid[i, j].gameObject.AddComponent<TerrainScript>();
                m_terrainGrid[i, j].transform.parent = gameObject.transform;

                m_terrainGrid[i, j].transform.position = new Vector3(
                m_terrainGrid[1, 1].transform.position.x + (i - 1) * m_landScapeSize, m_terrainGrid[1, 1].transform.position.y,
                m_terrainGrid[1, 1].transform.position.z + (j - 1) * m_landScapeSize);

                m_terrainGrid[i, j].treeDistance = m_treeDistance;
                m_terrainGrid[i, j].treeBillboardDistance = m_treeBillboardDistance;
                m_terrainGrid[i, j].treeCrossFadeLength = m_treeCrossFadeLength;
                m_terrainGrid[i, j].treeMaximumFullLODCount = m_treeMaximumFullLODCount;

                m_terrainGrid[i, j].detailObjectDensity = m_detailObjectDensity;
                m_terrainGrid[i, j].detailObjectDistance = m_detailObjectDistance;

                m_terrainGrid[i, j].GetComponent<Collider>().enabled = false;
                m_terrainGrid[i, j].basemapDistance = 4000;
                m_terrainGrid[i, j].castShadows = false;

                PatchManager.AddTerrainInfo(curGlobalIndexX + i - 1, curGlobalIndexZ + j - 1, m_terrainGrid[i, j], m_terrainGrid[i, j].transform.position);
            }
        }
        PatchManager.MakePatches();

        int patchCount = PatchManager.patchQueue.Count;
        for(int i = 0; i < patchCount; i++)
            PatchManager.patchQueue.Dequeue().ExecutePatch();

        UpdateIndexes();
        UpdateTerrainNeighbors();

        StartCoroutine(FlushTerrain());
        terrainIsFlushed = true;

        m_terrainGrid[curCyclicIndexX, curCyclicIndexZ].GetComponent<Collider>().enabled = false;
        m_terrainGrid[curCyclicIndexX, curCyclicIndexZ].GetComponent<Collider>().enabled = true;
    }

    void UpdateTerrainNeighbors()
    {
        int iC = curCyclicIndexX;           int jC = curCyclicIndexZ;
        int iP = PreviousCyclicIndex(iC);   int jP = PreviousCyclicIndex(jC);
        int iN = NextCyclicIndex(iC);       int jN = NextCyclicIndex(jC);

        m_terrainGrid[iP, jP].SetNeighbors(null, m_terrainGrid[iP, jC], m_terrainGrid[iC, jP], null);
        m_terrainGrid[iC, jP].SetNeighbors(m_terrainGrid[iP, jP], m_terrainGrid[iC, jC], m_terrainGrid[iN, jP], null);
        m_terrainGrid[iN, jP].SetNeighbors(m_terrainGrid[iC, jP], m_terrainGrid[iN, jC], null, null);
        m_terrainGrid[iP, jC].SetNeighbors(null, m_terrainGrid[iP, jN], m_terrainGrid[iC, jC], m_terrainGrid[iP, jP]);
        m_terrainGrid[iC, jC].SetNeighbors(m_terrainGrid[iP, jC], m_terrainGrid[iC, jN], m_terrainGrid[iN, jC], m_terrainGrid[iC, jP]);
        m_terrainGrid[iN, jC].SetNeighbors(m_terrainGrid[iC, jC], m_terrainGrid[iN, jN], null, m_terrainGrid[iN, jP]);
        m_terrainGrid[iP, jN].SetNeighbors(null, null, m_terrainGrid[iC, jN], m_terrainGrid[iP, jC]);
        m_terrainGrid[iC, jN].SetNeighbors(m_terrainGrid[iP, jN], null, m_terrainGrid[iN, jN], m_terrainGrid[iC, jC]);
        m_terrainGrid[iN, jN].SetNeighbors(m_terrainGrid[iC, jN], null, null, m_terrainGrid[iN, jC]);
    }

    private int NextCyclicIndex(int i)
    {
        if (i < 0 || i > dim - 1)
            Debug.LogError("index outside dim");
        return (i + 1) % dim;
    }

    private int PreviousCyclicIndex(int i)
    {
        if (i < 0 || i > dim - 1)
            Debug.LogError("index outside dim");
        return i == 0 ? dim - 1 : (i-1) % dim;
    }

    private void UpdateTerrainPositions()
	{
        if (curGlobalIndexZ != prevGlobalIndexZ && curGlobalIndexX != prevGlobalIndexX)
        {
            int z; int z0;
            if (curGlobalIndexZ > prevGlobalIndexZ)
            {
                z0 = curGlobalIndexZ + 1;
                z = PreviousCyclicIndex(prevCyclicIndexZ);
            }
            else
            {
                z0 = curGlobalIndexZ - 1;
                z = NextCyclicIndex(prevCyclicIndexZ);
            }

            int[] listX = { PreviousCyclicIndex(prevCyclicIndexX), prevCyclicIndexX, NextCyclicIndex(prevCyclicIndexX) };
            for (int i = 1; i < dim; i++)
            {
                Vector3 newPos = new Vector3(
                m_terrainGrid[prevCyclicIndexX, curCyclicIndexZ].transform.position.x + (i - 1) * m_landScapeSize,
                m_terrainGrid[prevCyclicIndexX, curCyclicIndexZ].transform.position.y,
                m_terrainGrid[prevCyclicIndexX, curCyclicIndexZ].transform.position.z + (curGlobalIndexZ - prevGlobalIndexZ) * m_landScapeSize);

                PatchManager.AddTerrainInfo(prevGlobalIndexX + i - 1, z0, m_terrainGrid[listX[i], z], newPos);
            }
            int x; int x0;
            if (curGlobalIndexX > prevGlobalIndexX)
            {
                x0 = curGlobalIndexX + 1;
                x = PreviousCyclicIndex(prevCyclicIndexX);
            }
            else
            {
                x0 = curGlobalIndexX - 1;
                x = NextCyclicIndex(prevCyclicIndexX);
            }

            int[] listZ = { PreviousCyclicIndex(curCyclicIndexZ), curCyclicIndexZ, NextCyclicIndex(curCyclicIndexZ) };
            for (int i = 0; i < dim; i++)
            {
                Vector3 newPos = new Vector3(
                m_terrainGrid[curCyclicIndexX, curCyclicIndexZ].transform.position.x + (curGlobalIndexX - prevGlobalIndexX) * m_landScapeSize,
                m_terrainGrid[curCyclicIndexX, curCyclicIndexZ].transform.position.y,
                m_terrainGrid[curCyclicIndexX, curCyclicIndexZ].transform.position.z + (i - 1) * m_landScapeSize);

                PatchManager.AddTerrainInfo(x0, curGlobalIndexZ + i - 1, m_terrainGrid[x, listZ[i]], newPos);
            }
        }
        else if (curGlobalIndexZ != prevGlobalIndexZ)
        {
            int z; int z0;
            if (curGlobalIndexZ > prevGlobalIndexZ)
            {
                z0 = curGlobalIndexZ + 1;
                z = PreviousCyclicIndex(prevCyclicIndexZ);
            }
            else
            {
                z0 = curGlobalIndexZ - 1;
                z = NextCyclicIndex(prevCyclicIndexZ);
            }
                int[] listX = { PreviousCyclicIndex(prevCyclicIndexX), prevCyclicIndexX, NextCyclicIndex(prevCyclicIndexX) };
                for (int i = 0; i < dim; i++)
                {
                    Vector3 newPos = new Vector3(
                    m_terrainGrid[curCyclicIndexX, curCyclicIndexZ].transform.position.x + (i - 1) * m_landScapeSize,
                    m_terrainGrid[curCyclicIndexX, curCyclicIndexZ].transform.position.y,
                    m_terrainGrid[curCyclicIndexX, curCyclicIndexZ].transform.position.z + (curGlobalIndexZ - prevGlobalIndexZ) * m_landScapeSize);

                    PatchManager.AddTerrainInfo(curGlobalIndexX + i - 1, z0, m_terrainGrid[listX[i], z], newPos);
                }
        }
        else if (curGlobalIndexX != prevGlobalIndexX)
        {
            int x; int x0;
            if (curGlobalIndexX > prevGlobalIndexX)
            {
                x0 = curGlobalIndexX + 1;
                x = PreviousCyclicIndex(prevCyclicIndexX);
            }
            else
            {
                x0 = curGlobalIndexX - 1;
                x = NextCyclicIndex(prevCyclicIndexX);
            }

            int[] listZ = { PreviousCyclicIndex(prevCyclicIndexZ), prevCyclicIndexZ, NextCyclicIndex(prevCyclicIndexZ) };
            for (int i = 0; i < dim; i++)
            {
                Vector3 newPos = new Vector3(
                m_terrainGrid[curCyclicIndexX, curCyclicIndexZ].transform.position.x + (curGlobalIndexX - prevGlobalIndexX) * m_landScapeSize,
                m_terrainGrid[curCyclicIndexX, curCyclicIndexZ].transform.position.y,
                m_terrainGrid[curCyclicIndexX, curCyclicIndexZ].transform.position.z + (i - 1) * m_landScapeSize);

                PatchManager.AddTerrainInfo(x0, curGlobalIndexZ + i - 1, m_terrainGrid[x, listZ[i]], newPos);
            }
        }
        PatchManager.MakePatches();
	}

    IEnumerator CountdownForPatch()
    {
        patchIsFilling = true;
        yield return new WaitForEndOfFrame();
        patchIsFilling = false;
    }

    IEnumerator FlushTerrain()
    {
        for (int i = 0; i < dim; i++)
            for (int j = 0; j < dim; j++)
            {
                m_terrainGrid[i, j].Flush();
                yield return new WaitForEndOfFrame();
            }
    }

    void Update()
    {
        base.Update();
        if (updateLandscape == true)
        {
            m_terrainGrid[curCyclicIndexX, curCyclicIndexZ].GetComponent<Collider>().enabled = true;        //Slow operation
            m_terrainGrid[prevCyclicIndexX, prevCyclicIndexZ].GetComponent<Collider>().enabled = false;

            UpdateTerrainNeighbors();
            UpdateTerrainPositions();
        }

        if (PatchManager.patchQueue.Count != 0)
        {
            terrainIsFlushed = false;
            if (patchIsFilling == false)
            {
                patchToBeFilled = PatchManager.patchQueue.Dequeue();
                StartCoroutine(CountdownForPatch());
            }
            if (patchToBeFilled != null)
            {
                patchToBeFilled.ExecutePatch();
                patchToBeFilled = null;
            }
        }
        else if (PatchManager.patchQueue.Count == 0 && terrainIsFlushed == false)
        {
            StartCoroutine(FlushTerrain());
            terrainIsFlushed = true;
        }
    }
}