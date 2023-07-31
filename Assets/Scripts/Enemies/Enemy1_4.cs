using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.Windows;

public class Enemy1_4 : MonoBehaviour
{
    public enum MindStates
    {
        kWait,
        kAttack01,
        kAttack02,
        kreturns,

    }
    public Transform point;
    public MindStates current_mind_state_;

    public float rSpeed;
    public float radio;
    private float xMovement;
    public float rSpeed2;
    public float radio2;
    private float xMovement2;

    public Vector3 playerV;
    public Transform playerPos;
    PlayerController playerScript;
    public float timingAttack;
    public float startTimingAttack;
    public float life;
    public float speed;
    public Vector3 absoluteVector;
    public float totalDistance;
    public float totalDistanceOrigen;
    public Transform throwpoint;
    public Vector3 sumV = new Vector3(0.0f, 12.0f, 0.0f);
    private bool canShoot;
    private GameObject obj;

    public ObjectPoolerScript basicPool;
    public float ProjectailSpeed;
   public float coolDownTime;
    private object maxSpeed;
    private object minSpeed;




    private float direction;

    private Rigidbody2D rbp;
    public float randomHorizontalSpeed;
    public float randomHorizontalAmplitude;

    public Transform spawn;
    public Transform ground;
    private Animator animator;
    private bool isAttacking;
    private bool isIdle;
    public AudioSource sources;
    public AudioClip dinoclip;
    // Start is called before the first frame update
    void Start()
    {
        canShoot = true;

        GameObject player = GameObject.FindWithTag("player");
        playerPos = player.transform;
        playerScript = player.GetComponent<PlayerController>();
        startTimingAttack = timingAttack;

         rbp = point.GetComponent<Rigidbody2D>();
        //startPos = transform.position;
        Physics2D.IgnoreLayerCollision(gameObject.layer, 8);
        Physics2D.IgnoreLayerCollision(gameObject.layer, 3);
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("attack", isAttacking);
      
        animator.SetBool("idle", isIdle);




        if (life <= 0.0f)
        {
            gameObject.SetActive(false);
        }
        if (current_mind_state_ == MindStates.kWait)
        {
            MindWait();
        }
        else if (current_mind_state_ == MindStates.kAttack01)
        {
            MindAttack01();
        }
        else if (current_mind_state_ == MindStates.kAttack02)
        {
            MindAttack02();
        }
        else if (current_mind_state_ == MindStates.kreturns)
        {
            MindReturns();
        }


        DistacePlayer();

        DistaceOrigen();






    }

    private void MindReturns()
    {
        BodyReturns();
        if (transform.position.y >= spawn.position.y)
        {
            current_mind_state_ = MindStates.kWait;

        }
    }

    private void BodyReturns()
    {
        transform.position = Vector3.MoveTowards(transform.position, spawn.position, speed * Time.deltaTime);

    }

    private void MindAttack02()
    {
        BodyAttack02();

        

    }

    private void BodyAttack02()
    {
        //CIRCLES
        float angle = Time.time * rSpeed;
        // Calcula las coordenadas X, Y, Z utilizando funciones trigonométricas
        float x = Mathf.Sin(angle) * radio - xMovement;
        float z = 0f; // Mantiene una órbita en el mismo plano horizontal
        float y = Mathf.Cos(angle) * radio;
        

        // Establece la posición relativa al objeto central
        transform.position = point.position + new Vector3(x, y, z);




        Vector3 direction =playerPos.position - throwpoint.position;
        direction.z = 0f;
        throwpoint.transform.right = direction + sumV;



        // Calcula el ángulo de rotación basado en el tiempo y la velocidad de rotación
        float angle2 = Time.time * rSpeed2;

        // Genera un valor aleatorio que cambia a lo largo del tiempo
        float randomValue = Mathf.PerlinNoise(Time.time * randomHorizontalSpeed, 0f) * 2f - 1f; // Genera un valor entre -1 y 1 usando PerlinNoise

        // Aplica el movimiento horizontal aleatorio a las coordenadas X y Z
        float x2 = (Mathf.Sin(angle2) + randomValue * randomHorizontalAmplitude) * radio2 - xMovement2;
        float z2 = (Mathf.Cos(angle2) + randomValue * randomHorizontalAmplitude) * radio2;

        // Mantén una órbita en el mismo plano horizontal, sin cambio en la coordenada Z
        float y2 = Mathf.Cos(angle2) * radio2;
      
        // Establece la posición relativa al objeto central
        point.transform.position = playerPos.position + new Vector3(x2, y2, z2);


        






        if (canShoot)
        {
            isAttacking = true;
            isIdle = false;
            canShoot = false;
            obj = basicPool.GetPooledObject();
            if (obj == null) return;

           
            
                obj.transform.position = throwpoint.position;
                obj.transform.rotation = throwpoint.rotation;

            
            sources.PlayOneShot(dinoclip);

            obj.SetActive(true);

            Rigidbody2D objRb = obj.GetComponent<Rigidbody2D>();
            objRb.AddForce(obj.gameObject.transform.right * ProjectailSpeed, ForceMode2D.Impulse);
            Invoke("ResetCooldown", coolDownTime);

        }
        




    }

    private void MindAttack01()
    {
        BodyAttack01();

        if (totalDistance <= 0.3f)
        {
            current_mind_state_ = MindStates.kAttack02;
        }
    }

    private void BodyAttack01()
    {
       ///Calcula la dirección hacia el objetivo
        Vector3 direction = playerPos.position - transform.position;

        // Normaliza la dirección para que la velocidad no dependa de la distancia
        direction.Normalize();

        // Mueve "objetoA" hacia el objetivo usando MoveTowards
        transform.position = Vector3.MoveTowards(transform.position, playerPos.position, speed * Time.deltaTime);
    }

    private void MindWait()
    {
        BodyWait();
        timingAttack -= Time.deltaTime;
       
          

        if (totalDistance<=2.2f && timingAttack<=0.0f)
        {
            timingAttack = startTimingAttack;
            current_mind_state_ = MindStates.kAttack01;

        }
    }

    private void BodyWait()
    {
    }

    protected void DistacePlayer()
    {
       
       
         totalDistance = Vector2.Distance(transform.position, playerPos.position);
    }
    protected void DistaceOrigen()
    {


        totalDistanceOrigen = Vector2.Distance(transform.position,spawn.position);
    }



    private void ResetCooldown()
    {
        // Reiniciar el indicador de si se puede lanzar un objeto
        canShoot = true;
        isAttacking = false;
        isIdle = true;

    }




    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "playerp")
        {
            life--;
        }
    }

}
