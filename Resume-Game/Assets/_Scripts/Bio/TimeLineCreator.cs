
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class TimeLineCreator : MonoBehaviour
{
    [SerializeField]
    BioInfo bioInfo;

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
    //Text t;

    int minPos;
    int maxPos;
    float range;

    RectTransform trans;
    public Vector2 timeLineSize;

    float verticalOffset = 40f;
    int verticalSwitch = 1;
    float triangleOffset = 30f;
    float buttonHeightPercentage = 0.9f;

    //Dictionary<int, string> monthLookup = new Dictionary<int, string>();

    void OnEnable()
    {
        trans = GetComponent<RectTransform>();
        timeLineSize = trans.rect.size;

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

        SetField(p.start, startPos,  1, p.title + ": StartDate", startColor);
        SetField(endDate, endPos,  -1, p.title + ": EndDate", endColor);

        SetButton(startPos, endPos, idx, p);
    }

    private void SetButton(float startPos, float endPos, int idx, Phase p)
    {
        GameObject b = Instantiate(buttonPrefab, trans);
        b.name = p.title + "-Button";
        float range = endPos - startPos;
        float pos = startPos + (range * 0.5f);
        float xPosition = (timeLineSize.x * pos) - (timeLineSize.x * 0.5f);
        RectTransform t = b.GetComponent<RectTransform>();
        //t.sizeDelta = new Vector2(range * timeLineSize.x, trans.sizeDelta.y); //approach depended on anchors being set on one point.

        //Set anchors and make the button fit this area.
        t.anchorMin = new Vector2(startPos, -buttonHeightPercentage * 0.5f);
        t.anchorMax = new Vector2(endPos, buttonHeightPercentage * 0.5f);
        t.sizeDelta = Vector3.one;

        t.localPosition = Vector3.right * xPosition;
        Text text = t.GetComponentInChildren<Text>();
        text.text = p.title;
        Image img = t.GetComponentInChildren<Image>();
        img.color = p.color;

        Button button = b.GetComponentInChildren<Button>();
        button.onClick.AddListener(delegate { bioInfo.ChangeInfo(idx); });
    }

    void SetField(Date d, float pos, int dir, string name, Color c)
    {
        GameObject field = Instantiate(dateField);
        field.name = name;
        field.transform.SetParent(transform);
        RectTransform t = field.GetComponent<RectTransform>();

        float xPosition = (timeLineSize.x * pos) - (timeLineSize.x * 0.5f);
        t.localPosition = new Vector3(xPosition, (timeLineSize.y * 0.5f + verticalOffset) * dir, 0f);

        Text text = field.GetComponentInChildren<Text>();
        text.text = d.GetString();
        text.color = c;

        RectTransform triangle = t.GetChild(0).GetComponent<RectTransform>();
        triangle.localPosition = Vector3.up * triangleOffset * -dir;

        Image triImg = field.GetComponentInChildren<Image>();
        if (dir > 0)
        {
            triangle.eulerAngles = new Vector3(0f, 0f, 180f);
            triImg.color = startColor;
        }
        else
        {
            triImg.color = endColor;
        }
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

    private void OnDisable()
    {
        foreach (Transform t in trans)
        {
            Destroy(t.gameObject);
        }
    }
}

