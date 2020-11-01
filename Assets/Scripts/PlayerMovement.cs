using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform groundCheck;
    public LayerMask groundMask;
    public float MoveSpeed = 6f;
    public float Gravity = -12f;
    public float GroundDistance = 0.4f;
    public float JumpHeight = 3f;

    private Vector3 _velocity;
    private bool _isGrounded;

    void Update()
    {
        _isGrounded = Physics.CheckSphere(groundCheck.position, GroundDistance, groundMask);

        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        var move = transform.right * x + transform.forward * z;

        controller.Move(move * MoveSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            _velocity.y = Mathf.Sqrt(JumpHeight * -2f * Gravity);
        }

        _velocity.y += Gravity * Time.deltaTime;

        controller.Move(_velocity * Time.deltaTime);
    }
}
