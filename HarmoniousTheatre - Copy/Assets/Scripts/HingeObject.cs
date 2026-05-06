using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class HingeObject : MonoBehaviour
{
    public float minAngle = 0f; //min angle the hinge can rotate to 0 = closed 
    public float maxAngle = 90f; //max angle the hinge can rotate to 90 = fully open!
    public bool useSpring = true; //if true the hinge will spring back towards the rest angle when released!
    public float springTargetAngle = 0f;// the angle the spring tries to return to!
    public float springForce = 50f; //how strong the spring force is 
    public float springDamper = 5f; // how much the spring dampens, reduces ociliation 

    //      events      //
    public UnityEvent OnReachMax;// fired when the hinge reaches or passes the max angle!
    public UnityEvent OnReachMin; //fired when the hinge reaches or passes the minimum angle!

    public float eventThreshold = 5f; //how close to the limit angle b4 the event fires (degrees not time)

    //priv 
    private HingeJoint _hinge;
    private bool _maxEventFired = false; 
    private bool _minEventFired = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _hinge = GetComponent<HingeJoint>();
        ConfigureHinge(); 

    }

    // Update is called once per frame
    void Update()
    {
        //check if we've hit the limtis and should fire puzzle events
        float currentAngle = _hinge.angle;

        if(!_maxEventFired && currentAngle >= maxAngle - eventThreshold)
        {
            _maxEventFired=true;
            _minEventFired = false;

            OnReachMax?.Invoke();
            Debug.Log(gameObject.name + "hinge reached max angle");

        } 
        if (!_minEventFired && currentAngle <= minAngle + eventThreshold)
        {
            _minEventFired =true;
            _maxEventFired = false; 

            OnReachMin?.Invoke();
            Debug.Log(gameObject.name + "hinge reached MIN angle");
        }


    }

    //configure hinge
    //sets up joint limits and spring through code 
    void ConfigureHinge()
    {
        //limits
        //jointlimits is a struct we all to set all fields then assign it back (struct holds multiple vairables)
        JointLimits limits = _hinge.limits;
        limits.min = minAngle;
        limits.max = maxAngle;
        limits.bounciness = 0f;
        limits.bounceMinVelocity = .2f;
        _hinge.limits = limits;
        _hinge.useLimits = true;


        if (useSpring)
        {
            JointSpring spring = _hinge.spring;
            spring.targetPosition = springTargetAngle; 
            spring.spring = springForce;
            spring.damper = springDamper;
            _hinge.spring = spring;
            _hinge.useSpring = true;
        }
        else
        {
            _hinge.useSpring = false; 
        }


    }

    public void DriveToMax()
    {
        //function here
        SetMotorTarget(maxAngle); 

    }

    public void DriveToMin()
    {
        SetMotorTarget(minAngle);
    }

    void SetMotorTarget(float targetAngle)
    {
        JointMotor motor = _hinge.motor; //make struct
        //motor velocity direction determines which way it moves
        //shorthand if statement: if targetangle is greater than _hinge -> velo is 50 otherwise -50
        motor.targetVelocity = targetAngle > _hinge.angle ? 50f : -50f;
        motor.force = 100f;
        motor.freeSpin = false; 
        _hinge.motor = motor;
        _hinge.useMotor = true;
    }


}
