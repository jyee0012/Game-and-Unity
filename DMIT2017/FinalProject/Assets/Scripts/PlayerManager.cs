using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance = null;
    [Header("Player Management")]
    [SerializeField]
    List<PlayerTextController> playerTextList = new List<PlayerTextController>();
    [SerializeField]
    [Tooltip("[0] = Front Paddle, [1] = Back Paddle, [2] = left Paddle, [3] = Right Paddle")]
    List<GameObject> playerPaddleList = new List<GameObject>();
    public bool[] activePlayers;
    public bool useScore = false, useLives = false, setupPlayers = false, checkWin = true;
    public int maxScore = 20, lifeCount = 10;
    [SerializeField]
    Slider scoreLimitSlider = null, lifeLimitSlider = null;
    [SerializeField]
    Text scoreLimitText = null, lifeLimitText = null;


    [Header("Ball Settings")]
    [Tooltip("[0] = chaotic balls, [1] = lame balls, [2] = normal balls")]
    public SpawnerScript[] ballSpawner = new SpawnerScript[3];
    [Tooltip("[0] = chaotic balls, [1] = lame balls, [2] = normal balls")]
    public int[] ballAmount = new int[3];
    // [0] = chaotic balls, [1] = lame balls, [2] = normal balls
    [SerializeField]
    [Tooltip("[0] = chaotic balls, [1] = lame balls, [2] = normal balls")]
    Slider[] ballSlider = new Slider[3];
    [SerializeField]
    [Tooltip("[0] = chaotic balls, [1] = lame balls, [2] = normal balls")]
    Text[] ballText= new Text[3];

    [Header("Menus")]
    [SerializeField]
    bool canPause = true;
    [SerializeField]
    KeyCode pauseBtn = KeyCode.Escape;
    public bool paused = false;
    bool showControls = true, showEndMenu = false, showOptions = false;
    [SerializeField]
    GameObject startMenu = null, pauseMenu = null, endMenu = null, optionsMenu = null, controlsMenu = null;
    [SerializeField]
    Text endingText = null;
    [SerializeField]
    float pauseDelay = 1f;
    float pauseTimeStamp = 0;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this)
        {
            // transfering data
            activePlayers = Instance.activePlayers;
            ballAmount = Instance.ballAmount;
            useLives = Instance.useLives;
            useScore = Instance.useScore;
            maxScore = Instance.maxScore;
            lifeCount = Instance.lifeCount;

            Destroy(Instance.gameObject);
            Instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        TogglePause();
        ToggleOptionsMenu();
        ToggleEndMenu();
        UpdateBallUI();
        UpdatePlayerWinUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (canPause && (Input.GetKeyDown(pauseBtn) || Input.GetAxis("Button7") > 0) && pauseTimeStamp < Time.time)
        {
            TogglePause(true);
            pauseTimeStamp = Time.time + pauseDelay;
        }
        if (setupPlayers)
        {
            SetPlayers();
            for (int i = 0; i < ballSpawner.Length; i++)
            {
                SetupBallSpawners(ref ballSpawner[i], ballAmount[i]);
            }
            setupPlayers = false;
        }
        if (checkWin)
        {
            if (CheckPlayerWin() && !endMenu.activeInHierarchy)
            {
                ToggleEndMenu(true);
            }
        }
    }
    #region Game Setup
    public void SetupThenPlayGame(int sceneIndex)
    {
        GrabActivePlayers();
        GrabBallSettings();
        GrabPlayerWinSettings();
        LoadScene(sceneIndex);
    }
    void GrabActivePlayers()
    {
        activePlayers = new bool[playerTextList.Count];
        int activePlayerCount = 0;
        for (int i = 0; i < playerTextList.Count; i++)
        {
            activePlayers[i] = playerTextList[i].activePlayer;
            if (playerTextList[i].activePlayer)
            {
                activePlayerCount++;
            }
        }
        Debug.Log("Active Players: " + activePlayerCount);
    }
    void GrabBallSettings()
    {
        if (ballSlider.Length > 0)
        {
            for(int i = 0; i< ballSlider.Length; i++)
            {
                ballAmount[i] = (int)ballSlider[i].value;
            }
        }
    }
    void GrabPlayerWinSettings()
    {
        if (lifeLimitSlider != null)
        {
            lifeCount = (int)lifeLimitSlider.value;
        }
        if (scoreLimitSlider != null)
        {
            maxScore = (int)scoreLimitSlider.value;
        }
    }
    void SetPlayers()
    {
        for (int i = 0; i < playerPaddleList.Count; i++)
        {
            PaddleScript playerPaddle = playerPaddleList[i].GetComponent<PaddleScript>();
            if (playerPaddle != null)
            {
                playerPaddle.activePlayer = activePlayers[i];
                playerPaddle.useLives = useLives;
                playerPaddle.useScore = useScore;
                playerPaddle.playerLives = lifeCount;
                playerPaddle.UpdateText();
            }
        }
    }
    void SetupBallSpawners(ref SpawnerScript ballSpawner, int spawnLimit = 1)
    {
        if (ballSpawner != null)
        {
            ballSpawner.endlessSpawn = true;
            ballSpawner.maxSpawnAmount = spawnLimit;
        }
    }
    #endregion
    bool CheckPlayerWin()
    {
        bool playerWin = false;
        if (useLives)
        {
            int livePlayerCount = 0;
            foreach(GameObject player in playerPaddleList)
            {
                PaddleScript playerPaddle = player.GetComponent<PaddleScript>();
                if (playerPaddle != null)
                {
                    if (playerPaddle.activePlayer) livePlayerCount++;
                    endingText.text = "Winner: Player " + playerPaddle.playerNum;
                }
            }
            playerWin = livePlayerCount == 1;
        }
        if (useScore)
        {
            foreach (GameObject player in playerPaddleList)
            {
                PaddleScript playerPaddle = player.GetComponent<PaddleScript>();
                if (playerPaddle != null)
                {
                    playerWin = playerPaddle.playerScore >= maxScore;
                    endingText.text = "Winner: Player " + playerPaddle.playerNum;
                }
            }
        }
        return playerWin;
    }
    #region Game Control
    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    #endregion
    #region UI Control
    public void TogglePause(bool togglePause = false)
    {
        if (togglePause) paused = !paused;
        Time.timeScale = paused ? 0 : 1;
        if (pauseMenu != null) pauseMenu.SetActive(paused);
    }
    public void ToggleMenu(ref GameObject menu, bool toggle = false)
    {
        if (menu == null) return;
        bool menuActive = menu.activeInHierarchy;
        if (toggle) menuActive = !menuActive;
        menu.SetActive(menuActive);
    }
    public void ToggleOptionsMenu(bool toggleOptions = false)
    {
        if (toggleOptions) showOptions = !showOptions;
        if (optionsMenu != null) optionsMenu.SetActive(showOptions);
    }
    public void ToggleEndMenu(bool toggleEnd = false)
    {
        if (toggleEnd) showEndMenu = !showEndMenu;
        Time.timeScale = showEndMenu ? 0 : 1;
        if (endMenu != null) endMenu.SetActive(showEndMenu);
    }
    public void ToggleControls(bool toggleControls = false)
    {
        if (toggleControls) showControls = !showControls;
        if (controlsMenu != null) controlsMenu.SetActive(showControls);
    }
    public void UpdateBallUI()
    {
        if (ballText[0] == null || ballText[1] == null || ballText[2] == null) return;
        //[0] = chaotic balls, [1] = lame balls, [2] = normal balls
        ballText[0].text = "Chaotic Ball Amount: ";
        ballText[1].text = "Lame Ball Amount: ";
        ballText[2].text = "Normal Ball Amount: ";
        if (ballSlider.Length > 0)
        {
            for (int i = 0; i < ballSlider.Length; i++)
            {
                ballText[i].text += (int)ballSlider[i].value;
            }
        }
    }
    public void UpdatePlayerWinUI()
    {
        if (scoreLimitText != null && scoreLimitSlider != null)
        {
            scoreLimitText.text = "Score Limit: " + scoreLimitSlider.value;
        }
        if (lifeLimitText != null && lifeLimitSlider != null)
        {
            lifeLimitText.text = "Starting Lives: " + lifeLimitSlider.value;
        }
    }
    public void ToggleUseScoreLimit()
    {
        useScore = !useScore;
    }
    public void ToggleUseLifeCount()
    {
        useLives = !useLives;
    }
    #endregion
}
