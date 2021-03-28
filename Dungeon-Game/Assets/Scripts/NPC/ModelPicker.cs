using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelPicker : MonoBehaviour
{
    [SerializeField] GameObject[] Models;

    public int ModelIndex = -1;

    private void Start()
    {
        RandomModel();
    }

    public void SelectByIndex(int index)
    {
        if (Models.Length > 0)
        {
            if (ModelIndex > -1)
            {
                Models[ModelIndex].SetActive(false);
            }

            ModelIndex = index;
            Models[ModelIndex].SetActive(true);
        }
    }

    public void RandomModel() 
    {   

        if (Models.Length > 0)
        {
            if (ModelIndex > -1)
            {
                Models[ModelIndex].SetActive(false);
            }

            ModelIndex = Random.Range(0, Models.Length);
            Models[ModelIndex].SetActive(true);
        }
    }

}
