using Utilities;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Game : Singleton<Game>
{
    [SerializeField]
    RawImage selectedView;

    [SerializeField]
    CanvasGroup selectionCanvasGroup;

    [SerializeField]
    CanvasGroup inspectionCanvasGroup;

    [SerializeField]
    Transform cameraParent;

    [SerializeField]
    float fadeTime = 1f;

    [SerializeField]
    [Tooltip("How the cameras and the UI elements crossfade.")]
    AnimationCurve crossfadeCurve;
    [SerializeField]
    AnimationCurve camMoveCurve;

    [SerializeField]
    [Tooltip("How far the character moves to the side when inspected")]
    float cameraSideOffset = -2f;

    public State GameState = State.Selection;

    WaitForEndOfFrame wait;

    TurntableManager tManager;

    Vector3 cameraDefaultPosition;
    Vector3 cameraSidePosition;


    private void Start()
    {
        inspectionCanvasGroup.alpha = 0f;
        inspectionCanvasGroup.gameObject.SetActive(false);

        tManager = TurntableManager.Instance;
        Color c = selectedView.color;
        c.a = 0f;
        selectedView.color = c;

        cameraDefaultPosition = cameraParent.position;
        cameraSidePosition = cameraDefaultPosition + Vector3.right * cameraSideOffset;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            SelectCharacter();
        }
    }

    public void SelectCharacter()
    {
        if (GameState != State.Selection) { return; }
        GameState = State.Transition;

        StartCoroutine(WaitForTurntables());
    }

    IEnumerator WaitForTurntables()
    {
        while (tManager.InAction)
        {
            yield return wait;
        }

        MarkAsSelected(tManager.Tables[tManager.Selected].transform);

        StartCoroutine(SwitchToInspection());
    }

    void MarkAsSelected(Transform t)
    {
        t.gameObject.layer = LayerMask.NameToLayer("Selected");
        for (int i = 0; i < t.childCount; i++)
        {
            MarkAsSelected(t.GetChild(i));
        }
    }

    IEnumerator SwitchToInspection()
    {
        float timer = 0f;
        float percentage = 0f;
        //Fade out the GameObjects necessary for the Selection.
        while (percentage < 1f)
        {
            percentage = timer / fadeTime;
            float adjustedPercentage = crossfadeCurve.Evaluate(percentage);

            //Make the renderPlane of 'Camera_selected' visible
            Color c = selectedView.color;
            c.a = Mathf.Lerp(0f, 1f, adjustedPercentage);
            selectedView.color = c;

            //Fade out the UI
            selectionCanvasGroup.alpha = 1 - adjustedPercentage;

            //Move camera the first 50% of the way to the side.
            float cameraPercentage = camMoveCurve.Evaluate(percentage / 2f);
            cameraParent.position = Vector3.Lerp(cameraDefaultPosition, cameraSidePosition, cameraPercentage);

            timer += Time.deltaTime;

            yield return wait;
        }

        selectionCanvasGroup.gameObject.SetActive(false);
        inspectionCanvasGroup.gameObject.SetActive(true);

        //Fade in the GameObjects necessary for Inspection.
        timer = 0f;
        percentage = 0f;
        while (percentage < 1f)
        {
            timer += Time.deltaTime;
            percentage = timer / fadeTime;

            float adjustedPercentage = 1 - crossfadeCurve.Evaluate(1 - percentage);

            inspectionCanvasGroup.alpha = adjustedPercentage;

            //Move camera the second 50% of the way to the side.
            //float cameraPercentage = camMoveCurve.Evaluate((percentage / 2f) + 0.5f);

            yield return wait;
        }

        GameState = State.Inspection;
    }
}

public enum State
{
    MainMenu, Selection, Inspection, Transition
}