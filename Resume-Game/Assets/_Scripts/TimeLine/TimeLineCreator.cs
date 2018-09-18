
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class TimeLineCreator : MonoBehaviour
{
    [SerializeField]
    GameObject dateField;

    [SerializeField]
    Color startColor = Color.white;

    [SerializeField]
    Color endColor = Color.white;

    public Phase scope;
    public Phase[] phases;
    Text t;

    int minPos;
    int maxPos;
    float range;

    RectTransform trans;
    Vector2 timeLineSize;

    float verticalOffset = 40f;
    float endOffsetAddition = 30f;
    int verticalSwitch = 1;

    Dictionary<int, string> monthLookup = new Dictionary<int, string>();


    void Start()
    {
        trans = GetComponent<RectTransform>();
        timeLineSize = trans.sizeDelta;

        t = GetComponentInChildren<Text>();
        //t.text = scope.start.GetString();

        minPos = scope.start.Position;
        maxPos = scope.end.Position;
        range = maxPos - minPos;

        for (int i = 0; i < phases.Length; i++)
        {
            VisualizePhase(phases[i]);
            verticalSwitch = -verticalSwitch;
        }
    }

    private void VisualizePhase(Phase p)
    {
        float startPos = MapPosition(p.start);
        float endPos = MapPosition(p.end);

        Date endDate = p.end;
        if (p.inProgress)
        {
            endPos = MapPosition(scope.end);
            endDate = scope.end;
        }

        SetField(p.start, startPos, verticalOffset, 1, p.title + ": StartDate", startColor);
        SetField(endDate, endPos, verticalOffset + endOffsetAddition, -1, p.title + ": EndDate", endColor);
    }

    void SetField(Date d, float pos, float offset, int dir, string name, Color c)
    {
        GameObject field = Instantiate(dateField);
        field.name = name;
        field.transform.SetParent(transform);
        RectTransform t = field.GetComponent<RectTransform>();

        float xPosition = (timeLineSize.x * pos) - (timeLineSize.x * 0.5f);
        t.localPosition = new Vector3(xPosition, (timeLineSize.y * 0.5f +  offset) * dir, 0f);

        Text text = field.GetComponentInChildren<Text>();
        text.text = d.GetString();
        text.color = c;
        //field.GetComponentInChildren<Image>().color = c;
    }

    float MapPosition(Date d)
    {
        float mappedPos = (d.Position - minPos) / range;
        //print("d.Position "+ d.Position + " minPos "+ minPos + " range: "+ range + " mappedPos "+ mappedPos);
        return mappedPos;
    }
}

