using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class CameraManager : MonoBehaviour
{ // SC-6794-49ZV-Y93J-MT3T-9NDJ
    Camera realMainCamera;
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
    RaycastHit raycastHit;
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
        if (CanvasManager.UIActive == false && defaultCamera.gameObject.activeSelf && Physics.Raycast(defaultCamera.transform.position, defaultCamera.transform.forward, out raycastHit, 2.7f,layerMask))
        {
            Debug.Log(raycastHit.transform.gameObject.name);

            Debug.DrawLine(defaultCamera.transform.position, raycastHit.point,Color.red,0.1f);
            if (raycastHit.transform.gameObject.CompareTag("Item"))
            {
                Debug.DrawLine(defaultCamera.transform.position, raycastHit.point, Color.blue, 0.1f );
            }
        }
    }
}
