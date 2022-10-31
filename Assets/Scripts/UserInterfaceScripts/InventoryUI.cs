using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Written by Nicholas Sebastian Hendrata on 16/08/2022.

// NOTE: This is to be attached to the Inventory UI canvas.

// NOTE: This class contains only all the base functionality:
//       to simply reflect the state and contents of the Inventory class.

public class InventoryUI : UIController
{
    public static InventoryUI Main { get; private set; }

    [Header("Style Settings")]
    public Sprite panelBackground;
    public Color panelColour;
    public Sprite slotBackground;
    public Color slotColour;
    public Sprite tooltipBackground;
    public Color tooltipColour;

    [Header("Size Settings")]
    public float containerPadding = 10;
    public float slotSize = 90;
    public float slotSpacing = 7.5f;
    public int slotsPerRow = 5;
    public Vector2 tooltipPadding = new (20, 10);
    public float tooltipFontSize = 30;

    public const float itemIconSize = 80;
    public const float itemQuantityFontSize = 30;
    public static readonly Vector2 itemQuantityOffset = new (28, -18);

    private GameObject container;
    [HideInInspector] public GameObject[] slots;

    [HideInInspector] public InventoryUISwapper swapper;
    [HideInInspector] public InventoryUITooltip tooltip;

    void Awake()
    {
        Main = this;
        swapper = gameObject.AddComponent<InventoryUISwapper>();
        tooltip = gameObject.AddComponent<InventoryUITooltip>();
    }

    void Start()
    {
        CreateContainer();
        CreateSlots();
        DisplayItems();
    }

    public void UpdateUI()
    {
        ClearSlots();
        DisplayItems();
        PlayerHUD.Main.toolbar.UpdateUI();
    }

    public void ResizeUI()
    {
        DestroyImmediate(container);
        CreateContainer();
        CreateSlots();
        DisplayItems();
    }

    private void CreateContainer()
    {
        var numberOfColumns = MinimumMultiple(GameManager.Instance.PlayerInventory.Items.Length, slotsPerRow);
        var sizeSpacing = slotSize + slotSpacing;
        var doublePadding = containerPadding * 2;

        var panelWidth = (slotsPerRow * sizeSpacing) + doublePadding;
        var panelHeight = (numberOfColumns * sizeSpacing) + doublePadding;
        var panelSize = new Vector2(panelWidth, panelHeight);

        // Create the inventory panel.
        container = CreatePanel("Panel", panelSize, panelColour, panelBackground);

        // Set the sizing, spacing and alignment for its content.
        var grid = container.AddComponent<GridLayoutGroup>();
        grid.cellSize = Vector2.one * slotSize;
        grid.spacing = Vector2.one * slotSpacing;
        grid.childAlignment = TextAnchor.MiddleCenter;
    }

    private void CreateSlots()
    {
        // Create a slot for every inventory item.
        slots = new GameObject[GameManager.Instance.PlayerInventory.Items.Length];

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = CreatePanel(i.ToString(), slotColour, slotBackground, container.transform);
            slots[i].GetComponent<Image>().raycastTarget = true;

            // We used the name to store the index of each slot LOL.
            AddClickListener(slots[i], slotName => swapper.SwapItem(int.Parse(slotName)));
        }
    }

    private void DisplayItems()
    {
        // Display the icon and quantity for every item in the inventory.
        for (int i = 0; i < slots.Length; i++)
        {
            var item = GameManager.Instance.PlayerInventory.Items[i];

            if (item is not null)
            {
                var slot = slots[i].transform;
                var icon = DisplayItemIcon(item.item, slot);
                DisplayItemQuantity(item.quantity, icon.transform);
            }
        }
    }

    private void ClearSlots()
    {
        foreach (var slot in slots)
            if (slot.transform.childCount > 0)
                DestroyImmediate(slot.transform.GetChild(0).gameObject);
    }

    public static GameObject DisplayItemIcon(GameItem item, Transform parent)
    {
        var itemIcon = CreatePanel(item.name + " Icon", item.icon, parent);

        var itemTransform = itemIcon.GetComponent<RectTransform>();
        itemTransform.localPosition = Vector3.zero;
        itemTransform.sizeDelta = Vector2.one * itemIconSize;

        return itemIcon;
    }

    public static GameObject DisplayItemQuantity(int quantity, Transform parent)
    {
        var itemQuantity = CreateText("Item Quantity", quantity.ToString(),
            itemQuantityFontSize, TextAlignmentOptions.Right, FontStyles.Bold, parent);


        var textTransform = itemQuantity.GetComponent<RectTransform>();
        textTransform.localPosition = itemQuantityOffset;
        textTransform.sizeDelta = new Vector2(60, 20);
        textTransform.pivot = new Vector2(1, 0);
        textTransform.anchorMin = new Vector2(1, 0);
        textTransform.anchorMax = new Vector2(1, 0);
        textTransform.anchoredPosition = new Vector2(0, 0);
        return itemQuantity;
    }

    private static int MinimumMultiple(int value, int multiple)
    {
        int i = multiple;
        while (i < value) i += multiple;
        return i / multiple;
    }
}
