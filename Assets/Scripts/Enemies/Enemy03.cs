using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy03 : MonoBehaviour
{
    public float limitUp;
    public float limitDown;
    public float limitL;
    public float limitR;
    private bool goingDown;
    private bool goingUp;
    public float speed;
    private bool goingLeft;
    private bool goingRigth;
    private SpriteRenderer sprite;
    public float life;

    // Start is called before the first frame update
    void Start()
    {
        goingDown = true;
        goingLeft= true;
        sprite=gameObject.GetComponent<SpriteRenderer>();
        ///Physics2D.IgnoreLayerCollision(gameObject.layer, 3);
    }

    // Update is called once per frame
    void Update()
    {
        if (life <= 0.0f)
        {
            gameObject.SetActive(false);
        }
        //Debug.Log(gameObject.transform.position.y);
        //Movement Y
        if (gameObject.transform.position.y >= limitUp)
        {
            goingUp = false;
            goingDown = true;
        }


        if (gameObject.transform.position.y <= limitDown)
        {

            goingDown = false;
            goingUp = true;
        }

        if (goingDown)
        {
            GoingDown();
        }

        if (goingUp)
        {
            GoingUp();
        }

        if (goingLeft)
        {
            GoingLeft();
        }

        if (goingRigth)
        {
            GoingRigth();
        }

        if (gameObject.transform.position.x>=limitR)
        {
            sprite.flipX = false;
            goingLeft = true;
            goingRigth = false;
        }

        if (gameObject.transform.position.x <= limitL)
        {
            sprite.flipX = true;
            goingLeft = false;
            goingRigth = true;
        }
        
       
    }

    private void GoingRigth()
    {
        gameObject.transform.Translate(Vector2.right * speed * Time.deltaTime);

    }

    private void GoingLeft()
    {

        gameObject.transform.Translate(Vector2.left * speed * Time.deltaTime);
    }

    private void GoingDown()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);
    }

    private void GoingUp()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "playerp")
        {
            life--;
        }
    }



}
