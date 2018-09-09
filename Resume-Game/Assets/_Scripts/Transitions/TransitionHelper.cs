using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class TransitionHelper : MonoBehaviour
{
    public CanvasGroup target;
    public TransType type;
    public float duration;

    public bool newCamPos;
    //[HideInInspector]
    public Vector3 newPos;
    //[HideInInspector]
    public Vector3 newRot;

    [HideInInspector]
    public float testValue = 5f;

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


//[CustomEditor(typeof(TransitionHelper))]
//[CanEditMultipleObjects]
//public class TransitionEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();
//        var script = target as TransitionHelper;

//        if (script.newCamPos)
//        {
//            script.newPos = EditorGUI.Vector3Field(new Rect(0, 15, 30, 30), "Label", Vector3.one);
//        }
//        //script.testValue = EditorGUILayout.Slider("I field:", script.testValue, 1, 100);

//    }
//}
