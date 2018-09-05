using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransportHut : MonoBehaviour {

    public int hutID;
    public int connectingHut_ID;

    public GameManager.Direction hutEntrance;
    
    public Vector3 getHutEntrance(GameObject interactingObject)
    {
        if (hutEntrance == GameManager.Direction.LEFT)
            return new Vector3(transform.position.x - interactingObject.GetComponent<SpriteRenderer>().bounds.size.x, transform.position.y, transform.position.z);

        if (hutEntrance == GameManager.Direction.RIGHT)
            return new Vector3(transform.position.x + GetComponent<SpriteRenderer>().bounds.size.x, transform.position.y, transform.position.z);

        if (hutEntrance == GameManager.Direction.UP)
            return new Vector3(transform.position.x, transform.position.y + interactingObject.GetComponent<SpriteRenderer>().bounds.size.y, transform.position.z);

        if (hutEntrance == GameManager.Direction.DOWN)
            return new Vector3(transform.position.x, transform.position.y - GetComponent<SpriteRenderer>().bounds.size.y, transform.position.z);

        return new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }
    
}
