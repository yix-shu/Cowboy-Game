using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Costume Object", menuName = "Inventory System/Items/Default")]
public abstract class CostumeObject : ItemObject
{
	public void Awake()
    {
        type = ItemType.Costume;
    }
}

