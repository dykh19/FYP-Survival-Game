using UnityEngine;

public abstract class Skill : ScriptableObject
{
    public new string name;
    public Sprite icon;
    public int maxLevel;
    public Skill prerequisite = null;
    public int refinedOresRequired;
    public ItemRequirement[] requirements;
    public abstract string Description { get; }

    public abstract void OnActivate();
    public abstract void OnLevelUp(int level);
    public abstract void Update();
}

[System.Serializable]
public class ItemRequirement
{
    public GameItem item;
    public int quantity;
}
