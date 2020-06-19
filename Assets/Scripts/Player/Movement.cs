using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [AddComponentMenu("Game Systems/RPG/Player/Movement")]
    [RequireComponent(typeof(CharacterController))]
    public class Movement : MonoBehaviour
    {
        [Header("Physics")]
        public float gravity = 20f;
        public CharacterController controller;
        [Header("Movement Variables")]
        public float speed = 5f;
        public float sprintSpeed = 10f;
        public float crouchSpeed = 2f;
        public float jumpSpeed = 8f;
        public Vector3 moveDirection;

        void Start()
        {
        //Grabs the Character controller attached to this object
            controller = GetComponent<CharacterController>();
        }
        void Update()
        {
            float horizontal = 0;
            float vertical = 0;
            if (Input.GetKey(KeyCode.W))
            {
                vertical++;
            }
            if (Input.GetKey(KeyCode.A))
            {
                horizontal--;
            }
            if (Input.GetKey(KeyCode.D))
            {
                horizontal++;
            }
            if (Input.GetButton("down"))
            {
                vertical--;
            }
            if (Input.GetButton("Sprint"))
            {
                moveDirection *= sprintSpeed;
            }
            if (Input.GetButton("Crouch"))
            {
                moveDirection *= crouchSpeed;
            }
            if (controller.isGrounded)
            {
                moveDirection = transform.TransformDirection(new Vector3(horizontal, 0, vertical));
                moveDirection *= speed;
                if(Input.GetButton("Jump"))
                {
                    moveDirection.y = jumpSpeed;
                }
            }
            moveDirection.y -= gravity * Time.deltaTime;
            controller.Move(moveDirection * Time.deltaTime);
        }
    }
}
