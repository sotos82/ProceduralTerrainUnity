using UnityEngine;
using System.Collections;

public class TerrainScript : MonoBehaviour
{
    /*public Texture2D Bump0;
    public Texture2D Bump1;
    public Texture2D Bump2;
    public Texture2D Bump3;
    private float Tile0;
    private float Tile1;
    private float Tile2;
    private float Tile3;

    private float terrainSizeX;
    private float terrainSizeZ;*/

	void Start ()
    {
        /*Tile0 = 150;
        Tile1 = 50;
        Tile2 = 40;
        Tile3 = 0;

        Bump0 = Resources.Load("Textures/" + "rock_2048-norm") as Texture2D;
        Bump1 = Resources.Load("Textures/" + "forst_1024-normal") as Texture2D;
        Bump2 = Resources.Load("Textures/" + "ground_1024-normal") as Texture2D;
        if (Bump0)
            Shader.SetGlobalTexture("_BumpMap0", Bump0);
        if (Bump1)
            Shader.SetGlobalTexture("_BumpMap1", Bump1);
        if (Bump2)
            Shader.SetGlobalTexture("_BumpMap2", Bump2);
        if (Bump3)
            Shader.SetGlobalTexture("_BumpMap3", Bump3);

        Shader.SetGlobalFloat("_Tile0", Tile0);
        Shader.SetGlobalFloat("_Tile1", Tile1);
        Shader.SetGlobalFloat("_Tile2", Tile2);
        Shader.SetGlobalFloat("_Tile3", Tile3);

        Terrain terrainComp = (Terrain)GetComponent(typeof(Terrain));

        terrainSizeX = terrainComp.terrainData.size.x;
        terrainSizeZ = terrainComp.terrainData.size.z;

        Shader.SetGlobalFloat("_TerrainX", terrainSizeX);
        Shader.SetGlobalFloat("_TerrainZ", terrainSizeZ);*/
	}
	
	void Update ()
    {
	
	}
}
