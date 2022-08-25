using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

// Written by Nicholas Sebastian Hendrata on 19/08/2022.

// NOTE: This class adds to the InventoryUI class functionality, displaying
//       a tooltip when the player hovers over an item in an inventory slot.

public class InventoryUITooltip : UIController
{
    private List<RaycastResult> raycastResults;
    private RectTransform tooltipPanel;
    private TextMeshProUGUI tooltipText;

    void Start()
    {
        raycastResults = new List<RaycastResult>();
        InitializeTooltip();
    }

    void Update()
    {
        var slotIndex = PointerOverSlot();
        var selectedItem = InventoryUI.Main.swapper.currentlySelected;

        if ((slotIndex is not null) && (selectedItem is null))
            HandleSlotHover(slotIndex);
        else
            TryHideTooltip();
    }

    void LateUpdate()
    {
        TooltipFollow();
    }

    private void InitializeTooltip()
    {
        var fontSize = InventoryUI.Main.tooltipFontSize;
        var padding = InventoryUI.Main.tooltipPadding;
        var paddingX = Mathf.RoundToInt(padding.x);
        var paddingY = Mathf.RoundToInt(padding.y);

        // Create the tooltip panel that will follow the pointer around.
        var tooltipPanelObject = CreatePanel("Tooltip",
            InventoryUI.Main.tooltipColour, InventoryUI.Main.tooltipBackground);
        tooltipPanelObject.SetActive(false);
        tooltipPanel = tooltipPanelObject.GetComponent<RectTransform>();

        // Set the tooltip text.
        var tooltipTextObject = CreateText("Tooltip Text", "", fontSize, tooltipPanel);
        tooltipText = tooltipTextObject.GetComponent<TextMeshProUGUI>();

        // Set the tooltip panel to have its size adjust to fit its content.
        var fitter = tooltipPanel.gameObject.AddComponent<ContentSizeFitter>();
        fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        // Set the tooltip panel's padding.
        var layout = tooltipPanel.gameObject.AddComponent<HorizontalLayoutGroup>();
        layout.padding = new RectOffset(paddingX, paddingX, paddingY, paddingY);
    }

    private int? PointerOverSlot()
    {
        // Get what the pointer is hovering on.
        var eventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        EventSystem.current.RaycastAll(eventData, raycastResults);

        foreach (var raycastResult in raycastResults)
        {
            var slotName = raycastResult.gameObject.name;

            // If the UI element being hovered on can be parsed as an int, then return it.
            // This suggests that it is a slot and the int is its index.

            if (int.TryParse(slotName, out int slotIndex))
                return slotIndex;
        }

        return null;
    }

    private void HandleSlotHover(int? slotIndex)
    {
        var slot = InventoryUI.Main.slots[(int)slotIndex];
        var slotHasItem = slot.transform.childCount > 0;

        if (slotHasItem)
        {
            TryShowTooltip();
            DisplayItem(Inventory.Main.Items[(int)slotIndex].item);
        }
        else
        {
            TryHideTooltip();
        }
    }

    private void TooltipFollow()
    {
        var paddingOffset = InventoryUI.Main.tooltipPadding;
        var offsetX = paddingOffset.x;
        var offsetY = paddingOffset.y;
        try
        {
            offsetX += tooltipText.preferredWidth / 2;
            offsetY += tooltipText.preferredHeight / 2;
        }
        catch (System.NullReferenceException)
        {
            // This is to surpress the unknown NullReferenceException that TextMeshPro
            // throws when trying to access the preferred width and height at the
            // very beginning when the inventory is opened for the first time.
        }
        finally
        {
            var offset = new Vector3(offsetX, -offsetY, 0);
            tooltipPanel.position = Input.mousePosition + offset;
        }
    }

    private void DisplayItem(GameItem item)
    {
        tooltipText.text = string.Format("<b>{0}</b>", item.name);
        if (item.description.Length > 0)
        {
            var description = FormatItemDescription(item.description);
            tooltipText.text += string.Format("\n<size=0.8em>{0}</size>", description);
        }
    }

    private void TryShowTooltip()
    {
        if (tooltipPanel.gameObject.activeSelf == false)
            tooltipPanel.gameObject.SetActive(true);
    }

    private void TryHideTooltip()
    {
        if (tooltipPanel.gameObject.activeSelf)
        {
            tooltipPanel.gameObject.SetActive(false);
            tooltipText.text = "";
        }
    }

    private string FormatItemDescription(string description, int wordPerLine = 5)
    {
        int spaceCounter = 0;
        var sb = new StringBuilder(description);

        for (int i = 0, j = 0; i < description.Length; i++)
            if (description[i] == ' ')
            {
                if (spaceCounter <= wordPerLine)
                    spaceCounter++;
                else
                {
                    sb.Insert(i + j + 1, '\n');
                    spaceCounter = 0;
                    j++;
                }
            }

        return sb.ToString();
    }
}
