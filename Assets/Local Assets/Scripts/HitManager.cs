using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitManager : MonoBehaviour
{
    public static HitManager instance;
    bool inFrameBulletHit;
    [SerializeField]
    bool endFrame;
    [SerializeField]
    Canvas canvas;
    [SerializeField]
    GameObject HitPrefab;
    static public Camera targetCam;
    [SerializeField]
    List<GameObject> hitPoints = new List<GameObject>();
    public Vector3 offset = new Vector3(0, -3, 0);
    [SerializeField]
    

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        StartCoroutine(frameCheck());
        wait = new WaitForSeconds(60f);
    }
    void Update()
    {

        if (endFrame)
        {
            endFrame = false;
        }
    }
    [SerializeField]
    WaitForSeconds wait;
    IEnumerator frameCheck()
    {
        while (true)
        {
            if (endFrame == false)
            {
                inFrameBulletHit = false;
                
                endFrame = true;
                //Debug.Log($"<color=purple>EndFrame : {endFrame}</color>");
            }
            yield return wait;
        }
    }
    public void BulletHit(Vector3 hitPoint, bool isHeadShot, float damage)
    {
        if (inFrameBulletHit == false)
        {
            HitPointReset();
        }

        GameObject Hit_G = Instantiate(HitPrefab, hitPoint + offset, Quaternion.identity, canvas.transform);
        HitScript hit = Hit_G.GetComponent<HitScript>();
        hit.damage = damage;
        hit.hitPosition = hitPoint;
        hit.headShot = isHeadShot;
        if (hitPoints.Count == 0)
            hit.firstHit = true;

        hitPoints.Add(Hit_G);
    }
    public void HitPointReset()
    {
        foreach (GameObject hit in hitPoints)
        {
            Destroy(hit);
        }
        hitPoints.Clear();
        inFrameBulletHit = true;
    }
}
