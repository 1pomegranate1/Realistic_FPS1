using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField]
    TrailScript trailScript;
    
    public GameObject threePersonView;
    public float damage;

     Rigidbody rb;
    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }
    private void Start()
    {
        rb.velocity = transform.forward * 730f;
        transform.up = rb.velocity;
    }
    float delayTime;
    private void FixedUpdate()
    {
        delayTime = delayTime + Time.fixedDeltaTime;
        if (transform.position.y < -500 || delayTime > 10)
        {
            trailScript.delaytime = 10f; 
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log($"<color=red>hit : {collision.gameObject.name}\n</color>time : {delayTime}");
        if (collision.gameObject.CompareTag("ShootingTarget") || collision.gameObject.CompareTag("Head"))
        {
            if (collision.gameObject.CompareTag("Head"))
                HitManager.instance.BulletHit(collision.GetContact(0).point, true, damage);
            else
                HitManager.instance.BulletHit(collision.GetContact(0).point, false, damage);

        }
        trailScript.delaytime =  3f;
        DestroyBullet();
    }
    void DestroyBullet()
    {
        if (trailScript.gameObject.activeSelf)
        {
            trailScript.transform.parent = null;
            trailScript.bulletDestryed = true;
        }
        else 

        Destroy(gameObject);
    }
}
