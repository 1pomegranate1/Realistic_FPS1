using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPanelController : MonoBehaviour
{
    [SerializeField]
    List<InputField> InputFields = new List<InputField>();
    [SerializeField]
    WeaponScript weaponScript;
    private void Start()
    {
        Refresh();
    }
    public void RPMInputEnd(InputField inputField)
    {
        weaponScript.roundsPerMinute = uint.Parse(inputField.text);
    }
    public void RecoilY(InputField inputField)
    {
        weaponScript.y_recoilPlus = float.Parse(inputField.text);
    }
    public void RecoilX(InputField inputField)
    {
        weaponScript.x_recoilPlus = float.Parse(inputField.text);
    }
    public void SpreadY(InputField inputField)
    {
        weaponScript.recoilSpreadY = float.Parse(inputField.text);
    }
    public void RecoulRecoveryDelay(InputField inputField)
    {
        weaponScript.recoilRecoveryDelay = float.Parse(inputField.text);
    }
    public void CameraRecoilY(InputField inputField)
    {
        weaponScript.CameraRecoilPlus = float.Parse(inputField.text);
    }



    public void RefreshButton() => Refresh();

    void Refresh()
    {
        foreach (var item in InputFields)
        {
            switch (item.transform.parent.name)
            {
                case "@Rpm":
                    item.text = weaponScript.roundsPerMinute.ToString();
                    break;
                case "@RecoilX":
                    item.text = weaponScript.x_recoilPlus.ToString();
                    break;
                case "@RecoilY":
                    item.text = weaponScript.y_recoilPlus.ToString();
                    break;
                case "@SpreadY":
                    item.text = weaponScript.recoilSpreadY.ToString();
                    break;
                case "@recoilRecoveryDelay":
                    item.text = weaponScript.recoilRecoveryDelay.ToString();
                    break;
                case "@recoilCameraPlus":
                    item.text = weaponScript.CameraRecoilPlus.ToString();
                    break;

                default:
                    Debug.Log("Don't Find");
                    return;
            }

        }
    }

}
