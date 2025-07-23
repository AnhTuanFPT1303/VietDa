using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Camera checkpointCamera; // 👈 Thêm dòng này

    private Animator anim;
    private bool hasBeenActivated = false;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasBeenActivated)
        {
            GameManager.instance.UpdateCheckpoint(this);

            if (anim != null)
            {
                anim.SetTrigger("StartWaving");
            }
        }
    }

    public void Activate()
    {
        hasBeenActivated = true;
        if (checkpointCamera != null)
        {
            checkpointCamera.gameObject.SetActive(true);
            GameManager.currentActiveCamera = checkpointCamera;
        }
    }

    public void Deactivate()
    {
        hasBeenActivated = false;
        if (checkpointCamera != null)
        {
            checkpointCamera.gameObject.SetActive(false);
        }
    }
}
