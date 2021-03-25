using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomisePiece : MonoBehaviour
{
    public List<GameObject> PieceSelections;
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("placed");
        int PieceIndex = Random.Range(0, PieceSelections.Count);
        Instantiate(PieceSelections[PieceIndex], transform.position, Quaternion.identity, transform.gameObject.transform);
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.frameCount == 1)
        {
           
        }
    }
}
