/* Code provided by Chris Morris of Six Times Nothing (http://www.sixtimesnothing.com) */
/* Free to use and modify */

using UnityEngine;
using System.Collections;

public class CustomTerrainScriptAtsV2 : MonoBehaviour {

    private Texture2D Bump0;
    private Texture2D Bump1;
    private Texture2D Bump2;
    private Texture2D Bump3;

    private float Tile0 = 300;
    private float Tile1 = 5;
    private float Tile2 = 10;
    private float Tile3 = 5;

    private float terrainSizeX;
	private float terrainSizeZ;
	
	void Start ()
    {

        Bump0 = Resources.Load("Textures/" + "rock_2048-norm") as Texture2D;

        InfiniteTerrain infTerrain = (InfiniteTerrain)GetComponent(typeof(InfiniteTerrain));
		
		if(Bump0)
			Shader.SetGlobalTexture("_BumpMap0", Bump0);
		
		if(Bump1)
			Shader.SetGlobalTexture("_BumpMap1", Bump1);
		
		if(Bump2)
			Shader.SetGlobalTexture("_BumpMap2", Bump2);
		
		if(Bump3)
			Shader.SetGlobalTexture("_BumpMap3", Bump3);
		
		Shader.SetGlobalFloat("_Tile0", Tile0);
		Shader.SetGlobalFloat("_Tile1", Tile1);
		Shader.SetGlobalFloat("_Tile2", Tile2);
		Shader.SetGlobalFloat("_Tile3", Tile3);

        terrainSizeX = InfiniteLandscape.m_landScapeSize;
        terrainSizeZ = InfiniteLandscape.m_landScapeSize;

        Shader.SetGlobalFloat("multiplier", 4f);

        Shader.SetGlobalFloat("_TerrainX", terrainSizeX);
        Shader.SetGlobalFloat("_TerrainZ", terrainSizeZ);
	}

}
