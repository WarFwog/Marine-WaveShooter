using UnityEngine;


public enum ItemType
{
    Money,
    Weapons,
    Default
}
public abstract class ItemObject : ScriptableObject 
{
    public GameObject prefab;
    public ItemType type;
    public string description;

    
}
