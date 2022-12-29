using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDataGame : MonoBehaviour
{
    [SerializeField] Knight[] knight;
    [SerializeField] GamePlay gamePlay;
    [SerializeField] GameState gameState;
    [SerializeField] Transform boardGame;
    public void SaveData()
    {
        PlayerPrefs.SetString("LoadedData", "LoadedData");
        this.SaveKnightData();
        this.SaveGameStateData();
        this.SaveGamePlayData();
        SaveBoardTransformData();
    }

    private void SaveBoardTransformData()
    {
        PlayerPrefs.SetFloat("xBoard", boardGame.transform.eulerAngles.x);
        PlayerPrefs.SetFloat("yBoard", boardGame.transform.eulerAngles.y);
        PlayerPrefs.SetFloat("zBoard", boardGame.transform.eulerAngles.z);
    }


    #region Save Knight Data
    private void SaveKnightData()
    {
        for (int i = 0; i <16 ; i++)
        {
            this.SaveKnightTransform(i);
            //this.SaveKnightState(i);
            this.SaveKnightCountMove(i);
            this.SaveKnightLocationInLand(i);
            this.SaveKnightLoactionInTarget(i);
        }
        Debug.Log("Save Knight");
    }
    private void SaveKnightTransform(int i)
    {
        PlayerPrefs.SetFloat("xPosition" + i, this.knight[i].transform.position.x);
        PlayerPrefs.SetFloat("yPosition" + i, this.knight[i].transform.position.y);
        PlayerPrefs.SetFloat("zPosition" + i, this.knight[i].transform.position.z);
        PlayerPrefs.SetFloat("xRotation" + i, this.knight[i].transform.eulerAngles.x);
        PlayerPrefs.SetFloat("yRotation" + i, this.knight[i].transform.eulerAngles.y);
        PlayerPrefs.SetFloat("zRotation" + i, this.knight[i].transform.eulerAngles.z);
        
    }
    private void SaveKnightState(int i)
    {
        PlayerPrefs.SetInt("knightState" + i, (int)this.knight[i].GetKnightState());
    }
    private void SaveKnightCountMove(int i)
    {
        PlayerPrefs.SetInt("countMove" + i, this.knight[i].GetCountMove());
    }
    private void SaveKnightLocationInLand(int i)
    {
        PlayerPrefs.SetInt("knightLocationInLand" +i,this.knight[i].GetKnightLocationInLand());
    }
    private void SaveKnightLoactionInTarget(int i)
    {
        PlayerPrefs.SetInt("KnightLocationInTarget" + i, this.knight[i].GetKnightLocationInTarget());
    }
    #endregion
    
    
    #region Save Gamestate Data
    private void SaveGameStateData()
    {
        this.SaveNumberPlayer();
        this.SaveTurn();
        this.SaveTurnOf();
        Debug.Log("SaveGameStateData");
    }
    private void SaveNumberPlayer()
    {
        PlayerPrefs.SetInt("numberPlayer", this.gameState.GetNumberPlayer());
    }
    private void SaveTurn()
    {
        PlayerPrefs.SetInt("turn", this.gameState.GetTurn());
    }
    private void SaveTurnOf()
    {
        for(int i = 0; i < 4; i++)
        {
            PlayerPrefs.SetString("turnOf" + i, this.gameState.GetTurnOf(i));
        }
    }
    #endregion


    #region Save GamePlay Data
    private void SaveGamePlayData()
    {
        this.SaveKnightTypesInland();
        this.SaveKnightTypesInTarget();
        Debug.Log("Save gamePlay data");
    }
    
    private void SaveKnightTypesInland()
    {
        for(int i = 0; i < 56; i++)
        {
            PlayerPrefs.SetString("knightTypesInLand" + i, this.gamePlay.GetKnightTypesInLand(i));
        }
    }
    private void SaveKnightTypesInTarget()
    {
        for(int i = 0; i < 24; i++)
        {
            PlayerPrefs.SetString("knightTypesInTarget"+i, this.gamePlay.GetKnightTypesInTarget(i));
        }
    }
    #endregion


    
    public void LoadData()
    {
        this.LoadKnightData();
        this.LoadGamePlayData();
        this.LoadGameStateData();
    }

    public void LoadBoardData()
    {
        float xBoard = PlayerPrefs.GetFloat("xBoard");
        float yBoard = PlayerPrefs.GetFloat("yBoard");
        float zBoard = PlayerPrefs.GetFloat("zBoard");
        this.boardGame.transform.rotation = Quaternion.Euler(new Vector3(xBoard, yBoard, zBoard));
    }
    #region Load Knight Data
    public void LoadKnightData()
    {
        for(int i = 0; i < 16; i++)
        {
            this.LoadKnightCountMove(i);
            this.LoadKnightLocationInLand(i);
            this.LoadKnightLocationInTarget(i);
            this.LoadKnightTransform(i);
            this.LoadKnightState(i);
        }
        Debug.Log("Load Knight Data Success");
    }
    private void LoadKnightTransform(int i)
    {
        float xPosition = PlayerPrefs.GetFloat("xPosition"+i);
        float yPosition = PlayerPrefs.GetFloat("yPosition"+i);
        float zPosition = PlayerPrefs.GetFloat("zPosition"+i);
        this.knight[i].transform.position = new Vector3(xPosition, yPosition, zPosition);
        float xRotation = PlayerPrefs.GetFloat("xRotation"+i);
        float yRotation = PlayerPrefs.GetFloat("yRotation"+i);
        float zRotation = PlayerPrefs.GetFloat("zRotation"+i);
        this.knight[i].transform.rotation = Quaternion.Euler(new Vector3(xRotation, yRotation, zRotation));
    }
    public void LoadKnightState(int i)
    {
        // KnightState knightState =  (KnightState)PlayerPrefs.GetInt("knightState" + i);
        if (this.knight[i].GetCountMove() == 0) this.knight[i].SetKnightState(KnightState.knightInCage);
        else if(this.knight[i].GetCountMove() >=56) this.knight[i].SetKnightState(KnightState.knightInTarget);
        else this.knight[i].SetKnightState(KnightState.knightInLand);
    }
    public void LoadKnightCountMove(int i)
    {
        int countMove = PlayerPrefs.GetInt("countMove" + i);
        this.knight[i].SetCountMove(countMove);
        Debug.Log(countMove);
    }
    public void LoadKnightLocationInLand(int i)
    {
        int knightLocationInLand = PlayerPrefs.GetInt("knightLocationInLand" + i);
        this.knight[i].SetKnightLocationInLand(knightLocationInLand);
    }    
    public void LoadKnightLocationInTarget(int i)
    {
        int knightLocationInTarget = PlayerPrefs.GetInt("knightLocationInTarget" + i);
        this.knight[i].SetKnightLocationInTarget(knightLocationInTarget);
    }
    #endregion

    #region Load GameState
    public void LoadGameStateData()
    {
        this.LoadTurn();
        this.LoadNumberPlayer();
        this.LoadTurnOf();
        Debug.Log("Load Game State Success");
    }
    private void LoadTurn()
    {
        int turn = PlayerPrefs.GetInt("turn");
        this.gameState.SetTurn(turn);
    }
    private void LoadTurnOf()
    {
        for(int i = 0; i < 4; i++)
        {
            string turnOf = PlayerPrefs.GetString("turnOf" + i);
            this.gameState.SetTurnOf(i, turnOf);
        }
    }
    private void LoadNumberPlayer()
    {
        int numberPlayer = PlayerPrefs.GetInt("numberPlayer");
        this.gameState.SetNumberPlayer(numberPlayer);
        Debug.Log("Number player is "+this.gameState.GetNumberPlayer());
    }
    public void LoadGamePlayData()
    {
        this.LoadKnightTypesInLand();
        this.LoadKnightTypesInTarget();
        Debug.Log("Load GamePlay Data Success");
    }
    #endregion

    #region Load GamePlay Data

    private void LoadKnightTypesInLand()
    {
        for(int i = 0; i < 56; i++)
        {
            string type = PlayerPrefs.GetString("knightTypesInLand" + i);
            this.gamePlay.SetKnightsTypeInLand(i, type); 
        }
    }  
    private void LoadKnightTypesInTarget()
    {
        for(int i = 0; i < 24; i++)
        {
            string type = PlayerPrefs.GetString("knightTypesInTarget" + i);
            this.gamePlay.SetKnightTypeInTarget(i, type); 
        }
    }
    #endregion
}
