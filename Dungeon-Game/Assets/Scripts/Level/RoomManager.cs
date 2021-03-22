using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    private float spawnAreaSize = 5f;

    /// <summary>
    /// Spawn (spawnCount) minions in this room, with an offset on the y axis
    /// </summary>
    /// <param name="minion"></param>
    /// <param name="spawnCount"></param>
    /// <param name="minionOffset"></param>
    public void Spawn(GameObject minion, int spawnCount, float minionOffset)
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
}

