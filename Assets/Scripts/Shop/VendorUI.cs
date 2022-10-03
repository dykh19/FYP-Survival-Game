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

    public int LMeleeCost = 10;
    public int HMeleeCost = 10;
    public int RifleCost = 10;
    public int ShotgunCost = 10;
    public int ArmorCost = 10;

    public int itemIndex;
    public int itemCount;

    private Transform LMeleeButton;
    private Transform HMeleeButton;
    private Transform RifleButton;
    private Transform ShotgunButton;
    private Transform EssExchangeButton;
    private Transform OreExchangeButton;
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
        LMeleeButton = CreateUpgradeButton("LMelee", Upgrade.EquipmentExchangeType.LMelee_1, "Light Melee Up", LMeleeCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 0);
        HMeleeButton = CreateUpgradeButton("HMelee", Upgrade.EquipmentExchangeType.HMelee_1, "Heavy Melee Up", HMeleeCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 1);
        RifleButton = CreateUpgradeButton("Rifle", Upgrade.EquipmentExchangeType.Rifle_1, "Rifle Up", RifleCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 2);
        ShotgunButton = CreateUpgradeButton("Shotgun", Upgrade.EquipmentExchangeType.Shotgun_1, "Shotgun Up", ShotgunCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 3);
        EssExchangeButton = CreateExchangeButton("EssExchange", Upgrade.EquipmentExchangeType.MonsterEss, "Monster Essence", Upgrade.ExchangeRate(Upgrade.EquipmentExchangeType.MonsterEss), Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.MonsterEss), 4);
        OreExchangeButton = CreateExchangeButton("OreExchange", Upgrade.EquipmentExchangeType.MonsterOre, "Synthesized Ore", Upgrade.ExchangeRate(Upgrade.EquipmentExchangeType.MonsterOre), Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.MonsterOre), 5);
        buttonCount = 6;
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
        if (item == Upgrade.EquipmentExchangeType.MonsterEss && itemCount - itemCost >= 0)
        {
            GameManager.Instance.PlayerInventory.RemoveItem(creepDrop, 10);
            GameManager.Instance.PlayerInventory.AddItem(monsterEss, 1);
        }
        else if (item == Upgrade.EquipmentExchangeType.MonsterOre && itemCount - itemCost >= 0)
        {
            GameManager.Instance.PlayerInventory.RemoveItem(creepDrop, 10);
            //GameManager.Instance.PlayerInventory.AddItem(monsterOre, 1);  <--- Add monsterOre gameitem.
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
            if (buttonType == "LMelee" && LMeleeLvl < 10)
            {
                LMeleeLvl++;
                Debug.Log("Light Melee Upgraded to:" + LMeleeLvl);
                GameManager.Instance.PlayerInventory.RemoveItem(monsterEss, itemCost);
                LMeleeCost = LMeleeCost + 5;

                //Insert Eddie's Upgrade Calculations for Light Melee Weapons

                this.LMeleeButton.GetComponent<Button>().interactable = false;
                this.LMeleeButton.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText(" ");
                this.LMeleeButton.Find("costText").GetComponent<TextMeshProUGUI>().SetText(" ");


                if (LMeleeLvl == 1)
                {
                    LMeleeButton = CreateUpgradeButton("LMelee", Upgrade.EquipmentExchangeType.LMelee_2, "Light Melee Up", LMeleeCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.LMelee_2), 0);
                }
                if(LMeleeLvl == 2)
                {
                    LMeleeButton = CreateUpgradeButton("LMelee", Upgrade.EquipmentExchangeType.LMelee_3, "Light Melee Up", LMeleeCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.LMelee_3),0);
                }
                if(LMeleeLvl == 3)
                {
                    LMeleeButton = CreateUpgradeButton("LMelee", Upgrade.EquipmentExchangeType.LMelee_4, "Light Melee Up", LMeleeCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.LMelee_4), 0);
                }
                if(LMeleeLvl == 4)
                {
                    LMeleeButton = CreateUpgradeButton("LMelee", Upgrade.EquipmentExchangeType.LMelee_5, "Light Melee Up", LMeleeCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.LMelee_5), 0);
                }
                if (LMeleeLvl == 5)
                {
                    LMeleeButton = CreateUpgradeButton("LMelee", Upgrade.EquipmentExchangeType.LMelee_6, "Light Melee Up", LMeleeCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.LMelee_6), 0);
                }
                if (LMeleeLvl == 6)
                {
                    LMeleeButton = CreateUpgradeButton("LMelee", Upgrade.EquipmentExchangeType.LMelee_7, "Light Melee Up", LMeleeCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.LMelee_7), 0);
                }
                if (LMeleeLvl == 7)
                {
                    LMeleeButton = CreateUpgradeButton("LMelee", Upgrade.EquipmentExchangeType.LMelee_8, "Light Melee Up", LMeleeCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.LMelee_8), 0);
                }
                if (LMeleeLvl == 8)
                {
                    LMeleeButton = CreateUpgradeButton("LMelee", Upgrade.EquipmentExchangeType.LMelee_9, "Light Melee Up", LMeleeCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.LMelee_9), 0);
                }
                if (LMeleeLvl == 9)
                {
                    LMeleeButton = CreateUpgradeButton("LMelee", Upgrade.EquipmentExchangeType.LMeleeMax, "Light Melee MAX", 0, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.LMeleeMax), 0);
                }

                Debug.Log("Upgraded to: " + upgradeType);
            }
            if (buttonType == "HMelee" && HMeleeLvl < 10)
            {
                HMeleeLvl++;
                Debug.Log("Heavy Melee Upgraded to:" + HMeleeLvl);
                GameManager.Instance.PlayerInventory.RemoveItem(monsterEss, itemCost);
                HMeleeCost = HMeleeCost + 5;

                //Insert Eddie's Upgrade Calculations for Light Melee Weapons

                this.HMeleeButton.GetComponent<Button>().interactable = false;
                this.HMeleeButton.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText(" ");
                this.HMeleeButton.Find("costText").GetComponent<TextMeshProUGUI>().SetText(" ");
                if (HMeleeLvl == 1)
                {
                    HMeleeButton = CreateUpgradeButton("LMelee", Upgrade.EquipmentExchangeType.HMelee_2, "Heavy Melee Up", HMeleeCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.HMelee_2), 1);
                }
                if (HMeleeLvl == 2)
                {
                    HMeleeButton = CreateUpgradeButton("LMelee", Upgrade.EquipmentExchangeType.HMelee_3, "Heavy Melee Up", HMeleeCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.HMelee_3), 1);
                }
                if (HMeleeLvl == 3)
                {
                    HMeleeButton = CreateUpgradeButton("LMelee", Upgrade.EquipmentExchangeType.HMelee_4, "Heavy Melee Up", HMeleeCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.HMelee_4), 1);
                }
                if (HMeleeLvl == 4)
                {
                    HMeleeButton = CreateUpgradeButton("LMelee", Upgrade.EquipmentExchangeType.HMelee_5, "Heavy Melee Up", HMeleeCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.HMelee_5), 1);
                }
                if (HMeleeLvl == 5)
                {
                    HMeleeButton = CreateUpgradeButton("LMelee", Upgrade.EquipmentExchangeType.HMelee_6, "Heavy Melee Up", HMeleeCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.HMelee_6), 1);
                }
                if (HMeleeLvl == 6)
                {
                    HMeleeButton = CreateUpgradeButton("LMelee", Upgrade.EquipmentExchangeType.HMelee_7, "Heavy Melee Up", HMeleeCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.HMelee_7), 1);
                }
                if (HMeleeLvl == 7)
                {
                    HMeleeButton = CreateUpgradeButton("LMelee", Upgrade.EquipmentExchangeType.HMelee_8, "Heavy Melee Up", HMeleeCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.HMelee_8), 1);
                }
                if (HMeleeLvl == 8)
                {
                    HMeleeButton = CreateUpgradeButton("LMelee", Upgrade.EquipmentExchangeType.HMelee_9, "Heavy Melee Up", HMeleeCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.HMelee_9), 1);
                }
                if (HMeleeLvl == 9)
                {
                    HMeleeButton = CreateUpgradeButton("LMelee", Upgrade.EquipmentExchangeType.HMeleeMax, "Heavy Melee MAX", 0, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.HMeleeMax), 1);
                }
                Debug.Log("Upgraded to: " + upgradeType);
            }
            if (buttonType == "Rifle" && RifleLvl < 4)
            {
                RifleLvl++;
                Debug.Log("Rifle Upgraded to:" + RifleLvl);
                GameManager.Instance.PlayerInventory.RemoveItem(monsterEss, itemCost);
                RifleCost = RifleCost + 5;

                //Insert Eddie's Upgrade Calculations for Light Melee Weapons

                this.RifleButton.GetComponent<Button>().interactable = false;
                this.RifleButton.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText(" ");
                this.RifleButton.Find("costText").GetComponent<TextMeshProUGUI>().SetText(" ");
                if (RifleLvl == 1)
                {
                    RifleButton = CreateUpgradeButton("Rifle", Upgrade.EquipmentExchangeType.Rifle_2, "Rifle Up", RifleCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Rifle_2), 2);
                }
                if (RifleLvl == 2)
                {
                    RifleButton = CreateUpgradeButton("Rifle", Upgrade.EquipmentExchangeType.Rifle_3, "Rifle Up", RifleCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Rifle_3), 2);
                }
                if (RifleLvl == 3)
                {
                    RifleButton = CreateUpgradeButton("Rifle", Upgrade.EquipmentExchangeType.Rifle_4, "Rifle Up", RifleCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Rifle_4), 2);
                }
                if (RifleLvl == 4)
                {
                    RifleButton = CreateUpgradeButton("Rifle", Upgrade.EquipmentExchangeType.Rifle_5, "Rifle Up", RifleCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Rifle_5), 2);
                }
                if (RifleLvl == 5)
                {
                    RifleButton = CreateUpgradeButton("Rifle", Upgrade.EquipmentExchangeType.Rifle_6, "Rifle Up", RifleCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Rifle_6), 2);
                }
                if (RifleLvl == 6)
                {
                    RifleButton = CreateUpgradeButton("Rifle", Upgrade.EquipmentExchangeType.Rifle_7, "Rifle Up", RifleCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Rifle_7), 2);
                }
                if (RifleLvl == 7)
                {
                    RifleButton = CreateUpgradeButton("Rifle", Upgrade.EquipmentExchangeType.Rifle_8, "Rifle Up", RifleCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Rifle_8), 2);
                }
                if (RifleLvl == 8)
                {
                    RifleButton = CreateUpgradeButton("Rifle", Upgrade.EquipmentExchangeType.Rifle_9, "Rifle Up", RifleCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Rifle_9), 2);
                }
                if (RifleLvl == 9)
                {
                    RifleButton = CreateUpgradeButton("Rifle", Upgrade.EquipmentExchangeType.RifleMax, "Rifle MAX", 0, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.RifleMax), 2);
                }
                Debug.Log("Upgraded to: " + upgradeType);
            }
            if (buttonType == "Shotgun" && ShotgunLvl < 4)
            {
                ShotgunLvl++;
                Debug.Log("Shotgun Upgraded to:" + ShotgunLvl);
                GameManager.Instance.PlayerInventory.RemoveItem(monsterEss, itemCost);
                ShotgunCost = ShotgunCost + 5;

                //Insert Eddie's Upgrade Calculations for Light Melee Weapons

                this.ShotgunButton.GetComponent<Button>().interactable = false;
                this.ShotgunButton.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText(" ");
                this.ShotgunButton.Find("costText").GetComponent<TextMeshProUGUI>().SetText(" ");
                if (ShotgunLvl == 1)
                {
                    ShotgunButton = CreateUpgradeButton("Shotgun", Upgrade.EquipmentExchangeType.Shotgun_2, "Shotgun Up", ShotgunCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Shotgun_2), 3);
                }
                if (ShotgunLvl == 2)
                {
                    ShotgunButton = CreateUpgradeButton("Shotgun", Upgrade.EquipmentExchangeType.Shotgun_3, "Shotgun Up", ShotgunCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Shotgun_3), 3);
                }
                if (ShotgunLvl == 3)
                {
                    ShotgunButton = CreateUpgradeButton("Shotgun", Upgrade.EquipmentExchangeType.Shotgun_4, "Shotgun Up", ShotgunCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Shotgun_4), 3);
                }
                if (ShotgunLvl == 4)
                {
                    ShotgunButton = CreateUpgradeButton("Shotgun", Upgrade.EquipmentExchangeType.Shotgun_5, "Shotgun Up", ShotgunCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Shotgun_5), 3);
                }
                if (ShotgunLvl == 5)
                {
                    ShotgunButton = CreateUpgradeButton("Shotgun", Upgrade.EquipmentExchangeType.Shotgun_6, "Shotgun Up", ShotgunCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Shotgun_6), 3);
                }
                if (ShotgunLvl == 6)
                {
                    ShotgunButton = CreateUpgradeButton("Shotgun", Upgrade.EquipmentExchangeType.Shotgun_7, "Shotgun Up", ShotgunCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Shotgun_7), 3);
                }
                if (ShotgunLvl == 7)
                {
                    ShotgunButton = CreateUpgradeButton("Shotgun", Upgrade.EquipmentExchangeType.Shotgun_8, "Shotgun Up", ShotgunCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Shotgun_8), 3);
                }
                if (ShotgunLvl == 8)
                {
                    ShotgunButton = CreateUpgradeButton("Shotgun", Upgrade.EquipmentExchangeType.Shotgun_9, "Shotgun Up", ShotgunCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Shotgun_9), 3);
                }
                if (ShotgunLvl == 9)
                {
                    ShotgunButton = CreateUpgradeButton("Shotgun", Upgrade.EquipmentExchangeType.ShotgunMax, "Shotgun MAX", ShotgunCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.ShotgunMax), 3);
                }
                Debug.Log("Upgraded to: " + upgradeType);
            }
            if (buttonType == "LMelee" && LMeleeLvl >= 10)
            {
                //Try to insert popup maybe?
                Debug.Log("Weapon Already at Max Level");
                LMeleeButton.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Light Melee MAX");
                LMeleeButton.Find("costText").GetComponent<TextMeshProUGUI>().SetText("0");
                LMeleeButton.GetComponent<Button>().interactable = false;
            }
            if (buttonType == "HMelee" && HMeleeLvl >= 10)
            {
                //Try to insert popup maybe?
                Debug.Log("Weapon Already at Max Level");
                HMeleeButton.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Heavy Melee MAX");
                HMeleeButton.Find("costText").GetComponent<TextMeshProUGUI>().SetText("0");
                HMeleeButton.GetComponent<Button>().interactable = false;
            }
            if (buttonType == "Rifle" && RifleLvl >= 10)
            {
                //Try to insert popup maybe?
                Debug.Log("Weapon Already at Max Level");
                RifleButton.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Rifle MAX");
                RifleButton.Find("costText").GetComponent<TextMeshProUGUI>().SetText("0");
                RifleButton.GetComponent<Button>().interactable = false;
            }
            if (buttonType == "Shotgun" && ShotgunLvl >= 10)
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
