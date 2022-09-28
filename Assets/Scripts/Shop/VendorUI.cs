using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;
using TMPro;

/*Written by Sean Ong on 26/9/22 12:10pm*/

public class VendorUI : MonoBehaviour
{
    public Transform container;
    public Transform vendorItemTemplate;

    public int LMeleeLvl = 0;
    public int HMeleeLvl = 0;
    public int RifleLvl = 0;
    public int ShotgunLvl = 0;
    public int buttonCount = 0;
    public int itemIndex;
    public int itemCount;

    private Transform LMeleeButton;
    private Transform HMeleeButton;
    private Transform RifleButton;
    private Transform ShotgunButton;
    private Transform ExchangeButton;
    public InventoryUI playerInventoryUI;
    public Inventory playerInventory;
    public GameItem creepDrop;
    public GameItem monsterEss;

    private void Awake()
    {
    }

    private void Start()
    {
        //Assigning the relevant objects
        playerInventoryUI = FindObjectOfType<InventoryUI>();
        playerInventory = GameManager.Instance.PlayerInventory;
        container = transform.Find("Container");
        vendorItemTemplate = container.Find("VendorItemTemplate");
        //vendorItemTemplate.gameObject.SetActive(false);

        //Creating the initial buttons
        LMeleeButton = CreateUpgradeButton("LMelee", Upgrade.EquipmentExchangeType.LMelee_1, "Light Melee Up", 1/*Upgrade.GetCost(Upgrade.EquipmentExchangeType.LMelee_1)*/, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.LMelee_1), 0);
        HMeleeButton = CreateUpgradeButton("HMelee", Upgrade.EquipmentExchangeType.HMelee_1, "Heavy Melee Up", 1/*Upgrade.GetCost(Upgrade.EquipmentExchangeType.HMelee_1)*/, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.HMelee_1), 1);
        RifleButton = CreateUpgradeButton("Rifle", Upgrade.EquipmentExchangeType.Rifle_1, "Rifle Up", 1/*Upgrade.GetCost(Upgrade.EquipmentExchangeType.Rifle_1)*/, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Rifle_1), 2);
        ShotgunButton = CreateUpgradeButton("Shotgun", Upgrade.EquipmentExchangeType.Shotgun_1, "Shotgun Up", 1/*Upgrade.GetCost(Upgrade.EquipmentExchangeType.Shotgun_1)*/, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Shotgun_1), 3);
        ExchangeButton = CreateExchangeButton("Exchange", Upgrade.EquipmentExchangeType.MonsterEss, "Monster Essence", Upgrade.ExchangeRate(Upgrade.EquipmentExchangeType.MonsterEss), Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.MonsterEss), 4);
        buttonCount = 5;
    }

    //Function to create the buttons. Function used in Start()
    private Transform CreateUpgradeButton(string buttonType, Upgrade.EquipmentExchangeType upgradeType, string itemName, int itemCost, string currency, int positionIndex)
    {
        //Creating the button and adjusting the height and text inside
        Transform vendorItemTransform = Instantiate(vendorItemTemplate, container);
        //vendorItemTransform.gameObject.SetActive(true);
        RectTransform vendorItemRectTransform = vendorItemTransform.GetComponent<RectTransform>();

        float vendorItemHeight = 90f;
        vendorItemRectTransform.anchoredPosition = new Vector2(0, -vendorItemHeight * positionIndex);
        vendorItemTransform.name = itemName;

        vendorItemTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText(itemName);
        vendorItemTransform.Find("costText").GetComponent<TextMeshProUGUI>().SetText(itemCost.ToString());

        //Calls the function to be used when button is pressed
        vendorItemTransform.GetComponent<Button_UI>().ClickFunc = () =>
        {
            upgradeItem(buttonType, upgradeType, itemCost, itemName);
        };

        //itemCost is 0 when item is max level.
        if (itemCost == 0)
        {
            vendorItemTransform.GetComponent<Button>().interactable = false;
            Debug.Log("Button Disabled");
        }

        return vendorItemTransform;
    }

    //Function used to create the button for exchanging currency. Function used in Start()
    private Transform CreateExchangeButton(string buttonType, Upgrade.EquipmentExchangeType Exchange, string itemName, int itemCost, string currency, int positionIndex)
    {
        //Similar concept as the CreateUpgradeButton function
        Transform vendorExchangeTransform = Instantiate(vendorItemTemplate, container);
        //vendorItemTransform.gameObject.SetActive(true);
        RectTransform vendorExchangeRectTransform = vendorExchangeTransform.GetComponent<RectTransform>();

        float vendorItemHeight = 90f;
        vendorExchangeRectTransform.anchoredPosition = new Vector2(0, -vendorItemHeight * positionIndex);
        vendorExchangeTransform.name = itemName;

        vendorExchangeTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText(itemName);
        vendorExchangeTransform.Find("costText").GetComponent<TextMeshProUGUI>().SetText(itemCost.ToString());

        vendorExchangeTransform.GetComponent<Button_UI>().ClickFunc = () =>
        {
            exchangeItem(Exchange, itemCost, itemName);
        };

        return vendorExchangeTransform;
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
        if (itemCount - itemCost >= 0)
        {
            GameManager.Instance.PlayerInventory.RemoveItem(creepDrop, 10);
            GameManager.Instance.PlayerInventory.AddItem(monsterEss, 1);
        }
        else
        {
            Debug.Log("Not enough Materials");
        }

        Debug.Log("Exchanged");
    }

    //Code for Upgrade Buttons
    private void upgradeItem(string buttonType, Upgrade.EquipmentExchangeType upgradeType, int itemCost, string itemName)
    {
        itemIndex = playerInventory.GetItemIndex(monsterEss);
        try
        {
            itemCount = GameManager.Instance.PlayerInventory.Items[itemIndex].quantity;
        }
        catch
        {
            itemCount = 0;
        }

        //Change weapon Values Here based on upgrade type

        if (itemCount - itemCost >= 0)
        {
            if (buttonType == "LMelee" && LMeleeLvl < 4)
            {
                LMeleeLvl++;
                Debug.Log("Light Melee Upgraded to:" + LMeleeLvl);
                GameManager.Instance.PlayerInventory.RemoveItem(monsterEss, itemCost);
                //Insert Eddie's Upgrade Calculations for Light Melee Weapons
                this.LMeleeButton.GetComponent<Button>().interactable = false;
                this.LMeleeButton.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText(" ");
                this.LMeleeButton.Find("costText").GetComponent<TextMeshProUGUI>().SetText(" ");
                if (LMeleeLvl == 1)
                {
                    LMeleeButton = CreateUpgradeButton("LMelee", Upgrade.EquipmentExchangeType.LMelee_2, "Light Melee Up", 2/*Upgrade.GetCost(Upgrade.EquipmentExchangeType.LMelee_2)*/, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.LMelee_2), 0);
                }
                if(LMeleeLvl == 2)
                {
                    LMeleeButton = CreateUpgradeButton("LMelee", Upgrade.EquipmentExchangeType.LMelee_3, "Light Melee Up", 3/*Upgrade.GetCost(Upgrade.EquipmentExchangeType.LMelee_3)*/, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.LMelee_3),0);
                }
                if(LMeleeLvl == 3)
                {
                    LMeleeButton = CreateUpgradeButton("LMelee", Upgrade.EquipmentExchangeType.LMelee_4, "Light Melee Up", 4 /*Upgrade.GetCost(Upgrade.EquipmentExchangeType.LMelee_4)*/, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.LMelee_4), 0);
                }
                if(LMeleeLvl == 4)
                {
                    LMeleeButton = CreateUpgradeButton("LMelee", Upgrade.EquipmentExchangeType.LMeleeMax, "Light Melee Up", 5 /*Upgrade.GetCost(Upgrade.EquipmentExchangeType.LMeleeMax)*/, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.LMeleeMax), 0);
                }
                Debug.Log("Upgraded to: " + upgradeType);
            }
            if (buttonType == "HMelee" && HMeleeLvl < 4)
            {
                HMeleeLvl++;
                Debug.Log("Heavy Melee Upgraded to:" + HMeleeLvl);
                GameManager.Instance.PlayerInventory.RemoveItem(monsterEss, itemCost);
                //Insert Eddie's Upgrade Calculations for Light Melee Weapons
                this.HMeleeButton.GetComponent<Button>().interactable = false;
                this.HMeleeButton.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText(" ");
                this.HMeleeButton.Find("costText").GetComponent<TextMeshProUGUI>().SetText(" ");
                if (HMeleeLvl == 1)
                {
                    HMeleeButton = CreateUpgradeButton("LMelee", Upgrade.EquipmentExchangeType.HMelee_2, "Heavy Melee Up", Upgrade.GetCost(Upgrade.EquipmentExchangeType.HMelee_2), Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.HMelee_2), 1);
                }
                if (HMeleeLvl == 2)
                {
                    HMeleeButton = CreateUpgradeButton("LMelee", Upgrade.EquipmentExchangeType.HMelee_3, "Heavy Melee Up", Upgrade.GetCost(Upgrade.EquipmentExchangeType.HMelee_3), Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.HMelee_3), 1);
                }
                if (HMeleeLvl == 3)
                {
                    HMeleeButton = CreateUpgradeButton("LMelee", Upgrade.EquipmentExchangeType.HMelee_4, "Heavy Melee Up", Upgrade.GetCost(Upgrade.EquipmentExchangeType.HMelee_4), Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.HMelee_4), 1);
                }
                if (HMeleeLvl == 4)
                {
                    HMeleeButton = CreateUpgradeButton("LMelee", Upgrade.EquipmentExchangeType.HMeleeMax, "Heavy Melee Up", Upgrade.GetCost(Upgrade.EquipmentExchangeType.HMeleeMax), Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.HMeleeMax), 1);
                }
                Debug.Log("Upgraded to: " + upgradeType);
            }
            if (buttonType == "Rifle" && RifleLvl < 4)
            {
                RifleLvl++;
                Debug.Log("Rifle Upgraded to:" + RifleLvl);
                GameManager.Instance.PlayerInventory.RemoveItem(monsterEss, itemCost);
                //Insert Eddie's Upgrade Calculations for Light Melee Weapons
                this.RifleButton.GetComponent<Button>().interactable = false;
                this.RifleButton.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText(" ");
                this.RifleButton.Find("costText").GetComponent<TextMeshProUGUI>().SetText(" ");
                if (RifleLvl == 1)
                {
                    RifleButton = CreateUpgradeButton("Rifle", Upgrade.EquipmentExchangeType.Rifle_2, "Rifle Up", Upgrade.GetCost(Upgrade.EquipmentExchangeType.Rifle_2), Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Rifle_2), 2);
                }
                if (RifleLvl == 2)
                {
                    RifleButton = CreateUpgradeButton("Rifle", Upgrade.EquipmentExchangeType.Rifle_3, "Rifle Up", Upgrade.GetCost(Upgrade.EquipmentExchangeType.Rifle_3), Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Rifle_3), 2);
                }
                if (RifleLvl == 3)
                {
                    RifleButton = CreateUpgradeButton("Rifle", Upgrade.EquipmentExchangeType.Rifle_4, "Rifle Up", Upgrade.GetCost(Upgrade.EquipmentExchangeType.Rifle_4), Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Rifle_4), 2);
                }
                if (RifleLvl == 4)
                {
                    RifleButton = CreateUpgradeButton("Rifle", Upgrade.EquipmentExchangeType.RifleMax, "Rifle Up", Upgrade.GetCost(Upgrade.EquipmentExchangeType.RifleMax), Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.RifleMax), 2);
                }
                Debug.Log("Upgraded to: " + upgradeType);
            }
            if (buttonType == "Shotgun" && ShotgunLvl < 4)
            {
                ShotgunLvl++;
                Debug.Log("Shotgun Upgraded to:" + ShotgunLvl);
                GameManager.Instance.PlayerInventory.RemoveItem(monsterEss, itemCost);
                //Insert Eddie's Upgrade Calculations for Light Melee Weapons
                this.ShotgunButton.GetComponent<Button>().interactable = false;
                this.ShotgunButton.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText(" ");
                this.ShotgunButton.Find("costText").GetComponent<TextMeshProUGUI>().SetText(" ");
                if (ShotgunLvl == 1)
                {
                    ShotgunButton = CreateUpgradeButton("Shotgun", Upgrade.EquipmentExchangeType.Shotgun_2, "Shotgun Up", Upgrade.GetCost(Upgrade.EquipmentExchangeType.Shotgun_2), Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Shotgun_2), 3);
                }
                if (ShotgunLvl == 2)
                {
                    ShotgunButton = CreateUpgradeButton("Shotgun", Upgrade.EquipmentExchangeType.Shotgun_3, "Shotgun Up", Upgrade.GetCost(Upgrade.EquipmentExchangeType.Shotgun_3), Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Shotgun_3), 3);
                }
                if (ShotgunLvl == 3)
                {
                    ShotgunButton = CreateUpgradeButton("Shotgun", Upgrade.EquipmentExchangeType.Shotgun_4, "Shotgun Up", Upgrade.GetCost(Upgrade.EquipmentExchangeType.Shotgun_4), Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Shotgun_4), 3);
                }
                if (ShotgunLvl == 4)
                {
                    ShotgunButton = CreateUpgradeButton("Shotgun", Upgrade.EquipmentExchangeType.ShotgunMax, "Shotgun Up", Upgrade.GetCost(Upgrade.EquipmentExchangeType.ShotgunMax), Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.ShotgunMax), 3);
                }
                Debug.Log("Upgraded to: " + upgradeType);
                Debug.Log("Upgraded to: " + upgradeType);
            }
            if (buttonType == "LMelee" && LMeleeLvl >= 4)
            {
                //Try to insert popup maybe?
                Debug.Log("Weapon Already at Max Level");
                LMeleeButton.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Light Melee MAX");
                LMeleeButton.Find("costText").GetComponent<TextMeshProUGUI>().SetText("0");
                LMeleeButton.GetComponent<Button>().interactable = false;
            }
            if (buttonType == "HMelee" && HMeleeLvl >= 4)
            {
                //Try to insert popup maybe?
                Debug.Log("Weapon Already at Max Level");
                HMeleeButton.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Heavy Melee MAX");
                HMeleeButton.Find("costText").GetComponent<TextMeshProUGUI>().SetText("0");
                HMeleeButton.GetComponent<Button>().interactable = false;
            }
            if (buttonType == "Rifle" && RifleLvl >= 4)
            {
                //Try to insert popup maybe?
                Debug.Log("Weapon Already at Max Level");
                RifleButton.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Rifle MAX");
                RifleButton.Find("costText").GetComponent<TextMeshProUGUI>().SetText("0");
                RifleButton.GetComponent<Button>().interactable = false;
            }
            if (buttonType == "Shotgun" && ShotgunLvl >= 4)
            {
                //Try to insert popup maybe?
                Debug.Log("Weapon Already at Max Level");
                ShotgunButton.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Shotgun MAX");
                ShotgunButton.Find("costText").GetComponent<TextMeshProUGUI>().SetText("0");
                ShotgunButton.GetComponent<Button>().interactable = false;
            }
        }
        else
        {
            Debug.Log("Not enough materials");
        }
    }
}
