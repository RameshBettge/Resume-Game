using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoftSkill : Skill
{

    private void Awake()
    {
        expPercentage = CheckPercentage(expPercentage);
    }

    public override bool IsHard()
    {
        return false;
    }
}
