using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailScript : MonoBehaviour
{
    float lerfTime;
    public float delaytime;
    public bool bulletDestryed;

    private void Update()
    {
        if (bulletDestryed)
        {
            if (lerfTime > delaytime)
                Destroy(gameObject);

            lerfTime += Time.deltaTime;
        }
    }

}
