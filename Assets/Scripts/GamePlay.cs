using JetBrains.Annotations;
using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class GamePlay : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private GameState gameState;

    [SerializeField] private Transform[] knightInCage = new Transform[4];

    [SerializeField] private int[] startLocation;
    [SerializeField] private int[] targetLocation;

    [SerializeField] private string[] knightsTypeInLand;
    [SerializeField] private string[] knightsTypeInTarget;
    [SerializeField] private string[] knightType;
    
    [SerializeField] private int countKnightCanSelected;
    private int turn;
    public bool nextTurn;
    public bool moveFromLandToTarget;
    private void Awake()
    {
        this.gameState = FindObjectOfType<GameState>();
    }
    private void Start()
    {
        this.moveFromLandToTarget = false;
        this.Set_KnightsTypeInTarget();
        this.SetStartLocation();
        this.SetKnightType();
        this.Set_KnightsTypeInLand();
    }
    private void Update()
    {
        this.turn = this.gameState.GetTurn();
    }
    public void SetKnightsTypeInLand(int location, string knightType)
    {
        this.knightsTypeInLand[location] = knightType;
    }
    public void CheckKnight()
    {
        for (int i = 0; i < 4; i++)
        {
            Transform knightInCage = this.knightInCage[turn].GetChild(i);
            Knight knight = knightInCage.GetComponent<Knight>();
            if (gameController.GetIsLucky())
            {
                this.CheckKnightInCage(knight);
            }
            this.CheckKnightInLand(knight, gameController.GetTotalScore());
            this.CheckKnightInTarget(knight, gameController.GetTotalScore());
        }
    }
    public void CheckKnightInCage(Knight knight)
    {
            if (knight.GetKnightState() == KnightState.knightInCage)
            {
                if (knightsTypeInLand[startLocation[turn]] != knightType[turn])
                {
                    knight.SetKnightCanSpawned(true);
                    if (!this.gameController.IsTurnOfComPuter()) knight.AlertKnightCanSelected(true);
                    this.countKnightCanSelected++;
                }
            }
    }
    public void CheckKnightInLand(Knight knight, int totalScore)
    {
        if (knight.GetKnightState() == KnightState.knightInLand)
        {
            int knightLocationInLand = knight.GetKnightLocationInLand();
            int knightCountMove = knight.GetCountMove();
            if (knightCountMove + totalScore > 56) 
            {
                bool canMoveToTarget = true;
                for (int i = 1; i <= (56 - knightCountMove); i++)
                {
                    if (this.knightsTypeInLand[(knightLocationInLand + i) % 56] != string.Empty)
                    {
                        canMoveToTarget = false;
                        break;
                    }
                }
                if (canMoveToTarget)
                {
                    if (totalScore - (56 - knightCountMove) <= 6)
                    {
                        for (int i = 0; i < totalScore - (56 - knightCountMove); i++)
                        {
                            if (this.knightsTypeInTarget[turn * 6 + i] != string.Empty)
                            {
                                canMoveToTarget = false;
                                break;
                            }
                        }
                        if (canMoveToTarget)
                        {
                            this.moveFromLandToTarget = true;
                            knight.SetKnightCanMoved(true);
                            if (!this.gameController.IsTurnOfComPuter()) knight.AlertKnightCanSelected(true);
                            this.countKnightCanSelected++;
                        }
                    }
                }
            }

            else
            {
                for (int i = 1; i <= totalScore; i++)
                {

                    if (this.knightsTypeInLand[(knightLocationInLand + i) % 56] != string.Empty && i != totalScore)
                    {
                        break;
                    }
                    if (i == totalScore && this.knightsTypeInLand[(knightLocationInLand + i) % 56] != knightType[this.turn])
                    {
                        knight.SetKnightCanMoved(true);
                        if (!this.gameController.IsTurnOfComPuter()) knight.AlertKnightCanSelected(true);
                        this.countKnightCanSelected++;

                        if (this.knightsTypeInLand[(knightLocationInLand + i) % 56] != string.Empty)
                        {
                            knight.SetKnightCanKick(true);
                        }
                    }
                }
            }
        }
    }
    public bool GetMoveFromLandToTarget()
    {
        return this.moveFromLandToTarget;
    }
    public void CheckKnightInTarget(Knight knight, int totalScore)
    {
        if (knight.GetKnightState() == KnightState.knightInTarget)
        {
            int knightLocationInTarget = knight.GetKnightLocationInTarget();
            if (knightLocationInTarget < 5 && (knightLocationInTarget == totalScore - 2 || gameController.GetIsLucky()))
            {
                if (knightsTypeInTarget[knightLocationInTarget + 1 + this.turn * 6] == string.Empty)
                {
                    this.countKnightCanSelected++;
                    knight.SetKnightCanTargeted(true);
                    if (!this.gameController.IsTurnOfComPuter()) knight.AlertKnightCanSelected(true);
                    //   // Debug.Log(knight.name + "Knight can Moved in target");
                }
            }
        }
    }

    public void AutoTurn()
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                Transform knightInCage = this.knightInCage[turn].GetChild(j);
                Knight knight = knightInCage.GetComponent<Knight>();
                if (i == 0 && knight.GetKnightCanSpawned())
                {
                    //    // Debug.Log("CanSpawnnnnnn");
                    knight.SetKnightSelected(true);
                    return;
                }
                if (i == 1 && knight.GetKnightCanKick() && knight.GetKnightCanMoved())
                {
                    knight.SetKnightSelected(true);
                    return;
                }
                if (i == 2&& (knight.GetKnightLocationInLand() - this.turn *14) == 1 && knight.GetKnightCanMoved() )
                {
                    knight.SetKnightSelected(true);
                    return;
                }
                if (i == 3 && knight.GetKnightCanTargeted())
                {
                    //   // Debug.Log("CanTargettttt");
                    knight.SetKnightSelected(true);
                    return;
                }
                if (i == 4 && knight.GetKnightCanMoved())
                {
                    //    // Debug.Log("CanMoveeeeee");
                    knight.SetKnightSelected(true);
                    return;
                }
            }
        }
    }

    #region Set
    public void SetCountKnightCanSelected(int countKnightCanSelected)
    {
        this.countKnightCanSelected = countKnightCanSelected;
    }
    #endregion
    #region SetForStartMethod
    private void SetStartLocation()
    {
        this.startLocation = new int[4] { 1, 15, 29, 43 };
    }
    private void SetKnightType()
    {
        this.knightType = new string[4] { "blue", "yellow", "green", "red" };
    }
    private void Set_KnightsTypeInLand()
    {
        this.knightsTypeInLand = new string[56];
        for (int i = 0; i < this.knightsTypeInLand.Length; i++)
        {
            this.knightsTypeInLand[i] = string.Empty;
        }
    }
    private void Set_KnightsTypeInTarget()
    {
        this.knightsTypeInTarget = new string[24];
        for (int i = 0; i < this.knightsTypeInTarget.Length; i++)
        {
            this.knightsTypeInTarget[i] = string.Empty;
        }
    }
    private void SetTargetLocation()
    {
        this.targetLocation = new int[4] { 0, 14, 28, 42 };
    }
    #endregion

    #region Get

    public string GetKnightType(int location)
    {
        return this.knightType[location];
    }
    public void SetKnightTypeInTarget(int location, string knightType)
    {
        this.knightsTypeInTarget[location] = knightType;
    }
    public string GetKnightTypesInLand(int i)
    {
        return this.knightsTypeInLand[i];
    }
    public string GetKnightTypesInTarget(int i)
    {
        return this.knightsTypeInTarget[i];
    }
    public int GetCountKnightCanSelected()
    {
        return this.countKnightCanSelected;
    }
    #endregion

    public void SetGamePlayNewGame()
    {
        this.Set_KnightsTypeInLand();
        this.Set_KnightsTypeInTarget();
        this.countKnightCanSelected = 0;
    }
}
