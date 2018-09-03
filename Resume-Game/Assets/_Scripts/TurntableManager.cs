using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurntableManager : MonoBehaviour
{
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
    int selected = 0;
    int nextSelection = 0;

    WaitForEndOfFrame wait = new WaitForEndOfFrame();

    void Start()
    {
        tables = GetComponentsInChildren<Turntable>();
        CreateDefaultPosition();
    }

    void CreateDefaultPosition()
    {
        int childCount = tables.Length;
        rotationIncrement = 360 / childCount;

        for (int i = 0; i < childCount; i++)
        {
            tables[i].transform.localPosition = Vector3.forward * defaultDistance;
            tables[i].transform.RotateAround(
                transform.position, Vector3.up, rotationIncrement * i
                );
        }
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
        int dir = clockwise ? 1 : -1;


        if (!inAction)
        {
            StartCoroutine(ChangeSelection(dir));
        }
    }

    IEnumerator ChangeSelection(int dir)
    {
        inAction = true;

        Vector3 startRot = transform.eulerAngles;

        float timer = 0f;
        float percentage = 0f;
        while (percentage < 1f)
        {
            percentage = timer / selectDelay;
            float adjustedPercentage = selectionCurve.Evaluate(percentage);
            transform.eulerAngles = startRot + (Vector3.up * rotationIncrement * adjustedPercentage * dir);

            timer += Time.deltaTime;
            yield return wait;
        }
        transform.eulerAngles = startRot + (Vector3.up * rotationIncrement * dir);

        inAction = false;
    }
}
