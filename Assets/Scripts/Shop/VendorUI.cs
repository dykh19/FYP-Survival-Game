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
    private int BaseCost = 25; //To update according to specs.

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
        AxeButton = CreateAxeButton("Axe", "Axe Upgrade!", Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 2);
        SwordButton = CreateSwordButton("Sword", "Sword Upgrade!", Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Weapon), 3);
        HealthButton = CreateHealthButton("Health", "Health Upgrade!", Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Health), 4);
        BaseButton = CreateBaseButton("Base", "Base Upgrade!", Upgrade.GetCurrency(Upgrade.EquipmentExchangeType.Base), 5);

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
            rifleButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Rifle Upgrade!");
        }
        if(upgradeManager.playerRifleLevel >= 10)
        {
            rifleButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Rifle Max Level!");
        }

        rifleButtonTransform.Find("costText").GetComponent<TextMeshProUGUI>().SetText(RifleCost.ToString());

        rifleButtonTransform.GetComponent<Button_UI>().ClickFunc = () =>
        {
            //itemIndex = playerInventory.GetItemIndex(monsterEss);
            try
            {
                currencyCount = GameManager.Instance.PlayerStats.CurrentEssenceInBase;
            }
            catch
            {
                currencyCount = 0;
            }

            if (currencyCount - RifleCost >= 0)
            {
                if (upgradeManager.playerRifleLevel < 10)
                {
                    GameManager.Instance.PlayerStats.DeductEssence(RifleCost);
                    upgradeManager.playerRifleLevel++;
                    upgradeManager.UpgradePlayerWeapon(0);
                    RifleCost = RifleCost + 5;
                    rifleButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Rifle Upgrade!");
                }
                if (upgradeManager.playerSwordLevel >= 10)
                {
                    rifleButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Rifle MAX Level!");
                    rifleButtonTransform.GetComponent<Button>().interactable = false;
                }

                rifleButtonTransform.Find("costText").GetComponent<TextMeshProUGUI>().SetText(RifleCost.ToString());
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
            shotgunButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Shotgun Upgrade!");
        }
        if (upgradeManager.playerShotgunLevel >= 10)
        {
            shotgunButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Shotgun Max Level!");
        }

        shotgunButtonTransform.Find("costText").GetComponent<TextMeshProUGUI>().SetText(ShotgunCost.ToString());

        shotgunButtonTransform.GetComponent<Button_UI>().ClickFunc = () =>
        {
            //itemIndex = playerInventory.GetItemIndex(monsterEss);
            try
            {
                currencyCount = GameManager.Instance.PlayerStats.CurrentEssenceInBase;
            }
            catch
            {
                currencyCount = 0;
            }

            if (currencyCount - ShotgunCost >= 0)
            {
                if (upgradeManager.playerShotgunLevel < 10)
                {
                    GameManager.Instance.PlayerStats.DeductEssence(ShotgunCost);
                    upgradeManager.playerShotgunLevel++;
                    upgradeManager.UpgradePlayerWeapon(1);
                    ShotgunCost = ShotgunCost + 5;
                    shotgunButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Shotgun Upgrade!");
                }
                if (upgradeManager.playerSwordLevel >= 10)
                {
                    shotgunButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Shotgun MAX Level!");
                    shotgunButtonTransform.GetComponent<Button>().interactable = false;
                }
                shotgunButtonTransform.Find("costText").GetComponent<TextMeshProUGUI>().SetText(ShotgunCost.ToString());
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
            axeButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Axe Upgrade!");
        }
        if (upgradeManager.playerAxeLevel >= 10)
        {
            axeButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Axe MAX Level!");
        }

        axeButtonTransform.Find("costText").GetComponent<TextMeshProUGUI>().SetText(AxeCost.ToString());

        axeButtonTransform.GetComponent<Button_UI>().ClickFunc = () =>
        {
           // itemIndex = playerInventory.GetItemIndex(monsterEss);
            try
            {
                currencyCount = GameManager.Instance.PlayerStats.CurrentEssenceInBase;
            }
            catch
            {
                currencyCount = 0;
            }

            if (currencyCount - AxeCost >= 0)
            {
                if (upgradeManager.playerAxeLevel < 10)
                {
                    GameManager.Instance.PlayerStats.DeductEssence(AxeCost);
                    upgradeManager.playerAxeLevel++;
                    upgradeManager.UpgradePlayerWeapon(2);
                    AxeCost = AxeCost + 5;
                    axeButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Axe Upgrade!");
                }
                if (upgradeManager.playerAxeLevel >= 10)
                {
                    axeButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Axe MAX Level!");
                    axeButtonTransform.GetComponent<Button>().interactable = false;
                }

                axeButtonTransform.Find("costText").GetComponent<TextMeshProUGUI>().SetText(AxeCost.ToString());
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
            swordButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Sword Upgrade!");
        }
        if(upgradeManager.playerSwordLevel >= 10)
        {
            swordButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Sword MAX Level!");
        }

        swordButtonTransform.Find("costText").GetComponent<TextMeshProUGUI>().SetText(SwordCost.ToString());

        swordButtonTransform.GetComponent<Button_UI>().ClickFunc = () =>
        {
            //itemIndex = playerInventory.GetItemIndex(monsterEss);
            try
            {
                currencyCount = GameManager.Instance.PlayerStats.CurrentEssenceInBase;
            }
            catch
            {
                currencyCount = 0;
            }

            if (currencyCount - SwordCost >= 0)
            {
                if (upgradeManager.playerSwordLevel < 10)
                {
                    GameManager.Instance.PlayerStats.DeductEssence(SwordCost);
                    upgradeManager.playerSwordLevel++;
                    upgradeManager.UpgradePlayerWeapon(3);
                    SwordCost = SwordCost + 5;
                    swordButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Sword Upgrade!");
                }
                if (upgradeManager.playerSwordLevel >= 10)
                {
                    swordButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Sword MAX Level!");
                    swordButtonTransform.GetComponent<Button>().interactable = false;
                }

                swordButtonTransform.Find("costText").GetComponent<TextMeshProUGUI>().SetText(SwordCost.ToString());
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
            healthButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Health Upgrade!");
        }
        if (upgradeManager.playerHealthLevel >= 10)
        {
            healthButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Health MAX Level!");
        }

        healthButtonTransform.Find("costText").GetComponent<TextMeshProUGUI>().SetText(HealthCost.ToString());

        healthButtonTransform.GetComponent<Button_UI>().ClickFunc = () =>
        {
            //itemIndex = playerInventory.GetItemIndex(monsterEss);
            try
            {
                currencyCount = GameManager.Instance.PlayerStats.CurrentEssenceInBase;
            }
            catch
            {
                currencyCount = 0;
            }

            if (currencyCount - HealthCost >= 0)
            {
                if (upgradeManager.playerHealthLevel < 10)
                {
                    GameManager.Instance.PlayerStats.DeductEssence(HealthCost);
                    upgradeManager.playerHealthLevel++;
                    upgradeManager.UpgradePlayerHealth();
                    HealthCost = HealthCost + 5;
                    healthButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Health Upgrade!");
                }
                if (upgradeManager.playerHealthLevel >= 10)
                {
                    healthButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Health MAX Level!");
                    healthButtonTransform.GetComponent<Button>().interactable = false;
                }

                healthButtonTransform.Find("costText").GetComponent<TextMeshProUGUI>().SetText(HealthCost.ToString());
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
            //itemIndex = playerInventory.GetItemIndex(syntheticOre);
            try
            {
                currencyCount = GameManager.Instance.PlayerStats.CurrentOresInBase;
            }
            catch
            {
                currencyCount = 0;
            }

            if (currencyCount - HealthCost >= 0)
            {
                if (upgradeManager.baseLevel < 10)
                {
                    GameManager.Instance.PlayerStats.DeductOres(BaseCost);
                    upgradeManager.baseLevel++;
                    upgradeManager.UpgradeBase();
                    BaseCost = BaseCost + 5;
                    baseButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Base Upgrade!");
                }
                if (upgradeManager.playerSwordLevel >= 10)
                {
                    baseButtonTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText("Base MAX Level!");
                    baseButtonTransform.GetComponent<Button>().interactable = false;
                }

                baseButtonTransform.Find("costText").GetComponent<TextMeshProUGUI>().SetText(BaseCost.ToString());
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
        }
        else
        {
            RifleButton.GetComponent<Button>().interactable = true;
        }

        if (GameManager.Instance.PlayerStats.CurrentEssenceInBase < ShotgunCost)
        {
            ShotgunButton.GetComponent<Button>().interactable = false;
        }
        else
        {
            ShotgunButton.GetComponent<Button>().interactable = true;
        }

        if (GameManager.Instance.PlayerStats.CurrentEssenceInBase < AxeCost)
        {
            AxeButton.GetComponent<Button>().interactable = false;
        }
        else
        {
            AxeButton.GetComponent<Button>().interactable = true;
        }

        if (GameManager.Instance.PlayerStats.CurrentEssenceInBase < SwordCost)
        {
            SwordButton.GetComponent<Button>().interactable = false;
        }
        else
        {
            SwordButton.GetComponent<Button>().interactable = true;
        }

        if (GameManager.Instance.PlayerStats.CurrentEssenceInBase < HealthCost)
        {
            HealthButton.GetComponent<Button>().interactable = false;
        }
        else
        {
            HealthButton.GetComponent<Button>().interactable = true;
        }

        if (GameManager.Instance.PlayerStats.CurrentOresInBase < BaseCost)
        {
            BaseButton.GetComponent<Button>().interactable = false;
        }
        else
        {
            BaseButton.GetComponent<Button>().interactable = true;
        }
    }

    private void UpdateResourceCount()
    {
        PlayerOreCount.GetComponent<TextMeshProUGUI>().text = "Total Ores: " + GameManager.Instance.PlayerStats.CurrentOresInBase;
        PlayerEssenceCount.GetComponent<TextMeshProUGUI>().text = "Total Essence: " + GameManager.Instance.PlayerStats.CurrentEssenceInBase;
    }
}
