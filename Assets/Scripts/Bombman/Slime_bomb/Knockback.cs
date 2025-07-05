using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Knockback : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private float strength = 20.0f, delay = 0.2f;
    public UnityEvent OnBegin, OnDone;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayKnockback(GameObject sender)
    {
        StopAllCoroutines();
        OnBegin?.Invoke();

        // Set flag
        Movement movement = GetComponent<Movement>();
        if (movement != null)
        {
            movement.isBeingKnockbacked = true;
        }

        Vector2 direction = (transform.position - sender.transform.position).normalized;
        rb.AddForce(direction * strength, ForceMode2D.Impulse);
        StartCoroutine(Reset());
    }

    private IEnumerator Reset()
    {
        yield return new WaitForSeconds(delay);
        rb.linearVelocity = Vector3.zero;

        // Reset flag
        Movement movement = GetComponent<Movement>();
        if (movement != null)
        {
            movement.isBeingKnockbacked = false;
        }

        OnDone?.Invoke();
    }

}
