using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchCanvasScript : MonoBehaviour
{
    public Canvas canvasToHide;
    public Canvas canvasToShow;
    public Button button;

    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(SwitchCanvas);
    }

    void SwitchCanvas()
    {
        canvasToHide.gameObject.SetActive(false);
        canvasToShow.gameObject.SetActive(true);
    }
}
