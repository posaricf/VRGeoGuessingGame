using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ToggleCanvasScript : MonoBehaviour
{
    public Canvas canvas;
    public InputActionReference showCanvasAction;
    //public Button button;
    // Start is called before the first frame update
    void Start()
    {
        showCanvasAction.action.performed += ShowCanvas;
    }

    private void ShowCanvas(InputAction.CallbackContext ctx)
    {
        var value = ctx.ReadValue<float>();
        if (value > 0)
        {
            if (canvas.enabled)
            {
                canvas.enabled = false;
            }
            else
            {
                canvas.enabled = true;
            }
        }

    }

}
