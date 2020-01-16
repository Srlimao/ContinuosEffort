using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterControl : MonoBehaviour
{

    [SerializeField] Booster starboardBooster = null;
    [SerializeField] Booster portBooster = null;
    // Start is called before the first frame update


    Booster starBoard, port;
    void Start()
    {
        starBoard = starboardBooster.GetComponent<Booster>();
        port = portBooster.GetComponent<Booster>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Thrust(bool isThrusting)
    {
        starBoard.Thrust(isThrusting);
        port.Thrust(isThrusting);
    }
}
