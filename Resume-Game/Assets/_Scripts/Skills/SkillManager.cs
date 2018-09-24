﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    [SerializeField]
    GameObject tutorial;

    [SerializeField]
    GameObject skillTemplate;

    [SerializeField]
    Transform hardSkillPanel;
    [SerializeField]
    Transform softSkillPanel;

    //Transform skillPanelParent;

    Skill[] hardSkills;
    Skill[] softSkills;

    Transform hardParent;
    Transform softParent;

    bool softActive = false;
    GameObject currentSkillDisplay;

    List<Button> hardButtons;
    List<Button> softButtons;

    [Header("Soft/Hard Switch")]
    [SerializeField]
    float switchTime = 1f;
    [SerializeField]
    float switchDistance = 10f;
    [SerializeField]
    AnimationCurve switchCurve;

    WaitForEndOfFrame wait = new WaitForEndOfFrame();

    void Start()
    {
        //skillPanelParent = hardSkillPanel.parent;

        hardButtons = new List<Button>();
        softButtons = new List<Button>();

        hardParent = transform.GetChild(0);
        softParent = transform.GetChild(1);

        hardSkills = hardParent.GetComponentsInChildren<Skill>(true);
        softSkills = softParent.GetComponentsInChildren<Skill>(true);

        SetInfos(hardParent, hardSkills);
        SetInfos(softParent, softSkills);

        FillList(hardSkills, hardSkillPanel);
        FillList(softSkills, softSkillPanel);

        SetButtons(hardButtons, true);
    }

    void OnEnable()
    {
        if(currentSkillDisplay != null)
        {
            currentSkillDisplay.SetActive(false);
        }

        ////Display first Skill
        //DisplaySkill(0);

        //OR show tutorial
        tutorial.SetActive(true);
        currentSkillDisplay = tutorial;
    }

    void SetInfos(Transform p, Skill[] skills)
    {
        for (int i = 0; i < p.childCount; i++)
        {
            GameObject child = p.GetChild(i).gameObject;
            child.SetActive(false);

            SetTitle(child.transform, skills[i]);
            skills[i].gameObject.name = skills[i].title;
        }
    }

    void SetTitle(Transform p, Skill s)
    {
        for (int i = 0; i < p.childCount; i++)
        {
            Transform child = p.GetChild(0);
            if (child.CompareTag("Title"))
            {
                child.GetComponent<Text>().text = s.title;
            }
            else
            {
                SetTitle(child, s);
            }
        }
    }

    void FillList(Skill[] skills, Transform skillPanel)
    {
        Transform skillList = null;

        for (int i = 0; i < skillPanel.childCount; i++)
        {
            if (skillPanel.GetChild(i).CompareTag("List"))
            {
                skillList = skillPanel.GetChild(i);
            }
        }

        for (int i = 0; i < skills.Length; i++)
        {
            Transform t = Instantiate(skillTemplate, skillList).transform;

            ApplySkillValues(t, skills[i], 0);

            Button b = t.GetComponentInChildren<Button>();

            //'idx' has to be created because otherwise 'i' will always be equal to skills.length.
            int idx = i;
            b.onClick.AddListener(delegate { DisplaySkill(idx); });


            if (skillPanel == hardSkillPanel)
            {
                hardButtons.Add(b);
            }
            else
            {
                softButtons.Add(b);
            }
        }
    }

    void DisplaySkill(int idx)
    {

        if (currentSkillDisplay != null)
        {
            currentSkillDisplay.SetActive(false);
        }

        if (softActive)
        {
            currentSkillDisplay = softSkills[idx].gameObject;
        }
        else
        {
            currentSkillDisplay = hardSkills[idx].gameObject;
        }

        currentSkillDisplay.SetActive(true);
    }

    //Optimization: Function doesn't return if all three components have been found.
    void ApplySkillValues(Transform parent, Skill skill, int found)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.CompareTag("ExpFill"))
            {
                child.GetComponent<Image>().fillAmount = skill.expPercentage;
                found++;
            }
            else if (child.CompareTag("PassionFill"))
            {
                child.GetComponent<Image>().fillAmount = skill.passionPercentage;
                found++;
            }
            else if (child.CompareTag("Title"))
            {
                parent.name = skill.title;
                child.GetComponent<Text>().text = skill.title;
                found++;
            }
            else
            {
                ApplySkillValues(child, skill, found);
            }
        }
    }

    void SetButtons(List<Button> buttons, bool b)
    {
        foreach (Button button in buttons)
        {
            button.interactable = false;
        }
    }

    public void SwitchSkillCategory()
    {
        StartCoroutine(SwitchRegister());
    }

    IEnumerator SwitchRegister()
    {
        RectTransform target = null;
        List<Button> targetButtons = null;

        if (softActive)
        {
            SetButtons(softButtons, false);
            targetButtons = hardButtons;
            target = softSkillPanel.GetComponent<RectTransform>();
        }
        else
        {
            SetButtons(hardButtons, false);
            targetButtons = softButtons;
            target = softSkillPanel.GetComponent<RectTransform>();
        }

        Vector3 startPos = target.position;

        float timer = 0f;
        float percentage = 0f;

        target.SetSiblingIndex(0);

        while (percentage < 0.5f)
        {
            percentage = timer / switchTime;
            float adjustedPercentage = switchCurve.Evaluate(percentage * 2f);

            target.position = startPos + (Vector3.right * switchDistance * adjustedPercentage);


            timer += Time.deltaTime;
            yield return wait;
        }

        while (percentage < 1f)
        {
            percentage = timer / switchTime;
            float adjustedPercentage = switchCurve.Evaluate((percentage - 0.5f) * 2f);

            target.position = startPos + (Vector3.right * switchDistance * (1 - adjustedPercentage));


            timer += Time.deltaTime;
            yield return wait;

        }

    }
}
