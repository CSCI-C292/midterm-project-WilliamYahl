using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    [SerializeField] float _speed;
    [SerializeField] float _jumpHeight;

    [SerializeField] Sprite _attacking;
    [SerializeField] Sprite _idle;

    [SerializeField] Aggro _myAggro;

    [SerializeField] GameObject _hpBar;
    [SerializeField] int _hp;

    private bool invincibility = false;

    private void Awake()
    {
        _myAggro.targetName = name;
        _myAggro.location = GetComponent<Rigidbody2D>().position;

        _hpBar.GetComponent<Text>().text = "HP: " + _hp;
    }

    // Update is called once per frame Time.deltaTime *
    void Update()
    {
        if (Input.GetAxis("Horizontal") < 0 && transform.rotation.y == 0f)
        {
            //GetComponent<Rigidbody2D>().SetRotation(180);
            transform.Rotate(new Vector3(0, 1, 0), 180f);   // rotating and rotating back, want to set rotation not rotate the object.
        }
        else if (Input.GetAxis("Horizontal") > 0 && transform.rotation.y > 0f)
        {
            //GetComponent<Rigidbody2D>().SetRotation(0);
            transform.Rotate(new Vector3(0, 1, 0), 180f);
        }

        Vector2 movementVector =  new Vector2(_speed * Input.GetAxis("Horizontal"), 0);
        GetComponent<Rigidbody2D>().MovePosition(GetComponent<Rigidbody2D>().position + movementVector);
        //transform.position += new Vector3(Time.deltaTime * speed * Input.GetAxis("Horizontal"), 0, 0);

        if(Input.GetButtonDown("Jump"))
        {
            if (GetComponent<Rigidbody2D>().velocity == new Vector2(0, 0))
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, _jumpHeight);
            }
        }

        if(Input.GetButtonDown("Fire1"))
        {
            attack();
            Invoke("startIdle", .1f);
        }

        _myAggro.location = GetComponent<Rigidbody2D>().position;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Pits")
        {
            GameState.instance.GameOver();
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!invincibility && collision.collider.name != "Tilemap")
        {
            invincibility = true;
            Invoke("invincibilityTimer", .2f);
            
            _hp--;
            _hpBar.GetComponent<Text>().text = "HP: " + _hp;
            if (_hp == 0 || collision.collider.name == "Pits")
            {
                GameState.instance.GameOver();
                _hpBar.SetActive(false);
                Destroy(gameObject);
            }
        }
    }

    void startIdle()
    {
        transform.Find("Swing").gameObject.SetActive(false);
        GetComponent<SpriteRenderer>().sprite = _idle;
    }

    void attack()
    {
        transform.Find("Swing").gameObject.SetActive(true);
        GetComponent<SpriteRenderer>().sprite = _attacking;
    }

    void invincibilityTimer()
    {
        invincibility = false;
    }
}
