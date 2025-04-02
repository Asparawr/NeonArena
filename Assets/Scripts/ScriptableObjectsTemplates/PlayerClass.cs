using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Class", menuName = "PlayerClass")]
public class PlayerClass : ScriptableObject
{
    public string className;
    public ClassItems items;
    public GameObject playerModel;
    public GameObject playerProjectile;
    public bool meeleClass = false;

}
