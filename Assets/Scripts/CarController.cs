using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public int maximumWheelRotationAngle = 30;
    public int maximumMotorTorque = 100;
    public float pedulum;

    public int gearCount;
    public int currentGear = 0;

    public float[] gearRPMs;
    public float[] gearMaxSpeeds;

    public ParticleSystem[] particleSystems;

    public WheelCollider[] wheelColliders;
    public Transform[] wheelTransforms;
    [HideInInspector] public float verticalInput;
    [HideInInspector] public float horizontalInput;
    [HideInInspector] public float carSpeed;
    public float carAcceleration;
    public float RPMA;
    public float rpmacc;
    public float Revs;

    [HideInInspector] public float rearSidewaySlip;
    [HideInInspector] public float frontForwardSlip;

    private bool emissionActivated = false;
    private WheelFrictionCurve originalBackSideawayCurve;
    private WheelFrictionCurve modifiedBackSideawayCurve;

    Rigidbody carRigidbody;
    public Transform centerOfMass;



    private void Awake()
    {
        carRigidbody = GetComponent<Rigidbody>();
        carRigidbody.centerOfMass = centerOfMass.localPosition;
        originalBackSideawayCurve = wheelColliders[2].sidewaysFriction;
        modifiedBackSideawayCurve = wheelColliders[2].sidewaysFriction;
        modifiedBackSideawayCurve.stiffness = 1;
    }

    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        UpdateWheelTransforms();
        carAcceleration = carRigidbody.velocity.magnitude * 3.6f - carSpeed;
        carSpeed = carRigidbody.velocity.magnitude * 3.6f;


        WheelHit hitRear;
        WheelHit hitFront;
        wheelColliders[2].GetGroundHit(out hitRear);
        wheelColliders[0].GetGroundHit(out hitFront);

        rearSidewaySlip = Mathf.Abs(hitRear.sidewaysSlip);
        frontForwardSlip = Mathf.Abs(hitFront.forwardSlip);

        //Debug.Log(hitFront.forwardSlip);

        if (!emissionActivated && (rearSidewaySlip > 0.2f || frontForwardSlip > 0.6f))
        {
            emissionActivated = true;
            for (int i = 0; i < particleSystems.Length; i++)
            {
                particleSystems[i].enableEmission = true;
            }
        }
        else
        {
            emissionActivated = false;
            for (int i = 0; i < particleSystems.Length; i++)
            {
                particleSystems[i].enableEmission = false;
            }
        }

        //RPMA = wheelColliders[0].rpm - rpmacc;
        //rpmacc = wheelColliders[0].rpm;
        //Debug.Log(RPMA);
        //CalculatePedulum();

        //Debug.Log(currentGear);
        //maximumMotorTorque = (int)gearRPMs[currentGear];
        /*
        if (carRigidbody.velocity.magnitude >= gearMaxSpeeds[currentGear])
        {
            //currentGear += 1;
            StartCoroutine(ChangeGear());
            
        }
        else if(carRigidbody.velocity.magnitude >= 0 && carRigidbody.velocity.magnitude <= gearMaxSpeeds[1])
        {
            currentGear = 1;
        }
        else if(carRigidbody.velocity.magnitude <= gearMaxSpeeds[currentGear - 1])
        {
            currentGear -= 1;
        }
        */
        //Debug.Log(currentGear);
    }

    IEnumerator ChangeGear()
    {
        maximumMotorTorque = 0;
        yield return new WaitForSeconds(1f);
        if(carRigidbody.velocity.magnitude >= gearMaxSpeeds[currentGear])
        {
            currentGear += 1;
        }
        maximumMotorTorque = (int)gearRPMs[currentGear];
    }

    private void UpdateWheelTransforms()
    {
        for (int i = 0; i < wheelColliders.Length; i++)
        {
            Vector3 loc;
            Quaternion rot;
            wheelColliders[i].GetWorldPose(out loc, out rot);
            wheelTransforms[i].rotation = rot;
            wheelTransforms[i].position = loc;
            
        }
    }

    void Update()
    {
#if UNITY_EDITOR
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
#endif

        HnadleInput(verticalInput, horizontalInput);
        
        //Debug.Log(wheelColliders[0].);
        
        
        RPMA = wheelColliders[0].rpm;
        Revs = carRigidbody.velocity.magnitude * 3.6f / 60;
        
        //Debug.Log(carAcceleration);
    }

    private void CalculatePedulum()
    {
        pedulum += verticalInput/50;
        if(verticalInput < 0.2f)
        {
            pedulum -= 0.015f;
        }
        
        pedulum = Mathf.Clamp(pedulum, 0, 1);
    }

    private void HnadleInput(float verticalInput,float horizontalInput)
    {
        for (int i = 0; i < 2; i++)
        {
            wheelColliders[i].motorTorque = verticalInput * maximumMotorTorque;
        }

        for (int i = 0; i < 2; i++)
        {
            wheelColliders[i].steerAngle = horizontalInput * maximumWheelRotationAngle;
        }

#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.Space))
        {
            Brake();
        }
        else
        {
            BrakeRelease();
        }
#endif
    }

    public void BrakeRelease()
    {
        for (int i = 0; i < 4; i++)
        {
            wheelColliders[i].brakeTorque = 0;
            //wheelColliders[i].sidewaysFriction = originalBackSideawayCurve;
        }
    }

    public void Brake()
    {
        for (int i = 0; i < wheelColliders.Length; i++)
        {
            wheelColliders[i].brakeTorque = 1000;
            //wheelColliders[i].sidewaysFriction = modifiedBackSideawayCurve;
        }
    }
}
