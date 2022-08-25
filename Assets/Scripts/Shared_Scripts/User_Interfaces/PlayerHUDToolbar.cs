using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Written by Nicholas Sebastian Hendrata on 20/08/2022.

// NOTE: This class adds to the PlayerHUD class functionality, displaying
//       a toolbar at the bottom of the screen, reflecting the currently
//       active item in the Inventory class.

public class PlayerHUDToolbar : UIController
{
    private GameObject container;
    private RectTransform selectionSquare;
    private GameObject[] slots;

    private float sizeSpacing;
    private float doublePadding;

    void Awake()
    {
        sizeSpacing = PlayerHUD.Main.toolbarSlotSize + PlayerHUD.Main.toolbarSlotSpacing;
        doublePadding = PlayerHUD.Main.toolbarPadding * 2;
    }

    void Start()
    {
        CreateContainer();
        CreateSlots();
        CreateSelectionSquare();
        DisplayItems();
    }

    void Update()
    {
        ScrollActiveItem();
        KeyActiveItem();

        if (Input.GetMouseButtonDown(0))
            Inventory.Main.Items[Inventory.Main.activeItemIndex]?.item.OnUse();

        Inventory.Main.Items[Inventory.Main.activeItemIndex]?.item.OnHoldStay();
    }

    public void UpdateUI()
    {
        ClearItems();
        DisplayItems();
    }

    public void UpdateSelectionSquare(int newValue)
    {
        // Call the 'OnHoldExit' method of the current active item.
        Inventory.Main.Items[Inventory.Main.activeItemIndex]?.item.OnHoldExit();

        // Update the current active item and move the selection square UI.
        Inventory.Main.activeItemIndex = newValue;
        selectionSquare.position = slots[newValue].transform.position;

        // Call the 'OnHoldEnter' method of the new active item.
        Inventory.Main.Items[newValue]?.item.OnHoldEnter();
    }

    private void ScrollActiveItem()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            if (Inventory.Main.activeItemIndex > 0)
                UpdateSelectionSquare(Inventory.Main.activeItemIndex - 1);
            else
                UpdateSelectionSquare(slots.Length - 1);
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            if (Inventory.Main.activeItemIndex < slots.Length - 1)
                UpdateSelectionSquare(Inventory.Main.activeItemIndex + 1);
            else
                UpdateSelectionSquare(0);
        }
    }

    private void KeyActiveItem()
    {
        for (int i = 0; i < Mathf.Min(PlayerHUD.Main.toolbarSlots, 9); i++)
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                UpdateSelectionSquare(i);
            }
    }

    private void CreateContainer()
    {
        var panelWidth = (PlayerHUD.Main.toolbarSlots * sizeSpacing) + doublePadding;
        var panelHeight = sizeSpacing + doublePadding;
        var panelSize = new Vector2(panelWidth, panelHeight);

        // Create the toolbar panel.
        container = CreatePanel("Toolbar", panelSize,
            PlayerHUD.Main.toolbarColour, PlayerHUD.Main.toolbarBackground);

        // Anchor and position the toolbar panel at the bottom of the screen.
        var body = container.GetComponent<RectTransform>();
        body.anchorMin = new Vector2(0.5f, 0);
        body.anchorMax = new Vector2(0.5f, 0);
        body.pivot = new Vector2(0.5f, 0);
        body.anchoredPosition = PlayerHUD.Main.toolbarOffset;

        // Set the sizing, spacing and alignment for its content.
        var grid = container.AddComponent<GridLayoutGroup>();
        grid.cellSize = Vector2.one * PlayerHUD.Main.toolbarSlotSize;
        grid.spacing = Vector2.one * PlayerHUD.Main.toolbarSlotSpacing;
        grid.childAlignment = TextAnchor.MiddleCenter;
    }

    private void CreateSelectionSquare()
    {
        var squareSize = Vector2.one * (sizeSpacing + doublePadding);
        var selectionSquareObject = CreatePanel("Selection Square", squareSize,
            PlayerHUD.Main.toolbarSelectorColour, PlayerHUD.Main.toolbarSelectorSquare);

        selectionSquareObject.SetActive(false);
        selectionSquare = selectionSquareObject.GetComponent<RectTransform>();

        StartCoroutine(SetSelectionSquarePosition());
    }

    private IEnumerator SetSelectionSquarePosition()
    {
        yield return new WaitForSeconds(0.1f);
        UpdateSelectionSquare(0);
        selectionSquare.gameObject.SetActive(true);
    }

    private void CreateSlots()
    {
        slots = new GameObject[PlayerHUD.Main.toolbarSlots];

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = CreatePanel(i.ToString(), PlayerHUD.Main.toolbarSlotColour,
                PlayerHUD.Main.toolbarSlotBackground, container.transform);
        }
    }

    private void DisplayItems()
    {
        // Display the icon and quantity for the first few items in the inventory.
        for (int i = 0; i < slots.Length; i++)
        {
            var item = Inventory.Main.Items[i];

            if (item is not null)
            {
                var slot = slots[i].transform;
                var icon = InventoryUI.DisplayItemIcon(item.item, slot);
                InventoryUI.DisplayItemQuantity(item.quantity, icon.transform);
            }
        }
    }

    private void ClearItems()
    {
        foreach (var slot in slots)
            if (slot.transform.childCount > 0)
                DestroyImmediate(slot.transform.GetChild(0).gameObject);
    }
}
