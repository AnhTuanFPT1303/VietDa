using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Knockback : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;
    const float strength = 22.0f, delay = 0.2f;
    public UnityEvent OnBegin, OnDone;
    [SerializeField]
    private AnimationCurve knockbackCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);
    [SerializeField]
    private float knockbackDuration = 0.5f;

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
        StartCoroutine(SmoothKnockback(direction));
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

    private IEnumerator SmoothKnockback(Vector2 direction)
    {
        float elapsed = 0f;

        while (elapsed < knockbackDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / knockbackDuration;

            float forceFactor = knockbackCurve.Evaluate(t);
            rb.linearVelocity = direction * strength * forceFactor;

            yield return null;
        }

        rb.linearVelocity = Vector2.zero;

        // Reset flag
        Movement movement = GetComponent<Movement>();
        if (movement != null)
        {
            movement.isBeingKnockbacked = false;
        }

        OnDone?.Invoke();
    }


}