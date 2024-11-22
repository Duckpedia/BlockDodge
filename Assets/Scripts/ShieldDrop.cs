using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ShieldDrop : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            FindFirstObjectByType<Items>().Shield();
            Destroy(gameObject);
        }
    }
}
