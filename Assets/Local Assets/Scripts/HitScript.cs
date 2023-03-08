using Mono.Cecil.Cil;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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

    private void Start()
    {
        Image = GetComponent<Image>();
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
        transform.position = screenPoint;//new Vector3(screenPoint.x, screenPoint.y, transform.position.z)}
    }
    public void DestroyHit()
    {
        Destroy(gameObject);
    }
}
