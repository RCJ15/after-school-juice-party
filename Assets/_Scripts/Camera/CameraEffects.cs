using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Grga
public class CameraEffects : MonoBehaviour 
{
    public static CameraEffects Instance;

    Camera _cam;
    [Header("Shake")]
    [SerializeField] float shakeIntensity = 0;
    [SerializeField] float shakeDuration = 0;
    [Header("Flash")]
    [SerializeField] float fadeDuration = 0;
    [SerializeField] float flashDuration = 0;
    [Tooltip("Add a image into the local canvas that is set to screen size. Add the image source as \"Assets/Textures/JUICE/Flash.png\" and put its alpha value to zero.")]
    [SerializeField] UnityEngine.UI.Image flashImage;
    //[SerializeField] bool flash;
    [Header("Zoom")]
    //[SerializeField] bool zoom;
    [SerializeField] float zoomInDuration = 0;
    [SerializeField] float zoomStayDuration = 0;
    [SerializeField] float zoomOutDuration = 0;
    [SerializeField] float zoomedIn = 0;
    [SerializeField] float zoomedOut = 0;

    private bool _flashPrority;

    Vector3 _StartPos;
    Coroutine _CurrentFadeRoutine;
    Coroutine _CurrentZoomRoutine;
    float _MaxShakeDuration;
    Vector3 _ZoomPos;

    private void Awake()
    {
        Instance = this; // Set static instance
    }

    // Start is called before the first frame update
    void Start()
    {
        _cam = Camera.main;

        _StartPos = transform.localPosition; // Start position of camera
        flashImage.color = new Color(flashImage.color.r, flashImage.color.g, flashImage.color.b, 0); // Hide flash
        _cam.fieldOfView = zoomedOut; // Set camera size
    }
    
    // Shake is in fixed update so shake has consistent interval between each shake
    private void FixedUpdate()
    {
        if (shakeDuration > 0)
        {
            // This is so that camera shake will change based on shake duration
            float t = shakeDuration / _MaxShakeDuration;

            Vector3 pos = Random.insideUnitCircle * shakeIntensity * t; // Shake
            pos.z = _StartPos.z; // Remove z axis value
            transform.localPosition = pos + _ZoomPos; // Apply to camera

            // Decrease duration
            shakeDuration -= Time.deltaTime;
        }
        else if (transform.localPosition != _StartPos + _ZoomPos) // If position is not start position
        {
            transform.localPosition = _StartPos + _ZoomPos; // Reset position
        }

        if (shakeDuration <= 0 && shakeIntensity > 0)
        {
            shakeIntensity = 0;
        }
    }

    IEnumerator Fade(Color color, bool unscaled)
    {
        flashImage.color = color; // Flash
        if (flashDuration > 0)
        {
            if (unscaled)
            {
                yield return new WaitForSecondsRealtime(flashDuration);
            }
            else
            {
                yield return new WaitForSeconds(flashDuration);
            }
        }

        if (fadeDuration > 0)
        {
            float timer = fadeDuration; // Keep on for some time

            while (timer > 0)
            {
                timer -= unscaled ? Time.unscaledDeltaTime : Time.deltaTime;
                flashImage.color = new Color(color.r, color.g, color.b, (timer / fadeDuration) * color.a); // Apply fade
                yield return null;
            }
        }

        flashImage.color = new Color(color.r, color.g, color.b, 0); // Set alpha to zero after done to be sure

        _flashPrority = false;
    }
    
    public static void Flash(float fadeDuration, bool unscaled = false, bool priority = false)
    {
        Flash(0, fadeDuration, Color.white, unscaled, priority);
    }

    public static void Flash(float fadeDuration, Color color, bool unscaled = false, bool priority = false)
    {
        Flash(0, fadeDuration, color, unscaled, priority);
    }

    public static void Flash(float holdDuration, float fadeDuration, bool unscaled = false, bool priority = false)
    {
        Flash(holdDuration, fadeDuration, Color.white, unscaled, priority);
    }

    /// <summary>
    /// Flashes the screen
    /// </summary>
    /// <param name="holdDuration">How long the flash should stay</param>
    /// <param name="fadeDuration">How long it takes for the flash to fade away</param>
    /// <param name="color">The color of the flash</param>
    public static void Flash(float holdDuration, float fadeDuration, Color color, bool unscaled = false, bool priority = false)
    {
        if (Instance._flashPrority)
        {
            return;
        }

        Instance._flashPrority = priority;

        Instance.flashDuration = holdDuration;
        Instance.fadeDuration = fadeDuration;

        if (Instance._CurrentFadeRoutine != null)
        {
            Instance.StopCoroutine(Instance._CurrentFadeRoutine); // Stop current flash
        }
        Instance._CurrentFadeRoutine = Instance.StartCoroutine(Instance.Fade(color, unscaled)); // Start flash
    }

    /// <summary>
    /// Shakes the camera
    /// </summary>
    /// <param name="intensity">The intensity of the shake</param>
    /// <param name="duration">How long the shake lasts</param>
    public static void Shake(float intensity, float duration)
    {
        if (intensity <= 0 || duration <= 0)
        {
            return;
        }

        if (Instance.shakeIntensity < intensity)
        {
            Instance.shakeIntensity = intensity;
        }

        if (Instance.shakeDuration < duration)
        {
            Instance.shakeDuration = duration;
        }

        Instance._MaxShakeDuration = Instance.shakeDuration;
    }

    public static void Zoom(float size, float time, Vector3 zoomLocation)
    {
        Zoom(size, Instance.zoomedOut, 0, 0, time, zoomLocation);
    }

    /// <summary>
    /// Zoom the camera on the position using easings, perspective or orthographic
    /// </summary>
    /// <param name="zoomedInSize"> Size when of camera when zoomed int</param>
    /// <param name="zoomedOutSize"> Size when of camera when zoomed out</param>
    /// <param name="zoomInTime">Time during zooming in</param>
    /// <param name="zoomedInStayDuration">Time to stayed zoomed in</param>
    /// <param name="zoomOutTime">Time during zooming out</param>
    /// <param name="zoomLocation">Place to come on to</param>
    public static void Zoom(float zoomedInSize, float zoomedOutSize, float zoomInTime, float zoomedInStayDuration, float zoomOutTime, Vector3 zoomLocation, bool unscaled = false)
    {
        Instance.zoomedIn = zoomedInSize;
        Instance.zoomedOut = zoomedOutSize;
        Instance.zoomInDuration = zoomInTime;
        Instance.zoomStayDuration = zoomedInStayDuration;
        Instance.zoomOutDuration = zoomOutTime;

        if (Instance._CurrentZoomRoutine != null)
        {
            Instance.StopCoroutine(Instance._CurrentZoomRoutine); // Stop current flash
        }

        Instance._CurrentZoomRoutine = Instance.StartCoroutine(Instance.ZoomCoRut(zoomLocation, unscaled)); // Start zoom
    }
    public IEnumerator ZoomCoRut(Vector3 zoomLocation, bool unscaled)
    {
        float timer = zoomInDuration;
        zoomLocation.z = _ZoomPos.z;
        Vector3 oldCamPos = _ZoomPos;

        while (timer > 0) // Zoom to in
        {
            timer -= unscaled ? Time.unscaledDeltaTime : Time.deltaTime;

            float t = Easing.OutCubic(1 - (timer / zoomInDuration));

            _ZoomPos = Vector3.LerpUnclamped(oldCamPos, zoomLocation, t);

            _cam.fieldOfView = Mathf.LerpUnclamped(zoomedOut, zoomedIn, t);

            yield return null;
        }

        _ZoomPos = zoomLocation;
        _cam.orthographicSize = zoomedIn;

        if (zoomStayDuration > 0)
        {
            if (unscaled)
            {
                yield return new WaitForSecondsRealtime(zoomStayDuration); // Wait zoomed in
            }
            else
            {
                yield return new WaitForSeconds(zoomStayDuration); // Wait zoomed in
            }
        }

        timer = zoomOutDuration;
        while (timer > 0) // Zoom to normal
        {
            timer -= unscaled ? Time.unscaledDeltaTime : Time.deltaTime;

            float t = Easing.InCubic (1 - (timer / zoomOutDuration));

            _ZoomPos = Vector3.Lerp(zoomLocation, oldCamPos, t);

            _cam.fieldOfView = Mathf.Lerp(zoomedIn, zoomedOut, t);

            yield return null;
        }
        _cam.fieldOfView = zoomedOut; // Just to be sure its zoomed out
        _CurrentZoomRoutine = null;

        _ZoomPos = Vector2.zero;
    }
}