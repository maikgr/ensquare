using UnityEngine;
using System.Collections;

public class ScreenShake : MonoBehaviour {
    public float shakeFramesDuration;
    public bool shake;
    public float intensity = 0.5f;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            shake = true;
            StartCoroutine(ShakeScreen(shakeFramesDuration));
        }
    }

    public IEnumerator ShakeScreen(float duration)
    {
        while (shake)
        {
            Vector3 originalPos = mainCamera.transform.localPosition;
            while (duration > 0)
            {
                Vector2 shakePos = Random.insideUnitCircle * (shakeFramesDuration / 50 * intensity);
                mainCamera.transform.localPosition = new Vector3(shakePos.x, shakePos.y, mainCamera.transform.localPosition.z);
                yield return null;
                duration -= 1;
            }

            mainCamera.transform.localPosition = originalPos;

            shake = false;
        }
    }
}
