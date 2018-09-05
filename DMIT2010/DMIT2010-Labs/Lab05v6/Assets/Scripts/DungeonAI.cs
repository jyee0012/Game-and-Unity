using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonAI : MonoBehaviour
{

    public enum State { Explore, Loot, Attack, Rest, GoHome, Sleep, GearUp, Dead }
    public State myState = State.Explore;
    public float health = 10, dmg = 0, money = 0, stamina = 10, maxHealth, gear = 0, gearCost = 100, speed = 2, mobHp = -10, level = 0, timeScale = 1;
    float mobmaxHp = 5, mobminHp = 10, mobminAtk = 1, mobmaxAtk = 3, minMoney = 20, maxMoney = 100, maxStamina = 10, baseHealth = 40, baseDMG = 2, timeStamp = 0;
    bool mobDead = true, locked = false, once = true;
    public List<GameObject> myPath;
    public GameObject currentBlock, targetBlock, lastBlock, destinationBlock = null;
    SpaceBlockScript currentBlockScript;
    public SpaceBlockScript.BlockID myBlockState;
    public Slider healthBar, staminaBar, timeSlider;
    public Text textState, moneyText, timeText, levelText;
    // Use this for initialization
    void Start()
    {
        GameObject.FindGameObjectWithTag("Grid").GetComponent<GridManager>().Begin();
        Spawn();
        GearUp();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBars();
        TextUpdate();
        if (timeStamp <= Time.time)
        {
            States();
            if (health <= 0 && once)
            {
                Death();
                once = false;
            }
        }
        #region Keys
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Restart();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            Immortality();
        }
        #endregion
    }
    #region States
    void States()
    {
        switch (myState)
        {
            #region Explore
            case State.Explore:
                if (stamina > 0)
                {
                    if (health < maxHealth / 4)
                    {
                        myState = State.Rest;
                    }
                    Movement();
                    BlockState();
                }
                else
                {
                    myState = State.GoHome;
                }
                break;
            #endregion
            #region Loot
            case State.Loot:
                money += RandomMoney();
                currentBlockScript.EmptyRoom();
                BlockState();
                break;
            #endregion
            #region Attack
            case State.Attack:
                if (mobDead)
                {
                    RandomMobHP();
                    mobDead = false;
                }
                if (mobHp >= 0)
                {
                    mobHp -= dmg;
                    health -= RandomMobAttack();
                }
                else
                {
                    currentBlockScript.MonsterToLoot();
                    mobDead = true;
                }

                BlockState();
                break;
            #endregion
            #region Rest
            case State.Rest:
                health += 2;
                BlockState();
                break;
            #endregion
            #region GoHome
            case State.GoHome:
                if (!GotHome())
                {
                    Movement();
                }
                else
                {
                    myState = State.GearUp;
                    timeStamp = Time.time + (5f / timeScale);
                }
                break;
            #endregion
            #region Sleep
            case State.Sleep:
                Sleep();
                myState = State.Explore;
                break;
            #endregion
            #region GearUp
            case State.GearUp:
                if (money > gearCost)
                {
                    newGear();
                }
                else
                {
                    myState = State.Sleep;
                    timeStamp = Time.time + (10f / timeScale);
                }
                break;
            #endregion
            default:
                break;
        }
    }
    #endregion
    #region Block State
    void BlockState()
    {
        GetBlockState();
        switch (myBlockState)
        {
            case SpaceBlockScript.BlockID.Empty:
                myState = State.Explore;
                //timeStamp = Time.time + 2f;
                break;
            case SpaceBlockScript.BlockID.SafeRoom:
                maxHealth = baseHealth + (gear * 4);
                if (health < maxHealth * 0.75f)
                {
                    myState = State.Rest;
                    timeStamp = Time.time + (3f / timeScale);
                }
                else
                {
                    myState = State.Explore;
                }
                break;
            case SpaceBlockScript.BlockID.Loot:
                myState = State.Loot;
                timeStamp = Time.time + (2f / timeScale);
                break;
            case SpaceBlockScript.BlockID.Monster:
                myState = State.Attack;
                timeStamp = Time.time + (1.5f / timeScale);
                break;
        }

    }
    #endregion
    #region Update Stats
    void GearUp()
    {
        maxHealth = baseHealth + (gear * 4);
        health = maxHealth;
        dmg = baseDMG + (gear * 2);
    }
    void newGear()
    {
        money -= gearCost;
        gearCost += 50;
        gear++;
        GearUp();
    }
    void Sleep()
    {
        if (level % 3 == 0)
        {
            maxStamina += 10;
        }
        level++;
        health = maxHealth;
        stamina = maxStamina;
        myPath.Clear();
        lastBlock = currentBlock;
        locked = false;
        GameObject.FindGameObjectWithTag("Grid").GetComponent<GridManager>().RandomizeGrid();
    }
    #endregion
    #region Spawn
    // If no currentNode, find closest Node and set it as currentNode.
    public void Spawn()
    {
        if (currentBlock == null)
        {
            GameObject[] allBlocks = GameObject.FindGameObjectsWithTag("Block");

            currentBlock = allBlocks[0];
            for (int i = 1; i < allBlocks.Length; i++)
            {
                if (Vector3.Distance(transform.position, allBlocks[i].transform.position) < Vector3.Distance(transform.position, currentBlock.transform.position))
                {
                    currentBlock = allBlocks[i];
                }
            }
            transform.position = currentBlock.transform.position;
            targetBlock = currentBlock;
        }
    }
    #endregion
    #region GetBlockState
    void GetBlockState()
    {
        myBlockState = currentBlockScript.thisBlock;
    }
    #endregion
    #region All Movement
    #region Movement
    // Movement
    public void Movement()
    {
        // Update all nodes when I reach target node
        if (Vector3.Distance(transform.position, targetBlock.transform.position) < 0.1f)
        {
            #region Update Variables
            stamina--;
            if (stamina < 0)
            {
                stamina = 0;
            }
            else
            {
                if (!myPath.Contains(currentBlock))
                {
                    if (currentBlock != null)
                        myPath.Add(currentBlock);
                }
                if (!myPath.Contains(lastBlock))
                {
                    if (lastBlock != null)
                        myPath.Add(lastBlock);
                }
            }
            transform.position = targetBlock.transform.position;
            lastBlock = currentBlock;
            currentBlock = targetBlock;
            currentBlockScript = currentBlock.GetComponent<SpaceBlockScript>();
            //GetBlockState();
            #endregion
            if (stamina > 0)
            {
                MoveRandom();
            }
            else
            {
                // Code move back using MyPath
                // and maybe code it to take the shortest path to the beginning of the path only following the path.
                MoveBackHome();
            }
        }
        else
        {
            BetweenNodeMovement();
        }
    }
    #endregion
    #region Between Node Movement
    void BetweenNodeMovement()
    {
        transform.LookAt(targetBlock.transform.position);
        // move forward
        transform.Translate(Vector3.forward * Time.deltaTime * speed);

    }
    #endregion
    #region Move Randomly
    void MoveRandom()
    {
        if (destinationBlock == null)
        {
            destinationBlock = myPath[0];
        }
        int randBlock = Random.Range(0, currentBlockScript.connections.Count);
        if (myPath.Contains(currentBlockScript.connections[randBlock]))
        {
            randBlock = Random.Range(0, currentBlockScript.connections.Count);
        }
        for (int i = 0; i < currentBlockScript.connections.Count; i++)
        {
            if (i == randBlock)
            {
                targetBlock = currentBlockScript.connections[i];
            }
        }
        //foreach (GameObject connect in currentBlockScript.connections)
        //{
        //    if (!myPath.Contains(connect))
        //    {
        //        targetBlock = connect;
        //    }
        //}
    }
    #endregion
    #region Move Back Home
    void MoveBackHome()
    {
        foreach (GameObject connect in currentBlockScript.connections)
        {
            if (myPath.Contains(connect))
            {
                if (!locked)
                {
                    if (Vector3.Distance(transform.position, destinationBlock.transform.position) > Vector3.Distance(connect.transform.position, destinationBlock.transform.position))
                    {
                        targetBlock = connect;
                        //myPath.Remove(currentBlock);
                    }
                    else //if (connect != lastBlock)
                    {
                        //targetBlock = connect;
                        locked = true;
                        myPath.Reverse();
                    }
                }
                else
                {
                    if (myPath[0] == null)
                    {
                        myPath.Remove(myPath[0]);
                    }
                    targetBlock = myPath[0];
                    if (currentBlock == myPath[0])
                    {
                        myPath.Remove(currentBlock);
                    }
                    //if (Vector3.Distance(transform.position, myPath[0].transform.position) > Vector3.Distance(connect.transform.position, myPath[0].transform.position) && connect != lastBlock)
                    //{
                    //    targetBlock = connect;
                    //}
                }
            }
        }
    }
    #endregion
    #region GotHome
    bool GotHome()
    {
        return currentBlock == destinationBlock;
    }
    #endregion
    #endregion
    #region Gismos
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        foreach (GameObject block in myPath)
        {
            Gizmos.DrawSphere(block.transform.position + new Vector3(0, 5, 0), 0.5f);
        }
    }
    #endregion
    #region Randoms
    float RandomMobAttack()
    {
        return Random.Range(mobminAtk + (2 * level), mobmaxAtk + (2 * level));
    }
    void RandomMobHP()
    {
        mobHp = Random.Range(mobminHp + (5 * level), mobmaxHp + (5 * level));
    }
    float RandomMoney()
    {
        return Random.Range(minMoney + (20 * level), maxMoney + (20 * level));
    }
    #endregion
    #region Death
    void Death()
    {
        //kill the character
        currentBlockScript.DiedHere();
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        //Destroy(gameObject.transform.GetChild(0).gameObject);
        //gameObject.GetComponent<DungeonAI>().enabled = false;
        myState = State.Dead;
    }
    void Restart()
    {
        transform.position += new Vector3(0, 0, 100);
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        myState = State.Explore;
        maxStamina = 10;
        stamina = maxStamina;
        money = 0;
        once = true;
        myPath.Clear();
        Start();
    }
    void Immortality()
    {
        once = false;
    }
    #endregion
    #region GUI
    void UpdateBars()
    {
        healthBar.value = health;
        healthBar.maxValue = maxHealth;
        staminaBar.value = stamina;
        staminaBar.maxValue = maxStamina;
        if (stamina <= 0 || myState == State.Dead)
        {
            staminaBar.fillRect.gameObject.SetActive(false);
        }
        else
        {
            staminaBar.fillRect.gameObject.SetActive(true);
        }
        if (health <= 0 || myState == State.Dead)
        {
            healthBar.fillRect.gameObject.SetActive(false);
        }
        else
        {
            healthBar.fillRect.gameObject.SetActive(true);
        }
        timeScale = timeSlider.value + 1;
    }
    void TextUpdate()
    {
        textState.text = "State: " + myState.ToString();
        moneyText.text = "$: " + ((int)money).ToString();
        timeText.text = "Time Scale: " + timeScale.ToString();
        levelText.text = "Level: " + level.ToString();
    }
    void DamageBar()
    {

    }
    #endregion
}
