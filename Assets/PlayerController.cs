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

    // public Transform attackPoint;
    // public float attackRange = 0.5f;
    // public LayerMask enemyLayers;

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
            // Debug.Log("Есть апдейт");
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

    //Создаём Damage круг в зависимости от того, в какую сторону повёрнут персонаж
    public void Punch() {
        //Debug.Log("нажато");
        isAttacking = true;
        if(isAttacking) {
            State = States.punch;
        
            if (sr.flipX == true) {
                Damage.GetComponent<SpriteRenderer>().flipX = true;
                Instantiate(Damage, new Vector2(transform.position.x - 0.3f, transform.position.y + 0.27f), Quaternion.identity);
            } 
            else if (sr.flipX == false) {
                Damage.GetComponent<SpriteRenderer>().flipX = false;
                Instantiate(Damage, new Vector2(transform.position.x + 0.3f, transform.position.y + 0.27f), Quaternion.identity);
            }
        }
        Invoke("PunchOff", 0.4f);
    }

    private void PunchOff() {
        isAttacking = false;
    }

    public void Kick() {
        //Debug.Log("нажато");
        isAttacking = true;
        if(isAttacking) {
            State = States.kick;
        
            if (sr.flipX == true) {
                damageKick.GetComponent<SpriteRenderer>().flipX = true;
                Instantiate(damageKick, new Vector2(transform.position.x - 0.304f, transform.position.y + 0.263f), Quaternion.identity);
            } 
            else if (sr.flipX == false) {
                damageKick.GetComponent<SpriteRenderer>().flipX = false;
                Instantiate(damageKick, new Vector2(transform.position.x + 0.304f, transform.position.y + 0.263f), Quaternion.identity);
            }
        }
        Invoke("KickOff", 0.5f);
    }

    private void KickOff() {
        isAttacking = false;
    }

    // void Attack() {
    //     Collider2D[] flyKick = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

    // }

    // void OnDrawGizmosSelected() {
    //     if (attackPoint == null)
    //         return;

    //     Gizmos.DrawWireSphere(attackPoint.position, attackRange);    
    // }
   
    //удар в прыжке работает не идеально
    //из-за того что объекты осязаемы персонаж затормаживается в воздухе
    //если они быстро не исчезают
    //+ невозможно заменить на capsuleCollider (для большей области поражения)
    public void Flying_kick() {
        //Debug.Log("нажато");
        isAttacking = true;
        if(isAttacking) {
            State = States.flying_kick;
            if (sr.flipX == true) {
                flyingKick.GetComponent<SpriteRenderer>().flipX = true;
                Instantiate(flyingKick, new Vector2(transform.position.x - 0.26013361f, transform.position.y + 0.220838f), Quaternion.identity);
            } else if (sr.flipX == false) {
                flyingKick.GetComponent<SpriteRenderer>().flipX = false;
                Instantiate(flyingKick, new Vector2(transform.position.x + 0.26013361f, transform.position.y + 0.220838f), Quaternion.identity);
                // Destroy(flyingKick, 0.15f);
            }
            // -0.8656384 1.713973
            // -0.6478877 1.604169
            // -0.433  1.828
            // -0,214887 -0,223831

            //-0.9175339 1.827993
            //-0.963     1.825
            // -0,04524661   0,002993
            
        }
        Invoke("Flying_kickOff", 0.2f);
    }



    private void Flying_kickOff() {
        isAttacking = false;
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
