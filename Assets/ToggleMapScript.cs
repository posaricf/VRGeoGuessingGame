using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ToggleMapScript : MonoBehaviour
{
    public GameObject map;
    public InputActionReference showMapAction;
    //public Button button;
    // Start is called before the first frame update
    void Start()
    {
        //button = GetComponent<Button>();
        //button.onClick.AddListener(DoOnClick);

        showMapAction.action.performed += ShowMap;
    }

    private void ShowMap(InputAction.CallbackContext ctx)
    {
        var value = ctx.ReadValue<float>();
        if (value > 0)
        {
            if (map.activeInHierarchy)
            {
                map.SetActive(false);
            }
            else
            {
                map.SetActive(true);
            }
        }
        
    }

}
