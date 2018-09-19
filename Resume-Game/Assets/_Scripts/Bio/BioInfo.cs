using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BioInfo : MonoBehaviour
{
    GameObject[] infos;

    GameObject startHint;
    GameObject activeInfo;


    Transform infoParent;

    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).CompareTag("Infos"))
            {
                infoParent = transform.GetChild(i);
            }
            else
            {
                startHint = transform.GetChild(i).gameObject;
            }
        }
    }
    private void OnEnable()
    {
        activeInfo = startHint;
        activeInfo.SetActive(true);

        infos = new GameObject[infoParent.childCount];
        for (int i = 0; i < infoParent.childCount; i++)
        {
            infos[i] = infoParent.GetChild(i).gameObject;
            infos[i].SetActive(false);
        }
    }

    public void ChangeInfo(int idx)
    {
        if(idx >= infos.Length || idx < 0)
        {
            Debug.LogError("There is no info child with index " + idx);
            return;
        }

        activeInfo.SetActive(false);
        infos[idx].SetActive(true);
        activeInfo = infos[idx];
    }

    //public void ResetHint()
    //{

    //}
}
