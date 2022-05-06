using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BackgroundChangeScript : MonoBehaviour
{
    public Material[] skybox;
    private Material currentSkybox;
    public InputActionReference backgroundAction = null;

    // Start is called before the first frame update
    void Start()
    {
        //skybox = GetComponent<Material>();
        backgroundAction.action.performed += ChangeBackground;
        currentSkybox = skybox[1];
    }

    private void ChangeBackground(InputAction.CallbackContext ctx)
    {
        var value = ctx.ReadValue<float>();
        if (value > 0)
        {
            RenderSettings.skybox = currentSkybox;
            DynamicGI.UpdateEnvironment();

            for (int i = 0; i < skybox.Length; i++)
            {
                if (i == skybox.Length - 1)
                {
                    currentSkybox = skybox[0];
                    break;
                }
                if (currentSkybox == skybox[i])
                {
                    currentSkybox = skybox[i+1];
                    break;
                }
            }
        }
    }
    
}
