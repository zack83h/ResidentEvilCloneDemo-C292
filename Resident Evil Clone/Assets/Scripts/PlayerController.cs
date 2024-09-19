using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float mouseSensitivity;
    [SerializeField] float verticalLookLimit;

    private bool isGrounded = true;
    private float xRotation; //set to 0 by default

    [SerializeField] Transform fpsCamera;
    private Rigidbody rb;
    [SerializeField] private Transform firePoint;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        LookAround();
        MovePlayer();

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        if(Input.GetMouseButtonDown(0)) //0 is the left mouse button, 1 is left 2 is middle
        {
            Shoot();
        }

    }

    void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -verticalLookLimit, verticalLookLimit);
        //localRotation vs just location | Quaternion.Euler -> bad to good units | only rotation on the local x axis 
        fpsCamera.localRotation = Quaternion.Euler(xRotation, 0, 0);
    }

    void MovePlayer()
    {
        float moveX = Input.GetAxis("Horizontal");
        //2d vs 3d
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ; //combination of all the vectors
        move.Normalize(); //strips magnitude, only gives direction
        Vector3 moveVelocity = move * moveSpeed;

        moveVelocity.y = rb.velocity.y;

        rb.velocity = moveVelocity;
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        //Vector3.up = (0,1,0)

        isGrounded = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground") ;
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground") ;
        {
            isGrounded = false;
        }
    }

    private void Shoot()
    {
        RaycastHit hit;
        if(Physics.Raycast(firePoint.position, firePoint.forward, out hit, 100))
        {
            Debug.DrawRay(firePoint.position, firePoint.forward * hit.distance, Color.red, 100);
            if(hit.transform.CompareTag("Zombie"))
            {
                this.transform.GetComponent<Enemy>().TakeDamage(1);
            }
        }
    }

    //private void ShootBullter()
    //{
    //    GameObject bullet = Instantiate(Projectile, firePoint.position, firePoint.forward);
    //    bullet.GetComponent<Rigidbody>().AddForce(firePoint.forward * 10, ForceMode.Impulse);
    //}

}
