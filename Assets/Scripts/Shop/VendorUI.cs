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
    public GameObject PlayerOreCount;
    public GameObject PlayerEssenceCount;

    [SerializeField]
    private int SwordCost = 1; //10 previously;
    [SerializeField]
    private int AxeCost = 1;
    [SerializeField]
    private int RifleCost = 1;
    [SerializeField]
    private int ShotgunCost = 1;
    [SerializeField]
    private int HealthCost = 1;
    [SerializeField]
    private int BaseCost = GameStats.PlayerBaseUpgradeCost[1]; //To update according to specs.

    public int itemIndex;
    public int currencyCount;

    private Transform SwordButton;
    private Transform AxeButton;
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

    private void Start()
    {
        //Assigning the relevant objects
        playerInventoryUI = FindObjectOfType<InventoryUI>();
        playerInventory = GameManager.Instance.PlayerInventory;
        upgradeManager = FindObjectOfType<UpgradeManager>();

        //Creating the initial buttons
        RifleButton = CreateRifleButton("Rifle", "Rifle Upgrade Button", Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 0);
        ShotgunButton = CreateShotgunButton("Shotgun", "Shotgun Upgrade Button", Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 1);
        AxeButton = CreateAxeButton("Axe", "Axe Upgrade Button", Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 2);
        SwordButton = CreateSwordButton("Sword", "Sword Upgrade Button", Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 3);
        HealthButton = CreateHealthButton("Health", "Health Upgrad Button", Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Health), 4);
        BaseButton = CreateBaseButton("Base", "Base Upgrade Button", Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Base), 5);

        UpdateResourceCount();
    }

    private void Update()
    {
        CheckIfCanUpgrade();
        UpdateResourceCount();
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
            rifleButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Upgrade Rifle to level " + (upgradeManager.playerRifleLevel + 1));
            rifleButtonTransform.Find("costText").GetComponent<TextMeshProUGUI>().SetText("Essence Cost: " + RifleCost.ToString());
        }
        if(upgradeManager.playerRifleLevel >= 10)
        {
            rifleButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Rifle Max Level!");
            rifleButtonTransform.Find("costText").GetComponent<TextMeshProUGUI>().SetText("");
        }

        rifleButtonTransform.GetComponent<Button_UI>().ClickFunc = () =>
        {
            currencyCount = GameManager.Instance.PlayerStats.CurrentEssenceInBase;
            if (currencyCount - RifleCost >= 0)
            {
                if (upgradeManager.playerRifleLevel < 10)
                {
                    GameManager.Instance.PlayerStats.DeductEssence(RifleCost);
                    upgradeManager.playerRifleLevel++;
                    upgradeManager.UpgradePlayerWeapon(0);
                    RifleCost = RifleCost + 5;
                    rifleButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Upgrade Rifle to level " + (upgradeManager.playerRifleLevel + 1));
                    rifleButtonTransform.Find("costText").GetComponent<TextMeshProUGUI>().SetText("Essence Cost: " + RifleCost.ToString());
                }
                if (upgradeManager.playerRifleLevel >= 10)
                {
                    rifleButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Rifle MAX Level!");
                    rifleButtonTransform.Find("costText").GetComponent<TextMeshProUGUI>().SetText("");
                }
            }
            UpdateResourceCount();
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
            shotgunButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Upgrade Shotgun to level " + (upgradeManager.playerShotgunLevel + 1));
            shotgunButtonTransform.Find("costText").GetComponent<TextMeshProUGUI>().SetText("Essence Cost: " + ShotgunCost.ToString());
        }
        if (upgradeManager.playerShotgunLevel >= 10)
        {
            shotgunButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Shotgun Max Level!");
            shotgunButtonTransform.Find("costText").GetComponent<TextMeshProUGUI>().SetText("");
        }

        

        shotgunButtonTransform.GetComponent<Button_UI>().ClickFunc = () =>
        {
            currencyCount = GameManager.Instance.PlayerStats.CurrentEssenceInBase;
            if (currencyCount - ShotgunCost >= 0)
            {
                if (upgradeManager.playerShotgunLevel < 10)
                {
                    GameManager.Instance.PlayerStats.DeductEssence(ShotgunCost);
                    upgradeManager.playerShotgunLevel++;
                    upgradeManager.UpgradePlayerWeapon(1);
                    ShotgunCost = ShotgunCost + 5;
                    shotgunButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Upgrade Shotgun to level " + (upgradeManager.playerShotgunLevel + 1));
                    shotgunButtonTransform.Find("costText").GetComponent<TextMeshProUGUI>().SetText("Essence Cost: " + ShotgunCost.ToString());
                }
                if (upgradeManager.playerShotgunLevel >= 10)
                {
                    shotgunButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Shotgun MAX Level!");
                    shotgunButtonTransform.Find("costText").GetComponent<TextMeshProUGUI>().SetText("");
                }
            }
            UpdateResourceCount();
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
            axeButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Upgrade Axe to level " + (upgradeManager.playerAxeLevel + 1));
            axeButtonTransform.Find("costText").GetComponent<TextMeshProUGUI>().SetText("Essence Cost: " + AxeCost.ToString());
        }
        if (upgradeManager.playerAxeLevel >= 10)
        {
            axeButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Axe MAX Level!");
            axeButtonTransform.Find("costText").GetComponent<TextMeshProUGUI>().SetText("");
        }

        axeButtonTransform.GetComponent<Button_UI>().ClickFunc = () =>
        {
            currencyCount = GameManager.Instance.PlayerStats.CurrentEssenceInBase;
            if (currencyCount - AxeCost >= 0)
            {
                if (upgradeManager.playerAxeLevel < 10)
                {
                    GameManager.Instance.PlayerStats.DeductEssence(AxeCost);
                    upgradeManager.playerAxeLevel++;
                    upgradeManager.UpgradePlayerWeapon(2);
                    AxeCost = AxeCost + 5;
                    axeButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Upgrade Axe to level " + (upgradeManager.playerAxeLevel + 1));
                    axeButtonTransform.Find("costText").GetComponent<TextMeshProUGUI>().SetText("Essence Cost: " + AxeCost.ToString());
                }
                if (upgradeManager.playerAxeLevel >= 10)
                {
                    axeButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Axe MAX Level!");
                    axeButtonTransform.Find("costText").GetComponent<TextMeshProUGUI>().SetText("");
                }
            }
            UpdateResourceCount();
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
            swordButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Upgrade Sword to level " + (upgradeManager.playerSwordLevel + 1));
            swordButtonTransform.Find("costText").GetComponent<TextMeshProUGUI>().SetText("Essence Cost: " + SwordCost.ToString());
        }
        if(upgradeManager.playerSwordLevel >= 10)
        {
            swordButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Sword MAX Level!");
            swordButtonTransform.Find("costText").GetComponent<TextMeshProUGUI>().SetText("");
        }

        

        swordButtonTransform.GetComponent<Button_UI>().ClickFunc = () =>
        {
            currencyCount = GameManager.Instance.PlayerStats.CurrentEssenceInBase;
            if (currencyCount - SwordCost >= 0)
            {
                if (upgradeManager.playerSwordLevel < 10)
                {
                    GameManager.Instance.PlayerStats.DeductEssence(SwordCost);
                    upgradeManager.playerSwordLevel++;
                    upgradeManager.UpgradePlayerWeapon(3);
                    SwordCost = SwordCost + 5;
                    swordButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Upgrade Sword to level " + (upgradeManager.playerSwordLevel + 1));
                    swordButtonTransform.Find("costText").GetComponent<TextMeshProUGUI>().SetText("Essence Cost: " + SwordCost.ToString());
                }
                if (upgradeManager.playerSwordLevel >= 10)
                {
                    swordButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Sword MAX Level!");
                    swordButtonTransform.Find("costText").GetComponent<TextMeshProUGUI>().SetText("");
                }
            }
            UpdateResourceCount();
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
            healthButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Upgrade Health to level " + (upgradeManager.playerHealthLevel + 1));
            healthButtonTransform.Find("costText").GetComponent<TextMeshProUGUI>().SetText("Essence Cost: " + HealthCost.ToString());
        }
        if (upgradeManager.playerHealthLevel >= 10)
        {
            healthButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Health MAX Level!");
            healthButtonTransform.Find("costText").GetComponent<TextMeshProUGUI>().SetText("");
        }

        healthButtonTransform.GetComponent<Button_UI>().ClickFunc = () =>
        {
            currencyCount = GameManager.Instance.PlayerStats.CurrentEssenceInBase;
            if (currencyCount - HealthCost >= 0)
            {
                if (upgradeManager.playerHealthLevel < 10)
                {
                    GameManager.Instance.PlayerStats.DeductEssence(HealthCost);
                    upgradeManager.playerHealthLevel++;
                    upgradeManager.UpgradePlayerHealth();
                    HealthCost = HealthCost + 5;
                    healthButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Upgrade Health to level " + (upgradeManager.playerHealthLevel + 1));
                    healthButtonTransform.Find("costText").GetComponent<TextMeshProUGUI>().SetText("Essence Cost: " + HealthCost.ToString());
                }
                if (upgradeManager.playerHealthLevel >= 10)
                {
                    healthButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Health MAX Level!");
                    healthButtonTransform.Find("costText").GetComponent<TextMeshProUGUI>().SetText("");
                }
            }
            UpdateResourceCount();
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

        if (upgradeManager.baseLevel < 4)
        {
            baseButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Upgrade Base to level " + (upgradeManager.baseLevel + 1));
            baseButtonTransform.Find("costText").GetComponent<TextMeshProUGUI>().SetText("Refined Ore Cost: " + BaseCost.ToString());
        }
        if (upgradeManager.baseLevel >= 4)
        {
            baseButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Base MAX Level!");
            baseButtonTransform.Find("costText").GetComponent<TextMeshProUGUI>().SetText("");
        }

        

        baseButtonTransform.GetComponent<Button_UI>().ClickFunc = () =>
        {
            currencyCount = GameManager.Instance.PlayerStats.CurrentOresInBase;
            if (currencyCount - BaseCost >= 0)
            {
                if (upgradeManager.baseLevel < 4)
                {
                    GameManager.Instance.PlayerStats.DeductOres(BaseCost);
                    upgradeManager.baseLevel++;
                    upgradeManager.UpgradeBase();
                    if (upgradeManager.baseLevel < 4)
                    {
                        BaseCost = GameStats.PlayerBaseUpgradeCost[upgradeManager.baseLevel + 1];
                        baseButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Upgrade Base to level " + (upgradeManager.baseLevel + 1));
                        baseButtonTransform.Find("costText").GetComponent<TextMeshProUGUI>().SetText("Refined Ore Cost: " + BaseCost.ToString());
                    }   
                }
                if (upgradeManager.baseLevel >= 4)
                {
                    baseButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Base MAX Level!");
                    baseButtonTransform.Find("costText").GetComponent<TextMeshProUGUI>().SetText("");
                }
            }
            UpdateResourceCount();
        };

        return baseButtonTransform;
    }

    private void CheckIfCanUpgrade()
    {
        if (GameManager.Instance.PlayerStats.CurrentEssenceInBase < RifleCost)
        {
            RifleButton.GetComponent<Button>().interactable = false;
            RifleButton.GetComponent<Button_UI>().enabled = false;
        }
        else
        {
            RifleButton.GetComponent<Button>().interactable = true;
            RifleButton.GetComponent<Button_UI>().enabled = true;
        }

        if (GameManager.Instance.PlayerStats.CurrentEssenceInBase < ShotgunCost)
        {
            ShotgunButton.GetComponent<Button>().interactable = false;
            ShotgunButton.GetComponent<Button_UI>().enabled = false;
        }
        else
        {
            ShotgunButton.GetComponent<Button>().interactable = true;
            ShotgunButton.GetComponent<Button_UI>().enabled = true;
        }

        if (GameManager.Instance.PlayerStats.CurrentEssenceInBase < AxeCost)
        {
            AxeButton.GetComponent<Button>().interactable = false;
            AxeButton.GetComponent<Button_UI>().enabled = false;
        }
        else
        {
            AxeButton.GetComponent<Button>().interactable = true;
            AxeButton.GetComponent<Button_UI>().enabled = true;
        }

        if (GameManager.Instance.PlayerStats.CurrentEssenceInBase < SwordCost)
        {
            SwordButton.GetComponent<Button>().interactable = false;
            SwordButton.GetComponent<Button_UI>().enabled = false;
        }
        else
        {
            SwordButton.GetComponent<Button>().interactable = true;
            SwordButton.GetComponent<Button_UI>().enabled = true;
        }

        if (GameManager.Instance.PlayerStats.CurrentEssenceInBase < HealthCost)
        {
            HealthButton.GetComponent<Button>().interactable = false;
            HealthButton.GetComponent<Button_UI>().enabled = false;
        }
        else
        {
            HealthButton.GetComponent<Button>().interactable = true;
            HealthButton.GetComponent<Button_UI>().enabled = true;
        }

        if (GameManager.Instance.PlayerStats.CurrentOresInBase < BaseCost)
        {
            BaseButton.GetComponent<Button>().interactable = false;
            BaseButton.GetComponent<Button_UI>().enabled = false;
        }
        else
        {
            BaseButton.GetComponent<Button>().interactable = true;
            BaseButton.GetComponent<Button_UI>().enabled = true;
        }
    }

    private void UpdateResourceCount()
    {
        PlayerOreCount.GetComponent<TextMeshProUGUI>().text = "Total Ores: " + GameManager.Instance.PlayerStats.CurrentOresInBase;
        PlayerEssenceCount.GetComponent<TextMeshProUGUI>().text = "Total Essence: " + GameManager.Instance.PlayerStats.CurrentEssenceInBase;
    }
}
