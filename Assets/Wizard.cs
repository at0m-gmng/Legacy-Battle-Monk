using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : MonoBehaviour
{
    public GameObject Fireball;
    SpriteRenderer sr;
    Animator anim;

    bool attack = false; //игрок в зоне атаки
    [SerializeField] private int lives = 2;

    private void Start() {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

    }

    // private void Update() {
    //     if (attack == false) {
    //         anim.SetInteger("Wizard", 0);
    //     }
    // }

    private void FixedUpdate() {      
        if(GameObject.Find("Hero").transform.position.x < transform.position.x)
            sr.flipX = false;
        else 
            sr.flipX = true;
            
        if (attack == false)
            anim.SetInteger("Wizard", 0);
        //if () {
        sr.material.color = new Color(1f, 1f, 1f);
        //}

    }

    private void OnTriggerStay2D(Collider2D collision) {
        if(collision.gameObject.tag == "Player") {
            attack = true;
            anim.SetInteger("Wizard", 1);
            // Invoke("SpawnFireball", 0.6f);
            //SpawnFireball();
        }
    }
    
    // Уничтожаем Wizard при касании с Damage кругом
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Damage") {
            //попробовать тег заменить на name и имя объекта
            sr.material.color = new Color(1f, 0f, 0f);
            lives-=2;
            sr.material.color = new Color(1f, 0f, 0f);
        }

        if ( lives < 1) {
            //sr.material.color = new Color(1f, 1f, 1f);
            anim.SetInteger("Wizard", 2);
            Destroy(gameObject, 0.4f);
        }
    }

    // public void TakeDamage () {
    //     sr.material.color = new Color(1f, 0f, 0f);
    //     // health -= damage;
    //     sr.material.color = new Color(1f, 0f, 0f);

    //     if (health <= 0) {
    //         anim.SetInteger("Wizard", 2);
    //         Destroy(gameObject, 0.4f);
    //     }
    // }

// анимация спавна файр бола бесконечная, надо пофиксить
    void SpawnFireball() {
        // if(attack == true) {
        // anim.SetInteger("Wizard", 1);
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
        // } 
        // if (attack == false) {
        //     anim.SetInteger("Wizard", 0);
        // }
    }
}
