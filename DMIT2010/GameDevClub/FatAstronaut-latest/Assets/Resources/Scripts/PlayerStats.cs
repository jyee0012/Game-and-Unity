using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour {
    //Player's health bar
    public int lives = 3;
    public int livesLeft = 3;
    public GameObject healthbar;
    public float heartSize = 50f;
    public Sprite healthSprite;
    public Sprite lostHealthSprite;

    //Player's Jetpack Fuel Gauge
    //public int maxFuelAmount;
    //public int currentFuelAmount;
    public float burnTime; //how long it takes to burn through 1 unit of fuel
    private float currentBurnTime;
    public Slider fuelSlider;

    public void Start()
    {
        livesLeft = lives;

        //initialize player's health
        if (healthbar != null && healthSprite != null && lostHealthSprite != null)
            InitializeHealth();
        else
        {
            Debug.Log("Need to set the sprites for the player's Lives");
            Application.Quit();
        }

        //initialize player's fuel
    }

    //set the health bar upon starting the game
    private void InitializeHealth()
    {
        //initialize the number of hearts onto the canvas        
        for (int i = 0; i < livesLeft; i++)
        {
            //Debug.Log("Creating new Heart object");
            GameObject heart = new GameObject("Heart");
            heart.tag = "Life";
            heart.AddComponent<Image>();

            heart.GetComponent<Image>().sprite = healthSprite;

            heart.transform.SetParent(healthbar.transform);

            heart.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
            heart.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
            heart.GetComponent<RectTransform>().pivot = new Vector2(0, 1);

            heart.GetComponent<RectTransform>().sizeDelta = new Vector2(35, 35);

            heart.transform.position = new Vector3((i * 40), healthbar.transform.position.y, healthbar.transform.position.z);
        }
    }

    //When the player gets hit and takes damage
    public void TakeDamage(int dmg)
    {
        GameObject[] lifeList = GameObject.FindGameObjectsWithTag("Life");

        if (dmg > livesLeft)
            dmg = livesLeft;

        //start at the last life and count down
        for (int i = lifeList.Length; i > 0; i --)
        {
            //if all the damage has been recorded
            if (dmg == 0)
                break;
            else
            {
                if (lifeList[i - 1].GetComponent<Image>().sprite == healthSprite)
                {
                    //change heart icon to tell player has lost a life
                    lifeList[i - 1].GetComponent<Image>().sprite = lostHealthSprite;
                    dmg -= 1;
                    livesLeft -= 1;
                }
            }
        }
    }

    public void Heal(int amount)
    {
        GameObject[] lifeList = GameObject.FindGameObjectsWithTag("Life");

        if (amount > lives - livesLeft)
            amount = lives - livesLeft;

        //loop through all the lives
        for (int i = 0; i < lives; i ++)
        {
            if (amount > 0 && lifeList[i].GetComponent<Image>().sprite != healthSprite)
            {
                amount--;
                livesLeft++;
                lifeList[i].GetComponent<Image>().sprite = healthSprite;
            }
        }

        //InitializeHealth();
    }

    public void Refuel(float amount)
    {
        float fuelLost = fuelSlider.maxValue - fuelSlider.value;

        //if the amount of fuel lost is less than the passed in amount
        if (fuelLost < amount)
            amount = fuelLost;

        fuelSlider.value += amount;
    }

    //burn a unit of fuel
    public void BurnFuel()
    {
        if (Time.time >= currentBurnTime)
        {
            currentBurnTime = Time.time + burnTime;
            fuelSlider.value -= 2;
        }
    }

    public bool CanUseJetPack()
    {
        if (fuelSlider.value != 0)
            return true;
        else
            return false;
    }

    public void Death()
    {
        //spawn a gravestone
        GameObject gravestone = (GameObject)Instantiate(Resources.Load("Prefabs/gravestone"));
        gravestone.GetComponent<SpawnGraveStone>().SetLocation(transform.position);

        //remove the player
        gameObject.SetActive(false);
    }
}
