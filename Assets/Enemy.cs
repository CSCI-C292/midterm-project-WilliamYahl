using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float _speed;
    [SerializeField] float _leapForward;
    [SerializeField] float _leapUp;
    [SerializeField] float _leapCooldown;
    bool canLeap = true;

    [SerializeField] float _aggroRange;
    [SerializeField] Aggro _target;
    Vector2 relativeTargetLocation = new Vector2(0,0);


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        relativeTargetLocation = _target.location - GetComponent<Rigidbody2D>().position;

        //print(_target.location + " " +  GetComponent<Rigidbody2D>().position );
        //Debug.Log(relativeTargetLocation + " " + -Math.Sign(relativeTargetLocation.x));
        RaycastHit2D hit = Physics2D.Raycast(GetComponent<Rigidbody2D>().position, relativeTargetLocation, 1.5f * _aggroRange);
        if(hit)
        {
            if(hit.collider.name == _target.targetName && hit.distance > _aggroRange )
            {
                leap();
            }else if(hit.collider.name == _target.targetName)
            {
                run();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.name == "Swing")
        {
            Destroy(gameObject);
        }
        else
        {
            run();
        }
    }

    private void run()
    {
        if (canLeap)
        {
            //print("run" + canLeap);
            GetComponent<Rigidbody2D>().MovePosition(GetComponent<Rigidbody2D>().position + new Vector2(_speed * Math.Sign(-relativeTargetLocation.x), 0));
        }
        //Debug.Log("RUN");
    }

    private void leap()
    {
        if (canLeap)
        {
            Invoke("resetLeap", _leapCooldown);
            GetComponent<Rigidbody2D>().velocity = new Vector2(-_leapForward * Math.Sign(-relativeTargetLocation.x), _leapUp);
            canLeap = false;
            //Debug.Log(new Vector2(relativeTargetLocation.x, _leapUp) + " " + relativeTargetLocation);
        }
        //Debug.Log("LEAP");
    }

    private void resetLeap()
    {
        //Debug.Log("hi");
        canLeap = true;
    }
}
