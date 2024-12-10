using UnityEngine;

public class Block : MonoBehaviour
{
    private Rigidbody2D rb;
    private float rotationSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0.5f;

        // Set a random rotation speed between -100 and 100 to get varied rotation directions and speeds
        rotationSpeed = Random.Range(-100f, 100f);
    }

    void Update()
    {
        // Apply the random rotation speed set in Start()
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

        if (transform.position.y < -4.5f)
        {
            WaveManager.RemoveEnemy(gameObject);
            Destroy(gameObject);
        }
    }
}
