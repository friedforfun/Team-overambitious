﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateContents : MonoBehaviour
{
    public int EnemyPoints;

    public List<GameObject> EnemySpawns;
    public List<GameObject> Enemies;
    public List<int> EnemyCosts;

    public List<GameObject> ObstacleSpawns;
    public List<GameObject> Obstacles;

    private int framecount;
    public int EnemyRoomMax;
    public int ObsticalRoomMax;

    private RoomTemplates Templates;
    private bool RoomSpawned = false;

    // Start is called before the first frame update
    void Start()
    {
        framecount = 0;
        Templates = GameObject.Find("ScriptHolder").GetComponent<RoomTemplates>();
        EnemyRoomMax = (int) Mathf.Ceil(Templates.AverageRoomScore * Random.Range(0.6f, 1.4f));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && RoomSpawned==false)
        {
            SpawnEnemies();
            SpawnObsticals();
            RoomSpawned = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    void SpawnEnemies()
    {
        int EnemyValue = 0;
        while (EnemyValue < EnemyRoomMax && EnemySpawns.Count > 0)
        {
            int EnemySpawnLocation = Random.Range(0, EnemySpawns.Count);
            var EnemySpawn = EnemySpawns[EnemySpawnLocation];

            int EnemyIndex = Random.Range(0, Enemies.Count);


            if (EnemyCosts[EnemyIndex] <= EnemyRoomMax + EnemyValue)
            {
                var Enemy = Enemies[EnemyIndex];
                EnemyValue = EnemyValue + EnemyCosts[EnemyIndex];
                Instantiate(Enemies[EnemyIndex], EnemySpawn.transform.position, Quaternion.identity, transform.gameObject.transform);
                //Debug.Log(transform.gameObject.transform.name);
                EnemySpawns.Remove(EnemySpawn);
            }

        }
    }

    void SpawnObsticals()
    {
        int ObstacleValue = 0;
        while (ObstacleValue < ObsticalRoomMax && ObstacleSpawns.Count > 0)
        {
            int ObstacleSpawnLocation = Random.Range(0, ObstacleSpawns.Count);
            var ObstacleSpawn = ObstacleSpawns[ObstacleSpawnLocation];

            int ObstacleIndex = Random.Range(0, Obstacles.Count);

            var Obstacle = Obstacles[ObstacleIndex];
            ObstacleValue += 1;
            Instantiate(Obstacles[ObstacleIndex], ObstacleSpawn.transform.position, Quaternion.identity, transform.gameObject.transform);
            ObstacleSpawns.Remove(ObstacleSpawn);

        }
    }
}
