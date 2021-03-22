using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextRemove : MonoBehaviour
{
    void Start()
    {
        transform.position = new Vector3(transform.position.x + 0.5f, transform.position.y + 1.5f, transform.position.z + 0.5f);
        transform.Rotate(90, 0, 0);
        Invoke("Remove", 0.5f);
    }

    void Remove()
    {
        Destroy(gameObject);
    }
}
