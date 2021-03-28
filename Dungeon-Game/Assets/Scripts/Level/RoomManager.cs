using System.Collections;

using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public int RoomID;
    public Team team;
    private float spawnAreaSize = 5f;
    [SerializeField] bool AllowSpawn = true;

    private void Start()
    {
        
        if (int.TryParse(gameObject.name, out RoomID))
        {
            //Debug.Log($"Sucess registering ROOM ID: {gameObject.name}");
        } 
        else
        {
            Debug.Log($"Failed registering ROOM ID: {gameObject.name}");
        }
    }


    /// <summary>
    /// Spawn (spawnCount) minions in this room, with an offset on the y axis
    /// </summary>
    /// <param name="minion"></param>
    /// <param name="spawnCount"></param>
    /// <param name="minionOffset"></param>
    public void Spawn(GameObject minion, int spawnCount, float minionOffset)
    {
        if (AllowSpawn)
        {
            for (int i = 0; i < spawnCount; i++)
            {
                Vector3 spawnPosition = GetSpawnPosition(minionOffset);

                Quaternion spawnRotation = new Quaternion();
                spawnRotation.eulerAngles = new Vector3(0.0f, Random.Range(0.0f, 360.0f));
                if (spawnPosition != Vector3.zero)
                {
                    Instantiate(minion, spawnPosition, spawnRotation);
                }
            }
        }
        
    }

    public void SpawnGhost(GameObject minion, int varient)
    {
        Vector3 spawnPosition = GetSpawnPosition(1f);

        Quaternion spawnRotation = new Quaternion();
        spawnRotation.eulerAngles = new Vector3(0.0f, Random.Range(0.0f, 360.0f));
        if (spawnPosition != Vector3.zero)
        {
            GameObject ghost = Instantiate(minion, spawnPosition, spawnRotation);
            CapsuleState gc = ghost.GetComponent<CapsuleState>();
            gc.Ghostify(varient);
        }
    }

    Vector3 GetSpawnPosition(float minionOffset)
    {
        Vector3 spawnPosition = transform.position;
        float startTime = Time.realtimeSinceStartup;
        bool test = false;
        while (test == false)
        {
            Vector2 spawnPositionRaw = Random.insideUnitCircle * spawnAreaSize;
            spawnPosition = new Vector3(spawnPosition.x + spawnPositionRaw.x, minionOffset, spawnPosition.z + spawnPositionRaw.y);
            test = !Physics.CheckSphere(spawnPosition, 0.75f);
            if (Time.realtimeSinceStartup - startTime > 0.5f)
            {
                Debug.Log("Time out placing Minion!");
                return Vector3.zero;
            }
        }
        return spawnPosition;
    }

    public void SetTeam(Team team)
    {
        this.team = team;
    }
}

