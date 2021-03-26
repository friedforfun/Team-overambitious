using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinaleRoom : MonoBehaviour
{
    [SerializeField] private Transform LPlayerSpawn;
    [SerializeField] private Transform RPlayerSpawn;

    public Transform GetPlayerSpawn(Team team)
    {
        if (team == Team.LEFT)
            return LPlayerSpawn;

        else
            return RPlayerSpawn;
    }


}
