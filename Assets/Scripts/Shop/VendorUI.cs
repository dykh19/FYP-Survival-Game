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

    [SerializeField]
    private int LMeleeCost = 1; //10 previously;
    [SerializeField]
    private int HMeleeCost = 1;
    [SerializeField]
    private int RifleCost = 1;
    [SerializeField]
    private int ShotgunCost = 1;
    [SerializeField]
    private int HealthCost = 1;
    [SerializeField]
    private int BaseCost = 25; //To update according to specs.

    public int itemIndex;
    public int currencyCount;

    private Transform LMeleeButton;
    private Transform HMeleeButton;
    private Transform RifleButton;
    private Transform ShotgunButton;
    private Transform HealthButton;
    private Transform BaseButton;
    public InventoryUI playerInventoryUI;
    public Inventory playerInventory;
    public UpgradeManager upgradeManager;
    public Health playerHealth;
    public GameItem monsterEss;
    public GameItem syntheticOre;

    private void Awake()
    {
       
    }

    private void Start()
    {
        //Assigning the relevant objects
        playerInventoryUI = FindObjectOfType<InventoryUI>();
        playerInventory = GameManager.Instance.PlayerInventory;
        upgradeManager = this.GetComponent<UpgradeManager>();

        //Creating the initial buttons
        RifleButton = CreateRifleButton("Rifle", "Rifle Upgrade!", Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 0);
        ShotgunButton = CreateShotgunButton("Shotgun", "Shotgun Upgrade!", Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 1);
        HMeleeButton = CreateAxeButton("Axe", "Axe Upgrade!", Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 2);
        LMeleeButton = CreateSwordButton("Sword", "Sword Upgrade!", Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 3);
        //Not Converted Yet
        HealthButton = CreateUpgradeButton("Health", Upgrade.EquipmentExchangeType.Health_1, "Health Up", HealthCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Health), 4);
        BaseButton = CreateUpgradeButton("Base", Upgrade.EquipmentExchangeType.Base_1, "Base Up", BaseCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Base), 5);
    }

    //Creation of the Rifle Upgrade Button
    private Transform CreateRifleButton(string buttonType, string itemName, string currency, int positionIndex)
    {
        Transform rifleButtonTransform = Instantiate(vendorItemTemplate, container);
        RectTransform rifleItemRectTransform = rifleButtonTransform.GetComponent<RectTransform>();

        float buttonHeight = 90f;
        rifleItemRectTransform.anchoredPosition = new Vector2(0, -buttonHeight * positionIndex);
        rifleButtonTransform.name = itemName;

        if(upgradeManager.playerRifleLevel < 10)
        {
            rifleButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Rifle Upgrade!");
        }
        if(upgradeManager.playerRifleLevel >= 10)
        {
            rifleButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Rifle Max Level!");
        }

        rifleButtonTransform.Find("costText").GetComponent<TextMeshProUGUI>().SetText(RifleCost.ToString());

        rifleButtonTransform.GetComponent<Button_UI>().ClickFunc = () =>
        {
            itemIndex = playerInventory.GetItemIndex(monsterEss);
            try
            {
                currencyCount = GameManager.Instance.PlayerInventory.Items[itemIndex].quantity;
            }
            catch
            {
                currencyCount = 0;
            }

            if (currencyCount - RifleCost >= 0)
            {
                if (upgradeManager.playerRifleLevel < 10)
                {
                    upgradeManager.playerRifleLevel++;
                    rifleButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Rifle Upgrade!");
                    //RifleCost = RifleCost + 5;
                    upgradeManager.UpgradePlayerWeapon(0);
                    GameManager.Instance.PlayerInventory.RemoveItem(monsterEss, RifleCost);
                }
                if (upgradeManager.playerSwordLevel >= 10)
                {
                    rifleButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Rifle MAX Level!");
                    rifleButtonTransform.GetComponent<Button>().interactable = false;
                }

                rifleButtonTransform.Find("costText").GetComponent<TextMeshProUGUI>().SetText(RifleCost.ToString());
            }
        };

        return rifleButtonTransform;
    }

    //Creation of the Shotgun Upgrade Button
    private Transform CreateShotgunButton(string buttonType, string itemName, string currency, int positionIndex)
    {
        Transform shotgunButtonTransform = Instantiate(vendorItemTemplate, container);
        RectTransform shotgunItemRectTransform = shotgunButtonTransform.GetComponent<RectTransform>();

        float buttonHeight = 90f;
        shotgunItemRectTransform.anchoredPosition = new Vector2(0, -buttonHeight * positionIndex);
        shotgunButtonTransform.name = itemName;

        if(upgradeManager.playerShotgunLevel < 10)
        {
            shotgunButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Shotgun Upgrade!");
        }
        if (upgradeManager.playerShotgunLevel >= 10)
        {
            shotgunButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Shotgun Max Level!");
        }

        shotgunButtonTransform.Find("costText").GetComponent<TextMeshProUGUI>().SetText(ShotgunCost.ToString());

        shotgunButtonTransform.GetComponent<Button_UI>().ClickFunc = () =>
        {
            itemIndex = playerInventory.GetItemIndex(monsterEss);
            try
            {
                currencyCount = GameManager.Instance.PlayerInventory.Items[itemIndex].quantity;
            }
            catch
            {
                currencyCount = 0;
            }

            if (currencyCount - ShotgunCost >= 0)
            {
                if (upgradeManager.playerShotgunLevel < 10)
                {
                    upgradeManager.playerShotgunLevel++;
                    shotgunButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Shotgun Upgrade!");
                    ShotgunCost = ShotgunCost + 5;
                    upgradeManager.UpgradePlayerWeapon(1);
                    GameManager.Instance.PlayerInventory.RemoveItem(monsterEss, ShotgunCost);
                }
                if (upgradeManager.playerSwordLevel >= 10)
                {
                    shotgunButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Shotgun MAX Level!");
                    shotgunButtonTransform.GetComponent<Button>().interactable = false;
                }
                shotgunButtonTransform.Find("costText").GetComponent<TextMeshProUGUI>().SetText(ShotgunCost.ToString());
            }
        };

        return shotgunButtonTransform;
    }

    //Creation of the Axe Upgrade Button
    private Transform CreateAxeButton(string buttonType, string itemName, string currency, int positionIndex)
    {
        Transform axeButtonTransform = Instantiate(vendorItemTemplate, container);
        RectTransform axeItemRectTransform = axeButtonTransform.GetComponent<RectTransform>();

        float buttonHeight = 90f;
        axeItemRectTransform.anchoredPosition = new Vector2(0, -buttonHeight * positionIndex);
        axeButtonTransform.name = itemName;

        if (upgradeManager.playerAxeLevel < 10)
        {
            axeButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Axe Upgrade!");
        }
        if (upgradeManager.playerAxeLevel >= 10)
        {
            axeButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Axe MAX Level!");
        }

        axeButtonTransform.Find("costText").GetComponent<TextMeshProUGUI>().SetText(HMeleeCost.ToString());

        axeButtonTransform.GetComponent<Button_UI>().ClickFunc = () =>
        {
            itemIndex = playerInventory.GetItemIndex(monsterEss);
            try
            {
                currencyCount = GameManager.Instance.PlayerInventory.Items[itemIndex].quantity;
            }
            catch
            {
                currencyCount = 0;
            }

            if (currencyCount - HMeleeCost >= 0)
            {
                if (upgradeManager.playerAxeLevel < 10)
                {
                    upgradeManager.playerAxeLevel++;
                    axeButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Axe Upgrade!");
                    HMeleeCost = HMeleeCost + 5;
                    upgradeManager.UpgradePlayerWeapon(2);
                    GameManager.Instance.PlayerInventory.RemoveItem(monsterEss, HMeleeCost);
                }
                if (upgradeManager.playerAxeLevel >= 10)
                {
                    axeButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Axe MAX Level!");
                    axeButtonTransform.GetComponent<Button>().interactable = false;
                }

                axeButtonTransform.Find("costText").GetComponent<TextMeshProUGUI>().SetText(HMeleeCost.ToString());
            }
        };

        return axeButtonTransform;
    }



    //Creation of the Sword Upgrade Button
    private Transform CreateSwordButton(string buttonType, string itemName, string currency, int positionIndex)
    {
        Transform swordButtonTransform = Instantiate(vendorItemTemplate, container);
        RectTransform swordItemRectTransform = swordButtonTransform.GetComponent<RectTransform>();

        float buttonHeight = 90f;
        swordItemRectTransform.anchoredPosition = new Vector2(0, -buttonHeight * positionIndex);
        swordButtonTransform.name = itemName;

        if(upgradeManager.playerSwordLevel < 10)
        {
            swordButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Sword Upgrade!");
        }
        if(upgradeManager.playerSwordLevel >= 10)
        {
            swordButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Sword MAX Level!");
        }

        swordButtonTransform.Find("costText").GetComponent<TextMeshProUGUI>().SetText(LMeleeCost.ToString());

        swordButtonTransform.GetComponent<Button_UI>().ClickFunc = () =>
        {
            itemIndex = playerInventory.GetItemIndex(monsterEss);
            try
            {
                currencyCount = GameManager.Instance.PlayerInventory.Items[itemIndex].quantity;
            }
            catch
            {
                currencyCount = 0;
            }

            if (currencyCount - LMeleeCost >= 0)
            {
                if (upgradeManager.playerSwordLevel < 10)
                {
                    upgradeManager.playerSwordLevel++;
                    swordButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Sword Upgrade!");
                    LMeleeCost = LMeleeCost + 5;
                    upgradeManager.UpgradePlayerWeapon(3);
                    GameManager.Instance.PlayerInventory.RemoveItem(monsterEss, LMeleeCost);
                }
                if (upgradeManager.playerSwordLevel >= 10)
                {
                    swordButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Sword MAX Level!");
                    swordButtonTransform.GetComponent<Button>().interactable = false;
                }

                swordButtonTransform.Find("costText").GetComponent<TextMeshProUGUI>().SetText(LMeleeCost.ToString());
            }
        };

        return swordButtonTransform;
    }

    //Creation of the Health Upgrade Button
    private Transform CreateHealthButton(string buttonType, string itemName, string currency, int positionIndex)
    {
        Transform healthButtonTransform = Instantiate(vendorItemTemplate, container);
        RectTransform healthItemRectTransform = healthButtonTransform.GetComponent<RectTransform>();

        float buttonHeight = 90f;
        healthItemRectTransform.anchoredPosition = new Vector2(0, -buttonHeight * positionIndex);
        healthButtonTransform.name = itemName;

        if (upgradeManager.playerHealthLevel < 10)
        {
            healthButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Health Upgrade!");
        }
        if (upgradeManager.playerHealthLevel >= 10)
        {
            healthButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Health MAX Level!");
        }

        healthButtonTransform.Find("costText").GetComponent<TextMeshProUGUI>().SetText(HealthCost.ToString());

        healthButtonTransform.GetComponent<Button_UI>().ClickFunc = () =>
        {
            itemIndex = playerInventory.GetItemIndex(monsterEss);
            try
            {
                currencyCount = GameManager.Instance.PlayerInventory.Items[itemIndex].quantity;
            }
            catch
            {
                currencyCount = 0;
            }

            if (currencyCount - HealthCost >= 0)
            {
                if (upgradeManager.playerHealthLevel < 10)
                {
                    upgradeManager.playerHealthLevel++;
                    healthButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Health Upgrade!");
                    HealthCost = HealthCost + 5;
                    upgradeManager.UpgradePlayerHealth();
                    GameManager.Instance.PlayerInventory.RemoveItem(monsterEss, HealthCost);
                }
                if (upgradeManager.playerHealthLevel >= 10)
                {
                    healthButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Health MAX Level!");
                    healthButtonTransform.GetComponent<Button>().interactable = false;
                }

                healthButtonTransform.Find("costText").GetComponent<TextMeshProUGUI>().SetText(HealthCost.ToString());
            }
        };

        return healthButtonTransform;
    }

    //Creation of the Base Upgrade Button
    private Transform CreateBaseButton(string buttonType, string itemName, string currency, int positionIndex)
    {
        Transform baseButtonTransform = Instantiate(vendorItemTemplate, container);
        RectTransform baseItemRectTransform = baseButtonTransform.GetComponent<RectTransform>();

        float buttonHeight = 90f;
        baseItemRectTransform.anchoredPosition = new Vector2(0, -buttonHeight * positionIndex);
        baseButtonTransform.name = itemName;

        if (upgradeManager.baseLevel < 10)
        {
            baseButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Base Upgrade!");
        }
        if (upgradeManager.baseLevel >= 10)
        {
            baseButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Base MAX Level!");
        }

        baseButtonTransform.Find("costText").GetComponent<TextMeshProUGUI>().SetText(BaseCost.ToString());

        baseButtonTransform.GetComponent<Button_UI>().ClickFunc = () =>
        {
            itemIndex = playerInventory.GetItemIndex(syntheticOre);
            try
            {
                currencyCount = GameManager.Instance.PlayerInventory.Items[itemIndex].quantity;
            }
            catch
            {
                currencyCount = 0;
            }

            if (currencyCount - HealthCost >= 0)
            {
                if (upgradeManager.baseLevel < 10)
                {
                    upgradeManager.baseLevel++;
                    baseButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Base Upgrade!");
                    BaseCost = BaseCost + 5;
                    upgradeManager.UpgradeBase();
                    GameManager.Instance.PlayerInventory.RemoveItem(monsterEss, BaseCost);
                }
                if (upgradeManager.playerSwordLevel >= 10)
                {
                    baseButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Base MAX Level!");
                    baseButtonTransform.GetComponent<Button>().interactable = false;
                }

                baseButtonTransform.Find("costText").GetComponent<TextMeshProUGUI>().SetText(BaseCost.ToString());
            }
        };

        return baseButtonTransform;
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
            upgradeItem(currency, buttonType, upgradeType, itemCost, itemName);
        };

        //itemCost is 0 when item is max level.
        if (itemCost == 0)
        {
            vendorItemTransform.GetComponent<Button>().interactable = false;
            Debug.Log("Button Disabled");
        }

        return vendorItemTransform;
    }

    //Code for Upgrade Buttons
    private void upgradeItem(string currency, string buttonType, Upgrade.EquipmentExchangeType upgradeType, int itemCost, string itemName)
    {
        //Check for currency != 0
        if(currency == "MonsterEss")
        {
            itemIndex = playerInventory.GetItemIndex(monsterEss);
            try
            {
                currencyCount = GameManager.Instance.PlayerInventory.Items[itemIndex].quantity;
            }
            catch
            {
                currencyCount = 0;
            }
        }
        else if(currency == "SyntheticOre")
        {
            itemIndex = playerInventory.GetItemIndex(syntheticOre);
            try
            {
                currencyCount = GameManager.Instance.PlayerInventory.Items[itemIndex].quantity;
            }
            catch
            {
                currencyCount = 0;
            }
        }
        

        //Change weapon Values Here based on upgrade type
        if (currencyCount - itemCost >= 0)
        {
            if (buttonType == "HMelee" && upgradeManager.playerAxeLevel < 10)
            {
                upgradeManager.playerAxeLevel++;
                Debug.Log("Heavy Melee Upgraded to:" + upgradeManager.playerAxeLevel);
                GameManager.Instance.PlayerInventory.RemoveItem(monsterEss, itemCost);
                HMeleeCost = HMeleeCost + 5;

                upgradeManager.UpgradePlayerWeapon(2);

                this.HMeleeButton.GetComponent<Button>().interactable = false;
                this.HMeleeButton.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText(" ");
                this.HMeleeButton.Find("costText").GetComponent<TextMeshProUGUI>().SetText(" ");
                if (upgradeManager.playerAxeLevel == 1)
                {
                    HMeleeButton = CreateUpgradeButton("HMelee", Upgrade.EquipmentExchangeType.HMelee_2, "Heavy Melee Up", HMeleeCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 1);
                }
                if (upgradeManager.playerAxeLevel == 2)
                {
                    HMeleeButton = CreateUpgradeButton("HMelee", Upgrade.EquipmentExchangeType.HMelee_3, "Heavy Melee Up", HMeleeCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 1);
                }
                if (upgradeManager.playerAxeLevel == 3)
                {
                    HMeleeButton = CreateUpgradeButton("HMelee", Upgrade.EquipmentExchangeType.HMelee_4, "Heavy Melee Up", HMeleeCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 1);
                }
                if (upgradeManager.playerAxeLevel == 4)
                {
                    HMeleeButton = CreateUpgradeButton("HMelee", Upgrade.EquipmentExchangeType.HMelee_5, "Heavy Melee Up", HMeleeCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 1);
                }
                if (upgradeManager.playerAxeLevel == 5)
                {
                    HMeleeButton = CreateUpgradeButton("HMelee", Upgrade.EquipmentExchangeType.HMelee_6, "Heavy Melee Up", HMeleeCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 1);
                }
                if (upgradeManager.playerAxeLevel == 6)
                {
                    HMeleeButton = CreateUpgradeButton("HMelee", Upgrade.EquipmentExchangeType.HMelee_7, "Heavy Melee Up", HMeleeCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 1);
                }
                if (upgradeManager.playerAxeLevel == 7)
                {
                    HMeleeButton = CreateUpgradeButton("HMelee", Upgrade.EquipmentExchangeType.HMelee_8, "Heavy Melee Up", HMeleeCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 1);
                }
                if (upgradeManager.playerAxeLevel == 8)
                {
                    HMeleeButton = CreateUpgradeButton("HMelee", Upgrade.EquipmentExchangeType.HMelee_9, "Heavy Melee Up", HMeleeCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 1);
                }
                if (upgradeManager.playerAxeLevel == 9)
                {
                    HMeleeButton = CreateUpgradeButton("HMelee", Upgrade.EquipmentExchangeType.HMeleeMax, "Heavy Melee MAX", 0, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 1);
                }
                Debug.Log("Upgraded to: " + upgradeType);
            }
            if (buttonType == "Rifle" && upgradeManager.playerRifleLevel < 10)
            {
                upgradeManager.playerRifleLevel++;
                Debug.Log("Rifle Upgraded to:" + upgradeManager.playerRifleLevel);
                GameManager.Instance.PlayerInventory.RemoveItem(monsterEss, itemCost);
                RifleCost = RifleCost + 5;

                upgradeManager.UpgradePlayerWeapon(0);

                this.RifleButton.GetComponent<Button>().interactable = false;
                this.RifleButton.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText(" ");
                this.RifleButton.Find("costText").GetComponent<TextMeshProUGUI>().SetText(" ");
                if (upgradeManager.playerRifleLevel == 1)
                {
                    RifleButton = CreateUpgradeButton("Rifle", Upgrade.EquipmentExchangeType.Rifle_2, "Rifle Up", RifleCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 2);
                }
                if (upgradeManager.playerRifleLevel == 2)
                {
                    RifleButton = CreateUpgradeButton("Rifle", Upgrade.EquipmentExchangeType.Rifle_3, "Rifle Up", RifleCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 2);
                }
                if (upgradeManager.playerRifleLevel == 3)
                {
                    RifleButton = CreateUpgradeButton("Rifle", Upgrade.EquipmentExchangeType.Rifle_4, "Rifle Up", RifleCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 2);
                }
                if (upgradeManager.playerRifleLevel == 4)
                {
                    RifleButton = CreateUpgradeButton("Rifle", Upgrade.EquipmentExchangeType.Rifle_5, "Rifle Up", RifleCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 2);
                }
                if (upgradeManager.playerRifleLevel == 5)
                {
                    RifleButton = CreateUpgradeButton("Rifle", Upgrade.EquipmentExchangeType.Rifle_6, "Rifle Up", RifleCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 2);
                }
                if (upgradeManager.playerRifleLevel == 6)
                {
                    RifleButton = CreateUpgradeButton("Rifle", Upgrade.EquipmentExchangeType.Rifle_7, "Rifle Up", RifleCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 2);
                }
                if (upgradeManager.playerRifleLevel == 7)
                {
                    RifleButton = CreateUpgradeButton("Rifle", Upgrade.EquipmentExchangeType.Rifle_8, "Rifle Up", RifleCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 2);
                }
                if (upgradeManager.playerRifleLevel == 8)
                {
                    RifleButton = CreateUpgradeButton("Rifle", Upgrade.EquipmentExchangeType.Rifle_9, "Rifle Up", RifleCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 2);
                }
                if (upgradeManager.playerRifleLevel == 9)
                {
                    RifleButton = CreateUpgradeButton("Rifle", Upgrade.EquipmentExchangeType.RifleMax, "Rifle MAX", 0, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 2);
                }
                Debug.Log("Upgraded to: " + upgradeType);
            }
            if (buttonType == "Shotgun" && upgradeManager.playerShotgunLevel < 10)
            {
                upgradeManager.playerShotgunLevel++;
                Debug.Log("Shotgun Upgraded to:" + upgradeManager.playerShotgunLevel);
                GameManager.Instance.PlayerInventory.RemoveItem(monsterEss, itemCost);
                ShotgunCost = ShotgunCost + 5;

                upgradeManager.UpgradePlayerWeapon(1);

                this.ShotgunButton.GetComponent<Button>().interactable = false;
                this.ShotgunButton.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText(" ");
                this.ShotgunButton.Find("costText").GetComponent<TextMeshProUGUI>().SetText(" ");
                if (upgradeManager.playerShotgunLevel == 1)
                {
                    ShotgunButton = CreateUpgradeButton("Shotgun", Upgrade.EquipmentExchangeType.Shotgun_2, "Shotgun Up", ShotgunCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 3);
                }
                if (upgradeManager.playerShotgunLevel == 2)
                {
                    ShotgunButton = CreateUpgradeButton("Shotgun", Upgrade.EquipmentExchangeType.Shotgun_3, "Shotgun Up", ShotgunCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 3);
                }
                if (upgradeManager.playerShotgunLevel == 3)
                {
                    ShotgunButton = CreateUpgradeButton("Shotgun", Upgrade.EquipmentExchangeType.Shotgun_4, "Shotgun Up", ShotgunCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 3);
                }
                if (upgradeManager.playerShotgunLevel == 4)
                {
                    ShotgunButton = CreateUpgradeButton("Shotgun", Upgrade.EquipmentExchangeType.Shotgun_5, "Shotgun Up", ShotgunCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 3);
                }
                if (upgradeManager.playerShotgunLevel == 5)
                {
                    ShotgunButton = CreateUpgradeButton("Shotgun", Upgrade.EquipmentExchangeType.Shotgun_6, "Shotgun Up", ShotgunCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 3);
                }
                if (upgradeManager.playerShotgunLevel == 6)
                {
                    ShotgunButton = CreateUpgradeButton("Shotgun", Upgrade.EquipmentExchangeType.Shotgun_7, "Shotgun Up", ShotgunCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 3);
                }
                if (upgradeManager.playerShotgunLevel == 7)
                {
                    ShotgunButton = CreateUpgradeButton("Shotgun", Upgrade.EquipmentExchangeType.Shotgun_8, "Shotgun Up", ShotgunCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 3);
                }
                if (upgradeManager.playerShotgunLevel == 8)
                {
                    ShotgunButton = CreateUpgradeButton("Shotgun", Upgrade.EquipmentExchangeType.Shotgun_9, "Shotgun Up", ShotgunCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 3);
                }
                if (upgradeManager.playerShotgunLevel == 9)
                {
                    ShotgunButton = CreateUpgradeButton("Shotgun", Upgrade.EquipmentExchangeType.ShotgunMax, "Shotgun MAX", ShotgunCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 3);
                }
                Debug.Log("Upgraded to: " + upgradeType);
            }
            if(buttonType == "Health" && upgradeManager.playerHealthLevel < 10)
            {
                upgradeManager.playerHealthLevel++;
                Debug.Log("Health Upgraded to:" + upgradeManager.playerHealthLevel);
                GameManager.Instance.PlayerInventory.RemoveItem(monsterEss, itemCost);
                HealthCost = HealthCost + 5;

                //Amount of Health to increase
                upgradeManager.UpgradePlayerHealth();

                this.HealthButton.GetComponent<Button>().interactable = false;
                this.HealthButton.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText(" ");
                this.HealthButton.Find("costText").GetComponent<TextMeshProUGUI>().SetText(" ");

                if (upgradeManager.playerHealthLevel == 1)
                {
                    HealthButton = CreateUpgradeButton("Health", Upgrade.EquipmentExchangeType.Health_2, "Health Up", HealthCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Health), 4);
                }
                if (upgradeManager.playerShotgunLevel == 2)
                {
                    HealthButton = CreateUpgradeButton("Health", Upgrade.EquipmentExchangeType.Health_3, "Health Up", HealthCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Health), 4);
                }
                if (upgradeManager.playerShotgunLevel == 3)
                {
                    HealthButton = CreateUpgradeButton("Health", Upgrade.EquipmentExchangeType.Health_4, "Health Up", HealthCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Health), 4);
                }
                if (upgradeManager.playerShotgunLevel == 4)
                {
                    HealthButton = CreateUpgradeButton("Health", Upgrade.EquipmentExchangeType.Health_5, "Health Up", HealthCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Health), 4);
                }
                if (upgradeManager.playerShotgunLevel == 5)
                {
                    HealthButton = CreateUpgradeButton("Health", Upgrade.EquipmentExchangeType.Health_6, "Health Up", HealthCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Health), 4);
                }
                if (upgradeManager.playerShotgunLevel == 6)
                {
                    HealthButton = CreateUpgradeButton("Health", Upgrade.EquipmentExchangeType.Health_7, "Health Up", HealthCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Health), 4);
                }
                if (upgradeManager.playerShotgunLevel == 7)
                {
                    HealthButton = CreateUpgradeButton("Health", Upgrade.EquipmentExchangeType.Health_8, "Health Up", HealthCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Health), 4);
                }
                if (upgradeManager.playerShotgunLevel == 8)
                {
                    HealthButton = CreateUpgradeButton("Health", Upgrade.EquipmentExchangeType.Health_9, "Health Up", HealthCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Health), 4);
                }
                if (upgradeManager.playerShotgunLevel == 9)
                { 
                    HealthButton = CreateUpgradeButton("Health", Upgrade.EquipmentExchangeType.HealthMax, "Health MAX", 0, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Health), 4);
                }
            }
            if(buttonType == "Base" && upgradeManager.baseLevel < 5)
            {
                upgradeManager.baseLevel++;
                Debug.Log("Base Upgraded to: " + upgradeManager.baseLevel);
                GameManager.Instance.PlayerInventory.RemoveItem(syntheticOre, itemCost);
                BaseCost = BaseCost * 2;

                upgradeManager.UpgradeBase();

                this.BaseButton.GetComponent<Button>().interactable = false;
                this.BaseButton.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText(" ");
                this.BaseButton.Find("costText").GetComponent<TextMeshProUGUI>().SetText(" ");

                if (upgradeManager.baseLevel == 1)
                {
                    BaseButton = CreateUpgradeButton("Base", Upgrade.EquipmentExchangeType.Base_2, "Base Up", BaseCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Base), 5);
                }
                if (upgradeManager.baseLevel == 2)
                {
                    BaseButton = CreateUpgradeButton("Base", Upgrade.EquipmentExchangeType.Base_3, "Base Up", BaseCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Base), 5);
                }
                if (upgradeManager.baseLevel == 3)
                {
                    BaseButton = CreateUpgradeButton("Base", Upgrade.EquipmentExchangeType.Base_4, "Base Up", BaseCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Base), 5);
                }
                if (upgradeManager.baseLevel == 4)
                {
                    BaseButton = CreateUpgradeButton("Base", Upgrade.EquipmentExchangeType.BaseMax, "Base MAX", 0, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Base), 5);
                }

            }
            if (buttonType == "HMelee" && upgradeManager.playerAxeLevel >= 10)
            {
                //Try to insert popup maybe?
                Debug.Log("Weapon Already at Max Level");
                HMeleeButton.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Heavy Melee MAX");
                HMeleeButton.Find("costText").GetComponent<TextMeshProUGUI>().SetText("0");
                HMeleeButton.GetComponent<Button>().interactable = false;
            }
            if (buttonType == "Rifle" && upgradeManager.playerRifleLevel >= 10)
            {
                //Try to insert popup maybe?
                Debug.Log("Weapon Already at Max Level");
                RifleButton.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Rifle MAX");
                RifleButton.Find("costText").GetComponent<TextMeshProUGUI>().SetText("0");
                RifleButton.GetComponent<Button>().interactable = false;
            }
            if (buttonType == "Shotgun" && upgradeManager.playerShotgunLevel >= 10)
            {
                //Try to insert popup maybe?
                Debug.Log("Weapon Already at Max Level");
                ShotgunButton.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Shotgun MAX");
                ShotgunButton.Find("costText").GetComponent<TextMeshProUGUI>().SetText("0");
                ShotgunButton.GetComponent<Button>().interactable = false;
            }
            if(buttonType == "Health" && upgradeManager.playerHealthLevel >= 10)
            {
                Debug.Log("Health Already at Max Level");
                HealthButton.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Health MAX");
                HealthButton.Find("costText").GetComponent<TextMeshProUGUI>().SetText("0");
                HealthButton.GetComponent<Button>().interactable = false;
            }
            if(buttonType == "Base" && upgradeManager.baseLevel >= 5)
            {
                Debug.Log("Base Already at Max Level");
                BaseButton.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Base MAX");
                BaseButton.Find("costText").GetComponent<TextMeshProUGUI>().SetText("0");
                BaseButton.GetComponent<Button>().interactable = false;
            }
        }
        else
        {
            Debug.Log("Not enough materials");
        }
    }
}
