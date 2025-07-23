using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static Camera currentActiveCamera;
    public static Checkpoint lastActivatedCheckpoint;

    [Header("Thành phần cần liên kết")]
    public Transform player;
    public Image fadeScreen;

    [Header("Cài đặt Hồi sinh")]
    public float fadeDuration = 0.5f;

    public bool hasKey = false; 
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

    //private IEnumerator RespawnAndReloadScene()
    //{
    //    yield return StartCoroutine(Fade(1f));
    //    string cameraName = currentActiveCamera != null ? currentActiveCamera.name : null;
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    //    yield return null;
    //    // Đặt lại vị trí player về checkpoint đã lưu
    //    if (lastActivatedCheckpoint != null && player != null)
    //    {
    //        player.position = lastActivatedCheckpoint.transform.position;
    //    }
    //    // Kích hoạt lại camera
    //    if (!string.IsNullOrEmpty(cameraName))
    //    {
    //        Camera foundCam = GameObject.Find(cameraName)?.GetComponent<Camera>();
    //        if (foundCam != null)
    //        {
    //            foundCam.gameObject.SetActive(true);
    //            currentActiveCamera = foundCam;
    //        }
    //    }
    //    yield return StartCoroutine(Fade(0f));
    //}

    private IEnumerator RespawnAndReloadScene()
    {
        yield return StartCoroutine(Fade(1f));

        // Lưu thông tin trước khi load
        Vector3 checkpointPos = lastActivatedCheckpoint != null ? lastActivatedCheckpoint.transform.position : Vector3.zero;
        string checkpointCamName = lastActivatedCheckpoint?.checkpointCamera?.name;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        yield return null; // Chờ 1 frame cho scene load

        // 🔁 Gán lại player
        player = GameObject.FindWithTag("Player")?.transform;

        // 🔁 Gán lại checkpoint từ vị trí (nên trùng chính xác)
        Checkpoint[] checkpoints = FindObjectsByType<Checkpoint>(FindObjectsSortMode.None);
        foreach (var cp in checkpoints)
        {
            if (cp.transform.position == checkpointPos)
            {
                lastActivatedCheckpoint = cp;
                cp.Activate();
            }
            else
            {
                cp.Deactivate();
            }
        }

        // 🔁 Đặt lại vị trí player cuối cùng
        if (player != null && lastActivatedCheckpoint != null)
        {
            player.position = lastActivatedCheckpoint.transform.position;
        }

        // 🔁 Kích hoạt camera từ tên đã lưu (nếu có)
        if (!string.IsNullOrEmpty(checkpointCamName))
        {
            Camera foundCam = GameObject.Find(checkpointCamName)?.GetComponent<Camera>();
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
        lastActivatedCheckpoint = newCheckpoint; // Lưu checkpoint hiện tại

        // Lưu camera hiện tại nếu có
        if (Camera.main != null)
        {
            currentActiveCamera = Camera.main;
        }
    }

    public void CollectKey()
    {
        hasKey = true;
        Debug.Log("Người chơi đã có chìa khóa!");
    }
    // ---------------------
}