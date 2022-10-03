using UnityEngine;

// Written by Nicholas Sebastian Hendrata on 13/08/2022.

// NOTE: Weapons, constructables and usables should extend GameItem.

[CreateAssetMenu(fileName = "New Game Item", menuName = "Game Item")]
[System.Serializable]
public class GameItem : ScriptableObject
{
    public new string name;
    public Sprite icon;
    [TextArea] public string description;

    public virtual void OnHoldEnter() { }
    public virtual void OnHoldStay() { }
    public virtual void OnHoldExit() { }
    public virtual void OnUse() { }
}
