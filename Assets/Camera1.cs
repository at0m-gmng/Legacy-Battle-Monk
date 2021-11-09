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

    // [SerializeField] float leftLimit,
    //     rightLimit,
    //     topLimit,
    //     bottomLimit;

    private void Update() {
        pos = player.position;
        pos.z = -10f;

        // if(GameObject.Find("Hero").transform) {
        //     transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime*2);    
        // }
        
        // pos2 = new Vector3
        // (
        // Mathf.Clamp(transform.position.x, leftLimit, rightLimit),
        // Mathf.Clamp(transform.position.y, topLimit, bottomLimit),
        // transform.position.z
        // );

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

    public void OpenLevelsList(int index)
    {
        SceneManager.LoadScene(index); //scene levelMenu
    }

    public void Select(int index)
    {
        SceneManager.LoadScene(index); 
    }

    // private void OnDrawGizmos() {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawLine(new Vector2(leftLimit, topLimit), new Vector2(rightLimit, topLimit));
    //     Gizmos.DrawLine(new Vector2(leftLimit, bottomLimit), new Vector2(rightLimit, bottomLimit));
    //     Gizmos.DrawLine(new Vector2(leftLimit, topLimit), new Vector2(leftLimit, bottomLimit));
    //     Gizmos.DrawLine(new Vector2(rightLimit, topLimit), new Vector2(rightLimit, bottomLimit));
    // }
}
