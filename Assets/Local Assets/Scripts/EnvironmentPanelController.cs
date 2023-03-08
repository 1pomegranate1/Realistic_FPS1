using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentPanelController : MonoBehaviour
{
    [SerializeField]
    HitManager hitManager;

    public void HitPointResetButton()
    {
        hitManager.HitPointReset();
    }



}
