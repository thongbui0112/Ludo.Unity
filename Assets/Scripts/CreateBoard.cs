using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBoard : MonoBehaviour
{
    [SerializeField] private Dice dice1, dice2;
    [SerializeField] private GameState gameState;
    [SerializeField] private GameObject board;
    [SerializeField] private GameObject throwBut;
    [SerializeField] private GameObject[] knightCage = new GameObject[4];

    private void Start()
    {
        this.board.SetActive(false);
    }

    public void SetBoard(bool set)
    {
        this.board.SetActive(set);
    }

    public void SetNewGame()
    {
        this.board.SetActive(true);
        this.SetKnightNewGame();
        this.SetBoardRotation();
        this.SetDicePosition();
    }
    public void SetLoadGame()
    {
        this.SetKnightLoadGame();
        this.board.SetActive(true);
      //  this.SetBoardRotation();
        this.SetDicePosition();
    }
    //public void SetExitGame()
    //{
    //    this.board.transform.rotation = Quaternion.Euler(Vector3.zero);
    //    this.board.SetActive(false);
    //    this.SetKnightExitGame();
    //}
    //public void SetGameOver()
    //{
    //    this.board.SetActive(false);
    //    this.SetKnightExitGame();
    //}


    public void SetBoardRotation()
    {
        this.board.transform.rotation = Quaternion.Euler(Vector3.zero);
        float xRotation = this.board.transform.eulerAngles.x;
        float yRotation = this.board.transform.eulerAngles.y;
        float zRotation = this.board.transform.eulerAngles.z;
        this.board.transform.rotation = Quaternion.Euler(new Vector3(xRotation, yRotation + this.gameState.GetTurnOfPlayer() * 90, zRotation));
    }
    public void SetKnightNewGame()
    {
        for (int i = 0; i < 4; i++)
        {
            if (this.gameState.GetTurnOf(i) != string.Empty)
            {
                knightCage[i].SetActive(true);
                for (int j = 0; j < 4; j++)
                {
                    Knight knight = knightCage[i].transform.GetChild(j).GetComponent<Knight>();
                    knight.SetKnightNewGame();
                }
            }
        }
    }
    private void SetKnightLoadGame()
    {
        for (int i = 0; i < 4; i++)
        {
            if (this.gameState.GetTurnOf(i) != string.Empty)
            {
                knightCage[i].SetActive(true);
            }
        }
    }

    private void FixBug()
    {
    }

    public void SetKnightExitGame()
    {
        for (int i = 0; i < 4; i++)
        {
            knightCage[i].SetActive(false);
            for (int j = 0; j < 4; j++)
            {
                Knight knight = knightCage[i].transform.GetChild(j).GetComponent<Knight>();
                knight.SetKnightExitGame();
            }
        }
    }
    public void SetDicePosition()
    {
        dice1.SetPosition();
        dice2.SetPosition();
    } 

}
