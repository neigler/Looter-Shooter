using Unity.VisualScripting;
using UnityEngine;

public class ShellCase : MonoBehaviour
{
    public float startYPosition;
    public float yVnot;
    public float xVnot;
    public float acceleration = -9.8f;

    public Rigidbody2D rb;

    private float offset = -0.5f;
    private float velocityTime;
    private float rotationSpeed = 400f;

    void Start()
    {
        startYPosition = transform.position.y;
        rb.linearVelocity = new Vector2(xVnot, yVnot);
    }

    private void FixedUpdate()
    {
        Acceleration();
    }

    void Acceleration()
    {
        velocityTime += Time.fixedDeltaTime;

        if (rb.linearVelocity.magnitude < 0.5f)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        transform.Rotate(0f, 0f, rotationSpeed * Time.fixedDeltaTime);

        if (transform.position.y <= startYPosition + offset && rb.linearVelocity.y < 0)
        {
            float yVelocity = -rb.linearVelocity.y * 0.25f;
            float xVelocity = rb.linearVelocity.x * 0.25f;

            rb.linearVelocity = new Vector2(xVelocity, yVelocity);
            velocityTime = 0;
        }
        else
        {
            float yVelocity = rb.linearVelocity.y + acceleration * velocityTime;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, yVelocity);
        }
    }
}
