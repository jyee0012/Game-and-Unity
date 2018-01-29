using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

    float speed = 2f;
    public enum PlayerState { Alive, Dead, BeenHit }
    public PlayerState player = PlayerState.Alive;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        PlayerMovement();
	}

    void PlayerStates()
    {
        switch(player)
        {
            case PlayerState.Alive:
                break;
            case PlayerState.Dead:
                break;
            case PlayerState.BeenHit:
                break;
        }
    }
    #region Movement
    void PlayerMovement()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Move(new Vector3(-1, 0, 0));
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            Move(new Vector3(1, 0, 0));
        }
    }
    void Move(Vector3 moveBy)
    {
        transform.Translate(moveBy * Time.deltaTime * speed);
    }
    #endregion
    #region Shoot
    void Shoot()
    {
        Vector3 bulletSpawnPos = transform.position + new Vector3(0, -gameObject.GetComponent<BoxCollider2D>().edgeRadius, 0);
        
    }
    #endregion
}
