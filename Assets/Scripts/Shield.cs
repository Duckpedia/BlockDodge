using UnityEngine;

public class Shield : MonoBehaviour
{
    private bool shieldActive = false;

    public void ActivateShield()
    {
        shieldActive = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (shieldActive)
        {
            if (collision.gameObject.CompareTag("Block") || collision.gameObject.CompareTag("Block2"))
            {
                Destroy(collision.gameObject);
                shieldActive = false;
                Destroy(gameObject);
            }
        }
    }
}
