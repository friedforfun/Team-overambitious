using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBar : MonoBehaviour
{

    public GameObject bar;
    public float multiplier;

    void Start()
    {
        bar = gameObject.transform.GetChild(1).gameObject;
        bar.transform.localScale = new Vector3(multiplier*0.4f, 0.1f, 1.0f);
        bar.transform.position = new Vector3(bar.transform.position.x - ((1f-multiplier)*0.95f), bar.transform.position.y, bar.transform.position.z);
        transform.Rotate(90, 0, 0);
        Invoke("Remove", 1f);
    }

    void Remove()
    {
        Destroy(gameObject);
    }
}
