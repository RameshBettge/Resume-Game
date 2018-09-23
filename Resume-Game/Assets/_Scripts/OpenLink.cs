using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenLink : MonoBehaviour {

    [SerializeField]
    string url;

	public void GoToURL()
    {
        Application.OpenURL(url);
    }
}
