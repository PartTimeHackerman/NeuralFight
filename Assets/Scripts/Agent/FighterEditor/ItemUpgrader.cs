using UnityEngine;

public  class ItemUpgrader : MonoBehaviour
{

    public static ItemUpgrader ItemUpgraderInstance;

    private void Awake()
    {
        ItemUpgraderInstance = this;
    }

    public static ItemUpgrader Get()
    {
        return ItemUpgraderInstance;
    }

    public void Upgrade(Item toDestroy, Item toUpgrade)
    {
        
        toUpgrade.Upgrade(toUpgrade.Level + 1);
        Storage.SaveItem(toUpgrade);
        Storage.DeleteItem(toDestroy);
        Destroy(toDestroy.gameObject);
        
    }
    
}