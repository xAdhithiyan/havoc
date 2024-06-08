using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] private float maxSpeed = 20f;
    [SerializeField] private float acceleration = 0.1f;
    private float currentSpeed = 0f;
    private float horizontalValue;
    private float horizontalValueWhenKeyReleased;
    private float verticalValue;
    private float verticalValueWhenKeyReleased;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();    
    }

    void Update()
    {
        //gets the horizontal/vertical range frmo - 1 to 1
        horizontalValue = Input.GetAxis("Horizontal");
        verticalValue = Input.GetAxis("Vertical");
        //direction is given. speed is is the magnitue of the vector(per sec)
        
        if(horizontalValue != 0 || verticalValue != 0)
        {
            if(currentSpeed < maxSpeed)
            {
                currentSpeed += acceleration;
                Debug.Log(currentSpeed);
                rb.velocity = new Vector3(horizontalValue * currentSpeed, verticalValue * currentSpeed, 0);
            }
            horizontalValueWhenKeyReleased = horizontalValue;
            verticalValueWhenKeyReleased = verticalValue;
        } else
        {
            if (currentSpeed > 0) 
            {
                currentSpeed -= acceleration;
                Debug.Log(currentSpeed);
                rb.velocity = new Vector3(horizontalValueWhenKeyReleased * currentSpeed, verticalValueWhenKeyReleased * currentSpeed, 0);
            }
        }
  
    }
}
