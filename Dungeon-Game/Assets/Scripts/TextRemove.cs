using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextRemove : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Remove", 1f);
    }

    void Remove()
    {
        Destroy(gameObject);
    }
}
