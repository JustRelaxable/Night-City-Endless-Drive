using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchScreenManager : MonoBehaviour
{
    public CarController car;
    public Text text;
    void Start()
    {
    }

    
    void Update()
    {
        car.horizontalInput = Input.acceleration.x;
    }

    public void GasTouched()
    {
        car.verticalInput = 1;
    }
    public void ReverseTouched()
    {
        car.verticalInput = -1;
    }
    public void ResetTouches()
    {
        car.verticalInput = 0;
    }

    public void BrakeTouched()
    {
        car.Brake();
    }
    public void BrakeRelease()
    {
        car.BrakeRelease();
    }
}
