using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

public class TurntableManager : MonoBehaviour
{
    [SerializeField]
    Text characterNameField;
    [SerializeField]
    GameObject selectButton;
    [SerializeField]
    GameObject LockedButton;

    [SerializeField]
    Renderer background;

    [SerializeField]
    float defaultDistance = 2f;
    [SerializeField]
    float selectedDistance = 3f;
    [SerializeField]
    float selectDelay = 1f;

    [SerializeField]
    [Tooltip("How the tables turn when the selection changes.")]
    AnimationCurve selectionCurve;

    Turntable[] tables;
    float[] selectRotations;

    float rotationIncrement;

    bool inAction;

    [SerializeField]
    int defaultSelected;
    int selected = 0;
    int nextSelection = 0;

    WaitForEndOfFrame wait = new WaitForEndOfFrame();

    void Start()
    {
        tables = GetComponentsInChildren<Turntable>();
        selected = defaultSelected;
        selected = Mathf.Clamp(selected, 0, tables.Length);
        nextSelection = selected;

        CreateDefaultPosition();

        background.material.color = tables[selected].bgColor;
        UpdateName();
        tables[selected].SetFade(0);
        for (int i = 0; i < tables.Length; i++)
        {
            if(i == selected) { return; }
            tables[i].SetFade(1);
        }

    }

    void CreateDefaultPosition()
    {
        int childCount = tables.Length;
        rotationIncrement = 360 / childCount;

        for (int i = 0; i < childCount; i++)
        {
            tables[i].transform.localPosition = Vector3.forward * defaultDistance;
            tables[i].transform.RotateAround(
                transform.position, Vector3.up, -rotationIncrement * i
                );
        }
        transform.eulerAngles = Vector3.up * rotationIncrement * selected;

        tables[selected].transform.localPosition = tables[selected].transform.localPosition.SetMagnitude(selectedDistance);
    }

    private void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        if (h > 0)
        {
            StartTransition(true);
        }
        else if (h < 0)
        {
            StartTransition(false);
        }
    }

    public void StartTransition(bool clockwise)
    {
        if (!inAction)
        {
            int dir = clockwise ? 1 : -1;
            UpdateSelection(dir);

            StartCoroutine(ChangeSelection(dir));
        }
    }

    void UpdateSelection(int dir)
    {
        nextSelection += dir;
        if (nextSelection == tables.Length) { nextSelection = 0; }
        else if(nextSelection < 0) { nextSelection = tables.Length - 1; }
    }

    IEnumerator ChangeSelection(int dir)
    {
        inAction = true;

        UpdateName();

        Vector3 startRot = transform.eulerAngles;

        float timer = 0f;
        float percentage = 0f;
        while (percentage < 1f)
        {
            //Rotate all children by rotating Manager
            percentage = timer / selectDelay;
            float adjustedPercentage = selectionCurve.Evaluate(percentage);
            transform.eulerAngles = startRot + (Vector3.up * rotationIncrement * adjustedPercentage * dir);

            //Move selected Character forward
            tables[selected].transform.localPosition = tables[selected].transform.localPosition.SetMagnitude(
                Mathf.Lerp(selectedDistance, defaultDistance, adjustedPercentage));
            tables[nextSelection].transform.localPosition = tables[nextSelection].transform.localPosition.SetMagnitude(
                Mathf.Lerp(selectedDistance, defaultDistance, 1 - adjustedPercentage));

            //Control fade
            tables[selected].SetFade(adjustedPercentage);
            tables[nextSelection].SetFade(1 - adjustedPercentage);

            //Set background color
            background.material.color = Color.Lerp(tables[selected].bgColor, tables[nextSelection].bgColor, adjustedPercentage);

            timer += Time.deltaTime;
            yield return wait;
        }
        transform.eulerAngles = startRot + (Vector3.up * rotationIncrement * dir);

        tables[selected].SetFade(1f);
        tables[nextSelection].SetFade(0f);


        background.material.color = tables[nextSelection].bgColor;


        selected = nextSelection;
        inAction = false;
    }

    void UpdateName()
    {
        characterNameField.text = tables[nextSelection].characterName;

        if (tables[nextSelection].locked)
        {
            selectButton.SetActive(false);
            LockedButton.SetActive(true);
            return;
        }

        selectButton.SetActive(true);
        LockedButton.SetActive(false);
    }
}
