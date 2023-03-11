using Mono.Cecil.Cil;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class HitScript : MonoBehaviour
{
    [SerializeField]
    Text DistanceText;
    Image Image;
    Vector3 screenPoint;
    public Vector3 offset = new Vector3(0,-3,0);

    public Vector3 hitPosition;
    Color hitColor;
    public float damage;
    public bool headShot;
    public bool firstHit;

    float screenWidth;
    float screenHeight;
    private void Start()
    {
        Debug.Log(Screen.width);
        Image = GetComponent<Image>();
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        if (firstHit == false)
        {
            DistanceText.gameObject.SetActive(false);
        }
        else
        {
            DistanceText.text = Vector3.Distance(HitManager.targetCam.transform.position, hitPosition).ToString("F1");
        }
        if (headShot)
        {
            damage *= 2;
        }
        GetComponent<Image>().color = new Color(1f,1 -  damage / 80, 1 - damage / 80);

    }

    void Update()
    {
        screenPoint = HitManager.targetCam.WorldToScreenPoint(hitPosition + offset);
        if (screenPoint.z < 0)
        {
            screenPoint *= -1;          
        }
        else
        {
            transform.position = screenPoint;//new Vector3(screenPoint.x, screenPoint.y, transform.position.z)}
            if(transform.position.x > screenWidth + screenWidth * 0.01f ||
                transform.position.x < 0 - screenWidth * 0.01f ||
                transform.position.y > screenHeight + screenHeight * 0.01f ||
                transform.position.y < 0 - screenHeight * 0.01f)
            { 
                Image.enabled = false;
            }
            else Image.enabled = true;
        }
    }
    public void DestroyHit()
    {
        Destroy(gameObject);
    }
}
