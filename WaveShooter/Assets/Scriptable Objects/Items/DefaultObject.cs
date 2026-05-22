using UnityEngine;
[CreateAssetMenu(fileName = "New Default Object", menuName = "Inventory System/Weapons/Default")]
public class DefaultObject : ItemObject
{
    public void Awake()
    {
        type = ItemType.Default;
    }

}
