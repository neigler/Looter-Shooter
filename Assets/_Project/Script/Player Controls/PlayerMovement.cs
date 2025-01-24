using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("Variables")]
    [SerializeField] public float currentSpeed;
    [SerializeField] public float noHeavySpeed;
    [SerializeField] public float lilHeavySpeed;
    [SerializeField] public float mediumHeavySpeed;
    [SerializeField] public float largeHeavySpeed;

    [Header("References")]
    [SerializeField] private Animator anim;
    private Rigidbody2D rb;
    private Vector2 dir;
    [HideInInspector] public bool canRotate;
    [HideInInspector] public bool canMove;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        canRotate = true;
        canMove = true;
    }

    private void Update()
    {
        // Manage the input
        InputManager();

        // Manage the rotation of the player character towards the mouse
        LookAtMouse();

        // Manage the animations of idle, movement, and others
        AnimationController();
    }

    private void FixedUpdate()
    {
        // Manage the (WASD) Movement
        MovementManager();
    }

    private void MovementManager()
    {
        // Stop function if cant move is true
        if (!canMove)
        {
            rb.linearVelocity = new Vector2(0, 0);
            return;
        }

        // Move up, down, left or right depending on the input
        rb.linearVelocity = new Vector2(dir.x * currentSpeed, dir.y * currentSpeed);
    }

    private void InputManager()
    {
        // Stop function if cant move is true
        if (!canMove)
            return;

        // X-Y Inputs to later use as movement
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        dir = new Vector2(x, y);
    }

    private void LookAtMouse()
    {
        // Rotate the player object towards the mouse
        if (!canRotate)
            return;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.up = mousePos - new Vector2(transform.position.x, transform.position.y);
    }

    private void AnimationController()
    {
        // If your moving play move animation. If not stande idle
        if (rb.linearVelocity.magnitude > 0.5)
        {
            anim.SetFloat("Input", 1);
        }
        else if (rb.linearVelocity.magnitude < 0.5)
        {
            anim.SetFloat("Input", 0);
        }
    }
}
