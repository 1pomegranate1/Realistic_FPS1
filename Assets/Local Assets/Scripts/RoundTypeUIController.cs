using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundTypeUIController : MonoBehaviour
{
    public delegate void RoundAddDelgate(int i);
    public RoundAddDelgate roundAdd;
    public WeaponScript.Round round = new WeaponScript.Round();
    public int ii;
    [SerializeField]
    Text roundNameText, roundStatText;
    public Button button;
       
    private void Start()
    {
        roundNameText.text = round.roundName;
        roundStatText.text = $"Damage: {round.damage}\n Tracer: {(round.isTracer ? "o" : "x")}";
        button.onClick.AddListener(FUck);
    }
    public void FUck()
    {
        Debug.Log("FFFFFFFFFUUUUUUUUUCCCCCCCKKKKKKKKKK1!!!!!!!!!!");
    }
}
