using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CharacterMovement : NetworkBehaviour
{

    [Header("Movement")]
    [SerializeField] float speed = 10;

    [Header("Jump")]
    [SerializeField] float gravity;
    [SerializeField] float jumpSpeed;
    [SerializeField] Transform legs;
    [SerializeField] LayerMask groundLayer;
    bool onGround;
    float ySpeed = 0;

    [Header("Components")]
    [SerializeField] Animator animator;
    [SerializeField] CharacterController controller;
    [SerializeField] CharacterInput characterInput;

    public override void OnStartClient()
    {
        if(!hasAuthority) this.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Calculation of speed's vector 
        Vector3 input = new Vector3(characterInput.Horizontal, 0, characterInput.Vertical);
        input = Vector3.ClampMagnitude(input, 1);

        //Setting animations
        animator.SetBool("Run", input.magnitude != 0);
        animator.SetBool("OnGround", onGround);

        if (input.magnitude != 0)
        {
            float targetAngle = Mathf.Atan2(input.x, input.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 0.1f);

            Vector3 moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward * speed;
            controller.Move(moveDirection * Time.deltaTime);
        }

        if (onGround && characterInput.Jump) ySpeed = jumpSpeed;
        if (!onGround) ySpeed = Mathf.Clamp(ySpeed - 2 * gravity * Time.deltaTime, -gravity, jumpSpeed);
        controller.Move(Vector3.up * ySpeed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        onGround = Physics.CheckSphere(legs.position, 0.5f, groundLayer);
    }
}
