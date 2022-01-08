using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Camera0 : MonoBehaviour
{
    [SerializeField] public GameObject buttonsMainMenu;
    [SerializeField] public GameObject levelList;
    [SerializeField] GameObject referencePanel;

    private void Awake() {
        levelList.SetActive(false);
        buttonsMainMenu.SetActive(true);
        referencePanel.SetActive(false);
    }

    public void Reference() {
        if(PlayerController.keyboard == false) {
            referencePanel.SetActive(false);
            buttonsMainMenu.SetActive(true);
        } else {
            referencePanel.SetActive(true);
            buttonsMainMenu.SetActive(false);
        }
    }

    public void ReferenceOff() {
        buttonsMainMenu.SetActive(true);
        referencePanel.SetActive(false);
    }

    public void OpenLevelsList() {
        levelList.SetActive(true);
        buttonsMainMenu.SetActive(false);
    }

    public void Select(int index) {
        Time.timeScale = 1;
        PlayerController.lives = 5;
        Collect.score = 0;
        SceneManager.LoadScene(index); 
    }
}
