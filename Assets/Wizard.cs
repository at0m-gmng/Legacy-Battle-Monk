using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : MonoBehaviour
{
    public GameObject Fireball;
    SpriteRenderer sr;
    Animator anim;
    // public int damage;

    bool attack = false; //игрок в зоне атаки
    [SerializeField] public static int lives = 2;

    private void Start() {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate() {      
        if(GameObject.Find("Hero").transform.position.x < transform.position.x)
            sr.flipX = false;
        else 
            sr.flipX = true;

        if (attack == false)
            anim.SetInteger("Wizard", 0);
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if(collision.gameObject.tag == "Player") {
            attack = true;
            anim.SetInteger("Wizard", 1);
            // Invoke("SpawnFireball", 0.6f);
            //SpawnFireball();
        }
    }

    void SpawnFireball() {
        if (sr.flipX == false) {
            Fireball.GetComponent<SpriteRenderer>().flipX = false;
            Instantiate(Fireball, new Vector2(transform.position.x -0.3f, transform.position.y), Quaternion.identity);
            attack = false;
        }
        else if (sr.flipX == true) {
            Fireball.GetComponent<SpriteRenderer>().flipX = true;
            Instantiate(Fireball, new Vector2(transform.position.x + 0.3f, transform.position.y), Quaternion.identity);
            attack = false;
        }
    }

    public void Die() {
        if ( lives < 1) {
            attack = false;
            // sr.material.color = new Color(1f, 0f, 0f);
            anim.SetInteger("Wizard", 2);
            // sr.material.color = new Color(1f, 0f, 0f);
            Destroy(gameObject, 0.4f);
        }
    }
}
