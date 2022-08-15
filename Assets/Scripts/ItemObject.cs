using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ItemType
{
	Costume,
	GunSkin
}
public abstract class ItemObject : ScriptableObject
{
	public GameObject prefab;
	public ItemType type;
	[TextArea(15, 20)]
	public string description;
}