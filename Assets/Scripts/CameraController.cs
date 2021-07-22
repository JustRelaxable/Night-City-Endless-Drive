using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform camera;

    public Transform idle;
    public Transform maximumThrottle;
    public Transform brakePoint;

    public CarController carController;

    public float lastValue;
    public float cameraValue = 0;
    void Start()
    {
    }

    
    void Update()
    {
        
    }
    private void LateUpdate()
    {
        transform.position = carController.transform.position;
        transform.rotation = Quaternion.Lerp(transform.rotation, carController.transform.rotation * Quaternion.Euler(5 * new Vector3(0, 0, Input.acceleration.x)), Time.deltaTime*2);

        //transform.position = Vector3.Lerp(transform.position, carController.transform.position, Time.deltaTime*50);

        float now = (carController.carAcceleration + lastValue)/2;
        cameraValue += now * Time.deltaTime * 5 / 3.6f;
        lastValue = carController.carAcceleration;



        cameraValue = Mathf.Clamp(cameraValue, -1, 1);
        //Debug.Log(cameraValue+ " and " + carController.carAcceleration);
        if (cameraValue >= 0)
        {
            camera.transform.position = Vector3.Slerp(idle.position, maximumThrottle.position, cameraValue);
        }
        else
        {
            camera.transform.position = Vector3.Slerp(idle.position,brakePoint.position, -cameraValue);
            cameraValue += 0.3f * Time.deltaTime;
        }
       
        //Debug.Log(carController.GetComponent<Rigidbody>().velocity.magnitude / 200 * carController.pedulum);
        //transform.LookAt(carController.transform.position);
        //transform.position = Vector3.Slerp(transform.position, targetTransform.position, Time.deltaTime * 5);

    }
}
