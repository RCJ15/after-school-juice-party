using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    Vector3 _StartPos;
    Coroutine _CurrentFadeRoutine;
    Coroutine _CurrentZoomRoutine;
    float _maxShakeDuration;

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

    /* old code
    // Update is called once per frame
    void Update()
    {
        if (flash || Input.GetKeyDown(KeyCode.S)) // Flash
        {
            flash = false;
        }
        if (zoom || Input.GetKeyDown(KeyCode.Z)) // Flash
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = -transform.localPosition.z;
            Zoom(zoomedIn, zoomedOut, zoomInDuration, zoomStayDuration, zoomOutDuration, _cam.ScreenToWorldPoint(mousePos));

        }
    }
    */
    
    // Shake is in fixed update so shake has consistent interval between each shake
    private void FixedUpdate()
    {
        if (shakeDuration > 0)
        {
            // This is so that camera shake will change based on shake duration
            float t = shakeDuration / _maxShakeDuration;

            Vector3 pos = Random.insideUnitCircle * shakeIntensity * t; // Shake
            pos.z = _StartPos.z; // Remove z axis value
            transform.localPosition = pos; // Apply to camera

            // Decrease duration
            shakeDuration -= Time.deltaTime;
        }
        else if (_CurrentZoomRoutine == null && transform.localPosition != _StartPos) // If position is not start position
        {
            transform.localPosition = _StartPos; // Reset position
        }
    }

    IEnumerator Fade(Color color)
    {
        flashImage.color = color; // Flash
        if (flashDuration > 0)
        {
            yield return new WaitForSeconds(flashDuration);
        }

        if (fadeDuration > 0)
        {
            float timer = fadeDuration; // Keep on for some time

            while (timer > 0)
            {
                timer -= Time.deltaTime;
                flashImage.color = new Color(color.r, color.g, color.b, (timer / fadeDuration) * color.a); // Apply fade
                yield return null;
            }
        }

        flashImage.color = new Color(color.r, color.g, color.b, 0); // Set alpha to zero after done to be sure
    }

    public static void Flash(float fadeDuration)
    {
        Flash(0, fadeDuration, Color.white);
    }

    public static void Flash(float fadeDuration, Color color)
    {
        Flash(0, fadeDuration, color);
    }

    public static void Flash(float holdDuration, float fadeDuration)
    {
        Flash(holdDuration, fadeDuration, Color.white);
    }

    /// <summary>
    /// Flashes the screen
    /// </summary>
    /// <param name="holdDuration">How long the flash should stay</param>
    /// <param name="fadeDuration">How long it takes for the flash to fade away</param>
    /// <param name="color">The color of the flash</param>
    public static void Flash(float holdDuration, float fadeDuration, Color color)
    {
        Instance.flashDuration = holdDuration;
        Instance.fadeDuration = fadeDuration;

        if (Instance._CurrentFadeRoutine != null)
        {
            Instance.StopCoroutine(Instance._CurrentFadeRoutine); // Stop current flash
        }
        Instance._CurrentFadeRoutine = Instance.StartCoroutine(Instance.Fade(color)); // Start flash
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

        if (Instance.shakeDuration <= 0)
        {
            Instance.shakeDuration = duration;
        }
        else
        {
            Instance.shakeDuration += duration;
        }

        Instance._maxShakeDuration = Instance.shakeDuration;
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
    public static void Zoom(float zoomedInSize, float zoomedOutSize, float zoomInTime, float zoomedInStayDuration, float zoomOutTime, Vector3 zoomLocation)
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

        Instance._CurrentZoomRoutine = Instance.StartCoroutine(Instance.ZoomCoRut(zoomLocation)); // Start zoom
    }
    public IEnumerator ZoomCoRut(Vector3 zoomLocation)
    {
        float timer = zoomInDuration;
        zoomLocation.z = transform.localPosition.z;
        Vector3 oldCamPos = transform.localPosition;
        while (timer > 0) // Zoom to in
        {
            timer -= Time.deltaTime;

            float Easing(float x)
            {
                return 1 - Mathf.Pow(1 - x, 3);
            }

            float t = Easing(1 - (timer / zoomInDuration));

            transform.localPosition = Vector3.LerpUnclamped(oldCamPos, zoomLocation, t);

            try { _cam.orthographicSize = Mathf.LerpUnclamped(zoomedOut, zoomedIn, t); }
            catch (System.Exception)
            {
                Debug.Log("Not orthographic size");
            }
            try
            {
                _cam.fieldOfView = Mathf.LerpUnclamped(zoomedOut, zoomedIn, t);
            }
            catch (System.Exception)
            {
                Debug.Log("Not Field of view");
            }
            yield return null;
        }

        transform.localPosition = zoomLocation;
        _cam.orthographicSize = zoomedIn;

        if (zoomStayDuration > 0)
        {
            yield return new WaitForSeconds(zoomStayDuration); // Wait zoomed in
        }

        timer = zoomOutDuration;
        while (timer > 0) // Zoom to normal
        {
            timer -= Time.deltaTime;

            float Easing(float x)
            {
                return x * x * x;
            }

            float t = Easing(1 - (timer / zoomOutDuration));

            transform.localPosition = Vector3.Lerp(zoomLocation, oldCamPos, t);
            try
            {
                _cam.orthographicSize = Mathf.Lerp(zoomedIn, zoomedOut, t);
            }
            catch (System.Exception)
            {
            }
            try
            {
                _cam.fieldOfView = Mathf.Lerp(zoomedIn, zoomedOut, t);
            }
            catch (System.Exception)
            {

            }

            yield return null;
        }
        _cam.fieldOfView = zoomedOut; // Just to be sure its zoomed out
        _CurrentZoomRoutine = null;
    }
}