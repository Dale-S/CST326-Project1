using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Ball : MonoBehaviour
{
    //Default values to reset after scaling
    //Everything is based on these, so if you want to change stuff change these.
    private int defSize = 50;
    private int defCam = 24;
    private int defP1 = -22;
    private int defP2 = 22;
    private float defG1 = -23.5f;
    private float defG2 = 23.5f;
    private float defH = -7.4f;
    private float defV = 0.0f;
    private float defInc = 1.2f;
    //Max speed to avoid collision detection
    private float MAXSPEED = 50.0f;

    //Scaling values
    private int size;
    private int camHeight;
    private int p1X;
    private int p2X;
    private float g1X;
    private float g2X;
    
    //Ball movement values
    private float drag = 0.000f;
    private float increment;
    private float hSpeed;
    private float vSpeed;
    
    //Game info tracking
    private int rally = 0;
    private int p1Score = 0;
    private int p2Score = 0;
    
    //Game objects 
    private GameObject camera;
    private GameObject ball;
    private GameObject p1;
    private GameObject p2;
    private GameObject g1;
    private GameObject g2;
    private GameObject wall1;
    private GameObject wall2;
    private GameObject back;
    private Rigidbody rbBall;
    
    //Sounds
    private AudioSource paddleSound;
    private AudioSource w1Sound;
    private AudioSource w2Sound;
    private AudioSource g1Sound;
    private AudioSource g2Sound;
    
    // Start is called before the first frame update
    void Start()
    {
        //Initializing scale values
        size = defSize;
        camHeight = defCam;
        p1X = defP1;
        p2X = defP2;
        g1X = defG1;
        g2X = defG2;
        
        //Initializing movement values
        increment = defInc;
        hSpeed = defH;
        vSpeed = defV;
        
        //Initializing game objects
        ball = GameObject.Find("Ball");
        p1 = GameObject.Find("Paddle1");
        p2 = GameObject.Find("Paddle2");
        g1 = GameObject.Find("Goal1");
        g2 = GameObject.Find("Goal2");
        wall1 = GameObject.Find("Top");
        wall2 = GameObject.Find("Bottom");
        back = GameObject.Find("Background");
        camera = GameObject.Find("Main Camera");
        
        //Getting ball rigid body
        rbBall = ball.GetComponent<Rigidbody>();
        
        //Initialize sounds
        paddleSound = ball.GetComponent<AudioSource>();
        w1Sound = wall1.GetComponent<AudioSource>();
        w2Sound = wall2.GetComponent<AudioSource>();
        g1Sound = g1.GetComponent<AudioSource>();
        g2Sound = g2.GetComponent<AudioSource>();
    }
    
    

    // Update is called once per frame

    void OnCollisionEnter(Collision collide)
    {
        if (collide.gameObject.CompareTag("Paddle"))
        {
            //Play paddle sound 
            paddleSound.Play();
            
            //Debug.Log("Curr Speed = " + hSpeed);
            //Ball direction decision code
            float currPos = ball.transform.position.z - collide.transform.position.z;
            //Debug.Log("currPos = " + currPos);
            if (currPos >= 0.6f)
            {
                vSpeed = 3.8f;
            }
            else if (currPos <= -0.6)
            {
                vSpeed = -3.8f;
            }
            else
            {
                vSpeed = 0.0f;
            }

            //Reverse direction when hitting paddle
            hSpeed = hSpeed * -1.0f;
            if (hSpeed <= MAXSPEED && hSpeed >= -MAXSPEED)
            {
                hSpeed += increment;
            }

            //Scaling if rally is greater than specified value
            if (rally >= 10)
            {
                if (increment < 0)
                {
                    increment -= 5.5f;
                }
                else if (increment > 0)
                {
                    increment += 5.5f;
                }

                if (hSpeed <= MAXSPEED && hSpeed >= -MAXSPEED)
                {
                    size += 20;
                    camHeight += 9;
                    p1X -= 9;
                    p2X += 9;
                    g1X -= 9;
                    g2X += 9;
                    wall1.transform.localScale = new Vector3(size, 1, 1);
                    wall2.transform.localScale = new Vector3(size, 1, 1);
                    back.transform.localScale = new Vector3(size / 10, 1, 2.2f);
                    camera.transform.position = new Vector3(0, camHeight, 0);
                    g1.transform.position = new Vector3(g1X, 0, 0);
                    g2.transform.position = new Vector3(g2X, 0, 0);
                    p1.transform.position = new Vector3(p1X, 0, p1.transform.position.z);
                    p2.transform.position = new Vector3(p2X, 0, p2.transform.position.z);
                }
            }
            //Plug in new velocity
            rbBall.velocity = new Vector3 (hSpeed, 0.0f, vSpeed);
            
            //Flip increment value so it works properly depending on the side
            increment = increment * -1.0f;
            
            //Increase rally
            rally++;
            //Debug.Log("Rally = " + rally);
        }
        
        //Rebounds off of walls
        if (collide.gameObject.CompareTag("Wall"))
        {
            vSpeed = vSpeed * -1.0f;
            if (ball.transform.position.z > 0)
            {
                w1Sound.Play();
            }
            else
            {
                w2Sound.Play();
            }
        }
        
        //Goal check and reset board
        if (collide.gameObject.CompareTag("Goal"))
        {
            hSpeed = defH;
            vSpeed = defV;
            increment = defInc;
            if (ball.transform.position.x > 0)
            {
                hSpeed = hSpeed * -1.0f;
                increment = increment * -1.0f;
                p1Score += 1;
                Debug.Log("P1 scored, current score is P1: " + p1Score + " | P2: " + p2Score);
                g1Sound.Play();
            }
            else
            {
                p2Score += 1;
                Debug.Log("P2 scored, current score is P1: " + p1Score + " | P2: " + p2Score);
                g2Sound.Play();
            }
            //Reset board
            wall1.transform.localScale = new Vector3(defSize, 1, 1);
            wall2.transform.localScale = new Vector3(defSize, 1, 1);
            back.transform.localScale = new Vector3(defSize / 10, 1, 2.2f);
            camera.transform.position = new Vector3(0, defCam, 0);
            g1.transform.position = new Vector3(defG1, 0, 0);
            g2.transform.position = new Vector3(defG2, 0, 0);
            p1.transform.position = new Vector3(defP1, 0, 0);
            p2.transform.position = new Vector3(defP2, 0, 0);
            ball.transform.position = new Vector3(0, 0, 0);
            size = defSize;
            camHeight = defCam;
            p1X = defP1;
            p2X = defP2;
            g1X = defG1;
            g2X = defG2;
            rally = 0;
            
            //Check win conditions
            if (p1Score >= 11)
            {
                Debug.Log("Game Over, Left Paddle Wins");
                p1Score = 0;
                p2Score = 0;
            }

            if (p2Score >= 11)
            {
                Debug.Log("Game Over, Right Paddle Wins");
                p1Score = 0;
                p2Score = 0;
            }
        }
    }
    void FixedUpdate()
    {
        rbBall.velocity = new Vector3(hSpeed, 0.0f, vSpeed);
        hSpeed = hSpeed - drag;
    }
}
