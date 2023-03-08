using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using static WeaponScript;

public class HUDManager : MonoBehaviour
{
    [SerializeField]
    WeaponScript weaponScript;
    public static HUDManager instance;  
    [SerializeField]
    GameObject MagazineHUDPrefab;
    [SerializeField]
    GameObject installMagazineHUD;
    [SerializeField]
    GameObject RoundInChamberHUD;
    [SerializeField]
    Transform HUDGroup;
    [SerializeField]
    List<GameObject> HUDList = new List<GameObject>();

    [Header("RoundTypeHUD"), SerializeField]
    GameObject RoundTypeHUD;
    Text roundTypeHUDText;
    int showTargetType;
    bool show;
    [SerializeField]
    float showTime, paidTime, showLerfTime;

    void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        roundTypeHUDText = RoundTypeHUD.GetComponent<Text>();
    }
    void Update()
    {
        if (show == false)
            return;

        showLerfTime -= Time.deltaTime;
        if(showLerfTime <= 0)
        {
            show = false;
            RoundTypeHUD.SetActive(false);
            showLerfTime = 0;
        }
        else if(showLerfTime <= paidTime)
        {
            roundTypeHUDText.color = new Color(1,1,1, showLerfTime/paidTime);
        }        
    }

    public void RoundTypeHUDShow(int targetType) // 0 => roundInChamber, 1 => magazineLastRound
    {
        if (!(targetType == 0 || targetType == 1))
            return;

        roundTypeHUDText.color = new Color(1, 1, 1, 1);

        showLerfTime = paidTime + showTime;
        show = true;
        RoundTypeHUD.SetActive(true);
        switch (targetType)
        {
            case 0:
                showTargetType = 0;
                if (weaponScript.roundInChamber == null)
                    roundTypeHUDText.text = "NULL";
                else
                    roundTypeHUDText.text = weaponScript.roundInChamber.roundName;

                break;

            case 1:
                showTargetType = 1;
                if (weaponScript.installMagazine == null)
                    roundTypeHUDText.text = "NULL";
                else if (weaponScript.installMagazine.rounds.Count <= 0)
                {
                    roundTypeHUDText.text = "NULL";
                }
                else
                    roundTypeHUDText.text = weaponScript.installMagazine.rounds[weaponScript.installMagazine.rounds.Count - 1].roundName;

                break;
        }
    }

    public void MagazineHUDReload()
    {
        //Debug.Log("weaponScript.magazines.Count: " + weaponScript.magazines.Count + " ? " + "HUDList.Count: " + HUDList.Count);
        if (weaponScript.magazines.Count > HUDList.Count)
        {
            while (weaponScript.magazines.Count > HUDList.Count)
            {
                //Debug.Log("Instantiate HUD");
                HUDList.Add(Instantiate(MagazineHUDPrefab, HUDGroup));
            }
        }
        else if (weaponScript.magazines.Count < HUDList.Count)
        {
            while (weaponScript.magazines.Count < HUDList.Count)
            {
                // Remove HUD game object from the list
                GameObject hudObjectToRemove = HUDList[HUDList.Count - 1];
                HUDList.RemoveAt(HUDList.Count - 1);

                // Destroy the HUD game object
                Destroy(hudObjectToRemove);
            }
        }
        int i = 0;
        foreach (Magazine item in weaponScript.magazines)
        {
            // Debug.Log("i: " + i);
            //Debug.Log(HUDList[i].GetComponentInChildren<Image>());
            HUDList[i].transform.GetChild(0).GetComponent<Image>().fillAmount = (float)item.rounds.Count / (float)item.maxSize;
            i++;
        }
        if (weaponScript.installMagazine == null)
        {
            installMagazineHUD.SetActive(false);
        }
        else
        {
            if (installMagazineHUD.activeSelf == false)
            {
                installMagazineHUD.SetActive(true);
            }
            if (weaponScript.installMagazine.rounds != null)
                installMagazineHUD.transform.GetChild(0).GetComponent<Image>().fillAmount = weaponScript.installMagazine.rounds.Count / (float)weaponScript.installMagazine.maxSize;
        }
        if (weaponScript.roundInChamber == null)
        {
            RoundInChamberHUD.SetActive(false);
        }
        else
        {
            if (RoundInChamberHUD.activeSelf == false)
            {
                RoundInChamberHUD.SetActive(true);
            }
        }
    }
}
