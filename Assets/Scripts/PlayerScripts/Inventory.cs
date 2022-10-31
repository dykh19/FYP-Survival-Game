using System;

// Written by Nicholas Sebastian Hendrata on 16/08/2022.
[System.Serializable]
public class Inventory
{
    public static Inventory Main { get; private set; }

    public InventoryItem[] Items { get; private set; }
    public int activeItemIndex;
    private const int defaultInventorySize = 20;

    private const int maxItemStack = 10;

    public Inventory()
    {
        if (Main != null && Main != this)
        {
            Main = this;
        }
        
        Items = new InventoryItem[defaultInventorySize];
    }

    public void AddItem(GameItem newItem, int quantity = 1)
    {
        var itemIndex = GetNotFullItemIndex(newItem);

        if (itemIndex != -1)
            Items[itemIndex].quantity += quantity;
        else
        {
            for (int i = 0; i < Items.Length; i++)
                if (Items[i] is null)
                {
                    Items[i] = new InventoryItem(newItem, quantity);
                    break;
                }
        }

        InventoryUI.Main?.UpdateUI();
        PlayerHUD.Main?.GetComponent<PlayerHUDToolbar>().UpdateUI();
    }

    public void RemoveItem(GameItem item, int quantity = 1)
    {
        var itemIndex = GetFullItemIndex(item);
        if (itemIndex == -1)
        {
            itemIndex = GetNotFullItemIndex(item);
        }
        if (itemIndex != -1)
        {
            Items[itemIndex].quantity -= quantity;

            if (Items[itemIndex].quantity <= 0)
                Items[itemIndex] = null;
        }
        InventoryUI.Main?.UpdateUI();
        PlayerHUD.Main?.GetComponent<PlayerHUDToolbar>()?.UpdateUI();
    }

    public int GetItemIndex(GameItem item)
    {
        return Array.FindIndex(Items, invItem =>
            (invItem != null) && (invItem.item.name == item.name));
    }

    public int GetFullItemIndex(GameItem item)
    {
        return Array.FindIndex(Items, invItem =>
            (invItem != null) && (invItem.item.name == item.name) && invItem.quantity == maxItemStack);
    }

    public int GetNotFullItemIndex(GameItem item)
    {
        return Array.FindIndex(Items, invItem =>
            (invItem != null) && (invItem.item.name == item.name) && invItem.quantity != maxItemStack);
    }

    public void ResizeInventory(int newSize)
    {
        var newArray = new InventoryItem[newSize];
        var elementsToCopy = Math.Min(Items.Length, newArray.Length);

        Array.Copy(Items, newArray, elementsToCopy);
        Items = newArray;
    }

    public bool CheckIfCanRemoveFullItem(GameItem item)
    {
        if (GetFullItemIndex(item) != -1)
        {
            return true;
        }
        return false;
    }

    public bool CheckIfCanRemoveNotFullItem(GameItem item)
    {
        if (GetNotFullItemIndex(item) != -1)
        {
            return true;
        }
        return false;
    }
}

[System.Serializable]
public class InventoryItem
{
    public GameItem item;
    public int quantity;

    public InventoryItem(GameItem item, int quantity = 1)
    {
        this.item = item;
        this.quantity = quantity;
    }
}
