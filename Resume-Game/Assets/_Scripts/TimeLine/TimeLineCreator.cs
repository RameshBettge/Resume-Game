
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
    GameObject buttonPrefab;

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

        minPos = scope.start.Position;
        maxPos = scope.end.Position;
        range = maxPos - minPos;

        for (int i = 0; i < phases.Length; i++)
        {
            VisualizePhase(phases[i], i);
            verticalSwitch = -verticalSwitch;
        }
    }

    private void VisualizePhase(Phase p, int idx)
    {
        float startPos = MapPosition(p.start, p);
        float endPos = MapPosition(p.end, p);

        Date endDate = p.end;
        if (p.inProgress)
        {
            endPos = MapPosition(scope.end, p);
            endDate = scope.end;
        }

        SetField(p.start, startPos, verticalOffset, 1, p.title + ": StartDate", startColor);
        SetField(endDate, endPos, verticalOffset + endOffsetAddition, -1, p.title + ": EndDate", endColor);

        SetButton(startPos, endPos, idx, p);
    }

    private void SetButton(float startPos, float endPos, int idx, Phase p)
    {
        GameObject b = Instantiate(buttonPrefab, trans);
        b.name = p.title + "-Button";
        float range = endPos - startPos; //Is also the range
        float pos = startPos + (range * 0.5f);
        float xPosition = (timeLineSize.x * pos) - (timeLineSize.x * 0.5f);
        RectTransform t = b.GetComponent<RectTransform>();
        t.sizeDelta = new Vector2(range * timeLineSize.x, trans.sizeDelta.y);
        t.localPosition = Vector3.right * xPosition;
        Text text = t.GetComponentInChildren<Text>();
        text.text = p.title;
        Image img = t.GetComponentInChildren<Image>();
        img.color = p.color;

    }

    void SetField(Date d, float pos, float offset, int dir, string name, Color c)
    {
        GameObject field = Instantiate(dateField);
        field.name = name;
        field.transform.SetParent(transform);
        RectTransform t = field.GetComponent<RectTransform>();

        float xPosition = (timeLineSize.x * pos) - (timeLineSize.x * 0.5f);
        t.localPosition = new Vector3(xPosition, (timeLineSize.y * 0.5f + offset) * dir, 0f);

        Text text = field.GetComponentInChildren<Text>();
        text.text = d.GetString();
        text.color = c;
        //field.GetComponentInChildren<Image>().color = c;
    }


    float MapPosition(Date d, Phase p)
    {
        float mappedPos = (d.Position - minPos) / range;
        //print("d.Position "+ d.Position + " minPos "+ minPos + " range: "+ range + " mappedPos "+ mappedPos);
        if (mappedPos > 1)
        {
            Debug.LogError(d.GetString() + " (" + p.title + ") is out of max scope: " + scope.end.GetString());
        }
        else if (mappedPos < 0 && !p.inProgress)
        {
            Debug.LogError(d.GetString() + " (" + p.title + ") is out of min scope: " + scope.start.GetString());
        }
        return mappedPos;
    }
}

