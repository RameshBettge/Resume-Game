using UnityEngine;

public class HardSkill : Skill
{
    public float passionPercentage;


    private void Awake()
    {
        hard = true;
        expPercentage = CheckPercentage(expPercentage);
        passionPercentage = CheckPercentage(passionPercentage);
    }
}
