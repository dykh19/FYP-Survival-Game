using System.Collections.Generic;
using UnityEngine;

// Written by Nicholas Sebastian Hendrata on 16/08/2022.

// NOTE: This is to be attached to the HUD canvas.

// NOTE: This class contains only all the base functionality:
//       to simply reflect the state of the PlayerStatus class.

public class PlayerHUD : UIController
{
    public static PlayerHUD Main { get; private set; }

    [Header("Status Settings")]
    public Sprite barSprite;
    public Vector2 barSize;
    public Vector2 barOffset;
    public Sprite barBackgroundSprite;
    public Color barBackgroundColour;
    public Color healthBarColour;
    public Vector2 healthBarPosition;
    public Color energyBarColour;
    public Vector2 energyBarPosition;
    public float subBarVertialOffset = 50;

    [Header("Toolbar Settings")]
    public Sprite toolbarSelectorSquare;
    public Color toolbarSelectorColour;
    public Sprite toolbarBackground;
    public Color toolbarColour;
    public Sprite toolbarSlotBackground;
    public Color toolbarSlotColour;
    public Vector2 toolbarOffset = new (0, 20);
    public float toolbarPadding = 10;
    public float toolbarSlotSize = 90;
    public float toolbarSlotSpacing = 7.5f;
    public int toolbarSlots = 5;

    [HideInInspector] public PlayerHUDToolbar toolbar;

    private RectTransform healthBar;
    private RectTransform energyBar;
    private GameObject crosshair;

    void Awake()
    {
        Main = this;
        toolbar = gameObject.AddComponent<PlayerHUDToolbar>();
    }

    void Start()
    {
        healthBar = CreateMainBar("Health", healthBarColour, healthBarPosition);
        //energyBar = CreateMainBar("Energy", energyBarColour, energyBarPosition);
    }

    void Update()
    {
        UpdateUI();

        // TODO: Display prompt if the player can interact with something.
    }

    public void SetCrosshair(Sprite crosshair, Vector2 size, Color? colour)
    {
        this.crosshair = CreatePanel("Crosshair", colour ?? Color.white, crosshair);
        var crosshairTransform = this.crosshair.GetComponent<RectTransform>();
        crosshairTransform.localPosition = Vector3.zero;
        crosshairTransform.sizeDelta = size;
    }

    public void RemoveCrosshair()
    {
        Destroy(crosshair);
    }

    private RectTransform CreateMainBar(string name, Color barColour, Vector2 position)
    {
        var barBackground = CreatePanel(name + " Bar Background", barBackgroundColour, barBackgroundSprite);
        var barBackgroundTransform = barBackground.GetComponent<RectTransform>();
        barBackgroundTransform.anchorMin = Vector2.zero;
        barBackgroundTransform.anchorMax = Vector2.zero;
        //barBackgroundTransform.anchoredPosition = position;
        barBackgroundTransform.anchoredPosition = new Vector2(120.0f, 50.0f);
        barBackgroundTransform.sizeDelta = barSize;

        var bar = CreatePanel(name + "Bar", barColour, barSprite, barBackground.transform);
        var barTransform = bar.GetComponent<RectTransform>();
        barTransform.anchorMin = new Vector2(0, 0.5f);
        barTransform.anchorMax = new Vector2(0, 0.5f);
        barTransform.pivot = new Vector2(0, 0.5f);
        barTransform.anchoredPosition = Vector3.right * barOffset.x;

        return barTransform;
    }

    private void UpdateUI()
    {
        var barSizeFullX = barSize.x - (barOffset.x * 2);
        var barDoubleOffsetY = barOffset.y * 2;

        //var healthRatio = GameManagerJoseph.Main.playerStatus.playerHealth / GameManagerJoseph.Main.playerStatus.maxPlayerHealth;
        //var energyRatio = GameManagerJoseph.Main.playerStatus.playerEnergy / GameManagerJoseph.Main.playerStatus.maxPlayerEnergy;

        var healthRatio = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>().CurrentHealth / GameObject.FindGameObjectWithTag("Player").GetComponent<Health>().MaxHealth;

        healthBar.sizeDelta = new Vector2(barSizeFullX * healthRatio, barSize.y - barDoubleOffsetY);
        //energyBar.sizeDelta = new Vector2(barSizeFullX * energyRatio, barSize.y - barDoubleOffsetY);
    }
}
