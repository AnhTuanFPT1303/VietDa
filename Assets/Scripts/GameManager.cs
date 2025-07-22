using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static Camera currentActiveCamera;

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
        StartCoroutine(RespawnAndReloadScene());
    }

    private IEnumerator RespawnAndReloadScene()
    {
        yield return StartCoroutine(Fade(1f));
        // Lưu lại tên camera hiện tại (nếu có)
        string cameraName = currentActiveCamera != null ? currentActiveCamera.name : null;
        // Reload scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        // Đợi scene load xong
        yield return null;
        // Sau khi load lại, tìm lại camera và kích hoạt nó
        if (!string.IsNullOrEmpty(cameraName))
        {
            Camera foundCam = GameObject.Find(cameraName)?.GetComponent<Camera>();
            if (foundCam != null)
            {
                foundCam.gameObject.SetActive(true);
                currentActiveCamera = foundCam;
            }
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