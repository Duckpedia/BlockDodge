using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Items : MonoBehaviour
{
    public GameObject shieldDropPrefab;
    private bool shieldDropped = false;
    private float xRange = 8f;
    private float spawnHeight = -4f;
    public GameObject shieldPrefab;
    public GameObject player;
    private GameObject activeShield;

    void Start()
    {
        StartCoroutine(SpawnShield());
    }

    public IEnumerator SpawnShield()
    {
        while(true)
        {
            if (GameManager.gameStarted && !shieldDropped)
            {
                yield return new WaitForSeconds(Random.Range(30f, 60f));
                Vector2 spawnPosition = new Vector2(Random.Range(-xRange, xRange), spawnHeight);
                Instantiate(shieldDropPrefab, spawnPosition, Quaternion.identity);
                shieldDropped = true;
            }
            else{
                yield return null;
            }
        }
    }

    public void Shield()
    {
        if (activeShield == null)
        {
            activeShield = Instantiate(shieldPrefab, player.transform);
            activeShield.transform.localPosition = Vector3.zero;
            activeShield.transform.localScale = new Vector3(1.7f, 1.7f, 1.7f);
            shieldDropped = false;
            print("shielddropped is now false");
            Shield shieldScript = activeShield.GetComponent<Shield>();
            if (shieldScript != null)
            {
                shieldScript.ActivateShield();
            }
        }
    }

    private void Update()
    {
        if (activeShield != null)
        {
            activeShield.transform.position = player.transform.position;
        }
    }
}
