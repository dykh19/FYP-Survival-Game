using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Written by Nicholas Sebastian Hendrata on 19/08/2022.

// NOTE: This class adds to the InventoryUI class functionality, allowing the user
//       to move and swap items around the inventory slots.

public class InventoryUISwapper : UIController
{
    [HideInInspector] public int? currentlySelected = null;
    private RectTransform pointerIcon;
    private Image pointerIconImage;
    private TextMeshProUGUI pointerIconText;

    void Start()
    {
        InitializePointerIcon();
    }

    void LateUpdate()
    {
        pointerIcon.position = Input.mousePosition;
    }

    private void InitializePointerIcon()
    {
        // Create the thing that will follow the pointer around to house the selected item.
        pointerIcon = CreatePanel("Pointer Icon").GetComponent<RectTransform>();
        pointerIcon.gameObject.SetActive(false);
        pointerIcon.sizeDelta = Vector2.one * InventoryUI.itemIconSize;
        pointerIconImage = pointerIcon.GetComponent<Image>();

        // Create the thing that will display the selected item quantity.
        pointerIconText = InventoryUI
            .DisplayItemQuantity(0, pointerIcon)
            .GetComponent<TextMeshProUGUI>();
    }

    public void SwapItem(int slotIndex)
    {
        if (currentlySelected is not null)
        {
            int oldIndex = (int)currentlySelected;
            int activeIndex = Inventory.Main.activeItemIndex;
            var activeItemSwapped = (oldIndex == activeIndex) || (slotIndex == activeIndex);

            // Call the 'OnHoldExit' method if the swapped item is the active item.
            if (activeItemSwapped)
                Inventory.Main.Items[Inventory.Main.activeItemIndex]?.item.OnHoldExit();

            // Swap the currently selected item with the item just clicked.
            var temp = Inventory.Main.Items[slotIndex];
            Inventory.Main.Items[slotIndex] = Inventory.Main.Items[oldIndex];
            Inventory.Main.Items[oldIndex] = temp;

            // Deselect the selected item and update the UI.
            DeselectItem();
            InventoryUI.Main.UpdateUI();

            // Call the 'OnHoldEnter' method if the swapped item is the active item.
            if (activeItemSwapped)
                Inventory.Main.Items[Inventory.Main.activeItemIndex]?.item.OnHoldEnter();
        }
        else
        {
            // If the slot isn't empty, select the item.
            if (Inventory.Main.Items[slotIndex] is not null)
                SelectItem(slotIndex);
        }
    }

    private void SelectItem(int slotIndex)
    {
        // Set the currentlySelected index.
        currentlySelected = slotIndex;

        var selectedItemSlot = InventoryUI.Main.slots[(int)currentlySelected];
        var selectedItem = Inventory.Main.Items[(int)currentlySelected];

        // Hide the item and set the icon to be following the mouse.
        selectedItemSlot.transform.GetChild(0).gameObject.SetActive(false);
        pointerIcon.gameObject.SetActive(true);
        pointerIconImage.sprite = selectedItem.item.icon;
        pointerIconText.text = selectedItem.quantity.ToString();
    }

    private void DeselectItem()
    {
        var selectedItemSlot = InventoryUI.Main.slots[(int)currentlySelected];

        // Unhide the selected item and remove the icon from following the mouse.
        selectedItemSlot.transform.GetChild(0).gameObject.SetActive(true);
        pointerIcon.gameObject.SetActive(false);
        pointerIconImage.sprite = null;

        // Deselect the selected item.
        currentlySelected = null;
    }
}
