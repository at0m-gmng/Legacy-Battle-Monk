using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Angel : MonoBehaviour
{
    //public GameObject Hero;

    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sr;
    bool attack = false; //игрок в зоне атаки

    [SerializeField] public static int lives = 4;

    private void Start() {
        rb  = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate() {
        if(attack == true) {
            anim.SetInteger("Angel", 1);
            transform.position = Vector2.Lerp(transform.position, GameObject.Find("Hero").transform.position, Time.deltaTime);
        }
        sr.material.color = new Color(1f, 1f, 1f);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "Player") {
            attack = true;
        }
    }
    
    // private void OnCollisionEnter2D(Collision2D collision) {
    //     if(collision.gameObject.tag == "Damage") {
    //         sr.material.color = new Color(1f, 0f, 0f);
    //         lives-=2;
    //         Debug.Log("Angel lives = " + lives); 
    //     } 
    public void Die() {
        if (lives < 1) {
            attack = false; // зануляем атаку, чтобы анимация атаки не мешала анимации смерти
            anim.SetInteger("Angel", 2);
            Destroy(gameObject, 0.4f);
            //Debug.Log("Противник убит"); 
        }
    }
}
