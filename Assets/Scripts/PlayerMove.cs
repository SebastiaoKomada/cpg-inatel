using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMove : MonoBehaviour
{
    public float playerSpeed = 20f;
    public float gravity = -10f;
    public float momentumDamping = 5f;
    public Animator camAnim;
    private bool isWalking = false;
    private CharacterController controller;
    private Vector3 inputVector;
    private Vector3 movementVector;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        GetInput();
        MovePlayer();
        CheckForHeadBob();

        camAnim.SetBool("isWalking", true);
    }

    void GetInput()
    {
        Vector2 keyboardInput = Vector2.zero;
        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) keyboardInput.y += 1;
        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) keyboardInput.y -= 1;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) keyboardInput.x += 1;
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) keyboardInput.x -= 1;

        Vector2 gamepadInput = Vector2.zero;
        if (Gamepad.current != null)
            gamepadInput = Gamepad.current.leftStick.ReadValue();

        Vector2 final2D;
        if (keyboardInput.sqrMagnitude > 0f)
        {
            final2D = keyboardInput.normalized;
        }
        else if (gamepadInput.sqrMagnitude > 0f)
        {
            final2D = gamepadInput.normalized;
        }
        else
        {
            final2D = Vector2.zero;
        }

        inputVector = transform.TransformDirection(new Vector3(final2D.x, 0f, final2D.y));

        inputVector = Vector3.Lerp(inputVector, Vector3.zero, Time.deltaTime * momentumDamping);

        movementVector = inputVector * playerSpeed + Vector3.up * gravity;
    }

    void MovePlayer()
    {
        controller.Move(movementVector * Time.deltaTime);
    }

    void CheckForHeadBob()
    {
        if (inputVector.magnitude > 0.1f)
        {
            if (!isWalking)
            {
                isWalking = true;
            }
        }
        else
        {
            if (isWalking)
            {
                isWalking = false;
            }
        }
    }
}
