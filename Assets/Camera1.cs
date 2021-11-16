using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Camera1 : MonoBehaviour
{
    [SerializeField] private Transform player;
    private Vector3 pos;

    [SerializeField] GameObject buttons;
    [SerializeField] GameObject referencePanel;
    [SerializeField] GameObject levelList;
    
    private void Start() {
        levelList.SetActive(false);
    }

    private void Update() {
        pos = player.position;
        pos.z = -10f;
    }

    private void Awake() {
        buttons.SetActive(true);
        referencePanel.SetActive(false);
    }

    public void Reference() {
        buttons.SetActive(false);
        referencePanel.SetActive(true);
    }

    public void ReferenceOff() {
        buttons.SetActive(true);
        referencePanel.SetActive(false);
    }

    public void OpenLevelsList()
    {
        levelList.SetActive(true);
        buttons.SetActive(false);
    }

    public void Select(int index)
    {
        SceneManager.LoadScene(index); 
    }
}
