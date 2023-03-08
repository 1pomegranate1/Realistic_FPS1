using InfimaGames.LowPolyShooterPack;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static WeaponScript;

public class MagazineSpawnPanelController : MonoBehaviour
{
    [SerializeField]
    WeaponScript weaponScript;
    WeaponScript.Magazine magazine = new WeaponScript.Magazine();
    List<Round> roundTypes = new List<Round>();
    GameObject selObject;
    [SerializeField]
    ScriptableObject magazineData;
    [SerializeField]
    List<RoundScriptableObject> RoundTypeData = new List<RoundScriptableObject>();
    [SerializeField]
    List<Image> images = new List<Image>();
    [SerializeField]
    GameObject RoundUIParent, RoundTypeUIParent;

    [SerializeField]
    GameObject RoundUIPrefab, RoundTypeUIPrefab;



    private void Start()
    {
        magazine = weaponScript.GetBasicMagazineType();
        foreach (RoundScriptableObject item in RoundTypeData)
        {
            roundTypes.Add(item.Round);
        }
        RoundTypeUILoad();
    }
    private void RoundTypeUILoad()
    {
        RoundTypeUIController roundTypeUIController;
        int i = 0;/*
        foreach (Round round in roundTypes)
        {
            GameObject roundType = Instantiate(RoundTypeUIPrefab, RoundTypeUIParent.transform);
            roundTypeUIController = roundType.GetComponent<RoundTypeUIController>();
            roundTypeUIController.round = round;
            int ii = i;
            roundType.GetComponent<Button>().onClick.AddListener(delegate { RoundAddButton(ii); });
            i++;
        }*/
        for(i = 0; i < roundTypes.Count; i++)
        {
            GameObject roundType = Instantiate(RoundTypeUIPrefab, RoundTypeUIParent.transform);
            roundTypeUIController = roundType.GetComponent<RoundTypeUIController>();
            roundTypeUIController.round = roundTypes[i];
            int temp = i;
            roundTypeUIController.button.GetComponent<Button>().onClick.AddListener(delegate { RoundAddButton(temp);});
        }
    }

    private void RoundsCheck()
    {
        for(int i = 0; i < RoundUIParent.transform.childCount; i++)
        {
            Destroy(RoundUIParent.transform.GetChild(i).gameObject);
        }
        foreach(Round item in magazine.rounds)
        {
            GameObject RoundUI = Instantiate(RoundUIPrefab, RoundUIParent.transform);
            RoundUI.GetComponentInChildren<Text>().text = item.roundName;
        }
    }
    public void ExitButton()
    {
        gameObject.SetActive(false);
    }
    public void MagazineDelete()
    {
        magazine = weaponScript.GetBasicMagazineType();
        RoundsCheck();

    }
    public void MagzineAdd()
    {
        weaponScript.magazines.Add(magazine);
        magazine = weaponScript.GetBasicMagazineType();
        RoundsCheck();
        HUDManager.instance.MagazineHUDReload();
    }
    public void RoundAddButton(int roundType)
    {
        if (roundType >= 0 && roundType <= roundTypes.Count - 1)
        {
            magazine.RoundAdd(roundTypes[roundType]);
        }
        else
            magazine.RoundAdd(roundTypes[Random.Range(0, roundTypes.Count)]);

        RoundsCheck();
    }
    public void FullMagazine762x39mmFMJ()
    {
        magazine.rounds.Clear();
        for (int i = 0; i < magazine.maxSize; i++)
        {
            magazine.rounds.Add(new Round());
            magazine.rounds[i] = roundTypes[0];
        }
        MagzineAdd();
        RoundsCheck();
    }
    /*
    public void RoundSelected(GameObject gameObject)
    {
        if (selObject)
            selObject.GetComponent<Image>().color = Color.white;

        selObject = gameObject;
        selObject.GetComponent<Image>().color = new Color(0.7f, 0.7f, 0.7f);
    }*/
}
