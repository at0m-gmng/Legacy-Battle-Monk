using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class inputWindow : MonoBehaviour {   
    private Button okButton;
    private Button cancelButton;
    private InputField inputField2; 
    [SerializeField] GameObject diePanelButtons;
 
    private void Awake() {
        // Hide();
        // Show();
        okButton = transform.Find("ok").GetComponent<Button>();
        cancelButton = transform.Find("cancel").GetComponent<Button>();
        inputField2 = transform.Find("InputField").GetComponent<InputField>();

    }
    private void Start() {
        diePanelButtons.SetActive(false);
    }

    public void SendRecord() {
        FindObjectOfType<highScoreTable>().LoadTableOrDefault();
        FindObjectOfType<highScoreTable>().AddHightScoreAndSave(Collect.score, inputField2.text);
        Hide();
        diePanelButtons.SetActive(true);
    }
    public void Show() {
        gameObject.SetActive(true);
    }
    public void Hide() {
        gameObject.SetActive(false);
        diePanelButtons.SetActive(true);
    }

    // private char validateChar(string validSymbols, char addedChar) {
    //     if(validSymbols.IndexOf(addedChar) != -1) {
    //         return addedChar;
    //     } else {
    //         return '\0';
    //     }
    // }

}
