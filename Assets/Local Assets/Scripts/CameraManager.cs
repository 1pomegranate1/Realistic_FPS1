using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    //Camera realMainCamera;
    [SerializeField]
    Camera defaultCamera, aimCamera;
    [SerializeField]
    LayerMask layerMask;
    


    void Start()
    {
        if (defaultCamera.gameObject.activeSelf)
        {
            HitManager.targetCam = defaultCamera;
        }
        else
        {
            HitManager.targetCam = aimCamera;
        }
    }

    
    void Update()
    {
        if (Input.GetMouseButtonDown(1) && CanvasManager.UIActive == false)
        {
            print(defaultCamera.gameObject.activeSelf);
            if (defaultCamera.gameObject.activeSelf)
            {
                defaultCamera.gameObject.SetActive(false);
                aimCamera.gameObject.SetActive(true);
                HitManager.targetCam = aimCamera;
            }
            else
            {
                defaultCamera.gameObject.SetActive(true);
                aimCamera.gameObject.SetActive(false);
                HitManager.targetCam = defaultCamera;
            }
        }
        RaycastHit[] hits = Physics.SphereCastAll(defaultCamera.transform.position, 2.7f ,Vector3.forward,2.7f,LayerMask.GetMask("Item"));
        RaycastHit raycastHit;
                //Debug.Log("item"+ hits.Length);
        if (CanvasManager.UIActive == false && defaultCamera.gameObject.activeSelf && hits.Length > 0)
        {
            foreach (var item in hits)
            {
                float angle = Vector3.Dot(item.transform.position, defaultCamera.transform.forward + transform.position);
                Debug.DrawLine(defaultCamera.transform.position, item.transform.position, Color.red, 1f);
                    Debug.Log(angle);
                if(angle < 15&& angle > -15)
                {

                   
                }

            }

            if (Physics.Raycast(defaultCamera.transform.position, defaultCamera.transform.forward, out raycastHit, 2.7f, layerMask) && raycastHit.transform.gameObject.CompareTag("Item")) 
            {
                Debug.DrawLine(defaultCamera.transform.position, raycastHit.point, Color.blue, 0.1f);
                if (HUDManager.itemCrossHairActive == false)
                    HUDManager.itemCrossHairActive = true;
            }
            else if (HUDManager.itemCrossHairActive)
                HUDManager.itemCrossHairActive = false;
        }
        else if (defaultCamera.gameObject.activeSelf == false)
        {
            if (HUDManager.itemCrossHairActive)
                HUDManager.itemCrossHairActive = false;
        }
    }
}
