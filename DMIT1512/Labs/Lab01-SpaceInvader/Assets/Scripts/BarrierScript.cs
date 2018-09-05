using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierScript : MonoBehaviour {
    public float hp = 25, maxHp = 25;
    public Sprite full, half, low;
    SpriteRenderer thisSprite;
	// Use this for initialization
	void Start () {
        thisSprite = gameObject.GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        CheckHealth();
	}
    void CheckHealth()
    {
        if (hp <= 0)
        {
            DisplayBarrier(false);
        }
        else if (hp <= 5)
        {
            thisSprite.sprite = low;
        }
        else if (hp <= 18)
        {
            thisSprite.sprite = half;
        }
        else
        {
            thisSprite.sprite = full;
        }
    }
    public void TakeDamage()
    {
        hp--;
    }
    public void TakeDamage(int dmg)
    {
        hp -= dmg;
    }
    public void ResetBarrier()
    {
        DisplayBarrier(true);
        hp = 25;
    }
    void DisplayBarrier(bool show)
    {
        if (show)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
