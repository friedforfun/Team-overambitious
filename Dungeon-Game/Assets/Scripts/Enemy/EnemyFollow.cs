using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    public NavMeshAgent enemy;
    public Transform Player;
    // Start is called before the first frame update
    void Start()
    {
        //Find the transform of game objects tagged "Player" 
        Player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        // Follow the player by setting a destination
        enemy.SetDestination(Player.position);   
    }
}
