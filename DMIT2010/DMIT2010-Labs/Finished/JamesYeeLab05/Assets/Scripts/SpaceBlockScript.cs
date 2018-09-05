using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceBlockScript : MonoBehaviour
{

    public enum BlockID { Empty, Loot, Monster, Chest, SafeRoom }
    public BlockID thisBlock = BlockID.Empty;
    public List<GameObject> connections;
    bool Grave = false;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ColorManage();
    }
    void ColorManage()
    {
        switch (thisBlock)
        {
            case BlockID.Empty:
                this.GetComponent<Renderer>().material = Resources.Load("White") as Material;
                break;
            case BlockID.Loot:
                this.GetComponent<Renderer>().material = Resources.Load("Yellow") as Material;
                break;
            case BlockID.Monster:
                this.GetComponent<Renderer>().material = Resources.Load("Red") as Material;
                break;
            case BlockID.SafeRoom:
                this.GetComponent<Renderer>().material = Resources.Load("Green") as Material;
                break;
            default:
                break;
        }
    }
    public void EmptyRoom()
    {
        thisBlock = BlockID.Empty;
    }
    public void MonsterToLoot()
    {
        if(thisBlock == BlockID.Monster)
        {
            thisBlock = BlockID.Loot;
        }
    }
    #region Connections
    public void AddConnections(GameObject space)
    {
        connections.Add(space);
    }
    public void RemoveConnections(GameObject space)
    {
        connections.Remove(space);
    }
    public void ClearConnections()
    {
        connections.Clear();
    }
    #endregion
    private void OnDrawGizmos()
    {
        if (Grave)
        {
            Gizmos.DrawSphere(transform.position + new Vector3(0, 3, 0), 0.5f);
        }
    }
    public void DiedHere()
    {
        Grave = true;
    }

}
