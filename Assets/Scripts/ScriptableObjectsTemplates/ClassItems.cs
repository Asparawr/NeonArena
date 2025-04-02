using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ClassItems", menuName = "ClassItems")]
public class ClassItems : ScriptableObject
{
    public List<ShopItem> items;
}
