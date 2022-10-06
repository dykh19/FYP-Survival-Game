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
    public int HealthLvl = 0;
    public int BaseLvl = 0;

    public int LMeleeCost = 10;
    public int HMeleeCost = 10;
    public int RifleCost = 10;
    public int ShotgunCost = 10;
    public int HealthCost = 10;
    public int BaseCost = 25; //To update according to specs.

    public int itemIndex;
    public int itemCount;

    private Transform LMeleeButton;
    private Transform HMeleeButton;
    private Transform RifleButton;
    private Transform ShotgunButton;
    private Transform HealthButton;
    private Transform BaseButton;
    public InventoryUI playerInventoryUI;
    public Inventory playerInventory;
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
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        container = transform.Find("Container");
        vendorItemTemplate = container.Find("VendorItemTemplate");
        //vendorItemTemplate.gameObject.SetActive(false);

        //Creating the initial buttons
        LMeleeButton = CreateUpgradeButton("LMelee", Upgrade.EquipmentExchangeType.LMelee_1, "Light Melee Up", LMeleeCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 0);
        HMeleeButton = CreateUpgradeButton("HMelee", Upgrade.EquipmentExchangeType.HMelee_1, "Heavy Melee Up", HMeleeCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 1);
        RifleButton = CreateUpgradeButton("Rifle", Upgrade.EquipmentExchangeType.Rifle_1, "Rifle Up", RifleCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 2);
        ShotgunButton = CreateUpgradeButton("Shotgun", Upgrade.EquipmentExchangeType.Shotgun_1, "Shotgun Up", ShotgunCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 3);
        HealthButton = CreateUpgradeButton("Health", Upgrade.EquipmentExchangeType.Health_1, "Health Up", HealthCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Health), 4);
        BaseButton = CreateUpgradeButton("Base", Upgrade.EquipmentExchangeType.Base_1, "Base Up", BaseCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Base), 5);
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
                itemCount = GameManager.Instance.PlayerInventory.Items[itemIndex].quantity;
            }
            catch
            {
                itemCount = 0;
            }
        }
        else if(currency == "SyntheticOre")
        {
            itemIndex = playerInventory.GetItemIndex(syntheticOre);
            try
            {
                itemCount = GameManager.Instance.PlayerInventory.Items[itemIndex].quantity;
            }
            catch
            {
                itemCount = 0;
            }
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
                //Damian to include functions in Weapons Script soon

                this.LMeleeButton.GetComponent<Button>().interactable = false;
                this.LMeleeButton.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText(" ");
                this.LMeleeButton.Find("costText").GetComponent<TextMeshProUGUI>().SetText(" ");


                if (LMeleeLvl == 1)
                {
                    LMeleeButton = CreateUpgradeButton("LMelee", Upgrade.EquipmentExchangeType.LMelee_2, "Light Melee Up", LMeleeCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 0);
                }
                if(LMeleeLvl == 2)
                {
                    LMeleeButton = CreateUpgradeButton("LMelee", Upgrade.EquipmentExchangeType.LMelee_3, "Light Melee Up", LMeleeCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon),0);
                }
                if(LMeleeLvl == 3)
                {
                    LMeleeButton = CreateUpgradeButton("LMelee", Upgrade.EquipmentExchangeType.LMelee_4, "Light Melee Up", LMeleeCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 0);
                }
                if(LMeleeLvl == 4)
                {
                    LMeleeButton = CreateUpgradeButton("LMelee", Upgrade.EquipmentExchangeType.LMelee_5, "Light Melee Up", LMeleeCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 0);
                }
                if (LMeleeLvl == 5)
                {
                    LMeleeButton = CreateUpgradeButton("LMelee", Upgrade.EquipmentExchangeType.LMelee_6, "Light Melee Up", LMeleeCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 0);
                }
                if (LMeleeLvl == 6)
                {
                    LMeleeButton = CreateUpgradeButton("LMelee", Upgrade.EquipmentExchangeType.LMelee_7, "Light Melee Up", LMeleeCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 0);
                }
                if (LMeleeLvl == 7)
                {
                    LMeleeButton = CreateUpgradeButton("LMelee", Upgrade.EquipmentExchangeType.LMelee_8, "Light Melee Up", LMeleeCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 0);
                }
                if (LMeleeLvl == 8)
                {
                    LMeleeButton = CreateUpgradeButton("LMelee", Upgrade.EquipmentExchangeType.LMelee_9, "Light Melee Up", LMeleeCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 0);
                }
                if (LMeleeLvl == 9)
                {
                    LMeleeButton = CreateUpgradeButton("LMelee", Upgrade.EquipmentExchangeType.LMeleeMax, "Light Melee MAX", 0, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 0);
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
                //Damian to include functions in Weapons Script soon

                this.HMeleeButton.GetComponent<Button>().interactable = false;
                this.HMeleeButton.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText(" ");
                this.HMeleeButton.Find("costText").GetComponent<TextMeshProUGUI>().SetText(" ");
                if (HMeleeLvl == 1)
                {
                    HMeleeButton = CreateUpgradeButton("HMelee", Upgrade.EquipmentExchangeType.HMelee_2, "Heavy Melee Up", HMeleeCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 1);
                }
                if (HMeleeLvl == 2)
                {
                    HMeleeButton = CreateUpgradeButton("HMelee", Upgrade.EquipmentExchangeType.HMelee_3, "Heavy Melee Up", HMeleeCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 1);
                }
                if (HMeleeLvl == 3)
                {
                    HMeleeButton = CreateUpgradeButton("HMelee", Upgrade.EquipmentExchangeType.HMelee_4, "Heavy Melee Up", HMeleeCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 1);
                }
                if (HMeleeLvl == 4)
                {
                    HMeleeButton = CreateUpgradeButton("HMelee", Upgrade.EquipmentExchangeType.HMelee_5, "Heavy Melee Up", HMeleeCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 1);
                }
                if (HMeleeLvl == 5)
                {
                    HMeleeButton = CreateUpgradeButton("HMelee", Upgrade.EquipmentExchangeType.HMelee_6, "Heavy Melee Up", HMeleeCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 1);
                }
                if (HMeleeLvl == 6)
                {
                    HMeleeButton = CreateUpgradeButton("HMelee", Upgrade.EquipmentExchangeType.HMelee_7, "Heavy Melee Up", HMeleeCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 1);
                }
                if (HMeleeLvl == 7)
                {
                    HMeleeButton = CreateUpgradeButton("HMelee", Upgrade.EquipmentExchangeType.HMelee_8, "Heavy Melee Up", HMeleeCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 1);
                }
                if (HMeleeLvl == 8)
                {
                    HMeleeButton = CreateUpgradeButton("HMelee", Upgrade.EquipmentExchangeType.HMelee_9, "Heavy Melee Up", HMeleeCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 1);
                }
                if (HMeleeLvl == 9)
                {
                    HMeleeButton = CreateUpgradeButton("HMelee", Upgrade.EquipmentExchangeType.HMeleeMax, "Heavy Melee MAX", 0, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 1);
                }
                Debug.Log("Upgraded to: " + upgradeType);
            }
            if (buttonType == "Rifle" && RifleLvl < 10)
            {
                RifleLvl++;
                Debug.Log("Rifle Upgraded to:" + RifleLvl);
                GameManager.Instance.PlayerInventory.RemoveItem(monsterEss, itemCost);
                RifleCost = RifleCost + 5;

                //Insert Eddie's Upgrade Calculations for Light Melee Weapons
                //Damian to include functions in Weapons Script soon

                this.RifleButton.GetComponent<Button>().interactable = false;
                this.RifleButton.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText(" ");
                this.RifleButton.Find("costText").GetComponent<TextMeshProUGUI>().SetText(" ");
                if (RifleLvl == 1)
                {
                    RifleButton = CreateUpgradeButton("Rifle", Upgrade.EquipmentExchangeType.Rifle_2, "Rifle Up", RifleCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 2);
                }
                if (RifleLvl == 2)
                {
                    RifleButton = CreateUpgradeButton("Rifle", Upgrade.EquipmentExchangeType.Rifle_3, "Rifle Up", RifleCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 2);
                }
                if (RifleLvl == 3)
                {
                    RifleButton = CreateUpgradeButton("Rifle", Upgrade.EquipmentExchangeType.Rifle_4, "Rifle Up", RifleCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 2);
                }
                if (RifleLvl == 4)
                {
                    RifleButton = CreateUpgradeButton("Rifle", Upgrade.EquipmentExchangeType.Rifle_5, "Rifle Up", RifleCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 2);
                }
                if (RifleLvl == 5)
                {
                    RifleButton = CreateUpgradeButton("Rifle", Upgrade.EquipmentExchangeType.Rifle_6, "Rifle Up", RifleCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 2);
                }
                if (RifleLvl == 6)
                {
                    RifleButton = CreateUpgradeButton("Rifle", Upgrade.EquipmentExchangeType.Rifle_7, "Rifle Up", RifleCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 2);
                }
                if (RifleLvl == 7)
                {
                    RifleButton = CreateUpgradeButton("Rifle", Upgrade.EquipmentExchangeType.Rifle_8, "Rifle Up", RifleCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 2);
                }
                if (RifleLvl == 8)
                {
                    RifleButton = CreateUpgradeButton("Rifle", Upgrade.EquipmentExchangeType.Rifle_9, "Rifle Up", RifleCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 2);
                }
                if (RifleLvl == 9)
                {
                    RifleButton = CreateUpgradeButton("Rifle", Upgrade.EquipmentExchangeType.RifleMax, "Rifle MAX", 0, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 2);
                }
                Debug.Log("Upgraded to: " + upgradeType);
            }
            if (buttonType == "Shotgun" && ShotgunLvl < 10)
            {
                ShotgunLvl++;
                Debug.Log("Shotgun Upgraded to:" + ShotgunLvl);
                GameManager.Instance.PlayerInventory.RemoveItem(monsterEss, itemCost);
                ShotgunCost = ShotgunCost + 5;

                //Insert Eddie's Upgrade Calculations for Light Melee Weapons
                //Damian to include functions in Weapons Script soon

                this.ShotgunButton.GetComponent<Button>().interactable = false;
                this.ShotgunButton.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText(" ");
                this.ShotgunButton.Find("costText").GetComponent<TextMeshProUGUI>().SetText(" ");
                if (ShotgunLvl == 1)
                {
                    ShotgunButton = CreateUpgradeButton("Shotgun", Upgrade.EquipmentExchangeType.Shotgun_2, "Shotgun Up", ShotgunCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 3);
                }
                if (ShotgunLvl == 2)
                {
                    ShotgunButton = CreateUpgradeButton("Shotgun", Upgrade.EquipmentExchangeType.Shotgun_3, "Shotgun Up", ShotgunCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 3);
                }
                if (ShotgunLvl == 3)
                {
                    ShotgunButton = CreateUpgradeButton("Shotgun", Upgrade.EquipmentExchangeType.Shotgun_4, "Shotgun Up", ShotgunCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 3);
                }
                if (ShotgunLvl == 4)
                {
                    ShotgunButton = CreateUpgradeButton("Shotgun", Upgrade.EquipmentExchangeType.Shotgun_5, "Shotgun Up", ShotgunCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 3);
                }
                if (ShotgunLvl == 5)
                {
                    ShotgunButton = CreateUpgradeButton("Shotgun", Upgrade.EquipmentExchangeType.Shotgun_6, "Shotgun Up", ShotgunCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 3);
                }
                if (ShotgunLvl == 6)
                {
                    ShotgunButton = CreateUpgradeButton("Shotgun", Upgrade.EquipmentExchangeType.Shotgun_7, "Shotgun Up", ShotgunCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 3);
                }
                if (ShotgunLvl == 7)
                {
                    ShotgunButton = CreateUpgradeButton("Shotgun", Upgrade.EquipmentExchangeType.Shotgun_8, "Shotgun Up", ShotgunCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 3);
                }
                if (ShotgunLvl == 8)
                {
                    ShotgunButton = CreateUpgradeButton("Shotgun", Upgrade.EquipmentExchangeType.Shotgun_9, "Shotgun Up", ShotgunCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 3);
                }
                if (ShotgunLvl == 9)
                {
                    ShotgunButton = CreateUpgradeButton("Shotgun", Upgrade.EquipmentExchangeType.ShotgunMax, "Shotgun MAX", ShotgunCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 3);
                }
                Debug.Log("Upgraded to: " + upgradeType);
            }
            if(buttonType == "Health" && HealthLvl < 10)
            {
                HealthLvl++;
                Debug.Log("Health Upgraded to:" + HealthLvl);
                GameManager.Instance.PlayerInventory.RemoveItem(monsterEss, itemCost);
                HealthCost = HealthCost + 5;

                //Amount of Health to increase
                playerHealth.SetHealth(playerHealth.MaxHealth + 20);
                playerHealth.Heal(playerHealth.MaxHealth - playerHealth.CurrentHealth);
                Debug.Log(playerHealth.CurrentHealth);

                this.HealthButton.GetComponent<Button>().interactable = false;
                this.HealthButton.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText(" ");
                this.HealthButton.Find("costText").GetComponent<TextMeshProUGUI>().SetText(" ");

                if (HealthLvl == 1)
                {
                    HealthButton = CreateUpgradeButton("Health", Upgrade.EquipmentExchangeType.Health_2, "Health Up", HealthCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Health), 4);
                }
                if (ShotgunLvl == 2)
                {
                    HealthButton = CreateUpgradeButton("Health", Upgrade.EquipmentExchangeType.Health_3, "Health Up", HealthCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Health), 4);
                }
                if (ShotgunLvl == 3)
                {
                    HealthButton = CreateUpgradeButton("Health", Upgrade.EquipmentExchangeType.Health_4, "Health Up", HealthCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Health), 4);
                }
                if (ShotgunLvl == 4)
                {
                    HealthButton = CreateUpgradeButton("Health", Upgrade.EquipmentExchangeType.Health_5, "Health Up", HealthCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Health), 4);
                }
                if (ShotgunLvl == 5)
                {
                    HealthButton = CreateUpgradeButton("Health", Upgrade.EquipmentExchangeType.Health_6, "Health Up", HealthCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Health), 4);
                }
                if (ShotgunLvl == 6)
                {
                    HealthButton = CreateUpgradeButton("Health", Upgrade.EquipmentExchangeType.Health_7, "Health Up", HealthCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Health), 4);
                }
                if (ShotgunLvl == 7)
                {
                    HealthButton = CreateUpgradeButton("Health", Upgrade.EquipmentExchangeType.Health_8, "Health Up", HealthCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Health), 4);
                }
                if (ShotgunLvl == 8)
                {
                    HealthButton = CreateUpgradeButton("Health", Upgrade.EquipmentExchangeType.Health_9, "Health Up", HealthCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Health), 4);
                }
                if (ShotgunLvl == 9)
                { 
                    HealthButton = CreateUpgradeButton("Health", Upgrade.EquipmentExchangeType.HealthMax, "Health MAX", 0, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Health), 4);
                }
            }
            if(buttonType == "Base" && BaseLvl < 5)
            {
                BaseLvl++;
                Debug.Log("Base Upgraded to: " + BaseLvl);
                GameManager.Instance.PlayerInventory.RemoveItem(syntheticOre, itemCost);
                BaseCost = BaseCost * 2;

                //Input code to upgrade

                this.BaseButton.GetComponent<Button>().interactable = false;
                this.BaseButton.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText(" ");
                this.BaseButton.Find("costText").GetComponent<TextMeshProUGUI>().SetText(" ");

                if (BaseLvl == 1)
                {
                    BaseButton = CreateUpgradeButton("Base", Upgrade.EquipmentExchangeType.Base_2, "Base Up", BaseCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Base), 5);
                }
                if (BaseLvl == 2)
                {
                    BaseButton = CreateUpgradeButton("Base", Upgrade.EquipmentExchangeType.Base_3, "Base Up", BaseCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Base), 5);
                }
                if (BaseLvl == 3)
                {
                    BaseButton = CreateUpgradeButton("Base", Upgrade.EquipmentExchangeType.Base_4, "Base Up", BaseCost, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Base), 5);
                }
                if (BaseLvl == 4)
                {
                    BaseButton = CreateUpgradeButton("Base", Upgrade.EquipmentExchangeType.BaseMax, "Base MAX", 0, Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Base), 5);
                }

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
            if(buttonType == "Health" && HealthLvl >= 10)
            {
                Debug.Log("Health Already at Max Level");
                HealthButton.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Health MAX");
                HealthButton.Find("costText").GetComponent<TextMeshProUGUI>().SetText("0");
                HealthButton.GetComponent<Button>().interactable = false;
            }
            if(buttonType == "Base" && BaseLvl >= 5)
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
