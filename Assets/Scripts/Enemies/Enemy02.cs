using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Enemy02 : MonoBehaviour
{
    public MindStates current_mind_state_;
    public Transform playerPos;
    public float playerDisX;
    public float speed;
    PlayerController playerScript;
    public float timingAttack;
    public float startTimingAttack;
    private float limitUp;
    private float limitDown;
    private bool goingUp;
    private bool goingDown;
    private float xMovement;
    private float rSpeed;
     float radio;
    private Transform point;

    public enum MindStates
    {
        kwait,
        kAnnoying,
      

    }
    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindWithTag("player");
        playerPos = player.transform;
        playerScript = player.GetComponent<PlayerController>();
        startTimingAttack = timingAttack;

    }

    // Update is called once per frame
    void Update()
    {
        DistacePlayer();
        //Mindstates
        if (current_mind_state_ == MindStates.kwait)
        {
            MindWait();
        }
        else if (current_mind_state_ == MindStates.kAnnoying)
        {
            MindAnnoy();
        }
      
    }

    private void DistacePlayer()
    {
        playerDisX = gameObject.transform.position.x - playerPos.position.x;

    }

    private void MindWait()
    {
        BodyWait();
    }

    private void BodyWait()
    {
       
    }

    private void MindAnnoy()
    {
        BodyAnnoy();
    }

    private void BodyAnnoy()
    {


        transform.Translate(Vector3.left * Time.deltaTime * speed);
        xMovement += Time.deltaTime * speed;
        float angle = Time.time * rSpeed;
        // Calcula las coordenadas X, Y, Z utilizando funciones trigonométricas
        float x = Mathf.Sin(angle) * radio - xMovement;
        float z = 0f; // Mantiene una órbita en el mismo plano horizontal
        float y = Mathf.Cos(angle) * radio;

        // Establece la posición relativa al objeto central
        transform.position = point.position + new Vector3(x, y, z);


    }

    private void GoingUp()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);

    }

    private void GoingDown()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);
    }
}
