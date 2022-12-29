using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowDice : MonoBehaviour
{
    [SerializeField] GameController gameController;
    [SerializeField] private GameObject pointer;
    [SerializeField] private GameObject throwButton;
    [SerializeField] private Transform[] dice = new Transform[2];
    [SerializeField] private Transform[] dicePos = new Transform[2];
    [SerializeField] private bool isThrowed;
    [SerializeField] private int speed;
    [SerializeField] private bool isCanGetScore;
  //  [SerializeField] private GameObject allertThrow;
    private bool isCanThrow;
    private Rigidbody[] rb = new Rigidbody[2];
    private void Start()
    {
        this.isThrowed = false;
        this.rb[0] = this.dice[0].gameObject.GetComponent<Rigidbody>();
        this.rb[1] = this.dice[1].gameObject.GetComponent<Rigidbody>();

    }
    private void Update()
    {
    }
    public void Throw()
    {
        for (int i = 0; i < dice.Length; i++)
        {
            Vector3 orientation = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
            Vector3 randomRotation = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
            this.dice[i].position = this.dicePos[i].position;
            this.dice[i].rotation = Quaternion.Euler(randomRotation);
            this.rb[i].AddRelativeTorque(orientation * speed * speed, ForceMode.VelocityChange);
        }
        this.throwButton.SetActive(false);
        this.isThrowed = true;
        Invoke("SetCanGetScore", 0.1f);
    }
    private void OnEnable()
    {
        Debug.Log("Enable roi ne");
        if(this.gameController.IsTurnOfPlayer())
        {
            this.pointer.SetActive(true);
            Debug.Log("Turn Of Player");
        }
    }
    private void OnDisable()
    {
        if(pointer) this.pointer.SetActive(false);
    }
    #region Get
    public bool GetIsThrowed()
    {
        return this.isThrowed;
    }
    public bool GetIsCanGetScore()
    {
        return this.isCanGetScore;
    }
    #endregion

    #region Set
    public void SetThrowButton(bool throwButton)
    {
        this.throwButton.SetActive(throwButton);
        
    }
    public void SetIsThrowed(bool isThrowed)
    {
        this.isThrowed = isThrowed;
    }
    public void SetCanGetScore()
    {
        this.isCanGetScore = true;
    }
    public void SetIsCanGetScore(bool isCanGetScore)
    {
        this.isCanGetScore = isCanGetScore;
    }
    #endregion
}
