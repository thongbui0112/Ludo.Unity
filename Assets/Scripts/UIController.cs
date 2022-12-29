using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIController : MonoBehaviour
{
//    [SerializeField] private Scene scene;
    [SerializeField] private GameController gameController;
    [SerializeField] private ThrowDice throwDice;
    [SerializeField] private GamePlay gamePlay;
    [SerializeField] private GameState gameState;
    [SerializeField] private SaveDataGame gameData;
    [SerializeField] private CreateBoard createBoard;

    private int timeScaler;
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject playGamePanel;
    [SerializeField] private GameObject howToPlayPanel;
    [SerializeField] private GameObject creditPanel;
    [SerializeField] private GameObject newGamePanel;
    [SerializeField] private GameObject inGamePanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private int multiplierTimscale;

    [SerializeField] private GameObject cannotSaveGamePanel;
    [SerializeField] private GameObject cannotLoadGame;
    [SerializeField] private GameObject chooseYourTeamText;
    [SerializeField] private GameObject chooseEnemyText;
    [SerializeField] private SpriteRenderer[] chooseSpriteRenderer;
   // [SerializeField] private GameObject[] chooseGamObject;
    [SerializeField] private Sprite playerSprite, computerSprite;
    [SerializeField] private bool chooseYourTeam, chooseEnemy;
    [SerializeField] private bool[] canChooseButton;
    [SerializeField] int numberPlayer;
    [SerializeField] GameObject startGameButton;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] private GameObject knightCages;
    [SerializeField] private GameObject board;
    [SerializeField] private GameObject audioMenuG;
    [SerializeField] private Slider volumeSlider;
    int turnOfPlayer;
    bool autoPlay;
    private void Start()
    {
        this.autoPlay = true;
        if (!PlayerPrefs.HasKey("volume"))
        {
            PlayerPrefs.SetFloat("volume", 1);
            volumeSlider.value = PlayerPrefs.GetFloat("volume");
        }
        else
        {
            volumeSlider.value = PlayerPrefs.GetFloat("volume");
        }
        this.numberPlayer = 0;
        this.timeScaler = 1;
        this.mainMenuPanel.SetActive(true);
        this.chooseYourTeam = true;
        this.chooseEnemy = false;
        this.SetStartCanChooseButton();
    }
    public void SetStartCanChooseButton()
    {
        this.canChooseButton = new bool[4];
        for(int i = 0; i < 4; i++)
        {
            this.canChooseButton[i] = true;
        }
    }
    #region MainMenu 
    public void PlayGame_Button()
    {
        this.mainMenuPanel.SetActive(false);
        this.playGamePanel.SetActive(true);
    }
    public void Howtoplay_Button()
    {
        this.mainMenuPanel.SetActive(false);
        this.howToPlayPanel.SetActive(true);
    }
    public void Credit_Button()
    {
        this.mainMenuPanel.SetActive(false);
        this.creditPanel.SetActive(true);
    }
    public void ExitGame_Button()
    {
        Application.Quit();
    }
    #endregion MainMenu 


    #region PlayGame Panel
    public void NewGame_Button()
    {
        this.cannotLoadGame.SetActive(false);
        this.newGamePanel.SetActive(true);
        this.playGamePanel.SetActive(false);
        this.ChooseAgainButton();    }
    public void LoadGame_Button()
    {
        if (PlayerPrefs.HasKey("LoadedData"))
        {
            this.StartLoadGame();
            this.createBoard.SetLoadGame();
            this.knightCages.transform.parent = null;
            this.gameData.LoadBoardData();
            this.knightCages.transform.parent = this.board.transform;
            this.audioMenuG.SetActive(false);
            this.turnOfPlayer = PlayerPrefs.GetInt("turnOfPlayer");
        }
        else
        {
            this.cannotLoadGame.SetActive(true);
        }
    }
    public void CancelPlayGamePanel_Button()
    {
        this.mainMenuPanel.SetActive(true);
        this.playGamePanel.SetActive(false);
        this.cannotLoadGame.SetActive(false);
    }
    #endregion  
    #region NewGame Panel
    public void CancelNewGamePanel_Button()
    {
        Debug.Log("it is calleed");
        this.playGamePanel.SetActive(true);
        this.newGamePanel.SetActive(false);
        this.ChooseAgainButton();
    }

    public void BlueTeamButton()
    {
        this.HandleChooseTeam(0);
    }
    public void YellowTeamButton()
    {
        this.HandleChooseTeam(1);
    }
    public void GreemTeamButton()
    {
        this.HandleChooseTeam(2);
    }
    public void RedTeamButton()
    {
        this.HandleChooseTeam(3);
    }
    private void HandleChooseTeam(int turn)
    {
        if (this.canChooseButton[turn])
        {
            if (this.chooseYourTeam)
            {
                this.turnOfPlayer = turn; 
                this.gameState.SetTurnOf(turn, "player");
                this.SwitchChooseEnemy();
                this.chooseSpriteRenderer[turn].sprite = playerSprite;
            }
            else if (this.chooseEnemy)
            {
                this.gameState.SetTurnOf(turn, "computer");
                this.chooseSpriteRenderer[turn].sprite = computerSprite;
            }
            this.numberPlayer++;
            this.canChooseButton[turn] = false;
        }
        if(this.numberPlayer >=2)
        {
            this.startGameButton.SetActive(true);
        }

    }
    public void ChooseAgainButton()
    {
        this.startGameButton.SetActive(false);
        this.gameState.SetStartTurnOf();
        this.ResetAvatarChooseTeam();
        this.SetStartCanChooseButton();
        this.chooseYourTeam = true;
        this.chooseEnemy = false;
        this.chooseEnemyText.SetActive(false);
        this.chooseYourTeamText.SetActive(true);
    }
    public void StartGameButton()
    {
        this.createBoard.SetNewGame();
        this.SetNewGame();
    }
    public void ResetAvatarChooseTeam()
    {
        for(int i = 0; i < 4; i++)
        {
            this.chooseSpriteRenderer[i].sprite = null ;
        }
    }

    private void SwitchChooseEnemy()
    {
        this.chooseYourTeam = false;
        this.chooseEnemy = true;
        this.chooseYourTeamText.SetActive(false);
        this.chooseEnemyText.SetActive(true);
    }
    public void SetNewGame()
    {
        PlayerPrefs.DeleteKey("LoadedData");
        this.gameController.SetGameControllerNewGame();
        this.gamePlay.SetGamePlayNewGame();
        this.SetFirstTurn();
        this.newGamePanel.SetActive(false);
        this.inGamePanel.SetActive(true);
        Invoke("SetStatePlayingGame",1f);
        this.audioMenuG.SetActive(false);
        Invoke("SetThrowButton", 1f);
    }
    private void SetFirstTurn()
    {
        for (; ; )
        {
            int a = Random.Range(0, 3);
            if(this.gameState.GetTurnOf(a) != string.Empty)
            {
                this.gameState.SetTurn(a);
                break;
            } 
        }
    }
    public void SetStatePlayingGame()
    {
        this.gameState.SetGameState(GameStates.PlayingGame);
    }
    public void StartLoadGame()
    {
        PlayerPrefs.DeleteKey("LoadedData");
        this.gameData.LoadData();
        this.playGamePanel.SetActive(false);
        this.inGamePanel.SetActive(true);
        Invoke("SetStatePlayingGame", 1.5f);
        Invoke("SetThrowButton", 1.7f);
        this.gameController.SetGameStatesController(GameStatesController.throwDice);
    }
    public void SetThrowButton()
    {
        this.throwDice.SetThrowButton(true);
    }

    #endregion  

    #region HowToPlay Panel
    public void CancelHowToPlay_Button()
    {
        this.howToPlayPanel.SetActive(false);
        this.mainMenuPanel.SetActive(true);
    }
    #endregion
    #region Credit Panel
    public void CancelCreditPanel_Button()
    {
        this.creditPanel.SetActive(false);
        this.mainMenuPanel.SetActive(true);
    }
    #endregion
    #region InGame Panel
    public void AutoPlay()
    {
        if (this.autoPlay)
        {
            this.gameState.SetTurnOf(this.turnOfPlayer, "computer");
            Time.timeScale = 5f;
            this.autoPlay = false;
        }
        else 
        {
            this.gameState.SetTurnOf(this.turnOfPlayer, "player");
            Time.timeScale = this.timeScaler;
            this.autoPlay = true;
        }
    }
    public void Speed_Button()
    {
        timeScaler %= 2;
        timeScaler++;
        Time.timeScale = timeScaler;
    }
    public void Pause_Button()
    {
        Time.timeScale = 0;
        this.pausePanel.SetActive(true);
        this.inGamePanel.SetActive(false);
    }
    #endregion

    #region Pause Panel
    public void SetVolume()
    {
        AudioListener.volume = this.volumeSlider.value;
        PlayerPrefs.SetFloat("volume", volumeSlider.value);
    }
    public void SaveAndExit_Button()
    {
        if (this.gameController.GetGameStatesController() == GameStatesController.throwDice && !this.gameController.IsTurnOfComPuter())
        {
            this.gameData.SaveData();
            Time.timeScale = this.timeScaler;
            PlayerPrefs.SetInt("turnOfPlayer", this.turnOfPlayer);
            SceneManager.LoadScene("SampleScene");
        }
        else
        {
            this.cannotSaveGamePanel.SetActive(true);
        }
    }
    
    public void Exit_Button()
    {
        SceneManager.LoadScene("SampleScene");
        Time.timeScale = this.timeScaler;
    }
    public void CancelPause_Button()
    {
        this.cannotSaveGamePanel.SetActive(false);
        this.inGamePanel.SetActive(true);
        this.pausePanel.SetActive(false);
        Time.timeScale = this.timeScaler;
    }
    #endregion
    #region GameOver Panel
    public void TapToContinueButton()
    {
        SceneManager.LoadScene("SampleScene");
    }
    #endregion
}
