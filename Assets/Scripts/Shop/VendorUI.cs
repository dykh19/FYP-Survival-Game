using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;
using TMPro;

/*Script First Created by Sean Ong on 26/9/22 12:10pm*/

public class VendorUI : MonoBehaviour
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

    private void Awake()
    {
    }

    private void Start()
    {
        container = transform.Find("Container");
        vendorItemTemplate = container.Find("VendorItemTemplate");
        //vendorItemTemplate.gameObject.SetActive(false);

        LMeleeButton = CreateUpgradeButton(Upgrade.EquipmentUpgradeType.LMelee_1, "Light Melee Up", Upgrade.GetCost(Upgrade.EquipmentUpgradeType.LMelee_1), 0);
        HMeleeButton = CreateUpgradeButton(Upgrade.EquipmentUpgradeType.HMelee_1, "Heavy Melee Up", Upgrade.GetCost(Upgrade.EquipmentUpgradeType.HMelee_1), 1);
        RifleButton = CreateUpgradeButton(Upgrade.EquipmentUpgradeType.Rifle_1, "Rifle Up", Upgrade.GetCost(Upgrade.EquipmentUpgradeType.Rifle_1), 2);
        ShotgunButton = CreateUpgradeButton(Upgrade.EquipmentUpgradeType.Shotgun_1, "Shotgun Up", Upgrade.GetCost(Upgrade.EquipmentUpgradeType.Shotgun_1), 3);
        buttonCount = 4;
    }

    private void Update()
    {
        /*Button for Light Melee Weapon Upgrade*/
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

        /*Button for Heavy Melee Weapon Upgrade*/
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

        /*Button for Rifle Weapon Upgrade*/
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

        /*Button for Shotgun Weapon Upgrade*/
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
        }
    }

    private Transform CreateUpgradeButton(Upgrade.EquipmentUpgradeType upgradeType, string itemName, int itemCost, int positionIndex)
    {

        Transform vendorItemTransform = Instantiate(vendorItemTemplate, container);
        //vendorItemTransform.gameObject.SetActive(true);
        RectTransform vendorItemRectTransform = vendorItemTransform.GetComponent<RectTransform>();

        float vendorItemHeight = 90f;
        vendorItemRectTransform.anchoredPosition = new Vector2(0, -vendorItemHeight * positionIndex);
        vendorItemTransform.name = itemName;

        vendorItemTransform.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText(itemName);
        vendorItemTransform.Find("costText").GetComponent<TextMeshProUGUI>().SetText(itemCost.ToString());

        vendorItemTransform.GetComponent<Button_UI>().ClickFunc = () =>
        {
            upgradeItem(upgradeType);
        };

        if (itemCost == 0)
        {
            vendorItemTransform.GetComponent<Button>().interactable = false;
            Debug.Log("Button Disabled");
        }

        return vendorItemTransform;
    }

    private void UpdateUpgradeButton(string buttonType, Upgrade.EquipmentUpgradeType upgradeType, string itemName, int itemCost, int positionIndex)
    {
        //chosenButton = GameObject.Find(itemName).GetComponent<Button>();
        //chosenButton.GetComponent<Text>().GetComponent("upgradeText").GetComponent<TextMeshProUGUI>().SetText(itemName);
        //chosenButton.GetComponent<Text>().GetComponent("costText").GetComponent<TextMeshProUGUI>().SetText(itemCost.ToString());
        //chosenButton.GetComponent("costText").GetComponent<TextMeshProUGUI>().SetText(itemCost.ToString());
        if(buttonType == "LMeleeButton")
        {
            LMeleeButton.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText(itemName);
            LMeleeButton.Find("costText").GetComponent<TextMeshProUGUI>().SetText(itemCost.ToString());
        }
        if(buttonType == "HMeleeButton")
        {
            HMeleeButton.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText(itemName);
            HMeleeButton.Find("costText").GetComponent<TextMeshProUGUI>().SetText(itemCost.ToString());
        }
        if(buttonType == "RifleButton")
        {
            RifleButton.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText(itemName);
            RifleButton.Find("costText").GetComponent<TextMeshProUGUI>().SetText(itemCost.ToString());
        }
        if(buttonType == "ShotgunButton")
        {
            ShotgunButton.Find("upgradeText").GetComponent<TextMeshProUGUI>().SetText(itemName);
            ShotgunButton.Find("costText").GetComponent<TextMeshProUGUI>().SetText(itemCost.ToString());
        }

    }

    private void upgradeItem(Upgrade.EquipmentUpgradeType upgradeType)
    {
        //Change weapon Values Here based on upgrade type
        Debug.Log("Upgraded to: " + upgradeType);

        if(upgradeType == Upgrade.EquipmentUpgradeType.LMelee_1 || upgradeType == Upgrade.EquipmentUpgradeType.LMelee_2 ||
            upgradeType == Upgrade.EquipmentUpgradeType.LMelee_3 || upgradeType == Upgrade.EquipmentUpgradeType.LMelee_4)
        {
            LMeleeLvl++;
            Debug.Log("Light Melee Upgraded to:" + LMeleeLvl);
            //Insert Eddie's Upgrade Calculations for Light Melee Weapons
        }
        else if (upgradeType == Upgrade.EquipmentUpgradeType.HMelee_1 || upgradeType == Upgrade.EquipmentUpgradeType.HMelee_2 ||
            upgradeType == Upgrade.EquipmentUpgradeType.HMelee_3 || upgradeType == Upgrade.EquipmentUpgradeType.HMelee_4)
        {
            HMeleeLvl++;
            Debug.Log("Heavy Melee Upgraded to:" + HMeleeLvl);
            //Insert Eddie's Upgrade Calculations for Light Melee Weapons
        }
        else if (upgradeType == Upgrade.EquipmentUpgradeType.Rifle_1 || upgradeType == Upgrade.EquipmentUpgradeType.Rifle_2 ||
    upgradeType == Upgrade.EquipmentUpgradeType.Rifle_3 || upgradeType == Upgrade.EquipmentUpgradeType.Rifle_4)
        {
            RifleLvl++;
            Debug.Log("Rifle Upgraded to:" + RifleLvl);
            //Insert Eddie's Upgrade Calculations for Light Melee Weapons
        }
        else if (upgradeType == Upgrade.EquipmentUpgradeType.Shotgun_1 || upgradeType == Upgrade.EquipmentUpgradeType.Shotgun_2 ||
    upgradeType == Upgrade.EquipmentUpgradeType.Shotgun_3 || upgradeType == Upgrade.EquipmentUpgradeType.Shotgun_4)
        {
            ShotgunLvl++;
            Debug.Log("Shotgun Upgraded to:" + ShotgunLvl);
            //Insert Eddie's Upgrade Calculations for Light Melee Weapons
        }
        else
        {
            //Try to insert popup maybe?
            Debug.Log("Weapon Already at Max Level");
        }
    }
}
