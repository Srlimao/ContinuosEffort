using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GeneralShip : MonoBehaviour
{
    enum State { Alive, Dying, Transcending, Win };

    #region Parametros Serializados
    [SerializeField] float trustSpeed = 1f;
    [SerializeField] float turnSpeed = 1f;
    [SerializeField] GameObject prefabParticulaExplosao = null;
    [SerializeField] GameObject prefabParticulaWin = null;
    [SerializeField] ParticleSystem prefabParticulaFire = null;
    [SerializeField] GameObject prefabLightFire = null;
    [SerializeField] AudioClip[] audios = null;
    [SerializeField] bool godMode = false;
    #endregion

    #region Private Stuff
    Rigidbody rb;
    AudioSource audioSource;
    State state = State.Alive;
    bool isThrusting = false;
    float rotatingForce = 0f;
    float thrustingForce = 0f;
    #endregion


    void Start()
    {
        #region GetComponents
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        #endregion

        prefabLightFire.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsAlive())
        {
            Thrust();
            Rotate();
        }
        
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

    #region InputProcess

    public void OnThrust(InputAction.CallbackContext ctx)
    {
        thrustingForce = ctx.ReadValue<float>();
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        rotatingForce = ctx.ReadValue<Vector2>().x;
    }

    private void Thrust()
    {
        isThrusting = (thrustingForce > 0f);
        if (IsAlive() && isThrusting)
        {
            prefabParticulaFire.Play();
            prefabLightFire.SetActive(true);
            rb.AddRelativeForce(new Vector3(0f, thrustingForce*trustSpeed * Time.deltaTime));

            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(audios[0]);
            }
        }else
        {
            audioSource.Stop();
            prefabParticulaFire.Stop();
            prefabLightFire.SetActive(false);
        }
    }

    private void Rotate()
    {
        RigidbodyConstraints rbc = rb.constraints;//Salva constraint anterior
        rb.freezeRotation = true; //Trava Rotacao para manual
        transform.Rotate(new Vector3(rotatingForce * turnSpeed * Time.deltaTime, 0f, 0f)); //adiciona rotacao conforme botao pressionado
        rb.constraints = rbc;//retorna constraint anterior
    }
    #endregion


    #region Stats GetterSetter

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
    #endregion
}
