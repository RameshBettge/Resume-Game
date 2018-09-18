
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TimeLineCreator : MonoBehaviour
{
    public Phase scope;
    public Phase[] phases;
    Text t;

    Dictionary<int, string> monthLookup = new Dictionary<int, string>();
    

    void Start()
    {
        t = GetComponentInChildren<Text>();
        t.text = scope.start.GetString();
    }

    void MapScope(Phase p)
    {
        int start = p.start.Position - scope.start.Position;
        int end = p.end.Position - scope.start.Position;
    }

    void Update()
    {

    }
}

