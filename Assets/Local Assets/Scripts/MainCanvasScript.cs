using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCanvasScript : MonoBehaviour
{ 

    static public Canvas MainCanvas;
    private void Awake()
    {
        MainCanvas = GetComponent<Canvas>();
    }
}
