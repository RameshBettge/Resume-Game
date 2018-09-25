using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public string title;
    public float expPercentage;

    [HideInInspector]
    public bool hard;


    protected float CheckPercentage(float p)
    {
        if (p > 1f || p < 0f)
        {
            Debug.LogWarning("Skill percentage was set to " + p + ". It has been clamped.");
            return Mathf.Clamp01(p);
        }
        else
        {
            return p;
        }
    }
}
