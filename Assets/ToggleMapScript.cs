using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleMapScript : MonoBehaviour
{
    public GameObject map;
    Button button;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(DoOnClick);
    }

    private void DoOnClick()
    {
        if (map.active)
        {
            map.SetActive(false);
        }
        else
        {
            map.SetActive(true);
        }
        
    }

}
