using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Skills
{
    [SerializeField] private Skill[] _skills;
    [HideInInspector] public PlayerSkill[] skills;
    public bool initialized = false;

    public void Initialize()
    {
        skills = new PlayerSkill[_skills.Length];

        for (int i = 0; i < skills.Length; i++)
            skills[i] = new PlayerSkill(_skills[i]);

        initialized = true;
    }

    public void Update()
    {
        foreach (var skill in skills)
            if (skill.level != 0)
                skill.skill.Update();
    }

    public void Reset()
    {
        skills = null;
        initialized = false;
    }

    public void LoadUpgrades() 
    {
        var loadSkillList = GameManager.Instance.PlayerSkills.skills;
        foreach (var s in loadSkillList)
        {
            if (s.level > 0)
            {
                s.skill.OnActivate();
                for (int i = 0; i < s.level; i++)
                {
                    s.skill.OnLevelUp(i+1);
                }
            }
        }
    }
}


[Serializable]
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
