using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscilator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector = new Vector3();
    [SerializeField] float periodo = 2f;
    


    Vector3 startingPos = new Vector3();
    private float movementFactor = 0f;

    void Start()
    {
        startingPos = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        ExcecuteOscilation();
    }

    private void ExcecuteOscilation()
    {

        if (periodo <= Mathf.Epsilon) { return; }
        float cycles = Time.time / periodo;

        const float tau = Mathf.PI * 2;

        float rawSinWave = Mathf.Sin(cycles * tau);

        movementFactor = (rawSinWave / 2f) + 0.5f;

        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPos + offset;
    }
}
