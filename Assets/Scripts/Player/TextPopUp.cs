using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextPopUp : MonoBehaviour
{
    public GameObject textMain;
    private void OnEnable()
    {
        Invoke("Hide", 0.5f);
    }

    public void Hide()
    {
        textMain.SetActive(false);
    }
}
