using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Thành phần cần liên kết")]
    public Transform player;
    public Image fadeScreen;

    [Header("Cài đặt Hồi sinh")]
    public float fadeDuration = 0.5f;

    public bool hasKey = false; // Biến để lưu trạng thái có chìa khóa
    // ---------------------

    private Vector3 lastCheckpointPosition;
    private List<Checkpoint> allCheckpoints = new List<Checkpoint>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (player != null)
        {
            lastCheckpointPosition = player.position;
        }
        allCheckpoints.AddRange(FindObjectsByType<Checkpoint>(FindObjectsSortMode.None));
        if (fadeScreen != null)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, 0);
        }
    }

    public void RespawnPlayer()
    {
        StartCoroutine(RespawnSequence());
    }

    private IEnumerator RespawnSequence()
    {
        yield return StartCoroutine(Fade(1f));
        if (player != null)
        {
            player.position = lastCheckpointPosition;
            player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        }
        yield return StartCoroutine(Fade(0f));
    }

    private IEnumerator Fade(float targetAlpha)
    {
        if (fadeScreen == null) yield break;
        float startAlpha = fadeScreen.color.a;
        float elapsedTime = 0f;
        Color color = fadeScreen.color;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            fadeScreen.color = new Color(color.r, color.g, color.b, newAlpha);
            yield return null;
        }
        fadeScreen.color = new Color(color.r, color.g, color.b, targetAlpha);
    }

    public void UpdateCheckpoint(Checkpoint newCheckpoint)
    {
        foreach (Checkpoint cp in allCheckpoints)
        {
            cp.Deactivate();
        }
        newCheckpoint.Activate();
        lastCheckpointPosition = newCheckpoint.transform.position;
    }

    public void CollectKey()
    {
        hasKey = true;
        Debug.Log("Người chơi đã có chìa khóa!");
    }
    // ---------------------
}