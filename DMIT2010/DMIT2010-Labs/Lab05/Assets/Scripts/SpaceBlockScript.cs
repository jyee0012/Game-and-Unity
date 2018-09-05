using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceBlockScript : MonoBehaviour
{

    public enum BlockID { Empty, Loot, Monster, Chest, SafeRoom }
    public BlockID thisBlock = BlockID.Empty;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
