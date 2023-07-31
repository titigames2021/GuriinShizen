using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy01 : MonoBehaviour
{
    //States 
    public MindStates current_mind_state_;
    public Transform playerPos;
    public float playerDisX;
    public float speed;
    PlayerController playerScript;
    public float timingAttack;
    public float startTimingAttack;
    public float life;
    private SpriteRenderer sprite;

    public float visionRange;
    public float attackRange;




    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;




    public enum MindStates
    {
        kpatrol,
        kAttack01,
        kAttack02,

    }
    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindWithTag("player");
        playerPos = player.transform;
        playerScript = player.GetComponent<PlayerController>();
        startTimingAttack = timingAttack;

        sprite = gameObject.GetComponent<SpriteRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        float clampedY = Mathf.Clamp(transform.position.y, minY, maxY);
        transform.position = new Vector3(clampedX, clampedY, 0.0f);
        if (life <= 0.0f)
        {
            gameObject.SetActive(false);
        }
        DistacePlayer();
        //Mindstates
        if (current_mind_state_ == MindStates.kpatrol)
        {
            MindPatrol();
        }
        else if (current_mind_state_ == MindStates.kAttack01)
        {
            MindAttack01();
        }
        else if (current_mind_state_ == MindStates.kAttack02)
        {
            MindAttack02();
        }


        if (playerPos.position.x <= transform.position.x)
        {
            sprite.flipX = false;
        }
        else
        {
           
            sprite.flipX = true;
        }
    }

    private void MindAttack02()
    {
        BodyAttack02();
        if (Mathf.Abs(playerDisX) >= attackRange)
        {
            current_mind_state_ = MindStates.kAttack01;
        }
    }

    private void BodyAttack02()
    {
        Debug.Log("Attack");

        timingAttack -= Time.deltaTime;
        if (timingAttack <= 0.0f)
        {
            timingAttack = startTimingAttack;

            playerScript.life--;

        }

       
    }

    

    protected void DistacePlayer()
    {
        playerDisX = gameObject.transform.position.x - playerPos.position.x;
    }

    private void MindAttack01()
    {
        BodyAttack01();
        if (Mathf.Abs(playerDisX) >= visionRange)
        {
            current_mind_state_ = MindStates.kpatrol;
        }

        if (Mathf.Abs(playerDisX) <=attackRange)
        {
            current_mind_state_ = MindStates.kAttack02;
        }
    }

    private void BodyAttack01()
    {
        Debug.Log("Pursuit");

        // Calcula la dirección hacia el objetivo
        Vector3 direction = playerPos.position - transform.position;

        // Normaliza la dirección para que la velocidad no dependa de la distancia
        direction.Normalize();

        // Mueve "objetoA" hacia el objetivo usando MoveTowards
        transform.position = Vector3.MoveTowards(transform.position, playerPos.position, speed * Time.deltaTime);

    }

    private void MindPatrol()
    {
        BodyPatrol01();
        if (Mathf.Abs(playerDisX) <= visionRange)
        {
            current_mind_state_ = MindStates.kAttack01;
        }
    }

    private void BodyPatrol01()
    {
        //Debug.Log("PATROLL");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "playerp")
        {
            life--;
        }
    }
}
