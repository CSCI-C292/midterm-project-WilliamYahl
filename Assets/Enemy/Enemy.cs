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


    // Update is called once per frame
    void Update()
    {
        relativeTargetLocation = _target.location - GetComponent<Rigidbody2D>().position;

        if (-relativeTargetLocation.x < 0 && transform.rotation.y == 0f)
        {
            //GetComponent<Rigidbody2D>().SetRotation(180);
            transform.Rotate(new Vector3(0, 1, 0), 180f);   // rotating and rotating back, want to set rotation not rotate the object.
        }
        else if (-relativeTargetLocation.x > 0 && transform.rotation.y > 0f)
        {
            //GetComponent<Rigidbody2D>().SetRotation(0);
            transform.Rotate(new Vector3(0, 1, 0), 180f);
        }

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
            GameState.instance.SpawnEnemy();
            GameState.instance.IncreaseScore();
            Destroy(gameObject);
        }
        if(collider.name == "Pits")
        {
            GameState.instance.SpawnEnemy();
            GameState.instance.IncreaseScore();
            Destroy(gameObject);
        }
        else
        {
            run();
        }
    }

    private void run()
    {
        if (canLeap && GetComponent<Rigidbody2D>().velocity.y == 0)
        {
            //print("run" + canLeap);
            GetComponent<Rigidbody2D>().MovePosition(GetComponent<Rigidbody2D>().position + new Vector2(_speed * Math.Sign(-relativeTargetLocation.x), 0));
        }
        //Debug.Log("RUN");
    }

    private void leap()
    {
        if (canLeap && GetComponent<Rigidbody2D>().velocity.y == 0)
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
