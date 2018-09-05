///////////////////////////////////////////////////////////////////////////////////////////////
//
// This script allows for two players to take turns playing a dice game
//
// Created By: John Winski
//
// Last Modified: June 2, 2017
//
///////////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DiceGame : MonoBehaviour
{
    int numTimesRolled; // This is the number of times that the player has rolled in a single turn.
    int playerTurn; // This determines which player's turn it is.
    int winner; // This holds the value of the player that has won.
    bool diceRolling; // This bool is true when the dice are rolling.
    bool diceChecked; // This bool is true if the dice have been check for combos.
    int randomValue;
    bool debugMode;

    public Button submitButton;

    #region Combo Identifiers
    int onePair; // This stores the dieValue of the first pair found.
    int twoPair; // This stores the dieValue of the second pair found.
    int threeKind; // This stores the dieValue that appears at least 3 times.
    int fourKind; // This stores the dieValue that appears at least 4 times.
    int smallStraight; // This stores the largest dieValue included in the straight.
    int largeStraight; // This stores the largest dieValue included in the straight.

    int numInStraight;
    #endregion

    #region Combo lists
    public PlayerCombo[] p1Combos; // This is an array of the combo information for player 1.
    public PlayerCombo[] p2Combos; // This is an array of the combo information for player 2.
    PlayerCombo[][] pCombos;
    #endregion

    #region Lists for the die values, totals and keep bools
    int[] dieValue; // This is the current value of each of the dice.
    public int[] diceTotals; // This is the current totals of the diceValues that were rolled.
    bool[] keep; // This is an array of which dice are currently being kept.
    // dieValue and keep array share index numbers
    // diceTotals is the amount of certain numbers
    #endregion

    #region GUI Variables and Lists
    public Image[] diceImages;
    public Sprite[] diceSprites;
    public Button[] keepButtons;
    public Button rollButton;
    public Text playerText;
    #endregion

    #region Variables to handle the rolling of the dice
    float rollTime = 0.5f;
    float rollStamp;
    #endregion

    #region AI Variables
    enum AIState { Roll, Wait, Check, Done }
    new AIState ai = AIState.Roll;
    int straightGap = 0, runTwo = -1, runThree = -1, player;
    bool ActiveAI1 = false, ActiveAI2 = false;
    #endregion

    #region Start
    // Initialization of variables
    void Start()
    {
        numTimesRolled = 0;
        playerTurn = 1;
        winner = 0;

        playerText.text = "Player 1";
        playerText.color = Color.green;

        dieValue = new int[5];
        diceTotals = new int[6];
        keep = new bool[5] { false, false, false, false, false };

        diceRolling = false;
        diceChecked = true;

        debugMode = false;

        onePair = -1;
        twoPair = -1;
        threeKind = -1;
        fourKind = -1;
        smallStraight = -1;
        largeStraight = -1;

        pCombos = new PlayerCombo[2][];
        pCombos[0] = p1Combos;
        pCombos[1] = p2Combos; 

    }
    #endregion

    #region Update
    // Update is called once per frame
    void Update()
    {
        #region Debug Mode
        // If debug mode in on allow the user to change the die values using the number keys from 1 - 5
        if (debugMode)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                dieValue[0]++;
                if (dieValue[0] > 5)
                {
                    dieValue[0] = 0;
                }
                diceImages[0].sprite = diceSprites[dieValue[0]];
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                dieValue[1]++;
                if (dieValue[1] > 5)
                {
                    dieValue[1] = 0;
                }
                diceImages[1].sprite = diceSprites[dieValue[1]];
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                dieValue[2]++;
                if (dieValue[2] > 5)
                {
                    dieValue[2] = 0;
                }
                diceImages[2].sprite = diceSprites[dieValue[2]];
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                dieValue[3]++;
                if (dieValue[3] > 5)
                {
                    dieValue[3] = 0;
                }
                diceImages[3].sprite = diceSprites[dieValue[3]];
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                dieValue[4]++;
                if (dieValue[4] > 5)
                {
                    dieValue[4] = 0;
                }
                diceImages[4].sprite = diceSprites[dieValue[4]];
            }
        }
        #endregion
        player = playerTurn - 1;
        if (playerTurn == 1 && ActiveAI1)
        {
            AIPlayer();
        }
        else if (playerTurn == 2 && ActiveAI2)
        {
            AIPlayer();
        }
        if (diceRolling)
        {
            if (!debugMode)
            {
                // Pick a random number for each die.
                for (int i = 0; i < 5; i++)
                {
                    if (!keep[i])
                    {
                        randomValue = Random.Range(0, 6);
                        dieValue[i] = randomValue;
                        diceImages[i].sprite = diceSprites[randomValue];
                    }
                }

                // End the rolling after the rollTime is up.
                if (Time.time > rollStamp + rollTime)
                {
                    diceRolling = false;
                    rollButton.gameObject.SetActive(true);
                    numTimesRolled++;
                    if (numTimesRolled == 2)
                    {
                        rollButton.GetComponentInChildren<Text>().text = "Done";
                    }
                }
            }
        }
        else
        {
            // If the dice have been rolled once and the keep buttons are currently inactive.
            if (numTimesRolled == 1 && keepButtons[0].gameObject.activeSelf == false)
            {
                // Activate all keep buttons and set to Roll.
                for (int i = 0; i < 5; i++)
                {
                    keepButtons[i].gameObject.SetActive(true);
                    keepButtons[i].GetComponentInChildren<Text>().text = "Roll";
                }
            }
            // Check the dice after each roll.
            if (!diceChecked)
            {
                CheckDice();
            }
        }
    }
    #endregion

    #region Game Methods
    public void RollDice()
    {
        #region Reset combo identifiers and buttons every time the dice are rolled.
        onePair = -1;
        twoPair = -1;
        threeKind = -1;
        fourKind = -1;
        smallStraight = -1;
        largeStraight = -1;

        ResetComboButtons();
        #endregion

        // If someone wins reset all variables and start a new game.
        if (winner > 0)
        {
            numTimesRolled = 0;
            playerTurn = 1;
            winner = 0;
            playerText.text = "Player 1";
            playerText.color = Color.green;
            dieValue = new int[5];
            diceTotals = new int[6];
            keep = new bool[5] { false, false, false, false, false };
            diceRolling = false;
            diceChecked = true;

            for (int i = 0; i < p1Combos.Length; i++)
            {
                p1Combos[i].SetSelected(false);
                p2Combos[i].SetSelected(false);
            }

            ResetComboButtons();
            rollButton.GetComponentInChildren<Text>().text = "Roll Dice";
        }
        else
        {
            // If the dice are not yet rolling and the player has rolled the dice less than two times.
            if (!diceRolling && numTimesRolled < 2)
            {
                diceRolling = true;
                rollStamp = Time.time;
                diceChecked = false;

                // Deactivate all keep and roll buttons.
                rollButton.gameObject.SetActive(false);

                for (int i = 0; i < 5; i++)
                {
                    keepButtons[i].gameObject.SetActive(false);
                }
            }
            // If the dice are done rolling and the player has rolled two times then the turn is over.
            if (!diceRolling && numTimesRolled == 2)
            {
                ResetComboButtons();

                if (playerTurn == 1)
                {
                    playerTurn = 2;
                    playerText.text = "Player 2";
                    playerText.color = Color.red;
                }
                else
                {
                    playerTurn = 1;
                    playerText.text = "Player 1";
                    playerText.color = Color.green;
                }
                numTimesRolled = 0;
                rollButton.GetComponentInChildren<Text>().text = "Roll Dice";

                // Reset all keep variables.
                for (int i = 0; i < 5; i++)
                {
                    keep[i] = false;
                }
            }
        }
    }

    public void debugToggle()
    {
        // Toggle debug mode.
        if (debugMode)
        {
            submitButton.gameObject.SetActive(false);
        }
        else
        {
            submitButton.gameObject.SetActive(true);
        }
        debugMode = !debugMode;
    }

    public void submitChanges()
    {
        if (diceRolling)
        {
            diceRolling = false;
            rollButton.gameObject.SetActive(true);
            numTimesRolled++;
            if (numTimesRolled == 2)
            {
                rollButton.GetComponentInChildren<Text>().text = "Done";
            }
        }
    }

    public void keepToggle(int index)
    {
        // Toggle keep buttons when pressed.
        if (keep[index])
        {
            keep[index] = false;
            keepButtons[index].GetComponentInChildren<Text>().text = "Roll";
        }
        else
        {
            keep[index] = true;
            keepButtons[index].GetComponentInChildren<Text>().text = "Keep";
        }
    }

    void CheckDice()
    {
        #region Reset straight check and dicTotals.
        numInStraight = 0;

        for (int i = 0; i < 6; i++)
        {
            diceTotals[i] = 0;
        }
        #endregion

        #region Add diceValues to the diceTotals.
        for (int i = 0; i < 5; i++)
        {
            diceTotals[dieValue[i]]++;
        }
        #endregion

        for (int i = 0; i < 6; i++)
        {
            // For each diceTotal that is more than 1 add to the straight check until a diceTotal of zero is found.
            // That will tell you how if you have four or five numbers in a row.
            if (diceTotals[i] > 0)
            {
                numInStraight++;
                if (numInStraight == 2)
                {
                    runTwo = i;
                }
                else if (numInStraight == 3)
                {
                    runThree = i;
                }
                else if (numInStraight == 4)
                {
                    smallStraight = i;
                }
                else if (numInStraight == 5)
                {
                    largeStraight = i;
                }
            }
            else
            {
                straightGap++;
                numInStraight = 0;
            }

            // If a diceTotal is equal to two and you have not yet identified a pair.
            if (diceTotals[i] == 2 && onePair == -1)
            {
                onePair = i;
            }
            // If a diceTotal is equal to two and you have identified a pair.
            // Check to make sure the pairs are not the same diceValue.
            if (diceTotals[i] == 2 && onePair > -1 && onePair != i)
            {
                twoPair = i;
            }
            // If a diceTotal is greater than two you have three of a kind.
            if (diceTotals[i] > 2)
            {
                threeKind = i;
            }
            // If a diceTotal is greater than three you have four of a kind.
            if (diceTotals[i] > 3)
            {
                fourKind = i;
            }
        }

        #region Check/Allow Combo
        // If a pair has been found along with either a second pair or three of a kind.
        // Then allow selection of the two pair button.
        if (twoPair > -1 || (onePair > -1 && threeKind > -1))
        {
            if (playerTurn == 1)
            {
                if (!p1Combos[0].GetSelected())
                {
                    p1Combos[0].buttonReference.gameObject.SetActive(true);
                    p1Combos[0].buttonReference.GetComponentInChildren<Text>().text = "Select";
                }
            }
            else
            {
                if (!p2Combos[0].GetSelected())
                {
                    p2Combos[0].buttonReference.gameObject.SetActive(true);
                    p2Combos[0].buttonReference.GetComponentInChildren<Text>().text = "Select";
                }
            }
        }

        // If a three of a kind has been found then allow selection of the three of a kind button.
        if (threeKind > -1)
        {
            if (playerTurn == 1)
            {
                if (!p1Combos[1].GetSelected())
                {
                    p1Combos[1].buttonReference.gameObject.SetActive(true);
                    p1Combos[1].buttonReference.GetComponentInChildren<Text>().text = "Select";
                }
            }
            else
            {
                if (!p2Combos[1].GetSelected())
                {
                    p2Combos[1].buttonReference.gameObject.SetActive(true);
                    p2Combos[1].buttonReference.GetComponentInChildren<Text>().text = "Select";
                }
            }
        }

        // If a four of a kind has been found then allow selection of the four of a kind button.
        if (fourKind > -1)
        {
            if (playerTurn == 1)
            {
                if (!p1Combos[2].GetSelected())
                {
                    p1Combos[2].buttonReference.gameObject.SetActive(true);
                    p1Combos[2].buttonReference.GetComponentInChildren<Text>().text = "Select";
                }
            }
            else
            {
                if (!p2Combos[2].GetSelected())
                {
                    p2Combos[2].buttonReference.gameObject.SetActive(true);
                    p2Combos[2].buttonReference.GetComponentInChildren<Text>().text = "Select";
                }
            }
        }

        // If a three of a kind and a pair has been found then allow selection of the full house button.
        if (threeKind > -1 && onePair > -1)
        {
            if (playerTurn == 1)
            {
                if (!p1Combos[3].GetSelected())
                {
                    p1Combos[3].buttonReference.gameObject.SetActive(true);
                    p1Combos[3].buttonReference.GetComponentInChildren<Text>().text = "Select";
                }
            }
            else
            {
                if (!p2Combos[3].GetSelected())
                {
                    p2Combos[3].buttonReference.gameObject.SetActive(true);
                    p2Combos[3].buttonReference.GetComponentInChildren<Text>().text = "Select";
                }
            }
        }

        // If a small straight has been found then allow selection of the small straight button.
        if (smallStraight > -1)
        {
            if (playerTurn == 1)
            {
                if (!p1Combos[4].GetSelected())
                {
                    p1Combos[4].buttonReference.gameObject.SetActive(true);
                    p1Combos[4].buttonReference.GetComponentInChildren<Text>().text = "Select";
                }
            }
            else
            {
                if (!p2Combos[4].GetSelected())
                {
                    p2Combos[4].buttonReference.gameObject.SetActive(true);
                    p2Combos[4].buttonReference.GetComponentInChildren<Text>().text = "Select";
                }
            }
        }

        // If a large straight has been found then allow selection of the large straight button.
        if (largeStraight > -1)
        {
            if (playerTurn == 1)
            {
                if (!p1Combos[5].GetSelected())
                {
                    p1Combos[5].buttonReference.gameObject.SetActive(true);
                    p1Combos[5].buttonReference.GetComponentInChildren<Text>().text = "Select";
                }
            }
            else
            {
                if (!p2Combos[5].GetSelected())
                {
                    p2Combos[5].buttonReference.gameObject.SetActive(true);
                    p2Combos[5].buttonReference.GetComponentInChildren<Text>().text = "Select";
                }
            }
        }
        diceChecked = true; // Identify that the dice have been checked for combos this roll.
        #endregion
    }

    public void SelectCombo(int index)
    {
        // Check to see which player selected the combo.
        if (playerTurn == 1)
        {
            p1Combos[index].SetSelected(true);
        }
        else
        {
            p2Combos[index].SetSelected(true);
        }

        // End the current player's turn.
        numTimesRolled = 2;
        rollButton.GetComponentInChildren<Text>().text = "Done";
        ResetComboButtons();

        // Deactivate all keep buttons.
        for (int i = 0; i < 5; i++)
        {
            keepButtons[i].gameObject.SetActive(false);
        }

        // Check to see if a player has won.
        CheckForWin();

        if (winner > 0)
        {
            rollButton.GetComponentInChildren<Text>().text = "Play Again";
        }
    }

    void ResetComboButtons()
    {
        // Hide all combo buttons that have not been selected and set all the rest to say done and not be interactable.
        for (int i = 0; i < p1Combos.Length; i++)
        {
            if (!p1Combos[i].GetSelected())
            {
                p1Combos[i].buttonReference.gameObject.SetActive(false);
                p1Combos[i].buttonReference.interactable = true;
            }
            else
            {
                p1Combos[i].buttonReference.GetComponentInChildren<Text>().text = "Done";
                p1Combos[i].buttonReference.interactable = false;
            }
        }

        for (int i = 0; i < p2Combos.Length; i++)
        {
            if (!p2Combos[i].GetSelected())
            {
                p2Combos[i].buttonReference.gameObject.SetActive(false);
                p2Combos[i].buttonReference.interactable = true;
            }
            else
            {
                p2Combos[i].buttonReference.GetComponentInChildren<Text>().text = "Done";
                p2Combos[i].buttonReference.interactable = false;
            }
        }
    }

    void CheckForWin()
    {
        bool possibleWin1 = true;
        bool possibleWin2 = true;

        // If any player has a combo not selected then that player has not won.
        for (int i = 0; i < p1Combos.Length; i++)
        {
            if (!p1Combos[i].GetSelected())
            {
                possibleWin1 = false;
            }
        }
        for (int i = 0; i < p2Combos.Length; i++)
        {
            if (!p2Combos[i].GetSelected())
            {
                possibleWin2 = false;
            }
        }

        if (possibleWin1)
        {
            winner = 1;
            playerText.GetComponentInChildren<Text>().text = "Player 1 Wins!";
            playerText.color = Color.blue;
        }
        else if (possibleWin2)
        {
            winner = 2;
            playerText.GetComponentInChildren<Text>().text = "Player 2 Wins!";
            playerText.color = Color.blue;
        }
    }
    #endregion

    public void ToggleAI1()
    {
        ActiveAI1 = !ActiveAI1;
    }
    public void ToggleAI2()
    {
        ActiveAI2 = !ActiveAI2;
    }
    public void TimerChange()
    {
        float scrollValue = GameObject.Find("TimerBar").GetComponent<Scrollbar>().value + 1f;
        rollTime = 0.5f * scrollValue;
    }

    void AIPlayer()
    {

        switch (ai)
        {
            #region AI Wait
            case AIState.Wait:
                #region Lock Buttons
                rollButton.interactable = false;
                for (int i = 0; i < 5; i++)
                {
                    keepButtons[i].interactable = false;
                    if (playerTurn == 1)
                    {
                        p1Combos[i].buttonReference.interactable = false;
                    }
                    else
                    {
                        p2Combos[i].buttonReference.interactable = false;
                    }
                }
                #endregion
                if (!diceRolling && Time.time > rollStamp + (rollTime * 3))
                {
                    ai = AIState.Check;
                    #region Release Buttons
                    rollButton.interactable = true;
                    for (int i = 0; i < 5; i++)
                    {
                        keepButtons[i].interactable = true;
                        if (playerTurn == 1)
                        {
                            p1Combos[i].buttonReference.interactable = true;
                        }
                        else
                        {
                            p2Combos[i].buttonReference.interactable = true;
                        }
                    }
                    #endregion
                }
                break;
            #endregion
            #region AI Roll
            case AIState.Roll:
                RollDice();
                ai = AIState.Wait;
                break;
            #endregion
            #region AI Check
            case AIState.Check:
                #region Top Priority
                if ((pCombos[player][5].buttonReference.IsActive() && !pCombos[player][5].selected)
                    || (pCombos[player][4].buttonReference.IsActive() && !pCombos[player][4].selected)
                    || (pCombos[player][3].buttonReference.IsActive() && !pCombos[player][3].selected)
                    || (pCombos[player][2].buttonReference.IsActive() && !pCombos[player][2].selected))
                {
                    #region Having Straights
                    // if you have large straight, take it
                    if (pCombos[player][5].buttonReference.IsActive() && !pCombos[player][5].selected)
                    {
                        SelectCombo(5);
                    }
                    // if I have a small straight, and I have yet to roll my second time
                    if ((pCombos[player][4].buttonReference.IsActive() && numTimesRolled < 2) && !pCombos[player][5].selected)
                    {
                        int straightStart = smallStraight - 3;
                        for (int i = 0; i < 5; i++)
                        {
                            if (dieValue[i] == straightStart)
                            {
                                keep[i] = true;
                                straightStart++;
                                i = 0;
                            }
                        }
                    }
                    // If I have a small straight, and I have already rolled twice, take it
                    else if ((pCombos[player][4].buttonReference.IsActive() && numTimesRolled == 2) && !pCombos[player][4].selected)
                    {
                        SelectCombo(4);
                    }
                    #endregion
                    #region Having Certain Combos
                    // If I have a full house, take it
                    if (pCombos[player][3].buttonReference.IsActive() && !pCombos[player][3].selected)
                    {
                        SelectCombo(3);
                    }
                    // else if I have a four of a kind, take it
                    else if (pCombos[player][2].buttonReference.IsActive() && !pCombos[player][2].selected)
                    {
                        SelectCombo(2);
                    }
                    #endregion
                }
                #endregion
                else
                {
                    #region Full House / Four of a Kind
                    // If I have three if a kind or two pairs
                    if ((threeKind > -1 || twoPair > -1) && (!pCombos[player][3].selected || !pCombos[player][2].selected))
                    {
                        // if I am missing a four of a kind
                        if (!pCombos[player][2].selected && numTimesRolled < 2)
                        {
                            // I have a three of a kind 
                            if (threeKind > -1)
                            {
                                for (int i = 0; i < 5; i++)
                                {
                                    if (dieValue[i] == threeKind)
                                    {
                                        keep[i] = true;
                                    }
                                }
                            }
                            else if (onePair > -1)
                            {
                                for (int i = 0; i < 5; i++)
                                {
                                    if (dieValue[i] == onePair)
                                    {
                                        keep[i] = true;
                                    }
                                }
                            }
                        }
                        // Do I have full house, three of a kind, or two pairs, and I have yet to roll my second time
                        if ((!pCombos[player][3].selected || !pCombos[player][1].selected || !pCombos[player][0].selected) && numTimesRolled < 2)
                        {
                            // check what do I have

                            // I have a three of a kind 
                            if (threeKind > -1)
                            {
                                for (int i = 0; i < 5; i++)
                                {
                                    if (dieValue[i] == threeKind)
                                    {
                                        keep[i] = true;
                                    }
                                }
                            }
                            // else I have two pairs
                            else if (twoPair > -1)
                            {
                                for (int i = 0; i < 5; i++)
                                {
                                    if (dieValue[i] == twoPair || dieValue[i] == onePair)
                                    {
                                        keep[i] = true;
                                    }
                                }
                            }
                            // else I have one pair
                            else if (onePair > -1)
                            {
                                for (int i = 0; i < 5; i++)
                                {
                                    if (dieValue[i] == onePair)
                                    {
                                        keep[i] = true;
                                    }
                                }
                            }
                        }
                        // If I don't have a full house, three of a kind, or two pairs, and I have rolled twice
                        else if ((!pCombos[player][1].selected || !pCombos[player][0].selected) && numTimesRolled == 2)
                        {
                            // if I don't have these combos, and I have rolled twice, take them
                            if (pCombos[player][1].buttonReference.IsActive() && !pCombos[player][1].selected)
                            {
                                SelectCombo(1);
                            }
                            else if (pCombos[player][0].buttonReference.IsActive() && !pCombos[player][0].selected)
                            {
                                SelectCombo(0);
                            }
                        }
                    }
                    #endregion
                    // If I am missing any straight
                    if (!pCombos[player][5].selected || !pCombos[player][4].selected && runThree > -1 && numTimesRolled < 2)
                    {

                        int straightStart;
                        if (runThree > -1)
                        {
                            straightStart = runThree - 2;
                            for (int i = 0; i < 5; i++)
                            {
                                if (dieValue[i] == straightStart)
                                {
                                    keep[i] = true;
                                    straightStart++;
                                    i = 0;
                                }
                            }
                        }
                    }
                    // If I am done everything, but straights
                    else if (pCombos[player][0].selected && pCombos[player][1].selected && pCombos[player][2].selected && pCombos[player][3].selected)
                    {
                        int straightStart;
                        if (runTwo > -1)
                        {
                            straightStart = runTwo - 1;
                            for (int i = 0; i < 5; i++)
                            {
                                if (dieValue[i] == straightStart)
                                {
                                    keep[i] = true;
                                    straightStart++;
                                    i = 0;
                                }
                            }
                        }
                    }
                }
                #region Basic AI
                // Basic AI: If you have a combo, take it
                /*
                if (playerTurn == 1)
                {
                    if (p1Combos[5].buttonReference.IsActive() && !p1Combos[5].selected)
                        SelectCombo(5);
                    else if (p1Combos[4].buttonReference.IsActive() && !p1Combos[4].selected)
                        SelectCombo(4);
                    else if (p1Combos[3].buttonReference.IsActive() && !p1Combos[3].selected)
                        SelectCombo(3);
                    else if (p1Combos[2].buttonReference.IsActive() && !p1Combos[2].selected)
                        SelectCombo(2);
                    else if (p1Combos[1].buttonReference.IsActive() && !p1Combos[1].selected)
                        SelectCombo(1);
                    else if (p1Combos[0].buttonReference.IsActive() && !p1Combos[0].selected)
                        SelectCombo(0);
                    else //Reroll
                        ai = AIState.Roll;
                }
                else
                {
                    if (p2Combos[5].buttonReference.IsActive() && !p2Combos[5].selected)
                        SelectCombo(5);
                    else if (p2Combos[4].buttonReference.IsActive() && !p2Combos[4].selected)
                        SelectCombo(4);
                    else if (p2Combos[3].buttonReference.IsActive() && !p2Combos[3].selected)
                        SelectCombo(3);
                    else if (p2Combos[2].buttonReference.IsActive() && !p2Combos[2].selected)
                        SelectCombo(2);
                    else if (p2Combos[1].buttonReference.IsActive() && !p2Combos[1].selected)
                        SelectCombo(1);
                    else if (p2Combos[0].buttonReference.IsActive() && !p2Combos[0].selected)
                        SelectCombo(0);
                    else //Reroll
                        ai = AIState.Roll;
                }
                */
                #endregion
                if (numTimesRolled < 2)
                {
                    ai = AIState.Roll;
                }
                else if (numTimesRolled == 2)
                {
                    ai = AIState.Done;
                }
                break;
            #endregion
            #region AI Done
            case AIState.Done:
                RollDice();
                ai = AIState.Roll;
                break;
            #endregion
            default:
                break;
        }
    }
}

// Must be set to serializable to show up in the inspector.
[System.Serializable]
public class PlayerCombo
{
    public bool selected = false;
    public Button buttonReference;

    public bool GetSelected()
    {
        return selected;
    }

    public void SetSelected(bool value)
    {
        selected = value;
    }
}
