using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    //Animation
    Animator a;
    // Start is called before the first frame update
    void Start()
    {
        a = gameObject.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        MovementAnimation();
    }

    private void MovementAnimation()
    {
        //Checking Movement
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            a.SetFloat("Mag", 1);

            if(Input.GetKey(KeyCode.LeftShift))
            {
                a.SetBool("Running", true);
            }
            else
            {
                a.SetBool("Running", false);
            }
        }
        else
        {
            a.SetBool("Running", false);
            a.SetFloat("Mag", 0);
        }
    }
}
