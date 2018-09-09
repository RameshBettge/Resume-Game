using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionHelper : MonoBehaviour
{
    public GameObject target;
    public TransType type;
    public float duration;

    public bool newCamPos;
    public Vector3 newPos;
    public Vector3 newRot;

    [HideInInspector]
    public CanvasGroup lastGroup;
    [HideInInspector]
    public Vector3 lastPos;
    [HideInInspector]
    public Vector3 lastRot;

    InspectionManager manager;

    public void SetManager(InspectionManager manager)
    {
        this.manager = manager;
    }

    public void TriggerTransition()
    {
        manager.Transition(this);
    }
}
