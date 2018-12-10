using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Serialize Region Info for inspector
#region Region Info
[System.Serializable]
public class RegionInfo
{
    private GameObject _regionObj;
    private int _regionSceneIndex;
    private bool _regionConqured;
    #region Old Vars
    /*
    public GameObject regionObject
    {
        get { return _regionObj; }
        set { _regionObj = value; }
    }
    public int regionSceneIndex
    {
        get { return _regionSceneIndex; }
        set { _regionSceneIndex = value; }
    }
    public bool regionConqured
    {
        get { return _regionConqured; }
        set { _regionConqured = value; }
    }
    */
    #endregion
    public GameObject regionObject;
    public int regionSceneIndex;
    public bool regionConqured;
}
#endregion

// Create UnitController and UnitPlaceHolder for spawning

public class RegionManager : MonoBehaviour
{
    private static RegionManager instance;

    public static RegionManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        // If no Player ever existed, we are it.
        if (instance == null)
            instance = this;
        // If one already exist, it's because it came from another level.
        else if (instance != this)
        {
            // destroy the other instance after taking its info.
            for (int i = 0; i < instance.regionList.Count; i++)
            {
                this.regionList[i].regionConqured = instance.regionList[i].regionConqured;
            }
            this.currentRegion = instance.currentRegion;
            this.retreatCount = instance.retreatCount;
            Destroy(instance.gameObject);
            instance = this;
            //Destroy(gameObject);
            return;
        }
    }
    [SerializeField]
    RegionInfo currentRegion;

    [SerializeField]
    bool isRegionMap = false;

    [SerializeField]
    List<RegionInfo> regionList;
    [SerializeField]
    int retreatCount = 5;
    [SerializeField]
    Text retreatText;
    [SerializeField]
    Material conquredMat, unconquredMat;

    bool canManage = true;

    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        //canManage = (regionList.Count != regionSceneInt.Length && regionList.Count != conquredRegions.Length);
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2 || SceneManager.GetActiveScene().buildIndex == 3)
        {
            Destroy(this.gameObject);
            Destroy(instance);
            instance = null;
        }
        UpdateText();
        if (isRegionMap)
        {
            for (int i = 0; i < regionList.Count; i++)
            {
                SetRegionMaterial(i);
            }
        }
        if (CheckWinCondition() || CheckLoseCondition())
        {
            GotoEndScene();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            Conqure();
        }
    }

    public bool CheckIfRegion(GameObject hitObj)
    {
        return (ReturnRegionSceneInt(hitObj) != null);
    }
    void CustomLoadScene(int sceneIndex)
    {
        if (SceneManager.sceneCountInBuildSettings > 0 && sceneIndex < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(sceneIndex);
    }
    int? ReturnRegionSceneInt(GameObject region)
    {
        int? regionInt = null;
        for (int i = 0; i < regionList.Count; i++)
        {
            if (regionList[i].regionObject == region)
                regionInt = i;
        }
        if (regionInt != null)
        {
            int regionIndex = int.Parse(regionInt.ToString());
            SetSelectedRegion(regionIndex);
            //Debug.Log(regionIndex);
            LoadRegionScene(regionIndex);
            //ConqureRegion(regionIndex);

        }
        return regionInt;
    }
    public void SetSelectedRegion(int regionIndex)
    {
        currentRegion = regionList[regionIndex];
    }
    public void ConqureRegion(int regionIndex, bool conqured = true)
    {
        regionList[regionIndex].regionConqured = conqured;
        //if (!conquredRegions[regionIndex]) ToggleConqureRegion(regionIndex);
    }
    public void ToggleConqureRegion(int regionIndex)
    {
        regionList[regionIndex].regionConqured = !regionList[regionIndex].regionConqured;
    }
    public void UnconqureAllRegions()
    {
        for (int i = 0; i < regionList.Count; i++)
        {
            regionList[i].regionConqured = false;
        }
    }
    public void LoadRegionScene(int regionIndex)
    {
        CustomLoadScene(regionList[regionIndex].regionSceneIndex);
    }
    public void FindAndConqureRegionByBuildIndex(int buildIndex)
    {
        int? regionBuildIndex = FindRegionByBuildIndex(buildIndex);
        if (regionBuildIndex != null)
        {
            ConqureRegion(int.Parse(regionBuildIndex.ToString()));
        }
    }
    public int? FindRegionByBuildIndex(int buildIndex)
    {
        int? regionIndex = null;
        for (int i = 0; i < regionList.Count; i++)
        {
            if (regionList[i].regionSceneIndex == buildIndex) regionIndex = i;
        }
        return regionIndex;
    }
    public void Retreat()
    {
        retreatCount--;
        CustomLoadScene(1);
    }
    public void Conqure()
    {
        if (isRegionMap) return;

        //currentRegion.regionConqured = true;
        FindAndConqureRegionByBuildIndex(currentRegion.regionSceneIndex);
        //Debug.Log(currentRegion.regionSceneIndex);
        CustomLoadScene(1);
    }
    public bool CheckWinCondition()
    {
        bool won = true;
        foreach (RegionInfo conqured in regionList)
        {
            if (!conqured.regionConqured) won = false;
        }
        return won;
    }
    public bool CheckLoseCondition()
    {
        return retreatCount <= 0;
    }
    public void GotoEndScene()
    {
        int endIndex = (CheckWinCondition()) ? 2 : 3;
        //Debug.Log(endIndex);
        UnconqureAllRegions();
        CustomLoadScene(endIndex);
    }
    void UpdateText()
    {
        if (retreatText != null) retreatText.text = retreatCount + " left"; //"Remaining Retreats: " +
    }
    void SetRegionMaterial(int regionIndex)
    {
        Renderer regionRenderer = null;
        if (regionList[regionIndex].regionObject.GetComponentInChildren<Renderer>() != null)
        {
            regionRenderer = regionList[regionIndex].regionObject.GetComponentInChildren<Renderer>();
        }
        if (conquredMat == null || unconquredMat == null || regionRenderer == null) return;
        //regionRenderer.material = (conquredRegions[regionIndex]) ? conquredMat : unconquredMat;
        if (regionList[regionIndex].regionObject.GetComponentsInChildren<Renderer>().Length > 0)
        {
            foreach (Renderer render in regionList[regionIndex].regionObject.GetComponentsInChildren<Renderer>())
            {
                render.material = (regionList[regionIndex].regionConqured) ? conquredMat : unconquredMat;
            }
        }
    }
}
