using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Horizontal or Vertical Layout which resizes children.
/// Grid layout could be added later.
/// </summary>
public class CustomLayout : MonoBehaviour
{
    [SerializeField] LayoutAlignment layoutAlignment;
    [SerializeField] Vector2 preferedCellSize;
    [SerializeField] Vector2 padding;
    [SerializeField] Vector2 spacing;


    public Vector2 actualCellSize;
    RectTransform[] elements;

    RectTransform trans;
    Vector2 gridSize;
    int rows;
    int columns;

    void Awake()
    {
        GetValues();
        CheckDifference();
        ScaleElements();
    }

    private void GetValues()
    {
        trans = GetComponent<RectTransform>();
        if (trans == null)
        {
            Debug.LogError("CustomHorizontalLayout.cs can only be used on UI Element.");
            this.enabled = false;
            return;
        }

        gridSize = trans.sizeDelta;

        elements = new RectTransform[trans.childCount];

        actualCellSize = preferedCellSize;
    }

    void CheckDifference()
    {
        float resizePercentage = 0f;
        if (layoutAlignment == LayoutAlignment.Horizontal)
        {
            resizePercentage = GetDifference(padding.x, spacing.x, preferedCellSize.x, gridSize.x);
        }
        else
        {
            resizePercentage = GetDifference(padding.y, spacing.y, preferedCellSize.y, gridSize.y);
        }

        Resize(resizePercentage);
    }

    float GetDifference(float padding, float spacing, float preferedCellSize, float gridSize)
    {
        float totalSize = (padding * 2) + (spacing * (trans.childCount - 1))
            + (preferedCellSize * trans.childCount);


        Vector2 difference = Vector2.zero;
        difference.x = totalSize - gridSize;
        print("difference.x " + difference.x);
        if (difference.x > 0f)
        {
            float individualDifference = difference.x / trans.childCount;
            float resizePercentage = individualDifference / preferedCellSize;

            return 1 - resizePercentage;

        }
        else
        {
            return 1f;
        }
    }

    void Resize(float resizePercentage)
    {
        actualCellSize.x = preferedCellSize.x * (resizePercentage);
        actualCellSize.y = preferedCellSize.y * (resizePercentage);
    }

    private void ScaleElements()
    {
        for (int i = 0; i < trans.childCount; i++)
        {
            elements[i] = trans.GetChild(i).GetComponent<RectTransform>();
            elements[i].sizeDelta = actualCellSize;
            // xPos =  Left end of transform   + (picture + spacing)             * (i + 0.5f)  + padding 
            //0.5f is added to i so that the first picture starts at the correct place

            if (layoutAlignment == LayoutAlignment.Horizontal)
            {
                float xPos = -(gridSize.x * 0.5f) + (actualCellSize.x * (i + 0.5f)) + (spacing.x * i) + padding.x;
                elements[i].localPosition = new Vector3(xPos, 0f, 0f);
            }
            else
            {
                float yPos = -(gridSize.y * 0.5f) + (actualCellSize.y * (i + 0.5f)) + (spacing.y * i) + padding.y;

                elements[i].localPosition = new Vector3(0f, yPos, 0f);
            }
        }
    }
}

public enum LayoutAlignment { Horizontal, Vertical }