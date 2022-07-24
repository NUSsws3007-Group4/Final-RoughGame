using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName="New Item",menuName="Inventory/New Item")]

public class itemstruct : ScriptableObject
{
    public string itemname;
    public Sprite itemimage;
    public int itemnum;
    public int itemnuminstore;
    public bool locked; //locked or unlocked
    public int itemprice;
    [TextArea]
    public string iteminfo;
    public string itemtype;
}
