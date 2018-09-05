using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collisions : MonoBehaviour {

    public bool transporting;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject temp = collision.gameObject;

        Debug.Log("Entering Collision");
        //is it a transport hut ?
        if (temp.GetComponent<TransportHut>() != null)
        {
            //save the hut id that it connects to
            int connectingID = temp.GetComponent<TransportHut>().connectingHut_ID;

            //get list of huts in the game scene
            GameObject[] huts = GameObject.FindGameObjectsWithTag("Transport Hut");
            Debug.Log("Huts in game: " + huts.Length);

            //find the hut thats connected and transport the player to it
            foreach (GameObject th in huts)
            {
                if (th.GetComponent<TransportHut>().hutID == connectingID)
                {
                    transform.position = th.transform.position;
                    Debug.Log("Transporting...");
                }
                    
            }
        }
    }
}
