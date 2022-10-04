using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;
using TMPro;

/*Written by Sean Ong on 4/10/22 3:10pm*/

public class ExchangeUI : MonoBehaviour
{
    public Transform container;
    public Transform exchangeItemTemplate;

    public int itemIndex;
    public int itemCount;

    private Transform EssExchangeButton;
    private Transform OreExchangeButton;
    public InventoryUI playerInventoryUI;
    public Inventory playerInventory;
    public GameItem creepDrop;
    public GameItem monsterEss;
    public GameItem SyntheticOre;

    // Start is called before the first frame update
    void Start()
    {
        //Assigning the relevant objects
        playerInventoryUI = FindObjectOfType<InventoryUI>();
        playerInventory = GameManager.Instance.PlayerInventory;
        container = transform.Find("Container");
        exchangeItemTemplate = container.Find("ExchangeItemTemplate");

        //Creating the initial buttons
        EssExchangeButton = CreateExchangeButton("EssExchange", Upgrade.EquipmentExchangeType.MonsterEss, "Monster Essence", Upgrade.ExchangeRate(Upgrade.EquipmentExchangeType.MonsterEss), Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.MonsterEss), 0);
        OreExchangeButton = CreateExchangeButton("OreExchange", Upgrade.EquipmentExchangeType.MonsterOre, "Synthetic Ore", Upgrade.ExchangeRate(Upgrade.EquipmentExchangeType.MonsterOre), Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.MonsterOre), 1);
    }

    //Function used to create the button for exchanging currency. Function used in Start()
    private Transform CreateExchangeButton(string buttonType, Upgrade.EquipmentExchangeType Exchange, string itemName, int itemCost, string currency, int positionIndex)
    {
        //Similar concept as the CreateUpgradeButton function
        Transform exchangeTransform = Instantiate(exchangeItemTemplate, container);
        //vendorItemTransform.gameObject.SetActive(true);
        RectTransform exchangeRectTransform = exchangeTransform.GetComponent<RectTransform>();

        float vendorItemHeight = 90f;
        exchangeRectTransform.anchoredPosition = new Vector2(0, -vendorItemHeight * positionIndex);
        exchangeTransform.name = itemName;

        exchangeTransform.Find("exchangeText").GetComponent<TextMeshProUGUI>().SetText(itemName);
        exchangeTransform.Find("costText").GetComponent<TextMeshProUGUI>().SetText(itemCost.ToString());

        exchangeTransform.GetComponent<Button_UI>().ClickFunc = () =>
        {
            exchangeItem(Exchange, itemCost, itemName);
        };

        return exchangeTransform;
    }



    //Code for Exchanging Monster Junk with Monster Essence
    private void exchangeItem(Upgrade.EquipmentExchangeType item, int itemCost, string itemName)
    {
        itemIndex = playerInventory.GetItemIndex(creepDrop);
        try
        {
            itemCount = GameManager.Instance.PlayerInventory.Items[itemIndex].quantity;
        }
        catch
        {
            itemCount = 0;
        }
        //Exchanges the item with the relevant item.
        if (item == Upgrade.EquipmentExchangeType.MonsterEss && itemCount - itemCost >= 0)
        {
            GameManager.Instance.PlayerInventory.RemoveItem(creepDrop, 10);
            GameManager.Instance.PlayerInventory.AddItem(monsterEss, 1);
        }
        else if (item == Upgrade.EquipmentExchangeType.MonsterOre && itemCount - itemCost >= 0)
        {
            GameManager.Instance.PlayerInventory.RemoveItem(creepDrop, 10);
            GameManager.Instance.PlayerInventory.AddItem(SyntheticOre, 1);
        }
        else
        {
            Debug.Log("Not enough Materials");
        }

        Debug.Log("Exchanged");
    }
}
