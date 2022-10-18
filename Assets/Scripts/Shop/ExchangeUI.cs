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
    public GameObject PlayerOreCount;
    public GameObject PlayerEssenceCount;

    //public int itemIndex;
    //public int itemCount;

    private Transform JunkToEssExchangeButton;
    private Transform JunkToOreExchangeButton;
    private Transform RawOreToOreExchangeButton;
    private Transform AllRawOreToOreExchangeButton;
    public InventoryUI playerInventoryUI;
    public Inventory playerInventory;
    public GameItem monsterJunk;
    public GameItem monsterEss;
    public GameItem Damianite;
    public GameItem Eddirite;
    public GameItem Josephite;
    public GameItem Nicholite;
    public GameItem Seanite;

    // Start is called before the first frame update
    void Start()
    {
        //Assigning the relevant objects
        playerInventoryUI = FindObjectOfType<InventoryUI>();
        playerInventory = GameManager.Instance.PlayerInventory;
        container = transform.Find("Container");
        exchangeItemTemplate = container.Find("ExchangeItemTemplate");

        //Creating the initial buttons
        JunkToEssExchangeButton = CreateExchangeButton(Upgrade.EquipmentExchangeType.JunkToMonsterEssence, "Monster Essence", "Junk", Upgrade.ExchangeRate(Upgrade.EquipmentExchangeType.JunkToMonsterEssence), 0);
        JunkToOreExchangeButton = CreateExchangeButton(Upgrade.EquipmentExchangeType.JunkToRefinedOre, "Refined Ore", "Junk", Upgrade.ExchangeRate(Upgrade.EquipmentExchangeType.JunkToRefinedOre), 1);
        RawOreToOreExchangeButton = CreateExchangeButton(Upgrade.EquipmentExchangeType.RawOreToRefinedOre, "Refined Ore", "Ore", Upgrade.ExchangeRate(Upgrade.EquipmentExchangeType.RawOreToRefinedOre), 2);
        AllRawOreToOreExchangeButton = CreateExchangeButton(Upgrade.EquipmentExchangeType.AllRawOreToRefinedOre, "Refined Ore", "All Ore", Upgrade.ExchangeRate(Upgrade.EquipmentExchangeType.AllRawOreToRefinedOre), 3);
        UpdateResourceCount();
    }

    private void Update()
    {
        CheckIfCanExchange();
        UpdateResourceCount();
    }
    //Function used to create the button for exchanging currency. Function used in Start()
    private Transform CreateExchangeButton(Upgrade.EquipmentExchangeType Exchange, string givenItemName, string takenItemName, int takenItemCost, int positionIndex)
    {
        //Similar concept as the CreateUpgradeButton function
        Transform exchangeTransform = Instantiate(exchangeItemTemplate, container);
        //vendorItemTransform.gameObject.SetActive(true);
        RectTransform exchangeRectTransform = exchangeTransform.GetComponent<RectTransform>();

        float vendorItemHeight = 90f;
        exchangeRectTransform.anchoredPosition = new Vector2(0, -vendorItemHeight * positionIndex);
        exchangeTransform.name = givenItemName;
        if (takenItemName == "Junk")
        {
            exchangeTransform.Find("exchangeText").GetComponent<TextMeshProUGUI>().SetText("Exchange 10 Monster Junk for 1 " + givenItemName);
        }
        else if (takenItemName == "Ore")
        {
            exchangeTransform.Find("exchangeText").GetComponent<TextMeshProUGUI>().SetText("Exchange 1 Raw Ore for 1 " + givenItemName);
        }
        else if (takenItemName == "All Ore")
        {
            exchangeTransform.Find("exchangeText").GetComponent<TextMeshProUGUI>().SetText("Exchange all Raw Ore to " + givenItemName);
        }
        //exchangeTransform.Find("exchangeText").GetComponent<TextMeshProUGUI>().SetText(itemName);
        //exchangeTransform.Find("costText").GetComponent<TextMeshProUGUI>().SetText(itemCost.ToString());
        exchangeTransform.Find("costText").GetComponent<TextMeshProUGUI>().SetText("");
        exchangeTransform.GetComponent<Button_UI>().ClickFunc = () =>
        {
            exchangeItem(Exchange, takenItemCost, takenItemName);
        };

        return exchangeTransform;
    }



    //Code for Exchanging Monster Junk with Monster Essence
    private void exchangeItem(Upgrade.EquipmentExchangeType item, int takenItemCost, string takenItemName)
    {
        /*        itemIndex = playerInventory.GetItemIndex(creepDrop);
                try
                {
                    itemCount = GameManager.Instance.PlayerInventory.Items[itemIndex].quantity;
                }
                catch
                {
                    itemCount = 0;
                }*/
        //Exchanges the item with the relevant item.
        if (item == Upgrade.EquipmentExchangeType.JunkToMonsterEssence && takenItemName == "Junk")
        {
            GameManager.Instance.PlayerInventory.RemoveItem(monsterJunk, takenItemCost);
            GameManager.Instance.PlayerStats.AddEssence(1);
            Debug.Log("Exchanged Junk for Monster Essence");
        }
        else if (item == Upgrade.EquipmentExchangeType.JunkToRefinedOre && takenItemName == "Junk")
        {
            GameManager.Instance.PlayerInventory.RemoveItem(monsterJunk, takenItemCost);
            GameManager.Instance.PlayerStats.AddOres(1);
            Debug.Log("Exchanged Junk for Ore");
        }
        else if (item == Upgrade.EquipmentExchangeType.RawOreToRefinedOre && takenItemName == "Ore")
        {
            if (GameManager.Instance.PlayerInventory.CheckIfCanRemoveNotFullItem(Damianite))
            {
                GameManager.Instance.PlayerInventory.RemoveItem(Damianite, takenItemCost);
            }
            else if (GameManager.Instance.PlayerInventory.CheckIfCanRemoveNotFullItem(Eddirite))
            {
                GameManager.Instance.PlayerInventory.RemoveItem(Eddirite, takenItemCost);
            }
            else if (GameManager.Instance.PlayerInventory.CheckIfCanRemoveNotFullItem(Josephite))
            {
                GameManager.Instance.PlayerInventory.RemoveItem(Josephite, takenItemCost);
            }
            else if (GameManager.Instance.PlayerInventory.CheckIfCanRemoveNotFullItem(Nicholite))
            {
                GameManager.Instance.PlayerInventory.RemoveItem(Nicholite, takenItemCost);
            }
            else if (GameManager.Instance.PlayerInventory.CheckIfCanRemoveNotFullItem(Seanite))
            {
                GameManager.Instance.PlayerInventory.RemoveItem(Seanite, takenItemCost);
            }
            GameManager.Instance.PlayerStats.AddOres(1);
            Debug.Log("Exchanged Raw Ore for Ore");
        }
        else if (item == Upgrade.EquipmentExchangeType.AllRawOreToRefinedOre && takenItemName == "All Ore")
        {
            while (GameManager.Instance.PlayerInventory.CheckIfCanRemoveNotFullItem(Damianite) ||
            GameManager.Instance.PlayerInventory.CheckIfCanRemoveNotFullItem(Eddirite) ||
            GameManager.Instance.PlayerInventory.CheckIfCanRemoveNotFullItem(Josephite) ||
            GameManager.Instance.PlayerInventory.CheckIfCanRemoveNotFullItem(Nicholite) ||
            GameManager.Instance.PlayerInventory.CheckIfCanRemoveNotFullItem(Seanite))
            {
                if (GameManager.Instance.PlayerInventory.CheckIfCanRemoveNotFullItem(Damianite))
                {
                    GameManager.Instance.PlayerInventory.RemoveItem(Damianite, takenItemCost);
                    GameManager.Instance.PlayerStats.AddOres(1);
                }
                else if (GameManager.Instance.PlayerInventory.CheckIfCanRemoveNotFullItem(Eddirite))
                {
                    GameManager.Instance.PlayerInventory.RemoveItem(Eddirite, takenItemCost);
                    GameManager.Instance.PlayerStats.AddOres(1);
                }
                else if (GameManager.Instance.PlayerInventory.CheckIfCanRemoveNotFullItem(Josephite))
                {
                    GameManager.Instance.PlayerInventory.RemoveItem(Josephite, takenItemCost);
                    GameManager.Instance.PlayerStats.AddOres(1);
                }
                else if (GameManager.Instance.PlayerInventory.CheckIfCanRemoveNotFullItem(Nicholite))
                {
                    GameManager.Instance.PlayerInventory.RemoveItem(Nicholite, takenItemCost);
                    GameManager.Instance.PlayerStats.AddOres(1);
                }
                else if (GameManager.Instance.PlayerInventory.CheckIfCanRemoveNotFullItem(Seanite))
                {
                    GameManager.Instance.PlayerInventory.RemoveItem(Seanite, takenItemCost);
                    GameManager.Instance.PlayerStats.AddOres(1);
                }
            }
        }
            UpdateResourceCount();
    }

    private void CheckIfCanExchange()
    {
        if (GameManager.Instance.PlayerInventory.CheckIfCanRemoveFullItem(monsterJunk))
        {
            JunkToEssExchangeButton.GetComponent<Button>().interactable = true;
            JunkToOreExchangeButton.GetComponent<Button>().interactable = true;
            JunkToEssExchangeButton.GetComponent<Button_UI>().enabled = true;
            JunkToOreExchangeButton.GetComponent<Button_UI>().enabled = true;
        }
        else
        {
            JunkToEssExchangeButton.GetComponent<Button>().interactable = false;
            JunkToOreExchangeButton.GetComponent<Button>().interactable = false;
            JunkToEssExchangeButton.GetComponent<Button_UI>().enabled = false;
            JunkToOreExchangeButton.GetComponent<Button_UI>().enabled = false;
        }
        if (GameManager.Instance.PlayerInventory.CheckIfCanRemoveNotFullItem(Damianite) ||
            GameManager.Instance.PlayerInventory.CheckIfCanRemoveNotFullItem(Eddirite)  ||
            GameManager.Instance.PlayerInventory.CheckIfCanRemoveNotFullItem(Josephite) ||
            GameManager.Instance.PlayerInventory.CheckIfCanRemoveNotFullItem(Nicholite) ||
            GameManager.Instance.PlayerInventory.CheckIfCanRemoveNotFullItem(Seanite))
        {
            RawOreToOreExchangeButton.GetComponent<Button>().interactable = true;
            AllRawOreToOreExchangeButton.GetComponent<Button>().interactable = true;
            RawOreToOreExchangeButton.GetComponent<Button_UI>().enabled = true;
            AllRawOreToOreExchangeButton.GetComponent<Button_UI>().enabled = true;
        }
        else
        {
            RawOreToOreExchangeButton.GetComponent<Button>().interactable = false;
            AllRawOreToOreExchangeButton.GetComponent<Button>().interactable = false;
            RawOreToOreExchangeButton.GetComponent<Button_UI>().enabled = false;
            AllRawOreToOreExchangeButton.GetComponent<Button_UI>().enabled = false;
        }
    }

    private void UpdateResourceCount()
    {
        PlayerOreCount.GetComponent<TextMeshProUGUI>().text = "Total Refined Ores: " + GameManager.Instance.PlayerStats.CurrentOresInBase;
        PlayerEssenceCount.GetComponent<TextMeshProUGUI>().text = "Total Essence: " + GameManager.Instance.PlayerStats.CurrentEssenceInBase;
    }
}
