using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupScript : MonoBehaviour {

    [SerializeField]
    float amountHealed = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.name);
        GameObject collideRoot = other.gameObject.transform.root.gameObject;
        //Debug.Log(collideRoot.name);

        if (collideRoot.GetComponent<PlayableUnits>() != null && collideRoot.GetComponent<HealthScript>() != null)
        {
            HealTarget(collideRoot, amountHealed);
            Destroy(gameObject);
        }
    }
    void HealTarget(GameObject target, float healAmount = 0)
    {
        if (target.GetComponent<HealthScript>() == null) return;

        HealthScript healer = target.GetComponent<HealthScript>();
        if (healAmount == 0) healer.FullHeal();
        else healer.AddHealth(healAmount);
    }
}
