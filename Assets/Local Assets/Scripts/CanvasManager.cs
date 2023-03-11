using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    //static public 
    static public bool UIActive = true;
    
    [SerializeField]
    GameObject visiblePanel;
    // Start is called before the first frame update
    void Start()
    {
        //Application.targetFrameRate = 60;
        PanelActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            PanelActive(!UIActive);
        }
    }
    void PanelActive(bool isActive)
    {
        if (UIActive == isActive)
        {
            visiblePanel.SetActive(isActive);
            return;
        }
        UIActive = isActive;
        visiblePanel.SetActive(isActive);
        Cursor.visible = isActive;
        if (isActive)
            Cursor.lockState = CursorLockMode.None;
        else
            Cursor.lockState = CursorLockMode.Locked;
    }
   
}
