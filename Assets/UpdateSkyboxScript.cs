using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateSkyboxScript : MonoBehaviour
{
    public Canvas canvasToHide;
    public List<Canvas> canvasList;
    public Material skybox;
    public Button button;

    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(UpdateSkybox);
    }

    void UpdateSkybox()
    {

        for (int i = 0; i < canvasList.Count; i++)
        {
            canvasList[i].gameObject.SetActive(true);
        }
        canvasToHide.gameObject.SetActive(false);

        RenderSettings.skybox = skybox;
		DynamicGI.UpdateEnvironment();
	}
}
