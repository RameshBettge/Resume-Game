using UnityEngine;

public class HardSkill : Skill
{
    public float passionPercentage;

    private void Awake()
    {
        expPercentage = CheckPercentage(expPercentage);
        passionPercentage = CheckPercentage(passionPercentage);
    }

    public override bool IsHard()
    {
        return true;
    }
}
