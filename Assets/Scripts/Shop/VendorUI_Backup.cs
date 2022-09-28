using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;
using TMPro;

/*Script First Created by Sean Ong on 26/9/22 12:10pm*/

public class VendorUI_Backup : MonoBehaviour
{
    public Transform container;
    public Transform vendorItemTemplate;

    public int LMeleeLvl = 0;
    public int HMeleeLvl = 0;
    public int RifleLvl = 0;
    public int ShotgunLvl = 0;
    public int buttonCount = 0;

    private Transform LMeleeButton;
    private Transform HMeleeButton;
    private Transform RifleButton;
    private Transform ShotgunButton;
    public InventoryUI playerInventoryUI;
    public Inventory playerInventory;
    public GameItem creepDrop;

    private void Awake()
    {
    }

    private void Start()
    {
        playerInventoryUI = FindObjectOfType<InventoryUI>();
        playerInventory = GameManager.Instance.PlayerInventory;
        container = transform.Find("Container");
        vendorItemTemplate = container.Find("VendorItemTemplate");
        //vendorItemTemplate.gameObject.SetActive(false);

        LMeleeButton = CreateUpgradeButton("LMelee", Upgrade.EquipmentExchangeType.LMelee_1, "Light Melee Up", Upgrade.GetCost(Upgrade.EquipmentExchangeType.LMelee_1), 0);
        HMeleeButton = CreateUpgradeButton("HMelee", Upgrade.EquipmentExchangeType.HMelee_1, "Heavy Melee Up", Upgrade.GetCost(Upgrade.EquipmentExchangeType.HMelee_1), 1);
        RifleButton = CreateUpgradeButton("Rifle", Upgrade.EquipmentExchangeType.Rifle_1, "Rifle Up", Upgrade.GetCost(Upgrade.EquipmentExchangeType.Rifle_1), 2);
        ShotgunButton = CreateUpgradeButton("Shotgun", Upgrade.EquipmentExchangeType.Shotgun_1, "Shotgun Up", Upgrade.GetCost(Upgrade.EquipmentExchangeType.Shotgun_1), 3);
        buttonCount = 4;
    }

    private void Update()
    {
        LMeleeButton = CreateUpgradeButton("LMelee", Upgrade.EquipmentExchangeType.LMelee_1, "Light Melee Up", Upgrade.GetCost(Upgrade.EquipmentExchangeType.LMelee_1), 0);
        HMeleeButton = CreateUpgradeButton("HMelee", Upgrade.EquipmentExchangeType.HMelee_1, "Heavy Melee Up", Upgrade.GetCost(Upgrade.EquipmentExchangeType.HMelee_1), 1);
        RifleButton = CreateUpgradeButton("Rifle", Upgrade.EquipmentExchangeType.Rifle_1, "Rifle Up", Upgrade.GetCost(Upgrade.EquipmentExchangeType.Rifle_1), 2);
        ShotgunButton = CreateUpgradeButton("Shotgun", Upgrade.EquipmentExchangeType.Shotgun_1, "Shotgun Up", Upgrade.GetCost(Upgrade.EquipmentExchangeType.Shotgun_1), 3);
        buttonCount = 4;
        /*//Button for Light Melee Weapon Upgrade
        if (LMeleeLvl == 0 && buttonCount < 4)
        {
            UpdateUpgradeButton("LMeleeButton", Upgrade.EquipmentUpgradeType.LMelee_1, "Light Melee Up", Upgrade.GetCost(Upgrade.EquipmentUpgradeType.LMelee_1), 0);
        }
        else if (LMeleeLvl == 1 && buttonCount < 5)
        {
            UpdateUpgradeButton("LMeleeButton", Upgrade.EquipmentUpgradeType.LMelee_2, "Light Melee Up", Upgrade.GetCost(Upgrade.EquipmentUpgradeType.LMelee_2), 0);
        }
        else if (LMeleeLvl == 2 && buttonCount < 5)
        {
            UpdateUpgradeButton("LMeleeButton", Upgrade.EquipmentUpgradeType.LMelee_3, "Light Melee Up", Upgrade.GetCost(Upgrade.EquipmentUpgradeType.LMelee_3), 0);
        }
        else if (LMeleeLvl == 3 && buttonCount < 5)
        {
            UpdateUpgradeButton("LMeleeButton", Upgrade.EquipmentUpgradeType.LMelee_4, "Light Melee Up", Upgrade.GetCost(Upgrade.EquipmentUpgradeType.LMelee_4), 0);
        }
        else if (LMeleeLvl > 3 && buttonCount < 5)
        {
            UpdateUpgradeButton("LMeleeButton", Upgrade.EquipmentUpgradeType.LMeleeMax, "Light Melee MAX", Upgrade.GetCost(Upgrade.EquipmentUpgradeType.LMeleeMax), 0);
        }

        //Button for Heavy Melee Weapon Upgrade
        if (HMeleeLvl == 0 && buttonCount < 5)
        {
            UpdateUpgradeButton("HMeleeButton", Upgrade.EquipmentUpgradeType.HMelee_1, "Heavy Melee Up", Upgrade.GetCost(Upgrade.EquipmentUpgradeType.HMelee_1), 1);
        }
        else if (HMeleeLvl == 1 && buttonCount < 5)
        {
            UpdateUpgradeButton("HMeleeButton", Upgrade.EquipmentUpgradeType.HMelee_2, "Heavy Melee Up", Upgrade.GetCost(Upgrade.EquipmentUpgradeType.HMelee_2), 1);
        }
        else if (HMeleeLvl == 2 && buttonCount < 5)
        {
            UpdateUpgradeButton("HMeleeButton", Upgrade.EquipmentUpgradeType.HMelee_3, "Heavy Melee Up", Upgrade.GetCost(Upgrade.EquipmentUpgradeType.HMelee_3), 1);
        }
        else if (HMeleeLvl == 3 && buttonCount < 5)
        {
            UpdateUpgradeButton("HMeleeButton", Upgrade.EquipmentUpgradeType.HMelee_4, "Heavy Melee Up", Upgrade.GetCost(Upgrade.EquipmentUpgradeType.HMelee_4), 1);
        }
        else if (HMeleeLvl > 3 && buttonCount < 5)
        {
            UpdateUpgradeButton("HMeleeButton", Upgrade.EquipmentUpgradeType.HMeleeMax, "Heavy Melee MAX", Upgrade.GetCost(Upgrade.EquipmentUpgradeType.HMeleeMax), 0);
        }

        //Button for Rifle Weapon Upgrade
        if (RifleLvl == 0 && buttonCount < 5)
        {
            UpdateUpgradeButton("RifleButton", Upgrade.EquipmentUpgradeType.Rifle_1, "Rifle Up", Upgrade.GetCost(Upgrade.EquipmentUpgradeType.Rifle_1), 1);
        }
        else if (RifleLvl == 1 && buttonCount < 5)
        {
            UpdateUpgradeButton("RifleButton", Upgrade.EquipmentUpgradeType.Rifle_2, "Rifle Up", Upgrade.GetCost(Upgrade.EquipmentUpgradeType.Rifle_2), 1);
        }
        else if (RifleLvl == 2 && buttonCount < 5)
        {
            UpdateUpgradeButton("RifleButton", Upgrade.EquipmentUpgradeType.Rifle_3, "Rifle Up", Upgrade.GetCost(Upgrade.EquipmentUpgradeType.Rifle_3), 1);
        }
        else if (RifleLvl == 3 && buttonCount < 5)
        {
            UpdateUpgradeButton("RifleButton", Upgrade.EquipmentUpgradeType.Rifle_4, "Rifle Up", Upgrade.GetCost(Upgrade.EquipmentUpgradeType.Rifle_4), 1);
        }
        else if (RifleLvl > 3 && buttonCount < 5)
        {
            UpdateUpgradeButton("RifleButton", Upgrade.EquipmentUpgradeType.RifleMax, "Rifle MAX", Upgrade.GetCost(Upgrade.EquipmentUpgradeType.RifleMax), 0);
        }

        //Button for Shotgun Weapon Upgrade
        if (ShotgunLvl == 0 && buttonCount < 5)
        {
            UpdateUpgradeButton("ShotgunButton", Upgrade.EquipmentUpgradeType.Shotgun_1, "Shotgun Up", Upgrade.GetCost(Upgrade.EquipmentUpgradeType.Shotgun_1), 1);
        }
        else if (ShotgunLvl == 1 && buttonCount < 5)
        {
            UpdateUpgradeButton("ShotgunButton", Upgrade.EquipmentUpgradeType.Shotgun_2, "Shotgun Up", Upgrade.GetCost(Upgrade.EquipmentUpgradeType.Shotgun_2), 1);
        }
        else if (ShotgunLvl == 2 && buttonCount < 5)
        {
            UpdateUpgradeButton("ShotgunButton", Upgrade.EquipmentUpgradeType.Shotgun_3, "Shotgun Up", Upgrade.GetCost(Upgrade.EquipmentUpgradeType.Shotgun_3), 1);
        }
        else if (ShotgunLvl == 3 && buttonCount < 5)
        {
            UpdateUpgradeButton("ShotgunButton", Upgrade.EquipmentUpgradeType.Shotgun_4, "Shotgun Up", Upgrade.GetCost(Upgrade.EquipmentUpgradeType.Shotgun_4), 1);
        }
        else if (ShotgunLvl > 3 && buttonCount < 5)
        {
            UpdateUpgradeButton("ShotgunButton", Upgrade.EquipmentUpgradeType.ShotgunMax, "Shotgun MAX", Upgrade.GetCost(Upgrade.EquipmentUpgradeType.ShotgunMax), 0);
        }*/
    }

    private Transform CreateUpgradeButton(string buttonType, Upgrade.EquipmentExchangeType upgradeType, string itemName, int itemCost, int positionIndex)
    {

        Transform vendorItemTransform = Instantiate(vendorItemTemplate, container);
        //vendorItemTransform.gameObject.SetActive(true);
        RectTransform vendorItemRectTransform = vendorItemTransform.GetComponent<RectTransform>();

        int itemIndex = playerInventory.GetItemIndex(creepDrop);
        int itemCount = GameManager.Instance.PlayerInventory.Items[itemIndex].quantity;

        float vendorItemHeight = 90f;
        vendorItemRectTransform.anchoredPosition = new Vector2(0, -vendorItemHeight * positionIndex);
        vendorItemTransform.name = itemName;

        vendorItemTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText(itemName);
        vendorItemTransform.Find("costText").GetComponent<TextMeshProUGUI>().SetText(itemCost.ToString());

        vendorItemTransform.GetComponent<Button_UI>().ClickFunc = () =>
        {
            if (itemCount - itemCost >= 0)
            {
                upgradeItem(buttonType, upgradeType, itemCost, itemName);
            }
            else
            {
                //Insert Popup code???
                Debug.Log("Not Enough Materials");
            }

        };

        if (itemCost == 0)
        {
            vendorItemTransform.GetComponent<Button>().interactable = false;
            Debug.Log("Button Disabled");
        }

        return vendorItemTransform;
    }

    private void upgradeItem(string buttonType, Upgrade.EquipmentExchangeType upgradeType, int itemCost, string itemName)
    {
        Debug.Log("Upgraded to: " + upgradeType);

        if (buttonType == "LMelee" && LMeleeLvl < 4)
        {
            LMeleeLvl++;
            Debug.Log("Light Melee Upgraded to:" + LMeleeLvl);
            //Insert Eddie's Upgrade Calculations for Light Melee Weapons
            LMeleeButton.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText(itemName);
            LMeleeButton.Find("costText").GetComponent<TextMeshProUGUI>().SetText(itemCost.ToString());
        }
        if (buttonType == "HMelee" && HMeleeLvl < 4)
        {
            HMeleeLvl++;
            Debug.Log("Heavy Melee Upgraded to:" + HMeleeLvl);
            //Insert Eddie's Upgrade Calculations for Light Melee Weapons
            HMeleeButton.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText(itemName);
            HMeleeButton.Find("costText").GetComponent<TextMeshProUGUI>().SetText(itemCost.ToString());
        }
        if (buttonType == "Rifle" && RifleLvl < 4)
        {
            RifleLvl++;
            Debug.Log("Rifle Upgraded to:" + RifleLvl);
            //Insert Eddie's Upgrade Calculations for Light Melee Weapons
            RifleButton.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText(itemName);
            RifleButton.Find("costText").GetComponent<TextMeshProUGUI>().SetText(itemCost.ToString());
        }
        if (buttonType == "Shotgun" && ShotgunLvl < 4)
        {
            ShotgunLvl++;
            Debug.Log("Shotgun Upgraded to:" + ShotgunLvl);
            //Insert Eddie's Upgrade Calculations for Light Melee Weapons
            ShotgunButton.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText(itemName);
            ShotgunButton.Find("costText").GetComponent<TextMeshProUGUI>().SetText(itemCost.ToString());
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
        }
        if (buttonType == "Rifle" && RifleLvl >= 4)
        {
            //Try to insert popup maybe?
            Debug.Log("Weapon Already at Max Level");
        }
        if (buttonType == "Shotgun" && ShotgunLvl >= 4)
        {
            //Try to insert popup maybe?
            Debug.Log("Weapon Already at Max Level");
        }
    }
}