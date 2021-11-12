using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skull : MonoBehaviour
{
    SpriteRenderer sr;
    Animator anim;
    Rigidbody2D rb;
    public int distance;
    float maxDistance;
    float minDistance;
    int speed = 1;
    [SerializeField]  public static int lives = 2;
    public int damage;

    private void Start() {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        maxDistance = transform.position.x + distance;
        minDistance = transform.position.x - distance;

    }

    private void FixedUpdate() {
        transform.Translate(transform.right * speed * Time.deltaTime);
        if (transform.position.x > maxDistance) {
            speed = -speed;
            sr.flipX = false;
        } else if (transform.position.x < minDistance) {    
            speed = -speed;
            sr.flipX = true;
        } 
        sr.material.color = new Color(1f, 1f, 1f);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player"){
            speed = -speed;
            sr.flipX = !sr.flipX;
        }
        // if (collision.gameObject.tag == "Damage"){
        //     sr.material.color = new Color(1f, 0f, 0f);
        //     lives-=2;
        // }

        
    }
    
    public void Die() {
        if ( lives < 1) {
            anim.SetInteger("Skull", 1);
            speed = 0;
            Destroy(gameObject, 0.4f);
        }
    }
}
