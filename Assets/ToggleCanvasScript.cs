using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ToggleCanvasScript : MonoBehaviour
{
    public Canvas helpCanvas;
    public Canvas scoreboardCanvas;
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
            if (helpCanvas.enabled && scoreboardCanvas.enabled)
            {
                helpCanvas.enabled = false;
                scoreboardCanvas.enabled = false;
            }
            else
            {
                helpCanvas.enabled = true;
                scoreboardCanvas.enabled = true;
            }
        }

    }

}
