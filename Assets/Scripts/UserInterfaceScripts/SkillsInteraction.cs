using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillsInteraction : MonoBehaviour
{
    [SerializeField] private Button background;
    [SerializeField] private RectTransform container;

    private new TMP_Text name;
    private TMP_Text description;
    private TMP_Text status;
    private Button button;

    private const int panelWidth = 400;
    private const int basePanelHeight = 150;
    private const int accessDistance = 15;

    private Transform player;
    private Vector3 baseLocation;
    private bool wasNearBase;

    private float ButtonHeight { get { return button.IsActive() ? button.GetComponent<RectTransform>().rect.height : 0; } }
    private TMP_Text ButtonText { get { return button.transform.GetChild(0).GetComponent<TMP_Text>(); } }

    void Awake()
    {
        name = container.Find("Name").GetComponent<TMP_Text>();
        description = container.Find("Description").GetComponent<TMP_Text>();
        status = container.Find("Status").GetComponent<TMP_Text>();
        button = container.Find("Button").GetComponent<Button>();
    }

    void Start()
    {
        baseLocation = GameObject.FindGameObjectWithTag("Base").transform.position;
        player = GameObject.Find("Player").transform;

        container.gameObject.SetActive(false);
        background.onClick.AddListener(() => container.gameObject.SetActive(false));
    }

    void Update()
    {
        bool nearBase = Vector3.Distance(player.position, baseLocation) < accessDistance;
        button.gameObject.SetActive(nearBase);

        if (!wasNearBase && nearBase)
            container.sizeDelta += Vector2.up * PadValue(ButtonHeight, 20);
        else if (wasNearBase && !nearBase)
            container.sizeDelta -= Vector2.up * PadValue(ButtonHeight, 20);

        wasNearBase = nearBase;
    }

    public void DisplaySkill(PlayerSkill skillRef)
    {
        container.gameObject.SetActive(true);

        name.text = skillRef.skill.name;
        description.text = skillRef.skill.Description + ItemsToString(skillRef.skill.requirements, skillRef.skill.refinedOresRequired);
        status.text = GetStatusString(skillRef.level);

        if (button.IsActive())
        {
            ButtonText.text = GetButtonString(skillRef);
            button.onClick.RemoveAllListeners();

            bool skillNotMax = skillRef.level < skillRef.skill.maxLevel;
            bool prereqUnlocked = CheckPrerequisiteUnlocked(skillRef);
            bool playerHasItems = CheckItemsOwned(skillRef.skill.requirements, skillRef.skill.refinedOresRequired);

            if (skillNotMax && prereqUnlocked && playerHasItems)
            {
                button.interactable = true;
                button.onClick.AddListener(() =>
                {
                    // TODO: Play sound effect.

                    RemoveItems(skillRef.skill.requirements, skillRef.skill.refinedOresRequired);
                    LevelUpSkill(skillRef);
                    DisplaySkill(skillRef);
                });
            }
            else
            {
                button.interactable = false;
            }
        }

        var panelHeight = basePanelHeight
            + PadValue(description.preferredHeight, 20)
            + PadValue(ButtonHeight, 20);

        container.sizeDelta = new Vector2(panelWidth, panelHeight);
    }

    private static string ItemsToString(ItemRequirement[] items, int refinedOres)
    {
        var stringBuilder = new StringBuilder();

        foreach (var itemRef in items)
        {
            stringBuilder.AppendFormat("{0} x{1}\n", itemRef.item.name, itemRef.quantity);
        }

        if (refinedOres > 0)
        {
            stringBuilder.AppendFormat("Refined ores x{0}\n", refinedOres);
        }

        var _string = stringBuilder.ToString();

        return (_string.Length > 0) ? ("\n\n" + _string) : null;
    }

    private static bool CheckItemsOwned(ItemRequirement[] items, int refinedOres)
    {
        foreach (var target in items)
        {
            var ok = false;
            if (target.item.name == "Monster Essence" && GameManager.Instance.PlayerStats.CurrentEssenceInBase > 0 )
                ok = true;
            else if  (target.item.name == "Boss Core" && GameManager.Instance.PlayerStats.CurrentBossCoresInBase > 0)
                ok = true;

            if (!ok) return false;
        }

        if (refinedOres > GameManager.Instance.PlayerStats.CurrentOresInBase)
            return false;

        return true;
    }

    private static void RemoveItems(ItemRequirement[] items, int refinedOres)
    {
        foreach (var target in items)
        {
            if (target.item.name == "Monster Essence" && GameManager.Instance.PlayerStats.CurrentEssenceInBase >= target.quantity )
            {
                GameManager.Instance.PlayerStats.DeductEssence(target.quantity);
            }
            else if (target.item.name == "Boss Core")
            {
                GameManager.Instance.PlayerInventory.RemoveItem(target.item, target.quantity);
            }
           
        }

        if (GameManager.Instance.PlayerStats.CurrentOresInBase >= refinedOres)
        {
            GameManager.Instance.PlayerStats.DeductOres(refinedOres);
        }
    }

    private void LevelUpSkill(PlayerSkill skillRef)
    {
        skillRef.level++;

        if (skillRef.level == 1)
        {
            skillRef.skill.OnActivate();
            skillRef.uiElement.color = Color.white;
        }

        skillRef.skill.OnLevelUp(skillRef.level);
    }

    private static string GetStatusString(int level)
    {
        return (level > 0) ? string.Format("Level {0}", level) : "Not Unlocked";
    }

    private static string GetButtonString(PlayerSkill skillRef)
    {
        if (skillRef.skill.prerequisite.skill != null &&  skillRef.level == 0)
            return string.Format("Req: {0}, {1}", skillRef.skill.prerequisite.skill.name, skillRef.skill.prerequisite.level);

        if (skillRef.level == 0)
            return "Unlock Skill";

        if (skillRef.level >= skillRef.skill.maxLevel)
            return "Skill MAXED!";

        return string.Format("Upgrade to Lvl.{0}", skillRef.level + 1);
    }

    private static bool CheckPrerequisiteUnlocked(PlayerSkill skillRef)
    {
        var prerequisite = skillRef.skill.prerequisite;

        if (prerequisite.skill != null)
        {
            var skills = GameManager.Instance.PlayerSkills.skills;
            var prereqRef = skills.FirstOrDefault(s => s.skill.name == prerequisite.skill.name);

            return prereqRef.level >= prerequisite.level;
        }

        return true;
    }

    private static float PadValue(float value, int padValue)
    {
        return (value > 0) ? (value + padValue) : 0;
    }
}
