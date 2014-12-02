using UnityEngine;
using System.Collections;

public class InfiniteWater : InfiniteLandscape
{
    private static GameObject[,] m_waterGrid = new GameObject[dim,dim];

    private const float m_waterTileSize = 100;

	void Start()
    {       
        for (int i = 0; i < dim; i++)
        {
            for (int j = 0; j < dim; j++)
            {
                m_waterGrid[i, j] = Instantiate(Resources.Load("Prefabs/WaterTile", typeof(GameObject))) as GameObject;
                m_waterGrid[i, j].transform.position = new Vector3(0, waterHeight, 0);
                m_waterGrid[i, j].transform.parent = InfiniteTerrain.m_terrainGrid[i, j].transform;
            }
        }

        for (int i = 0; i < dim; i++)
        {
            for (int j = 0; j < dim; j++)
            {
                m_waterGrid[i, j].transform.position = new Vector3((m_waterGrid[1, 1].transform.position.x + (i - 1)) * m_landScapeSize, m_waterGrid[1, 1].transform.position.y,
                                                                    (m_waterGrid[1, 1].transform.position.z + (j - 1)) * m_landScapeSize);
            }
        }
        for (int i = 0; i < dim; i++)
        {
            for (int j = 0; j < dim; j++)
            {
                m_waterGrid[i, j].transform.localScale = new Vector3(m_landScapeSize / m_waterTileSize, 1, m_landScapeSize / m_waterTileSize);
                m_waterGrid[i, j].transform.position += new Vector3(m_landScapeSize / 2, 0, m_landScapeSize / 2);
            }
        }
	}

    void Update()
    {
    }
}
