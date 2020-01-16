using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : MonoBehaviour
{
    [SerializeField] GameObject virtualPoint = null;
    [SerializeField] GameObject fireParticles = null;




    Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        fireParticles.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Thrust(bool isThrusting)
    {
        fireParticles.SetActive(isThrusting);
    }


}
