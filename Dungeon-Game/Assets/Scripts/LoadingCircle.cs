using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingCircle : MonoBehaviour
{
    private RectTransform wheelTransform;

    private void Start()
    {
        wheelTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        wheelTransform.Rotate(0f, 0f, -400f * Time.deltaTime);
    }
}
