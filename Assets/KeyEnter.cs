using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class KeyEnter : MonoBehaviour
{

    public string inputName;
    Button buttonMe;
    // Use this for initialization
    void Start()
    {
        buttonMe = GetComponent<Button>();
        buttonMe.onClick.AddListener(PerformOnClick);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("nesto");
        }

    }

    void PerformOnClick() 
    {
        

    }
}
