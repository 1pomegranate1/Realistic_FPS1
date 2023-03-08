using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class FPSGUI : MonoBehaviour
{
    bool isGUI = true;
    public float deltaTime;
    public float fps;

    void Update()
    {
        deltaTime += (Time.deltaTime  - deltaTime ) * 0.1f;
        fps = 1.0f / deltaTime * Time.timeScale;
    }

    void OnGUI()
    {
        GUI.matrix = Matrix4x4.TRS(Vector3.one, quaternion.identity, Vector3.one * 2f);
        if (isGUI)
        {
            GUI.Box(new Rect(3f, 6, 120, 20), $"FPS : {fps.ToString("F1")}");

        }

    }
}
