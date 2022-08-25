using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

// Written by Nicholas Sebastian Hendrata on 16/08/2022.

// NOTE: This script houses common functionality for UI controller scripts.

public class UIController : MonoBehaviour
{
    /// <summary>
    /// Creates a panel with the given name.
    /// </summary>
    protected GameObject CreatePanel(string name)
    {
        return CreatePanel(name, transform);
    }

    /// <summary>
    /// Creates a panel with the given name as a child of the given parent.
    /// </summary>
    protected static GameObject CreatePanel(string name, Transform parent)
    {
        var panel = CreateUIElement(name, parent);
        panel.AddComponent<CanvasRenderer>();
        panel.AddComponent<Image>().raycastTarget = false;

        return panel;
    }

    /// <summary>
    /// Creates a panel with the given name and sprite.
    /// </summary>
    protected GameObject CreatePanel(string name, Sprite sprite)
    {
        return CreatePanel(name, sprite, transform);
    }

    /// <summary>
    /// Creates a panel with the given name and sprite as a child of the given parent.
    /// </summary>
    protected static GameObject CreatePanel(string name, Sprite sprite, Transform parent)
    {
        var panel = CreatePanel(name, parent);
        var style = panel.GetComponent<Image>();
        style.sprite = sprite;
        style.type = Image.Type.Sliced;

        return panel;
    }

    /// <summary>
    /// Creates a panel with the given name, colour and sprite.
    /// </summary>
    protected GameObject CreatePanel(string name, Color colour, Sprite sprite)
    {
        return CreatePanel(name, colour, sprite, transform);
    }

    /// <summary>
    /// Creates a panel with the given name, colour and sprite as a child of the given parent.
    /// </summary>
    protected static GameObject CreatePanel(string name, Color colour, Sprite sprite, Transform parent)
    {
        var panel = CreatePanel(name, parent);
        var style = panel.GetComponent<Image>();

        style.color = colour;
        if (sprite is not null)
        {
            style.sprite = sprite;
            style.type = Image.Type.Sliced;
        }

        return panel;
    }

    /// <summary>
    /// Creates a panel with the given name, size, colour and sprite.
    /// </summary>
    protected GameObject CreatePanel(string name, Vector2 size, Color colour, Sprite sprite)
    {
        var panel = CreatePanel(name, colour, sprite);
        var body = panel.GetComponent<RectTransform>();
        body.sizeDelta = size;

        return panel;
    }

    /// <summary>
    /// Creates a text with the given name, content and font size.
    /// </summary>
    protected GameObject CreateText(string name, string content, float fontSize)
    {
        return CreateText(name, content, fontSize, transform);
    }

    /// <summary>
    /// Creates a text with the given name, content, and font size as a child of the given parent.
    /// </summary>
    protected static GameObject CreateText(string name, string content, float fontSize, Transform parent)
    {
        var textObject = CreateUIElement(name, parent);
        var text = textObject.AddComponent<TextMeshProUGUI>();
        text.text = content;
        text.fontSize = fontSize;
        text.raycastTarget = false;

        return textObject;
    }

    /// <summary>
    /// Creates a text with the given name, content, font size, alignment, and styling as a child of the given parent.
    /// </summary>
    protected static GameObject CreateText(string name, string content, float fontSize, TextAlignmentOptions alignment, FontStyles style, Transform parent)
    {
        var textObject = CreateText(name, content, fontSize, parent);
        var text = textObject.GetComponent<TextMeshProUGUI>();
        text.alignment = alignment;
        text.fontStyle = style;

        return textObject;
    }

    /// <summary>
    /// Creates a button with the given name, position, sprite and callback.
    /// </summary>
    protected GameObject CreateButton(string name, Vector2 position, Vector2 size, Sprite sprite, Color colour, float fontSize, Color textColour, Action Callback)
    {
        return CreateButton(name, position, size, sprite, colour, fontSize, textColour, Callback, transform);
    }

    /// <summary>
    /// Creates a button with the given name, position, sprite and callback as a child of the given parent.
    /// </summary>
    protected static GameObject CreateButton(string name, Vector2 position, Vector2 size, Sprite sprite, Color colour, float fontSize, Color textColour, Action Callback, Transform parent)
    {
        var objectName = name + " Button";
        var button = CreatePanel(objectName, colour, sprite, parent);
        button.GetComponent<Image>().raycastTarget = true;

        var buttonTransform = button.GetComponent<RectTransform>();
        buttonTransform.localPosition = position;
        buttonTransform.sizeDelta = size;

        var buttonTextObject = CreateText(objectName, name, fontSize, button.transform);
        buttonTextObject.GetComponent<RectTransform>().sizeDelta = size;

        var buttonText = buttonTextObject.GetComponent<TextMeshProUGUI>();
        buttonText.color = textColour;
        buttonText.fontStyle = FontStyles.Bold;
        buttonText.alignment = TextAlignmentOptions.Center;

        AddClickListener(button, _ => Callback(), Selectable.Transition.ColorTint);

        return button;
    }

    /// <summary>
    /// Creates an empty UI element with the given name as a child of the given parent.
    /// </summary>
    protected static GameObject CreateUIElement(string name, Transform parent)
    {
        var element = new GameObject(name);
        element.AddComponent<RectTransform>();
        element.transform.SetParent(parent, false);

        return element;
    }

    /// <summary>
    /// Attaches a click listener onto the given UI element to trigger the given callback.
    /// </summary>
    protected static void AddClickListener(GameObject uiElement, Action<string> Callback, Selectable.Transition transition = Selectable.Transition.None)
    {
        var button = uiElement.AddComponent<Button>();
        button.transition = transition;
        button.onClick.AddListener(() =>
        {
            var selected = EventSystem.current.currentSelectedGameObject;
            Callback(selected.name);
        });
    }
}
