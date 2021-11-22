using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Camera1 : MonoBehaviour
{
    [SerializeField] private Transform player;
    private Vector3 pos;


    [SerializeField] GameObject referencePanel;
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject diePanel;
    [SerializeField] GameObject levelNextPanel;
    [SerializeField] GameObject androidControl;
    [SerializeField] GameObject recordsTable;
    [SerializeField] GameObject pauseBattons;
    // [SerializeField] GameObject diePanelInputWindow;


    private void Update() {
        pos = player.position;
        pos.z = -10f;
        LevelEnding();
        if(PlayerController.lives < 1) {
            DieMenu();
        }
    }

    private void Awake() {
        // inputWindow = transform.Find("/DieMenu/InputWindow").GetComponent<GameObject>();

        recordsTable.SetActive(false);
        referencePanel.SetActive(false);
        pausePanel.SetActive(false);
        diePanel.SetActive(false);
        levelNextPanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void SetPause() {
        pausePanel.SetActive(true);
        Time.timeScale = 0;

    }

    public void PauseOff() {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void records() {
        pauseBattons.SetActive(false);
        recordsTable.SetActive(true);
    }

    public void recordsOff() {
        pauseBattons.SetActive(true);
        recordsTable.SetActive(false);
    }

    public void Reference() {
        if(PlayerController.keyboard == false) {
            referencePanel.SetActive(false);
            pausePanel.SetActive(true);
        } else {
            referencePanel.SetActive(true);
            pausePanel.SetActive(false);
        }
    }

    public void ReferenceOff() {
        referencePanel.SetActive(false);
        pausePanel.SetActive(true);
    }

    private void DieMenu() {
        Time.timeScale = 0;
        diePanel.SetActive(true);
        androidControl.SetActive(false);
        // if (PlayerController.lives < 1) {
        //     diePanelButtons.SetActive(false);
        // }
    }

    private void LevelEnding() {
        if(!GameObject.Find("Angel") ) {
            levelNextPanel.SetActive(true);
            Time.timeScale = 0;
        } 
    }

    public void Select(int index) {
        PauseOff();
        // diePanelButtons.SetActive(true);
        if(PlayerController.lives < 1) {
            Collect.score = 0;
        }
        SceneManager.LoadScene(index);
        PlayerController.lives = 5; 
    }
}
