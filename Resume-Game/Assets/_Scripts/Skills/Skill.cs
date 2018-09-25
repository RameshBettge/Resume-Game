using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    public string title;
    public float expPercentage;

    protected float CheckPercentage(float p)
    {
        if (p > 1f || p < 0f)
        {
            Debug.LogWarning("A percentage of '" +title + "' was set to " + p + ". It has been clamped.");
            return Mathf.Clamp01(p);
        }
        else
        {
            return p;
        }
    }

    public abstract bool IsHard();
}
