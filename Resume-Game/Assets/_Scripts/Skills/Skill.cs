using UnityEngine;

public class Skill : MonoBehaviour
{
    public string title;
    public float expPercentage;
    public float passionPercentage;


    private void Awake()
    {
        expPercentage = CheckPercentage(expPercentage);
        passionPercentage = CheckPercentage(passionPercentage);
    }

    float CheckPercentage(float p)
    {
        if (expPercentage > 1f || expPercentage < 0f)
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
