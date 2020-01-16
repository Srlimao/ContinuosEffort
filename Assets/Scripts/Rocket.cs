using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    #region Parametros Serializados
    [SerializeField] float trustSpeed = 1f;
    [SerializeField] float turnSpeed = 1f;
    [SerializeField] GameObject prefabParticulaExplosao = null;
    [SerializeField] GameObject prefabParticulaWin = null;
    [SerializeField] AudioClip[] audios = null;
    [SerializeField] bool godMode = false;
    #endregion


    enum State { Alive, Dying, Transcending, Win};





    //Private Stuff
    Rigidbody rb;    
    AudioSource audioSource;
    State state = State.Alive;
    BoosterControl boosterControl;

    bool isTrusting = false;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        audioSource=GetComponent<AudioSource>();
        boosterControl = GetComponent<BoosterControl>();

    }

    // Update is called once per frame
    void Update()
    {
        if (state==State.Alive)
        {
            Thrust();
            Rotate();
        }
        ProcessInput();
    }

   

    private void ProcessInput()
    {
        //rb.AddRelativeForce(new Vector3(0f, Input.GetAxis("Submit") * speed * Time.deltaTime));
        

        
    }

    private void Thrust()
    {
        isTrusting = Mathf.Abs(Input.GetAxis("Submit")) > Mathf.Epsilon;
        rb.AddRelativeForce(new Vector3(0f, Input.GetAxis("Submit") * trustSpeed * Time.deltaTime));

        if (isTrusting && state == State.Alive)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(audios[0]);
            }

            boosterControl.Thrust(true);

        }
        else
        {
            audioSource.Stop();
            boosterControl.Thrust(false);
        }



        /*
        starBoard.Trust(Input.GetAxis("Submit") * speed * Time.deltaTime);
        port.Trust(Input.GetAxis("Submit") * speed * Time.deltaTime);
        */
    }

    private void Rotate()
    {
        RigidbodyConstraints rbc = rb.constraints;//Salva constraint anterior
        rb.freezeRotation = true; //Trava Rotacao para manual
        transform.Rotate(new Vector3(Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime, 0f,0f)); //adiciona rotacao conforme botao pressionado
        rb.constraints = rbc;//retorna constraint anterior
    }

    private void OnCollisionEnter(Collision collision)
    {
        string tag = collision.transform.tag;
        if (state == State.Alive)
        {
            switch (tag)
            {
                case "CheckPoint":
                    Win(collision.contacts[0].point);
                    break;
                case "Friendly":
                    SetIsAlive(State.Alive);
                    break;
                default:
                    SetIsAlive(State.Dying);
                    Explode(collision.contacts[0].point);                    
                    break;
            }
        }
    }

    private void Explode(Vector3 hitPoint)
    {
        if (godMode)
        {
            Debug.Log("Death Hit");
        }
        else
        {
            rb.freezeRotation = false;
            rb.constraints = RigidbodyConstraints.None;
            rb.drag = 0;
            rb.AddExplosionForce(trustSpeed / 2, hitPoint, 10f);
            Instantiate(prefabParticulaExplosao, hitPoint, Quaternion.identity);
            audioSource.PlayOneShot(audios[2]);
        }
        
    }

    private void Win(Vector3 hitPoint)
    {
        Instantiate(prefabParticulaWin, hitPoint, Quaternion.identity);
        audioSource.PlayOneShot(audios[1]);
        state = State.Win;
    }

    private void SetIsAlive(State s)
    {
        if (godMode)
        {
            state = State.Alive;

        }
        else
        {
            state = s;
        }
        
    }

    public bool IsAlive()
    {
        return state == State.Alive;
    }

    public bool IsDying()
    {
        return state == State.Dying;
    }

    public bool IsTranscending()
    {
        return state == State.Transcending;
    }
    public bool IsWinning()
    {
        return state == State.Win;
    }

    public void SetTranscending()
    {
        state = State.Transcending;
    }

    public void SetAlive()
    {
        state = State.Alive;
    }

}
