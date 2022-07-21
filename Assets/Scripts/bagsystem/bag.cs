using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New Bag",menuName="Inventory/New Bag")]

public class bag : ScriptableObject
{
    public List<itemstruct> itemlist=new List<itemstruct>();
}
