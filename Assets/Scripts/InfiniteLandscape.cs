using UnityEngine;
using System.Collections;

public class InfiniteLandscape : MonoBehaviour
{
    public GameObject PlayerObject;

    public static float waterHeight = 50;

    public static float m_landScapeSize = 3072;

    protected const int dim = 3;
    public static int initialGlobalIndex = 9003;

    protected bool patchIsFilling = false;
    protected int prevGlobalIndexX = -1;
    protected int prevGlobalIndexZ = -1;
    protected int curGlobalIndexX = initialGlobalIndex + 1;
    protected int curGlobalIndexZ = initialGlobalIndex + 1;

    protected int prevLocalIndexX = -1;
    protected int prevLocalIndexZ = -1;
    protected int curLocalIndexX = 1;
    protected int curLocalIndexZ = 1;

    protected int prevCyclicIndexX = -1;
    protected int prevCyclicIndexZ = -1;
    protected int curCyclicIndexX = 1;
    protected int curCyclicIndexZ = 1;

    protected bool updateLandscape = false;

    protected bool UpdateIndexes()
    {

        int currentLocalIndexX = GetLocalIndex(PlayerObject.transform.position.x);
        int currentLocalIndexZ = GetLocalIndex(PlayerObject.transform.position.z);

        if (curLocalIndexX != currentLocalIndexX || curLocalIndexZ != currentLocalIndexZ)
        {
            prevLocalIndexX = curLocalIndexX;
            curLocalIndexX = currentLocalIndexX;
            prevLocalIndexZ = curLocalIndexZ;
            curLocalIndexZ = currentLocalIndexZ;

            int dx = curLocalIndexX - prevLocalIndexX;
            int dz = curLocalIndexZ - prevLocalIndexZ;
            prevGlobalIndexX = curGlobalIndexX;
            curGlobalIndexX += dx;
            prevGlobalIndexZ = curGlobalIndexZ;
            curGlobalIndexZ += dz;

            prevCyclicIndexX = curCyclicIndexX;
            curCyclicIndexX = curGlobalIndexX % dim;
            prevCyclicIndexZ = curCyclicIndexZ;
            curCyclicIndexZ = curGlobalIndexZ % dim;

            return true;
        }
        else return false;
    }

    protected int GetLocalIndex(float x)
    {
        return (Mathf.CeilToInt(x / m_landScapeSize));
    }

    void Start()
    {

    }

    protected void Update()
    {
        if (UpdateIndexes())
            updateLandscape = true;
        else
            updateLandscape = false;
    }
}
