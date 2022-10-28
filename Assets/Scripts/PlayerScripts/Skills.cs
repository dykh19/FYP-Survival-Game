using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Skills
{
    [SerializeField] private Skill[] _skills;
    [HideInInspector] public PlayerSkill[] skills;

    public void Initialize()
    {
        skills = new PlayerSkill[_skills.Length];

        for (int i = 0; i < skills.Length; i++)
            skills[i] = new PlayerSkill(_skills[i]);
    }

    public void Update()
    {
        foreach (var skill in skills)
            if (skill.level != 0)
                skill.skill.Update();
    }
}

public class PlayerSkill
{
    public Skill skill;
    public int level;
    public Image uiElement;

    public PlayerSkill(Skill skill)
    {
        this.skill = skill;
        level = 0;
        uiElement = null;
    }
}
