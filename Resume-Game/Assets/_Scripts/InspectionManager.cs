using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class InspectionManager : MonoBehaviour
{
    [SerializeField]
    AnimationCurve smoothTransition;

    public CanvasGroup currentGroup;

    WaitForEndOfFrame wait = new WaitForEndOfFrame();

    Transform cameras;
    Vector3 camDefaultPos;
    Quaternion camDefaultRot;

    Stack<TransitionHelper> lastHelpers = new Stack<TransitionHelper>();

    void Awake()
    {
        cameras = Camera.main.transform.parent;
        camDefaultPos = cameras.position;
        camDefaultRot = cameras.rotation;

        DisableInactiveUI();
        SetManagerReferences();
    }

    void DisableInactiveUI()
    {
        CanvasGroup[] groups = gameObject.GetComponentsInChildren<CanvasGroup>(true);

        for (int i = 0; i < groups.Length; i++)
        {
            if (groups[i].gameObject == this.gameObject) { continue; }
            else if (currentGroup == null)
            {
                currentGroup = groups[i];
                groups[i].gameObject.SetActive(true);
                groups[i].alpha = 1f;
                continue;
            }

            groups[i].alpha = 0f;
            groups[i].gameObject.SetActive(false);
            SetButtons(groups[i].gameObject, false);
        }
    }

    private void SetManagerReferences()
    {
        TransitionHelper[] helpers = GetComponentsInChildren<TransitionHelper>();
        for (int i = 0; i < helpers.Length; i++)
        {
            helpers[i].SetManager(this);
        }
    }

    //public void Transition(GameObject targetCanvas, TransType type, float duration)
    public void Transition(TransitionHelper tH)
    {
        tH.lastGroup = currentGroup;
        tH.lastPos = cameras.position;
        tH.lastRot = cameras.eulerAngles;

        switch (tH.type)
        {
            case TransType.Blend:
                StartCoroutine(Blend(tH));
                break;
            case TransType.FromLeft:
                StartCoroutine(Slide(tH, -1));
                break;
            case TransType.FromRight:
                StartCoroutine(Slide(tH, 1));
                break;
            case TransType.Immediate:
                break;
            default:
                break;
        }
        if (tH.newCamPos)
        {
            StartCoroutine(SetCameraTransform(tH));
        }

        lastHelpers.Push(tH);
    }

    public void RevertTransition()
    {
        TransitionHelper tH = lastHelpers.Pop();
        tH.lastGroup.gameObject.SetActive(true);

        //Debug.Log("target: " + tH.target.gameObject.name);
        //Debug.Log("current: " + currentGroup.gameObject.name);

        switch (tH.type)
        {
            case TransType.Blend:
                StartCoroutine(Blend(tH, true));
                break;
            case TransType.FromLeft:
                StartCoroutine(Slide(tH, -1, true));
                break;
            case TransType.FromRight:
                print("revert slide");
                StartCoroutine(Slide(tH, 1, true));
                break;
            case TransType.Immediate:
                break;
            default:
                break;
        }

        if (tH.newCamPos)
        {
            StartCoroutine(SetCameraTransform(tH, true));
        }
    }

    IEnumerator Blend(TransitionHelper tH, bool revert = false)
    {
        tH.target.gameObject.SetActive(true);
        CanvasGroup targetGroup = tH.target;
        if (revert)
        {
            targetGroup = tH.lastGroup;
        }

        SetButtons(currentGroup.gameObject, false);
        SetButtons(tH.target.gameObject, false);

        float timer = 0f;
        float percentage = 0f;


        while (percentage < 1f)
        {
            percentage = timer / tH.duration;

            float smoothPercentage = smoothTransition.Evaluate(percentage);

            currentGroup.alpha = 1 - smoothPercentage;
            targetGroup.alpha = smoothPercentage;

            timer += Time.deltaTime;
            yield return wait;
        }
        currentGroup.alpha = 0f;
        targetGroup.alpha = 1f;

        currentGroup.gameObject.SetActive(false);
        currentGroup = targetGroup;

        SetButtons(targetGroup.gameObject, true);
    }

    IEnumerator Slide(TransitionHelper tH, int dir, bool revert = false)
    {
        float timer = 0f;
        float percentage = 0f;

        tH.target.gameObject.SetActive(true);

        CanvasGroup targetGroup = tH.target;
        targetGroup.alpha = 1f;
        RectTransform target = tH.target.GetComponent<RectTransform>();
        SetButtons(currentGroup.gameObject, false);
        SetButtons(tH.target.gameObject, false);

        Vector3 startOffset = Vector3.right * Screen.width * dir;

        while (percentage < 1f)
        {
            if (revert)
            {
                if (percentage > 0.5f)
                {
                    float alphaPercentage = (percentage - 0.5f) * 2;
                    float smoothAlpha = smoothTransition.Evaluate(alphaPercentage);
                    tH.lastGroup.alpha = smoothAlpha;
                }
                percentage = 1 - percentage;

            }
            else
            {
                currentGroup.alpha = Mathf.Clamp01(1 - (percentage * 3f));
            }

            float smoothPercentage = smoothTransition.Evaluate(percentage);

            percentage = timer / tH.duration;

            target.localPosition = startOffset * (1 - smoothPercentage);

            timer += Time.deltaTime;
            yield return wait;
        }
        target.localPosition = Vector3.zero;

        currentGroup.gameObject.SetActive(false);
        currentGroup = targetGroup;
        if (revert)
        {
            currentGroup = tH.lastGroup;
        }
        SetButtons(currentGroup.gameObject, true);
    }

    IEnumerator SetCameraTransform(TransitionHelper tH, bool revert = false)
    {
        float timer = 0f;
        float percentage = 0f;

        Vector3 startPos = cameras.position;
        Vector3 startRot = cameras.eulerAngles;

        Vector3 targetPos = Vector3.zero;
        Vector3 targetRot = Vector3.zero;

        if (revert)
        {
            targetPos = tH.lastPos;
            targetRot = tH.lastRot;
        }
        else
        {
            targetPos = tH.newPos;
            targetRot = tH.newRot;
        }

        while (percentage < 1f)
        {
            percentage = timer / tH.duration;

            float smoothPercentage = smoothTransition.Evaluate(percentage);

            cameras.position = Vector3.Lerp(startPos, targetPos, smoothPercentage);
            cameras.eulerAngles = LerpEulerAngles(startRot, targetRot, smoothPercentage);

            timer += Time.deltaTime;
            yield return wait;
        }
    }

    Vector3 LerpEulerAngles(Vector3 euler, Vector3 target, float percentage)
    {
        Vector3 output = new Vector3(
        Mathf.LerpAngle(euler.x, target.x, percentage),
        Mathf.LerpAngle(euler.y, target.y, percentage),
        Mathf.LerpAngle(euler.z, target.z, percentage));

        return output;
    }

    public void SetButtons(GameObject canvas, bool active)
    {
        Button[] buttons = canvas.GetComponentsInChildren<Button>(true);
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].enabled = active;
        }
    }
}
