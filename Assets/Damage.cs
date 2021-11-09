using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Damage : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;


    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    public float life_hit_circle;

    private void Update() {
        Destroy(gameObject, life_hit_circle);
    }
    // public int damage2 = 2;
    // private void OnTriggerStay2D(Collider2D collision) {
    //     if(collision.gameObject.tag == "Wizard" || collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Boss" ) {
    //         Wizard.health -= damage2;
    //     }
    // }

    //уничтожаем наш Damage круг при касании с любым другим объектом, чтобы очистить память
    private void OnCollisionEnter2D(Collision2D collision) {
        //if(collision.gameObject.tag == "Player")
        if(collision.gameObject.tag == "Wizard") {
            Collect.score+=100;
            //Debug.Log("score = " + score);
        }
        if(collision.gameObject.tag == "Enemy") {
            Collect.score+=50;
            //Debug.Log("score = " + score);
        }
        if(collision.gameObject.tag == "Boss") {
            Collect.score+=(150/2);
            
        }
    }
}
