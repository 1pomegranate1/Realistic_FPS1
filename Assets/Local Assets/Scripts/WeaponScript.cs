
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using Random = UnityEngine.Random;

public class WeaponScript : MonoBehaviour
{
    public enum TriggerType
    {
        Semi,
        Auto,
        ShotGun
    }
    [Serializable]
    public class Magazine
    {
        public short maxSize;
        public List<Round> rounds;

        public Magazine MagazineDeepCopy()
        {
            Magazine clone = new Magazine();
            clone.maxSize = maxSize;
            clone.rounds = rounds.ToList();
            return clone;
        }

        public void RoundAdd(Round round)
        {
            if(rounds.Count < maxSize)
            {
                rounds.Add(round);
            }
            else
            {
                Debug.Log($"<color=yellow>RoundAdd</color><color=red>Error :maxSize={maxSize}</color>");
            }
        }
        public Round RoundOut()
        {
            Round round;
            round = rounds[rounds.Count - 1];
            rounds.RemoveAt(rounds.Count - 1);

            return round;
        }
    }

    [SerializeField]
    public Magazine BasicMagazineType;
    public Magazine GetBasicMagazineType()
    {
        return BasicMagazineType.MagazineDeepCopy();
    }
    public List<Magazine> magazines = new List<Magazine>();
    public Magazine installMagazine;

    [Serializable]
    public class Round
    {
        public string roundName;
        public bool isTracer;
        public float damage;
        public Round RoundDeepCopy()
        {
            Round clone = new Round();
            clone.roundName = roundName;
            clone.isTracer = isTracer;
            clone.damage = damage;
            return clone;
        }
    }
    public Round roundInChamber;

    [SerializeField]
    PlayerMovement playerMovement;
    [SerializeField]
    AnimationCurve accuracyCurve;
    TriggerType triggerType = TriggerType.Semi;
    [SerializeField]
    GameObject BulletPrefab;
    [SerializeField]
    Camera mainCamera;

    [SerializeField]
    Transform muzzleTransform;
    [SerializeField]
    GameObject Gun;
    [SerializeField]
    Light gunLight;




    float accuracy;

    [SerializeField]
    public uint roundsPerMinute;
    float fireDelayTime;
    [SerializeField]
    public float recoilRecoveryDelay;
    [SerializeField]
    public float recoilSpreadX, recoilSpreadY;
    bool fireing = false;
    byte lightMode = 0;
    [SerializeField]
    GameObject BoxPrefab;
    private void Start()
    {
        roundInChamber = null;
        installMagazine = null;
        HUDManager.instance.MagazineHUDReload();
        for (int i = 0; i < 100; i++)
        {
            Instantiate(BoxPrefab, new Vector3(Mathf.Clamp01(accuracyCurve.Evaluate(Random.Range(0f, 1f)))-0.5f, 1/100f, Mathf.Clamp01(accuracyCurve.Evaluate(Random.Range(0f, 1f)))-0.5f).normalized * 100, Quaternion.identity);
        }
    }
    float jInputTime, mInputTime;
    bool jInputEnd, mInputEnd;
    void Update()
    {
        // 1 -> 0
        //defaultCamera.transform.Rotate(cameraRecoilPluspref =+ Mathf.Lerp(0, -CameraRecoilPlus - cameraRecoilPluspref, fireDelayTime / ((float)(1f / (roundsPerMinute / 60)))), 0, 0);
        //Debug.Log($"CamerRecoil: {cameraRecoilPluspref}, {-CameraRecoilPlus - cameraRecoilPluspref}");
        if (fireDelayTime <= 0)
        {
            TryFire();
        }
        else
            fireDelayTime -= Time.deltaTime;
        if (CanvasManager.UIActive == true)
        {
            return;
        }
        else
        {

            if (Input.GetKeyDown(KeyCode.V))
            {
                if (Enum.GetValues(typeof(TriggerType)).Length <= (int)triggerType + 1)
                    triggerType = TriggerType.Semi;
                else
                    triggerType++;

                Debug.Log($"<color=pink>{triggerType}</color>");
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                if (lightMode <= 0)
                {
                    lightMode = 1;
                    gunLight.gameObject.SetActive(true);
                    gunLight.intensity = 1f;
                }
                else if (lightMode == 1)
                {
                    lightMode = 2;
                    gunLight.intensity = 4.23f;
                }
                else
                {
                    lightMode = 0;
                    gunLight.gameObject.SetActive(false);
                }
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                int mostCount = 0;
                int i = 0, mosti = 0;
                foreach (Magazine item in magazines)
                {
                    if (mostCount < item.rounds.Count)
                    {
                        mostCount = item.rounds.Count;
                        mosti = i;
                        Debug.Log($"mostCount = {mostCount}, {i}");
                    }
                    i++;
                }

                if (installMagazine != null)
                {
                    magazines.Add(installMagazine.MagazineDeepCopy());
                    Debug.Log("magazine!=null");
                }

                installMagazine = magazines[mosti].MagazineDeepCopy();
                magazines.RemoveAt(mosti);
                if (roundInChamber == null && installMagazine.rounds.Count > 0)
                {
                    reloadChamber();
                }
                Debug.Log("MagazineChange");
                HUDManager.instance.MagazineHUDReload();
            }

            //////////////////////////////////////////////////////////////////////////
            if (Input.GetKey(KeyCode.J))
            {
                jInputTime += Time.deltaTime;
                if(jInputTime >= 0.22f && jInputEnd == false)
                {
                    HUDManager.instance.RoundTypeHUDShow(0);
                    jInputEnd = true;
                }
            }
            else if (Input.GetKeyUp(KeyCode.J))
            {
                if(jInputTime < 0.22f && roundInChamber != null)
                {
                    reloadChamber();
                    HUDManager.instance.MagazineHUDReload();
                }
                jInputTime = 0;
                jInputEnd = false;
            }

            ////////////////////////////////////////////////////////////////////////////
            if (Input.GetKey(KeyCode.M))
            {
                mInputTime += Time.deltaTime;
                if (mInputTime >= 0.22f && mInputEnd == false)
                {
                    HUDManager.instance.RoundTypeHUDShow(1);
                    mInputEnd = true;
                }
            }
            else if (Input.GetKeyUp(KeyCode.M))
            {
                if (mInputTime < 0.22f && installMagazine != null)
                {
                    detachMagazine();
                    HUDManager.instance.MagazineHUDReload();
                }
                mInputTime = 0;
                mInputEnd = false;
            }
        }
    }

    void reloadChamber()
    {
        if (installMagazine != null && installMagazine.rounds.Count > 0)
        {
            roundInChamber = installMagazine.RoundOut();
        }
        else
        {
            roundInChamber = null;
        }
    }
    void detachMagazine()
    {
        if(installMagazine != null)
        {
            magazines.Add(installMagazine);
            installMagazine = null;
            Debug.Log(installMagazine == null);
        }
    }

    void TryFire()
    {
        if (CanvasManager.UIActive || roundInChamber == null)
        {
            Debug.Log("NoAmmo");
            return;
        }
        switch (triggerType)
        {
            case TriggerType.Semi:
                if (Input.GetMouseButtonDown(0))
                {
                    accuracy = 0.15f;
                    shotGunRPMPower = 1f;
                    Shot(1);
                }
                break;

            case TriggerType.Auto:

                if (Input.GetMouseButton(0))
                {
                    accuracy = 0.15f;
                    Shot(1);
                }

                break;
            case TriggerType.ShotGun:
                if (Input.GetMouseButton(0))
                {
                    accuracy = 1.2f;
                    shotGunRPMPower = 1.5f;
                    Shot(12);
                }
                break;
        }
        if (fireDelayTime < 0)
            fireDelayTime = 0;
    }
    float shotGunRPMPower;
    //float fireDelayTimeError;
    [SerializeField]
    public float x_recoilPlus,y_recoilPlus;
    [SerializeField]
    Vector3 fireCorrection = new Vector3(89.8799973f, 0, 0);
    public float CameraRecoilPlus;

    float cameraRecoilPluspref;

    GameObject bullet;
    void Shot(int bulletMakeCount)
    {
        
        //Debug.Log(Gun.transform.rotation.eulerAngles);  
        //   Instantiate(BulletPrefab, muzzleTransform.position, Quaternion.Euler(Gun.transform.rotation.eulerAngles - new Vector3(89.7f, 0.4f, 180.4f)));

        //var fireDirection = Quaternion.FromToRotation(transform.rotation.eulerAngles,Vector3.up);
        //fireDirection = Quaternion.AngleAxis(Random.Range(-1f, 1f), Vector3.up) * transform.rotation;

        Round round = roundInChamber;
        if (bulletMakeCount == 1)
        {
            bullet = Instantiate(BulletPrefab, muzzleTransform.position, Quaternion.Euler(Gun.transform.rotation.eulerAngles)
                    * Quaternion.Euler(fireCorrection) * Quaternion.Euler(new Vector2(Mathf.Clamp01(accuracyCurve.Evaluate(Random.Range(0f, 1f))) - 0.5f, Mathf.Clamp01(accuracyCurve.Evaluate(Random.Range(0f, 1f))) - 0.5f) * accuracy));
            bullet.GetComponent<BulletScript>().damage = round.damage;
        }
        else
        {
            for (int i = 0; i < bulletMakeCount; i++)
            {
                bullet = Instantiate(BulletPrefab, muzzleTransform.position, Quaternion.Euler(Gun.transform.rotation.eulerAngles)
                    * Quaternion.Euler(fireCorrection) * Quaternion.Euler(new Vector2(Mathf.Clamp01(accuracyCurve.Evaluate(Random.Range(0f, 1f))) - 0.5f, Mathf.Clamp01(accuracyCurve.Evaluate(Random.Range(0f, 1f))) - 0.5f) * accuracy));
                bullet.GetComponent<BulletScript>().damage = round.damage;
                //bullet.GetComponent<BulletScript>().threePersonView.AddComponent<Camera>();
                //Debug.Log(Mathf.Clamp01(accuracyCurve.Evaluate(Random.Range(0f, 1f))));
            }
        }
        Debug.Log("RoundOutName: " + round.roundName + round.isTracer);
        if (round.isTracer)
        {
            Debug.Log("isTracer");
        }
        reloadChamber();
        HUDManager.instance.MagazineHUDReload();

        fireDelayTime = (1f / ((float)roundsPerMinute / 60)) * shotGunRPMPower + fireDelayTime;
        Debug.Log($"<color=orange>{fireDelayTime}</color>");
        playerMovement.aimRecoil.x = Mathf.Clamp(playerMovement.aimRecoil.x + Random.Range(-x_recoilPlus * (1 + recoilSpreadX), x_recoilPlus * (1 + recoilSpreadX))  * shotGunRPMPower, -playerMovement.aimMaxRecoil, playerMovement.aimMaxRecoil);
        playerMovement.aimRecoil.y = Mathf.Clamp(playerMovement.aimRecoil.y + Random.Range(y_recoilPlus * (1 - recoilSpreadY), y_recoilPlus * (1 + recoilSpreadY)) * shotGunRPMPower, -playerMovement.aimMaxRecoil, playerMovement.aimMaxRecoil);
        playerMovement.recoilRecoveryDelay = (1f / ((float)roundsPerMinute / 60))* recoilRecoveryDelay ;
        cameraRecoilPluspref = 0;
        mainCamera.transform.Rotate(new Vector3(-CameraRecoilPlus *shotGunRPMPower, 0, 0));
        fireing = true;
    }
}
