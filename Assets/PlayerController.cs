using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum States {
    idle,
    walk,
    jump, 
    punch,
    kick,
    crouch,
    flying_kick,
    crouch_kick
}

public class PlayerController : MonoBehaviour
{    
    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sr;

    [SerializeField] private AudioSource missPunchSound;
    [SerializeField] private AudioSource missKickSound;
    [SerializeField] private AudioSource missCrouchKickSound;
    [SerializeField] private AudioSource flyingKickSound;
    [SerializeField] private AudioSource PunchSound;
    [SerializeField] private AudioSource takeMoneySound;

    public bool isGrounded = false;
    public bool isGroundedStatic = false;
    public Transform groungCheck;
    

    public LayerMask Ground;
    public LayerMask GroundStatic;

    public static bool keyboard = false; //клавиатура не подкл, подкл джойстик
    [SerializeField] GameObject androidControl;
    public Joystick joystick;

    private States State {
        get { 
            return (States)anim.GetInteger("Anim"); 
            }
        set { 
            anim.SetInteger("Anim", (int)value);
        }
    }

    [SerializeField] private float speed = 2f;
    [SerializeField] public static int lives = 5;
    [SerializeField] private int health;
    [SerializeField] private Image[] hearts;
    [SerializeField] private float jumpForce = 10f;

    [SerializeField] private Sprite aliveHeart;
    [SerializeField] private Sprite deathHeart;

    // [SerializeField] GameObject dieMenu;
    // [SerializeField] GameObject levelEnding;
    [SerializeField] GameObject pause;
    // [SerializeField] GameObject Canvas;

    public Transform PunchPosObject;  // объект для удара рукой
    public float PunchRange;
    public Transform KickPosObject; // объект для удара ногой
    public float KickRange;
    public Transform FlyKickPosObject; // объект для удара ногой
    public float FlyKickRange;
    public Transform CrouchKickPosObject; // объект для удара ногой
    public float CrouchKickRange;
    public LayerMask enemy; // маска врага
    public int damage; // наносимый урон
    
    private bool isAttacking = false; // атакуем ли мы сейчас
    private bool isCrouching = false; // в присиде ли    

    private const float DOUBLE = 2f;
    private float lastClick;

    public Collider2D poseStand;
    public Collider2D poseCrouch;
    private int crouchCounter = 0;
    private int pauseCounter = 0;

    // private int maxCrouchCounter = 2;


    private void Start() {
        СontrolOnLevel();
        health = lives;
        //Instance = this;
        sr = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>(); 
        //isRecharged = true;   
    }


    private void FixedUpdate() {
    }

    private void Update() {
        ChechGround();
        // LevelEnding();
        if ((isGrounded || isGroundedStatic) && !isAttacking ) {
            // Debug.Log("Есть апдейт");s
            //isAttacking = false;
            State = States.idle;
        }

        if (!keyboard) {
            if(!isAttacking && ((joystick.Horizontal > 0.2f) || (joystick.Horizontal < -0.2f) ) && !isCrouching)
                Run();    
            if(joystick.Vertical > 0.3f)
                Jump();
            if(!isAttacking && (isGrounded || isGroundedStatic) && (joystick.Vertical < -0.2f) && crouchCounter==0) {
                poseStand.enabled = false;
                poseCrouch.enabled = true;
                isCrouching = true;
                // Down(); //не забыть добавить
                State = States.crouch;
            }
            if((joystick.Vertical > -0.1f) ) {
                crouchCounter = 1;
                poseStand.enabled = true;
                poseCrouch.enabled = false;
                isCrouching = false;
                Invoke("Reset_crouch", 0.3f);
            }
            if ((joystick.Vertical < -0.2f) && crouchCounter==1)
                Down();
            // if((isGrounded || isGroundedStatic) && (joystick.Vertical < -0.3f))
            //     ();
        } else {
            if(Input.GetButton("Horizontal") && !isCrouching)
                Run(); 
         
            if((isGrounded || isGroundedStatic) && Input.GetButton("Vertical") && crouchCounter==0) {    
                poseStand.enabled = false;
                poseCrouch.enabled = true;
                isCrouching = true;
                if(Input.GetButtonDown("Fire2")) { // работает, но анимация срабатывает мгновенно
                    Crouch_kick();             // костыль написать GetButton
                } else if (!isAttacking) {
                    State = States.crouch;  
                }
                // Debug.Log(crouchCounter);                
            } else if(Input.GetButtonUp("Vertical")) {
                crouchCounter = 1;
                // Debug.Log(crouchCounter);
                poseStand.enabled = true;
                poseCrouch.enabled = false;
                isCrouching = false;
                Invoke("Reset_crouch", 0.2f); // будет влиять на скорость проваливания сквозь платформы
            }
            if (Input.GetButtonDown("Vertical") && crouchCounter==1)
                Down();
            if(Input.GetButtonDown("Jump")) {
                Jump();
            }
            if ((isGrounded || isGroundedStatic) && Input.GetButtonDown("Fire1"))
                Punch();
            if ((isGrounded || isGroundedStatic) && Input.GetButtonDown("Fire2") && !isCrouching) 
                Kick();
            if (!isGroundedStatic && !isGrounded && Input.GetButton("Fire2")) {
                Flying_kick();   
            }   
            if (Input.GetButtonDown("Cancel") && pauseCounter==0) {
                pause.SetActive(true);
                Time.timeScale = 0;
                pauseCounter = 1;
            } else if (Input.GetButtonDown("Cancel") && pauseCounter==1) {
                pause.SetActive(false);
                Time.timeScale = 1;
                pauseCounter = 0; 
            }

        }

        if (health > lives) {
            health = lives;
        }

        for (int i = 0; i < hearts.Length; i++) {
            if (i < health) {
                hearts[i].sprite = aliveHeart;
            } else {
                hearts[i].sprite = deathHeart;
            }

        }

        //поворот области нанесения урона в сторону персонажа
        if (sr.flipX == true) {
            PunchPosObject.transform.position = new Vector2(rb.position.x - 0.234999f, rb.position.y + 0.271f);
            KickPosObject.transform.position = new Vector2(rb.position.x - 0.181f, rb.position.y + 0.247f);
            FlyKickPosObject.transform.position = new Vector2(rb.position.x - 0.196f, rb.position.y + 0.245f);
            CrouchKickPosObject.transform.position = new Vector2(rb.position.x - 0.234f, rb.position.y + 0.155f);
        }
        else if (sr.flipX == false) {
            PunchPosObject.transform.position = new Vector2(rb.position.x + 0.234999f, rb.position.y + 0.271f);
            KickPosObject.transform.position = new Vector2(rb.position.x + 0.181f, rb.position.y + 0.247f);
            FlyKickPosObject.transform.position = new Vector2(rb.position.x + 0.196f, rb.position.y + 0.245f);
            CrouchKickPosObject.transform.position = new Vector2(rb.position.x + 0.234f, rb.position.y + 0.155f);
        }
    }

    public void Сontrol() {
        
         if (!keyboard) {
            androidControl.gameObject.SetActive(false);
            keyboard = !keyboard; // по надатии на кнопку убирает панель джойстика
        } else {
            androidControl.gameObject.SetActive(true);
            keyboard = !keyboard; // по надатии на кнопку убирает панель джойстика
        }
    }

        private void СontrolOnLevel() {
        
         if (!keyboard) {
            androidControl.gameObject.SetActive(true);
        } else {
            androidControl.gameObject.SetActive(false);
        }
    }

    private void Run() {
        isAttacking = false;
        if((isGrounded || isGroundedStatic))
            State = States.walk;
            // Flying_kick();
            
        if(!keyboard) {
            Vector3 dir = transform.right * joystick.Horizontal;
            transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);
            sr.flipX = dir.x < 0.0f;
        } else {
            Vector3 dir = transform.right * Input.GetAxis("Horizontal");
            transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);
            sr.flipX = dir.x < 0.0f;
        }
    }

    // проваливаемся вниз сквозь платформы игнорируя слои
    private void Down() { 
        Physics2D.IgnoreLayerCollision(6, 8, true);
        Invoke("IgnoreLayerOff", 0.3f);
        Reset_crouch(); 
    }

    private void IgnoreLayerOff() {
        //Debug.Log("Откл");
        Physics2D.IgnoreLayerCollision(6, 8, false);
    }

    // private void Crouch() {
    //     isCrouching = true;
    //     poseStand.enabled = false;
    //     poseCrouch.enabled = true;
    // }

    private void Reset_crouch() {
        if (crouchCounter == 1) {
            crouchCounter = 0;
            isCrouching = false; // была добавлена из-за того, что после проскальзывания сквозь платформы персонаж не мог ходить
            // Debug.Log(crouchCounter);
        }
    }

    private void Jump() {
        if ((isGrounded || isGroundedStatic) && !isAttacking)
            rb.velocity = Vector2.up * jumpForce;

        Reset_crouch();
    }

    private void ChechGround() {
        isGrounded = Physics2D.OverlapCircle(groungCheck.position, 0.08f, Ground);
        isGroundedStatic = Physics2D.OverlapCircle(groungCheck.position, 0.08f, GroundStatic);
        if(!isGroundedStatic && !isGrounded && !isAttacking) {
            State = States.jump;
        }
        // Debug.Log("на платформе " + isGrounded + " на земле " + isGroundedStatic + " в присяде " + isCrouching + " персонаж атакует " + isAttacking);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Spell" || collision.gameObject.tag == "Boss") {
            lives--;
            Debug.Log("Player lives = " + lives);
        }    

        // if (lives < 1) {
                //Destroy(gameObject, 0.5f);
            // Debug.Log("Вы мертвы");
            // Collect.score = 0;
            // SetPause();
            // Camera1.SetPause();
        // }
                
    }
    private void OnTriggerStay2D(Collider2D collision) {
        if(collision.gameObject.tag == "gold") {
            takeMoneySound.Play();
            Collect.score +=150;
        }
    }

    //отрисовка области поражения
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(PunchPosObject.position, PunchRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(KickPosObject.position, KickRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(FlyKickPosObject.position, FlyKickRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(CrouchKickPosObject.position, CrouchKickRange);
    }

    //удар рукой
    public void Punch() {
        isAttacking = true;
        if(isAttacking) {
            State = States.punch;
            damage = 2;
            Collider2D[] enemies = Physics2D.OverlapCircleAll(PunchPosObject.position, PunchRange, enemy);
            if (enemies.Length == 0)
                missPunchSound.Play();
            else
                PunchSound.Play();
            for (int i = 0; i < enemies.Length; i++) {
                    enemies[i].GetComponent<damagebleObject>().TakeDamage(damage);
            }
        }
        Invoke("PunchOff", 0.4f);
    }

    private void PunchOff() {
        isAttacking = false;
        damage  = 0;
    }

    //удар ногой
    public void Kick() {
        // Debug.Log("пинок ");
        isAttacking = true;
        if(isAttacking) {
            State = States.kick;
            damage = 2;
            Collider2D[] enemies1 = Physics2D.OverlapCircleAll(KickPosObject.position, KickRange, enemy);
            if (enemies1.Length == 0)
                missKickSound.Play();
            else
                PunchSound.Play();
            for (int i = 0; i < enemies1.Length; i++) {
                    enemies1[i].GetComponent<damagebleObject>().TakeDamage(damage);
            }
        }
        Invoke("KickOff", 0.5f);
    }

    private void KickOff() {
        isAttacking = false;
        damage  = 0;
    }

    //удар в прыжке 
    public void Flying_kick() {
        // Debug.Log("удар в прыжке");
        isAttacking = true;
        if(isAttacking && !isGroundedStatic && !isGrounded) {
            State = States.flying_kick;
            damage = 2;
            Collider2D[] enemies2 = Physics2D.OverlapCircleAll(FlyKickPosObject.position, FlyKickRange, enemy);
            if (enemies2.Length != 0)
                // flyingKickSound.Play();
                PunchSound.Play();
            // else
            for (int i = 0; i < enemies2.Length; i++) {
                    enemies2[i].GetComponent<damagebleObject>().TakeDamage(damage);
            }
        }
        Invoke("Flying_kickOff", 0.2f);
    }

    private void Flying_kickOff() {
        isAttacking = false;
        damage = 0;
    }

    //удар в присяде 
    public void Crouch_kick() {
        // Debug.Log("пинок в присяде");
        isAttacking = true;
        if(isAttacking && isCrouching) {
            State = States.crouch_kick;
            damage = 2;
            Collider2D[] enemies3 = Physics2D.OverlapCircleAll(CrouchKickPosObject.position, CrouchKickRange, enemy);
            if (enemies3.Length == 0)
                missCrouchKickSound.Play();
            else
                PunchSound.Play();
            for (int i = 0; i < enemies3.Length; i++) {
                    enemies3[i].GetComponent<damagebleObject>().TakeDamage(damage);
            }
        }
        Invoke("Crouch_kickOff", 0.4f);
    }

    private void Crouch_kickOff() {
        isAttacking = false;
        damage = 0;
    }

}
