using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

public class GameState : MonoBehaviour
{
    [SerializeField] private TurnGame gameState;
    [SerializeField] private GameStates gameStates;

    [SerializeField] private GameController gameController;
    [SerializeField] private GameObject panelTurn;
    [SerializeField] private Text turn_Text;
    [SerializeField] private int numberPlayer;
    [SerializeField] private int turn;

    [SerializeField] private string[] turnOf; 

    [SerializeField] private GameObject gameOver;
    [SerializeField] private Text gameOver_Text;
    [SerializeField] private GameObject[] spotLightTurn;
    private string gameOverText;
    private Color color;
    private void Start()
    {
        this.gameStates = GameStates.NotPlayingGame;
        this.turn = -1;
        this.SetStartTurnOf();
    }
    private void Update()
    {
        if (this.gameStates == GameStates.PlayingGame)
        {
            this.StateHandle();
            this.SetTextState();
            this.GameOverHandle();
            this.SpotLightTurnHandle();
        }
    }
    public void GameOverHandle()
    {
        if (this.gameController.IsGameOver())
        {
            if (this.turn == 0) this.gameOverText = "Blue Win";
            if (this.turn == 1) this.gameOverText = "Yellow Win";
            if (this.turn == 2) this.gameOverText = "Green Win";
            if (this.turn == 3) this.gameOverText = "Red Win";
            if (this.turnOf[this.turn] == "player") this.gameOverText = "You Win";
            this.gameOver_Text.text = this.gameOverText;
            this.gameOver_Text.color = this.color;
            this.gameOver.SetActive(true);
            this.gameStates = GameStates.NotPlayingGame;
        }
    }
    public void SpotLightTurnHandle()
    {
        for(int i = 0; i < spotLightTurn.Length; i++)
        {
            if (this.turn == i&& this.turnOf[this.turn] != string.Empty)
            {
                this.spotLightTurn[i].SetActive(true);
            }
            else this.spotLightTurn[i].SetActive(false);
        }
    }
    private void StateHandle()
    {
        switch (this.turn)
        {
            case 0 :
                this.gameState = TurnGame.TurnOfBlue;
                this.color = Color.blue;
                break;
            case 1:
                this.gameState = TurnGame.TurnOfYellow;
                this.color = Color.yellow;
                break;
            case 2:
                this.gameState = TurnGame.TurnOfGreen;
                this.color = Color.green;
                break;
            case 3:
                this.gameState = TurnGame.TurnOfRed;
                this.color = Color.red;
                break;
            default: break;
        }
        
    }
    #region Set
    public void SetStartTurnOf()
    {
        this.turnOf = new string[4];
        for(int i = 0; i < 4; i++)
        {
            turnOf[i] = string.Empty;
        }
    }
    public void SetTurnOf(int i,string turnOf)
    {
        this.turnOf[i] = turnOf;
    }
    public void SetGameState(GameStates gameStates)
    {
        this.gameStates = gameStates;
    }
    private void SetTextState()
    {
        this.turn_Text.text = this.gameState.ToString();
        this.turn_Text.color = this.color;
    }
    public void SetPanelTurn(bool panel)
    {
        this.panelTurn.SetActive(panel);
    }
    public void SetNumberPlayer(int num)
    {
        this.numberPlayer=num;
    }
    public void SetTurn(int a)
    {
        this.turn = a;
        this.turn %= 4;
    }
    #endregion

    #region Get
    public string GetTurnOf(int i)
    {
        return this.turnOf[i];
    }
    public int GetTurnOfPlayer()
    {
        for (int i = 0; i < 4; i++)
        {
            if (this.GetTurnOf(i) == "player")
            {
                return i;
            }
        }
        return -1;
    }
    public GameStates GetGameStates()
    {
        return this.gameStates;
    }
    public int GetNumberPlayer()
    {
        return this.numberPlayer;
    }
    public int GetTurn()
    {
        return this.turn;
    }

    #endregion
}
public enum TurnGame
{
    StartGame,
    TurnOfBlue,
    TurnOfYellow,
    TurnOfGreen,
    TurnOfRed,
    GameOver
}
public enum GameStates
{
    PlayingGame,
    NotPlayingGame
}
