using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Knight : MonoBehaviour
{
    #region
    [SerializeField] private GameState gameState;
    [SerializeField] private GamePlay gamePlay;
    [SerializeField] private Parabol parabol;
    [SerializeField] private KnightState knightState;
    [SerializeField] private GameController gameController;
    #endregion
    [SerializeField] private int countMove = 0 ;      // Number of knight move , if count Move = 56 , knight can move to target
    [SerializeField] private int knightLocationInLand;  // Location of Knight in land
   // [SerializeField] private int knightLocationInCage;   //Location of knight in Cage
    [SerializeField] private int knightLocationInTarget; //location of knight in target

    [SerializeField] string knightType;
    [SerializeField] private bool knightCanMoved, knightCanSpawned,knightCanTargeted;

    [SerializeField] private bool knightSelected, knightCanSelected;
    private bool knightKicked;

    [SerializeField] private Transform knightCageTransform;
    private Vector3 landingPosition;
    [SerializeField] private Vector3 knightCagePosition;
    [SerializeField] private Quaternion knightCageRotation;
    private Rigidbody rb;
    private bool knightCanKick;

    [SerializeField] private float animationTime, moveTime, moveHeight;
    private void Awake()
    {
        this.gameState = FindObjectOfType<GameState>();
        this.gamePlay = FindObjectOfType<GamePlay>();
        this.gameController = FindObjectOfType<GameController>();
        this.rb = GetComponent<Rigidbody>();
        this.parabol = FindObjectOfType<Parabol>();
        this.knightCagePosition = this.knightCageTransform.position;
        this.knightCageRotation = this.knightCageTransform.rotation;
    }
    private void Start()
    {
        Debug.Log("kkkkk");
        this.knightKicked = false;
        this.knightCanSelected = true;
    }
    private void Update()
    {
        this.knightCagePosition = this.knightCageTransform.position;
        this.knightCageRotation = this.knightCageTransform.rotation;
    }
    private void FixedUpdate()
    {
        if (this.knightKicked)
        {
            this.MoveKnightKicked();
        }
    }
    private void OnMouseDown()
    {
        if(this.gameController.GetGameStatesController() == GameStatesController.handleKnightSelected && this.knightCanSelected  ) 
        {
            this.GetMouseDown();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (this.knightType != this.gamePlay.GetKnightType(this.gameState.GetTurn()))
        {
            this.knightKicked = true;
            this.landingPosition = this.transform.position;
        }
    }
    private void MoveKnightKicked()
    {
        if (this.animationTime == 0) this.gameController.SpawnedSound();
        if (this.animationTime <= this.moveTime)
        {
            this.transform.position = this.parabol.Parabola(this.landingPosition, this.knightCagePosition, this.moveHeight, this.animationTime / this.moveTime);
            this.animationTime+= Time.deltaTime;
            this.gameObject.layer = LayerMask.NameToLayer("Ignore");
        }
        else
        {
            this.countMove = 0;
            this.animationTime = 0;
            this.knightKicked = false;
            this.transform.position = this.knightCageTransform.position;
            this.transform.rotation = this.knightCageTransform.rotation;
            this.knightState = KnightState.knightInCage;
            this.gameObject.layer = LayerMask.NameToLayer("Knight");
        }
    }
    public void SetKnightSelected(bool knightSelected)
    {
        this.knightSelected = knightSelected;
 
    }
    private void GetMouseDown()
    {
        if(this.knightCanMoved || this.knightCanSpawned||this.knightCanTargeted)
        {
            this.knightSelected = true;
          //  Debug.Log("knight selected");
        }
    }
    // Called when knight canSpawned/canMoved/canTargeted
    public void AlertKnightCanSelected(bool isAlert)
    {
        this.transform.GetChild(0).gameObject.SetActive(isAlert);
    }
    // Called when turn is end
    public void SetKnightNewTurn()
    {
        this.knightCanKick = false;
        this.knightCanMoved = false;
        this.knightCanSpawned = false;
        this.knightCanTargeted = false;
        this.knightSelected = false;
        this.knightCanSelected = true;
    }
    public void SetKnightNewGame()
    {
        this.countMove = 0;
        this.knightCanMoved = this.knightCanTargeted = this.knightCanSpawned = false;
        this.knightCanSelected = true;
        this.knightLocationInLand = this.knightLocationInTarget = 0;
        this.knightState = KnightState.knightInCage;
    }
    public void SetKnightExitGame()
    {
        this.transform.position = this.knightCageTransform.position;
        this.transform.rotation = this.knightCageTransform.rotation;
    }


    #region GET 
    public bool GetKnightCanKick()
    {
        return this.knightCanKick;
    }
    //Get position for Knight where knight is spawned
    public Vector3 GetKnightCagePosition()
    {
        return this.knightCagePosition;
    }
    //

    // Get state of Knight
    public KnightState GetKnightState()
    {
        return this.knightState;
    }
    //

    //Get bool for Knight
    public bool GetKnightSelected()
    {
        return this.knightSelected;
    }
    public bool GetKnightCanMoved()
    {
        return this.knightCanMoved;
    }
    public bool GetKnightCanSpawned()
    {
        return this.knightCanSpawned;
    }
    public bool GetKnightCanTargeted()
    {
        return this.knightCanTargeted;
    }
    //

    //Get Location for Knight
    public int GetKnightLocationInLand()
    {
        return this.knightLocationInLand;
    }
 //   public int GetKnightLocationInCage()
   // {
    //    return this.knightLocationInCage;
    //}
    public int GetKnightLocationInTarget()
    {
        return this.knightLocationInTarget;
    }
    public int GetCountMove()
    {
        return this.countMove;
    }
    //
    #endregion GET 

    #region SET 
    public void SetKnightCanKick(bool knightCanKick)
    {
        this.knightCanKick = knightCanKick;
    }
    public void SetCountMove(int countMove)
    {
        this.countMove = countMove;
    }
    public void SetKnightLocationInLand(int knightLocationInLand)
    {
        this.knightLocationInLand = knightLocationInLand;
    }
    public void SetKnightLocationInTarget(int knightLocationInTarget)
    {
        this.knightLocationInTarget = knightLocationInTarget;
    }
    ///
    //Set bool for Knight
    public void SetKnightCanSelected(bool knightCanSelected)
    {
        this.knightCanSelected = knightCanSelected;
    }
    public void SetKnightCanMoved(bool knightCanMoved)
    {
        this.knightCanMoved = knightCanMoved;
        Debug.Log("knight Selected");
    }
    public void SetKnightCanSpawned(bool knightCanSpawned)
    {
        this.knightCanSpawned = knightCanSpawned;
    }
    public void SetKnightCanTargeted(bool knightCanTargeted)
    {
        this.knightCanTargeted = knightCanTargeted;
    }
    public void SetKnightState(KnightState knightState)
    {
        this.knightState = knightState;
    }
    #endregion SET

}

//  Place of Knight 
public enum KnightState
{
    knightInCage,
    knightInLand,
    knightInTarget
}




