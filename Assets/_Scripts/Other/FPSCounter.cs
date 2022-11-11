using UnityEngine;
using TMPro;

/// <summary>
/// 
/// </summary>
public class FPSCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text fpsText;
    [SerializeField] private float hudRefreshRate = 1f;

    private float _timer;

    private void Update()
    {
        if (Time.unscaledTime <= _timer)
        {
            return;
        }

        int fps = (int)(1f / Time.unscaledDeltaTime);
        fpsText.text = "FPS: " + fps;
        _timer = Time.unscaledTime + hudRefreshRate;
    }
}

