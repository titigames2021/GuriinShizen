using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public AudioSource sources;
    public AudioClip shootclip;
    public AudioClip itemclip;

    //Mov
    private InputController input;
    private Vector2 currentInputVector;
    public float smoothInputSpeed;
    public float playerSpeed;
    public SpriteRenderer playerSprite;
    private bool j_input;

    //Jump
    public float jumpForce = 5f;
    public float jumpTime = 0.25f;
    public float jumpGraceTime = 0.1f;
    public float coyoteTime = 0.1f;

    [SerializeField] LayerMask floorLayer;
    float tiempoEnElAire;
    public Transform raycastL;
    public Transform raycastR;
    private Rigidbody2D rb;
   

    public float raycastlengt;

    private Collider2D playercoll;

    public float triggerTime;


    //Shooting

    private GameObject obj;
    public ObjectPoolerScript basicPool;
    public float ProjectailSpeed;
    public Transform[] throwpoint;
    private bool s_input;
    private bool canShoot;
    public float life;
    
    public float lifeMax;
    public GameObject energyBar;
    public bool canJum;
    public Image[] itemUI; //de izq a derecha del mapa 

    public SpriteRenderer[] newitemUI;
    public float rayCastL;
    public bool isGrounded;

    public Sprite spriteup;

    private float coyoteTimeCounter;




    [SerializeField] private float groundRaycastLength = 0.2f;
    private bool lookUp;
    private bool lookRL;


    
    private Animator animator;
    private bool diagonal;
    private bool oneside;
    private bool item1obtained;
    private bool item2obtained;

    private void Awake()
    {
        animator= GetComponent<Animator>();
        playercoll = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        input = new InputController();

        input.General.Jump.performed += j_performed =>
        {
            if (canJum)
            {
               
                j_input = j_performed.ReadValueAsButton();
            }
               

            

        };



        input.General.Jump.canceled += j_performed =>
        {
            canJum= true;
            j_input = j_performed.ReadValueAsButton();



        };

        input.General.Shoot.performed += s_performed =>
        {
            if (canShoot)
            {
                s_input = s_performed.ReadValueAsButton();
            }

           


        };



        input.General.Shoot.canceled+= s_performed =>
        {

            s_input = s_performed.ReadValueAsButton();

            canShoot = true;

        };
    }
    // Start is called before the first frame update
    void Start()
    {
        canJum = true;
        canShoot = true;
        playerSprite = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        if (life <= 0.0f)
        {
            SceneManager.LoadScene(2);
        }

        if(gameObject.transform.position.y<= -4.159f)
        {
            life--;
        }


        animator.SetBool("up", lookUp);
        animator.SetBool("side", lookRL);
        animator.SetBool("grounded", isGrounded);

        //Mov 
        Vector2 move_input = input.General.Mov.ReadValue<Vector2>();
        currentInputVector = Vector2.Lerp(currentInputVector, move_input, smoothInputSpeed);
        Vector3 move = new Vector3(currentInputVector.x,0, 0);
        transform.Translate(move * playerSpeed * Time.deltaTime);
        Debug.Log(move_input);
        if (move_input.x > 0.0f)
        {
            oneside = true;
            playerSprite.flipX = true;
        }
        else if(move_input.x<0.0f) { oneside = true;  playerSprite.flipX = false; }
        else
        {
            oneside = false;
        }

        float healthRatio = life / lifeMax;
        healthRatio = Mathf.Clamp(healthRatio, 0f, 1f); // Asegurarse de que la escala esté entre 0 y 1.

        energyBar.transform.localScale = new Vector3(healthRatio * 3f, 1f, 1f); // Establecer la escala solo en el eje X.

        if(move_input.y>0.0f)
        {
            Debug.Log("LookUp");

            lookUp = true;
        }
        else
        {
            lookUp = false;
        }

        if(move_input.x>0.0f || move_input.x < 0.0f)
        {
            lookRL = true;
           
        }
        else
        {
            lookRL = false;
        }

        if(lookRL && lookUp)
        {
            diagonal = true;
        }
        else
        {
            diagonal = false;

        }






        /*
        //Jump

        RaycastHit2D raycastFloor = Physics2D.Raycast(raycastL.position, Vector2.down, raycastlengt, floorLayer);
        //RaycastHit2D raycastFloorL = Physics2D.Raycast(raycastL.position, Vector2.down, 0.25f, floorLayer);
        //RaycastHit2D raycastFloorR = Physics2D.Raycast(raycastR.position, Vector2.down, 0.25f, floorLayer);
        Debug.DrawLine(raycastL.position, raycastFloor.point, Color.blue);
        //Debug.DrawLine(raycastL.position, raycastFloorL.point, Color.blue);
        //Debug.DrawLine(raycastR.position, raycastFloorR.point, Color.blue);
       
        if (j_input && canJum)
        {
            canJum=false;
           
            if (raycastFloor)// si toca suelo 
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                tiempoEnElAire = 0.0f;
            }
            else
            {
                tiempoEnElAire += Time.deltaTime;
                //Comprobar tiempo, si es menor de 0.25 salta 
                if (tiempoEnElAire < coyoteTime)
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                }
            }
           

        }
        */

        //newJump


        // Detect ground using a raycast
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundRaycastLength, floorLayer);

        // Jump input detection
        // Coyote time countdown
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // Jump input detection
        if (canJum)
        {
            if ((isGrounded || coyoteTimeCounter > 0f) && j_input)
            {
                canJum = false;
                animator.SetTrigger("jump");
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                coyoteTimeCounter = 0f;
            }
        }
       





        //SHOOT

        if (s_input&&canShoot)
        {
            canShoot = false;
            obj = basicPool.GetPooledObject();
            if (obj == null) return;
            sources.PlayOneShot(shootclip);
            if (playerSprite.flipX)
            {
                if (diagonal)
                {
                    obj.transform.position = throwpoint[2].position;
                    obj.transform.rotation = throwpoint[2].rotation;
                }
                else
                {
                    obj.transform.position = throwpoint[0].position;
                    obj.transform.rotation = throwpoint[0].rotation;
                }


                if(lookUp & diagonal==false)
                {
                    obj.transform.position = throwpoint[4].position;
                    obj.transform.rotation = throwpoint[4].rotation;

                }
                
            }
            else
            {
                if (diagonal)
                {
                    obj.transform.position = throwpoint[3].position;
                    obj.transform.rotation = throwpoint[3].rotation;
                }
                else
                {
                    obj.transform.position = throwpoint[1].position;
                    obj.transform.rotation = throwpoint[1].rotation;
                }



                if (lookUp&& diagonal==false)
                {
                    obj.transform.position = throwpoint[5].position;
                    obj.transform.rotation = throwpoint[5].rotation;
                }
                
            }
           
            obj.SetActive(true);

            Rigidbody2D objRb = obj.GetComponent<Rigidbody2D>();
            objRb.AddForce(obj.gameObject.transform.right * ProjectailSpeed, ForceMode2D.Impulse);

        }





     




    }
   

 
  
    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    private void OnTriggerExit(Collider other)
    {
        other.isTrigger = false;
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        
       


        switch (collision.gameObject.tag)
        {

            case "item1":

                Debug.Log("item1");
                collision.gameObject.SetActive(false);
                sources.PlayOneShot(itemclip);


                Color color0 = itemUI[0].color;
                color0.a = 1f;
                itemUI[0].color = color0;
                item1obtained = true;
                break;

            case "item2":

                Debug.Log("item2");
                sources.PlayOneShot(itemclip);

                collision.gameObject.SetActive(false);
                Color color1 = itemUI[1].color;
                color1.a = 1f;
                itemUI[1].color = color1;

                break;


            case "item3":
                sources.PlayOneShot(itemclip);

                Debug.Log("item3");
                collision.gameObject.SetActive(false);

                Color color2= itemUI[2].color;
                color2.a = 1f;
                itemUI[2].color = color2;
                item2obtained= true;
                break;


            case "enemyp":
                life--;
                break;
        }






    }




    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "nave")
        {
            if (item1obtained & item2obtained)
            {
                SceneManager.LoadScene(3);
            }
        }


    }
}
