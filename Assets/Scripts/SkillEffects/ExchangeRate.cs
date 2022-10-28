using UnityEngine;

[CreateAssetMenu(fileName = "Exchange Rate", menuName = "Skills/Exchange Rate")]
public class ExchangeRate : Skill
{
    public override string Description => "";

    public override void OnActivate()
    {
        Debug.Log("Skill Activated");
    }

    public override void OnLevelUp(int level)
    {
        Debug.Log("Skill Leveled Up to Level " + level);
    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Skill Effect");
        }
    }
}
