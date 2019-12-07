using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlSprite : MonoBehaviour {

    public float speed;
    public float jumpH;
    Animator animator;
    bool facingRight = true;
    int clickNumber = 0;

    // Use this for initialization
    void Start() {
        animator = GetComponent<Animator>();
        clickNumber = 0;
    }

    // Update is called once per frame
    void Update() {
        playerMove();
        playerJump();
    }

    void playerMove()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, 0.0f);

        transform.Translate(movement * speed * Time.deltaTime);

        if (moveHorizontal > 0 && !facingRight)
        {
            Flip();
        }
        else if (moveHorizontal < 0 && facingRight)
        {
            Flip();
        }
    }

    void playerJump()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            float disToGround = GetComponent<Collider>().bounds.extents.y;
            print(disToGround);
            if (Physics.Raycast(transform.position, -Vector3.up, 0.1f))
            {
                GetComponent<Rigidbody>().AddForce(Vector3.up * jumpH);
            }
        }
    }

        void Flip()
    {
            facingRight = !facingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }

    }

