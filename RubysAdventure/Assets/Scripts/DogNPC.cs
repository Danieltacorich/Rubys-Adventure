using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogNPC : MonoBehaviour

{
    public float displayTime = 4.0f;
    public GameObject dialogBox;
    public GameObject dialog;
    float timerDisplay;
    
    void Start()
    {
        dialogBox.SetActive(false);
        timerDisplay = -1.0f;

        dialog.SetActive(false);
        timerDisplay = -1.0f;
    }
    
    void Update()
    {
        if (timerDisplay >= 0)
        {
            timerDisplay -= Time.deltaTime;
            if (timerDisplay < 0)
            {
                dialogBox.SetActive(false);
            }
        }
        //RubyController controller = other.GetComponent<RubyController>();
        else if (timerDisplay >= 0)
        {
            timerDisplay -= Time.deltaTime;
            if (timerDisplay < 0)
            {
                dialog.SetActive(false);
            }
        }

    }
    
    public void DisplayDialog()
    {
        timerDisplay = displayTime;
        dialogBox.SetActive(true);
        dialog.SetActive(true);
    }
}
