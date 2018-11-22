using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


// Serialize Region Info for inspector
#region Region Info
public struct RegionInfo
{
    private GameObject _regionObj;
    private int _regionSceneIndex;
    private bool _regionConqured;

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
            this.conquredRegions = instance.conquredRegions;
            this.retreatCount = instance.retreatCount;
            Destroy(instance.gameObject);
            instance = this;
            //Destroy(gameObject);
            return;
        }
    }
    [SerializeField]
    GameObject selectedRegion;
    [SerializeField]
    int selectedRegionInt;
    [SerializeField]
    bool selectedRegionConqure, isRegionMap = false;
    [SerializeField]
    int currentSelectedRegionInt;

    [SerializeField]
    List<GameObject> regionList;
    [SerializeField]
    int[] regionSceneInt;
    [SerializeField]
    bool[] conquredRegions;
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
        canManage = (regionList.Count != regionSceneInt.Length && regionList.Count != conquredRegions.Length);
    }

    // Update is called once per frame
    void Update()
    {
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
            if (regionList[i] == region)
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
        currentSelectedRegionInt = regionIndex;
        selectedRegion = regionList[regionIndex];
        selectedRegionInt = regionSceneInt[regionIndex];
        selectedRegionConqure = conquredRegions[regionIndex];
    }
    public void ConqureRegion(int regionIndex, bool conqured = true)
    {
        conquredRegions[regionIndex] = conqured;
        //if (!conquredRegions[regionIndex]) ToggleConqureRegion(regionIndex);
    }
    public void ToggleConqureRegion(int regionIndex)
    {
        conquredRegions[regionIndex] = !conquredRegions[regionIndex];
    }
    public void UnconqureAllRegions()
    {
        for(int i = 0; i < conquredRegions.Length; i++)
        {
            conquredRegions[i] = false;
        }
    }
    public void LoadRegionScene(int regionIndex)
    {
        CustomLoadScene(regionSceneInt[regionIndex]);
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
        for(int i = 0;i < regionList.Count; i++)
        {
            if (regionSceneInt[i] == buildIndex) regionIndex = i;
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

        selectedRegionConqure = true;
        ConqureRegion(currentSelectedRegionInt);
        CustomLoadScene(1);
    }
    public bool CheckWinCondition()
    {
        bool won = true;
        foreach (bool conqured in conquredRegions)
        {
            if (!conqured) won = false;
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
        if (regionList[regionIndex].GetComponentInChildren<Renderer>() != null)
        {
            regionRenderer = regionList[regionIndex].GetComponentInChildren<Renderer>();
        }
        if (conquredMat == null || unconquredMat == null || regionRenderer == null) return;
        //regionRenderer.material = (conquredRegions[regionIndex]) ? conquredMat : unconquredMat;
        if (regionList[regionIndex].GetComponentsInChildren<Renderer>().Length > 0)
        {
            foreach (Renderer render in regionList[regionIndex].GetComponentsInChildren<Renderer>())
            {
                render.material = (conquredRegions[regionIndex]) ? conquredMat : unconquredMat;
            }
        }
    }
}
