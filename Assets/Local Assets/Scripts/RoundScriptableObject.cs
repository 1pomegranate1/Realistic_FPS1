using InfimaGames.LowPolyShooterPack;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

[CreateAssetMenu(fileName ="RoundType",menuName = "Scriptable Object/Round Type", order =int.MinValue)]
public class RoundScriptableObject : ScriptableObject
{
    [SerializeField]
    WeaponScript.Round round;

    public WeaponScript.Round Round { get { return round; } }
}
