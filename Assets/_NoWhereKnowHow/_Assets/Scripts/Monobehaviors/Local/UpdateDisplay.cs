using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateDisplay : MonoBehaviour
{
    public int index;
    TextMeshPro text;

    void Start()
    {
        text = GetComponentInChildren<TextMeshPro>(true);

        EventManagerClient.onCardsChangedList[index].AddListener(UpdateBillboard);
    }
    void OnDestroy()
    {
        EventManagerClient.onCardsChangedList[index].RemoveListener(UpdateBillboard);
    }

    void UpdateBillboard(string s)
    {
        if (s == "~HideCard~")
        {
            text.text = "";
            text.gameObject.SetActive(false);
            return;
        }

        if (!text.gameObject.activeSelf)
        {
            text.gameObject.SetActive(true);
        }

        text.text = s;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
