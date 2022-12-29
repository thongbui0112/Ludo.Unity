using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    [SerializeField] private Transform[] diceSide = new Transform[6];
   
    [SerializeField] private float maxDistance;
    [SerializeField] private bool isLanded;
    [SerializeField] private LayerMask layerMask;
    
    [SerializeField] private int score;
    [SerializeField] private Transform startPositionDice;
    public void SetPosition()
    {
        this.transform.position = startPositionDice.position;
    }
    private Rigidbody rb;
    private void Start()
    {
        this.rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        this.Score();
    }
    public void Score()
    {
        RaycastHit hit;
        for(int i = 0; i < diceSide.Length; i++)
        {
            Debug.DrawRay(this.diceSide[i].position, this.diceSide[i].forward * this.maxDistance);
            if (Physics.Raycast(this.diceSide[i].position, this.diceSide[i].forward, out hit, this.maxDistance, this.layerMask))
            {
                this.rb.angularVelocity = Vector3.zero;
                this.rb.velocity = Vector3.zero;
                this.isLanded = true;
                this.score = i + 1;
            }
        }
        if (this.rb.velocity != Vector3.zero) this.isLanded = false;
    }
    #region Get
    public bool GetIsLanded()
    {
        return this.isLanded;
    }
    public int GetScore()
    {
        return this.score; 
    }
    #endregion
    public void ShowScore()
    {
        Debug.Log(this.score);
    }
}
