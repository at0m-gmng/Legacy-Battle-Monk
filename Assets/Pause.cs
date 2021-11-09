using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject referencePanel;

    public void Awake() {
        pausePanel.SetActive(false);
        referencePanel.SetActive(false);
    }

    public void SetPause() {
        pausePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void PauseOff() {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void Reference() {
        referencePanel.SetActive(true);
        pausePanel.SetActive(false);
    }

    public void ReferenceOff() {
        pausePanel.SetActive(true);
        referencePanel.SetActive(false);
    }

    public void OpenLevelsList(int index)
    {   if (index == 0) {
            Collect.score = 0;
            PauseOff();
        }
        SceneManager.LoadScene(index); //scene levelMenu
    }
}
