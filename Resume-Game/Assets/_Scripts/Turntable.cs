using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turntable : MonoBehaviour
{
    public string characterName = "Name";
    [SerializeField]
    bool locked = false;

    Material[] mats;

    SpriteRenderer spriteRend;

    private void Awake()
    {
        Renderer[] rends = GetComponentsInChildren<Renderer>();
        mats = new Material[rends.Length];
        for (int i = 0; i < rends.Length; i++)
        {
            mats[i] = rends[i].material;
        }

        if (locked)
        {
            SetFade(1f);

            GameObject spritePrefab = Resources.Load("Lock") as GameObject;
            Transform sp = Instantiate(spritePrefab, transform).transform;
            sp.localPosition = new Vector3(0f, 0.8f, 0.8f);

            for (int i = 0; i < mats.Length; i++)
            {
                mats[i].SetFloat("Amount", 1f);
            }
        }
    }

    public void SetFade(float amount)
    {
        if (locked) { return; }

        for (int i = 0; i < mats.Length; i++)
        {
            mats[i].SetFloat("Amount", amount);
        }
    }
}
