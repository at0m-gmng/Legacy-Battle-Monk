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
    flying_kick
}

public class PlayerController : MonoBehaviour
{    
    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sr;

    public bool isGrounded = false;
    public bool isGroundedStatic = false;
    public Transform groungCheck;
    

    public LayerMask Ground;
    public LayerMask GroundStatic;

    private static bool keyboard = true; //клавиатура не подкл, подкл джойстик
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
    [SerializeField] private int lives = 5;
    [SerializeField] private int health;
    [SerializeField] private Image[] hearts;
    [SerializeField] private float jumpForce = 10f;

    [SerializeField] private Sprite aliveHeart;
    [SerializeField] private Sprite deathHeart;

    [SerializeField] GameObject dieMenu;
    [SerializeField] GameObject levelEnding;
    [SerializeField] GameObject pause;

    public GameObject Damage;
    public GameObject damageKick;
    public GameObject flyingKick;

    public Transform PunchPosObject;  // объект для удара рукой
    public float PunchRange;
    public Transform KickPosObject; // объект для удара ногой
    public float KickRange;
    public Transform FlyKickPosObject; // объект для удара ногой
    public float FlyKickRange;
    public LayerMask enemy; // маска врага
    public int damage; // наносимый урон
    
    private bool isAttacking = false; // атакуем ли мы сейчас
    private bool isCrouching = false; // в присиде ли    

    private const float DOUBLE = 2f;
    private float lastClick;

    public Collider2D poseStand;
    public Collider2D poseCrouch;
    private int crouchCounter = 0;

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
        ChechGround();
        LevelEnding();
    }

    private void Update() {
        if ((isGrounded || isGroundedStatic) && !isAttacking ) {
            // Debug.Log("Есть апдейт");s
            //isAttacking = false;
            State = States.idle;
        }

        if (!keyboard) {
            if(!isAttacking && (joystick.Horizontal != 0) && !isCrouching)
                Run();    
            if(!isAttacking && (isGrounded || isGroundedStatic) && (joystick.Vertical > 0.3f))
                Jump();
            if((isGrounded || isGroundedStatic) && !isAttacking && (joystick.Vertical < -0.3f)) {
                //Debug.Log("Срабатывает");
                Down();    
            }
        } else {
            if(Input.GetButton("Horizontal") && !isCrouching)
                Run();          
            if(Input.GetButton("Vertical") && crouchCounter==0) {
                Crouch();
                // Debug.Log(crouchCounter);                
            } 
            if(Input.GetButtonUp("Vertical")) {
                crouchCounter = 1;
                // Debug.Log(crouchCounter);
                poseStand.enabled = true;
                poseCrouch.enabled = false;
                isCrouching = false;
                Invoke("Reset_crouch", 0.2f);
            }
            if (Input.GetButtonDown("Vertical") && crouchCounter==1)
                Down();
            if(Input.GetButtonDown("Jump")) {
                Jump();
            }
                
            // if(Input.GetButtonDown("Jump") && Input.GetButtonDown("Fire2")) {
            //     // Flying_kick();
            //     State = States.flying_kick;
            // }
            if ((isGrounded || isGroundedStatic) && Input.GetButtonDown("Fire1"))
                Punch();
            if ((isGrounded || isGroundedStatic) && Input.GetButtonDown("Fire2")) 
                Kick();
            if (Input.GetButtonDown("Cancel")) {
                pause.SetActive(true);
                Time.timeScale = 0;
            }   
        }

        // if (Input.GetButtonDown("Vertical") && (crouchCounter >= maxCrouchCounter)) {
        //     crouchCounter = 0;
        // }

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
        }
        else if (sr.flipX == false) {
            PunchPosObject.transform.position = new Vector2(rb.position.x + 0.234999f, rb.position.y + 0.271f);
            KickPosObject.transform.position = new Vector2(rb.position.x + 0.181f, rb.position.y + 0.247f);
            FlyKickPosObject.transform.position = new Vector2(rb.position.x + 0.196f, rb.position.y + 0.245f);
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

    private void Crouch() {
        State = States.crouch;
        isCrouching = true;
        poseStand.enabled = false;
        poseCrouch.enabled = true;
    }

    private void Reset_crouch() {
        if (crouchCounter == 1) {
            crouchCounter = 0;
            // Debug.Log(crouchCounter);
        }
    }

    private void Jump() {
        if ((isGrounded || isGroundedStatic) && !isAttacking)
            rb.velocity = Vector2.up * jumpForce;

        Reset_crouch();


    }
    // x -0.025 ; y -0.3 ; rad 0.08

    //public bool Ground;
    // private void ChechGround() {
    //     Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position, 0.08f);
    //     isGrounded = collider.Length > 1;
    //     if(!isGrounded) 
    //         State = States.jump; 
    // }


    private void ChechGround() {
        isGrounded = Physics2D.OverlapCircle(groungCheck.position, 0.08f, Ground);
        isGroundedStatic = Physics2D.OverlapCircle(groungCheck.position, 0.08f, GroundStatic);
        if(!isGroundedStatic && !isGrounded) {
            State = States.jump;  
            // Flying_kick(); 
        } 
        if (!isGroundedStatic && !isGrounded && Input.GetButton("Fire2")) {
            // State = States.flying_kick;
            // Debug.Log("удар в прыжке");
            Flying_kick();   
        }
        // Flying_kickOff();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Spell" || collision.gameObject.tag == "Boss") {
            lives--;
            Debug.Log("Player lives = " + lives);
        }    
        if(collision.gameObject.tag == "gold") {
            Collect.score +=150;
        }
        if (lives < 1) {
                //Destroy(gameObject, 0.5f);
            Debug.Log("Вы мертвы");
            Collect.score = 0;
            SetPause();
        }
                
    }

    private void Awake() {
        dieMenu.SetActive(false);
        levelEnding.SetActive(false);
        Time.timeScale = 1;
    }

    public void SetPause() {
        dieMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void PauseOff() {
        dieMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void OpenLevelsList(int index) {
            PauseOff();
            if(index == 0)
                Collect.score = 0;
            SceneManager.LoadScene(index); 
    }


    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(PunchPosObject.position, PunchRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(KickPosObject.position, KickRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(FlyKickPosObject.position, FlyKickRange);
    }

    //Создаём Damage круг в зависимости от того, в какую сторону повёрнут персонаж
    public void Punch() {
        //Debug.Log("нажато");
        isAttacking = true;
        if(isAttacking) {
            State = States.punch;
            damage = 2;
            Collider2D[] enemies = Physics2D.OverlapCircleAll(PunchPosObject.position, PunchRange, enemy);
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

    public void Kick() {
        //Debug.Log("нажато");
        isAttacking = true;
        if(isAttacking) {
            State = States.kick;
            damage = 2;
            Collider2D[] enemies1 = Physics2D.OverlapCircleAll(KickPosObject.position, KickRange, enemy);
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

    //удар в прыжке работает не идеально
    //из-за того что объекты осязаемы персонаж затормаживается в воздухе
    //если они быстро не исчезают
    //+ невозможно заменить на capsuleCollider (для большей области поражения)
    public void Flying_kick() {
        //Debug.Log("нажато");
        isAttacking = true;
        if(isAttacking) {
            State = States.flying_kick;
            damage = 2;
            Collider2D[] enemies2 = Physics2D.OverlapCircleAll(FlyKickPosObject.position, FlyKickRange, enemy);
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

    private void LevelEnding() {
        if(!GameObject.Find("Angel") ) {
            NextLevel();
        } 
    }

    public void NextLevel() {
        levelEnding.SetActive(true);
        Time.timeScale = 0;
    }
}
