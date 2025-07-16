using UnityEngine;

public class CollectibleItem : MonoBehaviour
{

    private bool hasBeenCollected = false;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasBeenCollected)
        {
            hasBeenCollected = true;
            GameManager.instance.CollectKey();
            Destroy(gameObject);
        }
    }

}