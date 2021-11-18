using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class money : MonoBehaviour {
    
    // private void OnTriggerStay2D(Collider2D collision) {
    //     if(collision.gameObject.tag == "Player") {
    //         Destroy(gameObject);
    //     }
    // }
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "Player") {
            Collect.score +=150;
            Destroy(gameObject);
        }
    }
    // private void OnTriggerExit2D(Collider2D collision) {
    //     if(collision.gameObject.tag == "Player") {
    //         Destroy(gameObject);
    //     }
    // }
}
