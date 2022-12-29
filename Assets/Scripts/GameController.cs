using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameController : MonoBehaviour
{
    [SerializeField] private Dice dice1, dice2;
    [SerializeField] private GamePlay gamePlay;
    [SerializeField] private ThrowDice throwDice;
    [SerializeField] private GameState gameState;
    [SerializeField] private Parabol parabol;


    [SerializeField] private Transform[] spawnPosition = new Transform[4];  // Position to spawn Knight 
    [SerializeField] private Transform[] knightInCage = new Transform[4];   // Position of Knight In Cage
    [SerializeField] private Transform[] targetPosition = new Transform[24];    // Position of Target
    [SerializeField] private Transform stepsPosition;           //Paren of stepsPosition 
    [SerializeField] private Transform[] stepPosition;          //Position of steps
    [SerializeField] private int[] changeRotationKnight;        //Change rotation of Knight when knight translated

    [SerializeField] private GameStatesController gameStatesController;

    /// float to translate Knight
    [SerializeField] private float animationTime;
    [SerializeField] private float spawnTime, moveTime, kickedTime;
    [SerializeField] private float spawnHeight, moveHeight, kickedHeight;
    [SerializeField] private bool finishTranslate;
    /////////////

    private int score1, score2, totalScore;
    private int turn;
    private bool isLucky;   // ChechkScore

    private Vector3[] getPosition = new Vector3[13]; //Get position steps to translate Knight
    private bool isGetPosition;                      //if already getPosition return true

    private bool movedFromLandToTarget;

    private int[] spawnLocation;
    private bool startHandleKnightSelected;
    private bool finishSpawn, finishMove, finishTarget;
    private int countMoved;
    private bool isAutoThrow;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] audioClip;
    [SerializeField] AudioClip diceSound;
    [SerializeField] AudioClip spawnedSound;
    private void Awake()
    {
        this.audioSource = this.GetComponent<AudioSource>();
    }
    private void Start()
    {
        StartGameSetting();
        Time.timeScale = 1f;
    }

    private void StartGameSetting()
    {
        this.gameStatesController = GameStatesController.startGame;
        this.isAutoThrow = true;
        this.isGetPosition = false;
        this.movedFromLandToTarget = false; ;
        this.countMoved = 0;
        this.animationTime = 0;
        this.finishTarget = this.finishSpawn = this.finishMove = this.finishTranslate = false;
        this.SetStepPosition();
        this.SetSpawnLocation();
        this.SetChangeRotationKnight();
        this.gameStatesController = GameStatesController.startGame;
    }

    private void Update()
    {
        if (this.gameState.GetGameStates() == GameStates.PlayingGame)
        {
            this.turn = this.gameState.GetTurn();
            this.Controller();
        }
    }
    private void FixedUpdate()
    {
        if (this.gameStatesController == GameStatesController.getScore)
        {
            this.GetScore();
        }
        if (this.gameStatesController == GameStatesController.checkKnight)
        {
            this.CheckKnight();
        }
        if (this.gameStatesController == GameStatesController.handleKnightSelected)
        {
            this.HandleAutoKnightSelected();
            this.HandleKnightSelected();
        }
    }
    public void Controller()
    {
        switch (gameStatesController)
        {
            case GameStatesController.startGame:
                break;
            case GameStatesController.throwDice:
                HandleAutoThrowDice();
                this.ThrowDice();
                break;
            case GameStatesController.getScore:
                //this.GetScore();
                break;
            case GameStatesController.checkScore:
                this.CheckScore();
                break;
            case GameStatesController.checkKnight:
                //this.CheckKnight();
                break;
            case GameStatesController.handleKnightSelected:
                //this.HandleAutoKnightSelected();
                //this.HandleKnightSelected();
                break;
            case GameStatesController.newTurn:
                NewTurn();
                break;
            default: break;
        }
    }

    #region Controller
    public void ThrowDice()
    {
        if (this.throwDice.GetIsThrowed())
        {
            Invoke("PlayDiceSound", 0.65f);
            this.gameStatesController = GameStatesController.getScore;
            this.throwDice.SetIsThrowed(false);
        }
    }
    public void PlayDiceSound()
    {
        this.audioSource.Play();
    }
    public void HandleAutoThrowDice()
    {
        if (this.IsTurnOfComPuter() && this.isAutoThrow)
        {
            Invoke("AutoThrowDice", 0.5f);
            this.isAutoThrow = false;
        }
    }
    public void AutoThrowDice()
    {
        this.throwDice.Throw();
    }
    public void HandleAutoKnightSelected()
    {
        if (IsTurnOfComPuter())
        {
            this.gamePlay.AutoTurn();
        }
    }
    public bool IsTurnOfPlayer()
    {
        if (this.gameState.GetTurnOf(this.gameState.GetTurn()) == "player") return true;
        else return false;
    }
    public bool IsTurnOfComPuter()
    {
        if (this.gameState.GetTurnOf(this.gameState.GetTurn()) == "computer") return true;
        else return false;
    }
    public void GetScore()
    {
        if (this.throwDice.GetIsCanGetScore() && this.dice1.GetIsLanded() && this.dice2.GetIsLanded())
        {
            this.score1 = this.dice1.GetScore();
            this.score2 = this.dice2.GetScore();
            this.totalScore = this.score1 + this.score2;
            this.throwDice.SetIsCanGetScore(false);
            this.gameStatesController = GameStatesController.checkScore;
        }
    }
    public void CheckScore()
    {
        if ((this.score1 == 1 && this.score2 == 6) || (this.score1 == 6 && this.score2 == 1) || this.score1 == this.score2)
        {
            this.isLucky = true;
        }
        else isLucky = false;
        this.gameStatesController = GameStatesController.checkKnight;
    }

    public void CheckKnight()
    {
        this.gamePlay.CheckKnight();
        this.gameStatesController = GameStatesController.handleKnightSelected;
    }

    public void HandleKnightSelected()
    {
        for (int i = 0; i < 4; i++)
        {
            Transform knightInCage = this.knightInCage[this.turn].GetChild(i);
            Knight knight = knightInCage.GetComponent<Knight>();
            if (knight.GetKnightSelected())
            {
                this.HandleKnight(knight);
                this.startHandleKnightSelected = true;
            }
            if (this.startHandleKnightSelected)
            {
                knight.SetKnightCanSelected(false);
                knight.AlertKnightCanSelected(false);
            }
        }
        if (gamePlay.GetCountKnightCanSelected() == 0)
        {
            this.gameStatesController = GameStatesController.newTurn;
        }
    }
    public void HandleKnight(Knight knight)
    {
        if (knight.GetKnightCanMoved())
        {
            this.HandleKnightCanMoved(knight);
        }

        if (knight.GetKnightCanSpawned())
        {
            this.HandleKnightCanSpawned(knight);
        }

        if (knight.GetKnightCanTargeted())
        {
            this.HandleKnightCanTargeted(knight);
        }

        if (this.finishMove || this.finishSpawn || this.finishTarget)
        {
            this.gameStatesController = GameStatesController.newTurn;
        }
    }
    #endregion

    #region KinghtMoved

    public void HandleKnightCanMoved(Knight knight)
    {
        MoveKnight(knight);
    }

    private void MoveKnight(Knight knight)
    {
        int knightLocationInLand = knight.GetKnightLocationInLand();
        GetPositionToMoveKnight(knight, knightLocationInLand);

        if (knight.transform.position != getPosition[totalScore])
        {
            this.TranslateKnight(knight, getPosition[this.countMoved], getPosition[this.countMoved + 1], this.moveHeight, this.moveTime);
            if (this.finishTranslate && this.countMoved <= totalScore)
            {
                this.animationTime = 0;
                this.finishTranslate = false;
                this.countMoved++;
                knight.SetCountMove(knight.GetCountMove() + 1);
            }
            ChangeRotationKnight(knight);
        }
        else
        {
            this.finishMove = true;
            this.SetStateAfterMove(knight, knightLocationInLand);
        }
    }

    private void GetPositionToMoveKnight(Knight knight, int knightLocationInLand)
    {
        if (!this.isGetPosition)
        {
            int countMove = knight.GetCountMove();
            int locationTarget = 0;
            for (int i = 0; i <= totalScore; i++)
            {
                if (countMove + i <= 56)
                {
                    getPosition[i] = this.stepPosition[(knightLocationInLand + i) % 56].position;
                }
                else
                {
                    getPosition[i] = this.targetPosition[this.turn * 6 + locationTarget].position;
                    locationTarget++;
                    this.movedFromLandToTarget = true;
                }
            }
            this.isGetPosition = true;
        }
    }

    public void ChangeRotationKnight(Knight knight)
    {
        for (int i = 0; i < this.changeRotationKnight.Length; i++)
        {
            if (knight.GetCountMove() == changeRotationKnight[i])
            {
                Quaternion newRotation = stepPosition[(knight.GetCountMove() + this.turn * 14) % 56].rotation;
                knight.transform.rotation = newRotation;
                break;
            }
        }
    }

    private void SetStateAfterMove(Knight knight, int knightLocation)
    {
        this.countMoved = 0;
        this.gamePlay.SetKnightsTypeInLand(knightLocation, string.Empty);

        if (movedFromLandToTarget)
        {
            knight.SetKnightLocationInTarget((knight.GetCountMove() - 56 - 1));
            this.gamePlay.SetKnightTypeInTarget(knight.GetKnightLocationInTarget() + this.turn * 6, this.gamePlay.GetKnightType(this.turn));
            knight.SetKnightState(KnightState.knightInTarget);
            this.movedFromLandToTarget = false;
        }
        else
        {

            int newKnightLocation = (knightLocation + this.totalScore) % 56;
            knight.SetKnightLocationInLand(newKnightLocation);
            this.gamePlay.SetKnightsTypeInLand(newKnightLocation, this.gamePlay.GetKnightType(this.turn));
        }
    }
    #endregion

    //////////

    #region KnightSpawned
    ///Spawn Knight Selected
    public void HandleKnightCanSpawned(Knight knight)
    {
        if (this.animationTime == 0) this.SpawnedSound();
        Vector3 startPosition = knight.GetKnightCagePosition();
        Vector3 endPosition = this.spawnPosition[turn].position;
        this.TranslateKnight(knight, startPosition, endPosition, this.spawnHeight, this.spawnTime);
        if (this.finishTranslate)
        {
            this.finishSpawn = true;
            this.SetStateAfterSpawned(knight);
        }
    }

    public void SpawnedSound()
    {
        this.audioSource.PlayOneShot(spawnedSound, 1f);
    }

    private void SetStateAfterSpawned(Knight knight)
    {
        knight.SetCountMove(knight.GetCountMove() + 1);  // increase count knight moved 
        this.gamePlay.SetKnightsTypeInLand(this.spawnLocation[turn], this.gamePlay.GetKnightType(turn));   // set name of slot whic Knight Landed
        knight.SetKnightLocationInLand(this.spawnLocation[turn]);           // set location of knight In Land
        knight.SetKnightState(KnightState.knightInLand);        //swicht state to knightInLand
    }

    //////////
    #endregion

    #region KnightTargeted
    public void HandleKnightCanTargeted(Knight knight)
    {
        int startLocation = (knight.GetCountMove() - 56 - 1) + this.turn * 6;
        int endLocation = (knight.GetCountMove() - 56 - 1) + 1 + this.turn * 6;
        Vector3 startPosition = this.targetPosition[startLocation].position;
        Vector3 endPosition = this.targetPosition[endLocation].position;

        this.TranslateKnight(knight, startPosition, endPosition, this.moveHeight, this.moveTime);
        if (this.finishTranslate)
        {
            this.finishTarget = true;
            this.SetStateAfterTarget(knight);
        }
    }
    public void SetStateAfterTarget(Knight knight)
    {
        knight.SetCountMove(knight.GetCountMove() + 1);
        knight.SetKnightLocationInTarget(knight.GetCountMove() - 56 - 1);
        this.gamePlay.SetKnightTypeInTarget(knight.GetKnightLocationInTarget() + this.turn * 6, this.gamePlay.GetKnightType(this.turn));         //new Position
        this.gamePlay.SetKnightTypeInTarget(knight.GetKnightLocationInTarget() - 1 + this.turn * 6, string.Empty);                                      // old Position
    }
    #endregion

    public bool IsGameOver()
    {
        int sum = 0;
        if (this.gameStatesController == GameStatesController.newTurn)
        {
            for (int i = 0; i < 4; i++)
            {
                Transform knightInCage = this.knightInCage[turn].GetChild(i);
                Knight knight = knightInCage.GetComponent<Knight>();
                sum += knight.GetKnightLocationInTarget();
            }
            if (sum == 14) return true;
            else return false;
        }
        return false;
    }

    public void TranslateKnight(Knight knight, Vector3 startPonit, Vector3 endPoint, float translateHeight, float translateTime)
    {
        if (this.animationTime <= translateTime)
        {
            knight.transform.position = parabol.Parabola(startPonit, endPoint, translateHeight, this.animationTime / translateTime);
            this.animationTime += Time.deltaTime;
        }
        else
        {
            this.audioSource.PlayOneShot(audioClip[this.turn], 1f);
            knight.transform.position = endPoint;
            this.finishTranslate = true;
        }
    }

    #region NewTurn
    public void NewTurn()
    {
        if (this.IsGameOver()) Time.timeScale = 0f;
        else this.SetNewTurn();
    }
    public void HandleGameOver()
    {

    }
    public void SetNewTurn()
    {
        this.SetGameStateNewTurn();
        this.SetGameControllerNewTurn();
        this.SetGamePlayNewTurn();
        this.SetKnightNewTurn();
        this.SetThrowDiceNewTurn();
        this.gameStatesController = GameStatesController.throwDice;
    }
    public void SetGameControllerNewTurn()
    {
        this.isAutoThrow = true;
        this.animationTime = 0;
        this.startHandleKnightSelected = false;
        this.finishTranslate = false;
        this.finishMove = false;
        this.finishSpawn = false;
        this.finishTarget = false;
        this.isGetPosition = false;
    }
    public void SetThrowDiceNewTurn()
    {
        this.throwDice.SetThrowButton(true);

    }
    public void SetGamePlayNewTurn()
    {
        this.gamePlay.SetCountKnightCanSelected(0);
    }
    public void SetGameStateNewTurn()
    {
        if (this.isLucky) this.gameState.SetTurn(this.turn);
        else
        {
            for (int i = 0; i < 5; i++)
            {
                this.gameState.SetTurn(this.gameState.GetTurn() + 1);
                if (this.gameState.GetTurnOf(this.gameState.GetTurn()) == string.Empty) continue;
                break;
            }
        }
    }
    public void SetKnightNewTurn()
    {
        for (int i = 0; i < 4; i++)
        {
            Knight knight = this.knightInCage[turn].GetChild(i).GetComponent<Knight>();
            knight.SetKnightNewTurn();
        }
    }
    public void SetGameControllerNewGame()
    {
        this.finishMove = this.finishSpawn = this.finishTarget = this.finishTranslate = false;
        this.animationTime = 0;
        this.movedFromLandToTarget = false;
        this.isGetPosition = false;
        this.countMoved = 0;
        this.isAutoThrow = true;
        this.gameStatesController = GameStatesController.throwDice;
    }
    #endregion

    #region Get
    public bool GetIsLucky()
    {
        return this.isLucky;
    }
    public int GetTotalScore()
    {
        return this.totalScore;
    }
    #endregion

    #region  SET
    private void SetSpawnLocation()
    {
        this.spawnLocation = new int[4] { 1, 15, 29, 43 };
    }

    public void SetChangeRotationKnight()
    {
        this.changeRotationKnight = new int[13] { 1, 7, 13, 15, 21, 27, 29, 35, 41, 43, 49, 55, 56 };
    }
    public void SetStepPosition()
    {
        this.stepPosition = new Transform[this.stepsPosition.childCount];
        for (int i = 0; i < stepPosition.Length; i++)
        {
            this.stepPosition[i] = this.stepsPosition.GetChild(i);
        }
    }
    public GameStatesController GetGameStatesController()
    {
        return this.gameStatesController;
    }
    public void SetGameStatesController(GameStatesController state)
    {
        this.gameStatesController = state;
    }

    #endregion
}
public enum GameStatesController
{
    startGame,
    throwDice,
    getScore,
    checkScore,
    checkKnight,
    handleKnightSelected,
    newTurn
}
