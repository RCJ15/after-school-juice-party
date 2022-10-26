using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffects : MonoBehaviour
{
    Camera _cam;
    [Header("Shake")]
    [SerializeField] float intensity = 0;
    [SerializeField] bool shake;
    [Header("Flash")]
    [SerializeField] float fadeDuration = 0;
    [SerializeField] float flashDuration = 0;
    [Tooltip("Add a image into the local canvas that is set to screen size. Add the image source as \"Assets/Textures/JUICE/Flash.png\" and put its alpha value to zero.")]
    [SerializeField] UnityEngine.UI.Image flashImage;
    [SerializeField] bool flash;
    [Header("Zoom")]
    [SerializeField] bool zoom;
    [SerializeField] float zoomInDuration = 0;
    [SerializeField] float zoomStayDuration = 0;
    [SerializeField] float zoomOutDuration = 0;
    [SerializeField] float zoomedIn = 0;
    [SerializeField] float zoomedOut = 0;

    Vector3 _StartPos;
    Coroutine _CurrentFadeRoutine;
    Coroutine _CurrentZoomRoutine;

    // Start is called before the first frame update
    void Start()
    {
        _cam = Camera.main;

        _StartPos = _cam.transform.position; // Start position of camera
        flashImage.color = new Color(flashImage.color.r, flashImage.color.g, flashImage.color.b, 0); // Hide flash
        _cam.fieldOfView = zoomedOut; // Set camera size
    }

    // Update is called once per frame
    void Update()
    {
        if (shake)
        {
            Vector3 pos = Random.insideUnitCircle * intensity; // Shake
            pos.z = _StartPos.z; // Remove z axis value
            _cam.transform.position = pos; // Apply to camera
        }
        else if (_CurrentZoomRoutine == null && _cam.transform.position != _StartPos) // If position is not start position
        {
            _cam.transform.position = _StartPos; // Reset position
        }
        if (flash || Input.GetKeyDown(KeyCode.S)) // Flash
        {
            flash = false;
            if (_CurrentFadeRoutine != null)
            {
                StopCoroutine(_CurrentFadeRoutine); // Stop current flash
            }
            _CurrentFadeRoutine = StartCoroutine(Fade()); // Start flash
        }
        if (zoom || Input.GetKeyDown(KeyCode.Z)) // Flash
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = -_cam.transform.position.z;
            Zoom(zoomedIn, zoomedOut, zoomInDuration, zoomStayDuration, zoomOutDuration, _cam.ScreenToWorldPoint(mousePos));

        }
    }
    IEnumerator Fade()
    {
        flashImage.color = new Color(flashImage.color.r, flashImage.color.g, flashImage.color.b, 1); // Show white image
        float timer = fadeDuration; // Keep on for some time
        yield return new WaitForSeconds(flashDuration);
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            flashImage.color = new Color(flashImage.color.r, flashImage.color.g, flashImage.color.b, timer / fadeDuration); // Apply fade
            yield return null;
        }

        flashImage.color = new Color(flashImage.color.r, flashImage.color.g, flashImage.color.b, 0); // Set alpha to zero after done to be sure
    }

    /// <summary>
    /// Zoom the camera on the position using easings, perspective or orthographic
    /// </summary>
    /// <param name="zoomedIn"> Size when of camera when zoomed int</param>
    /// <param name="zoomedOut"> Size when of camera when zoomed out</param>
    /// <param name="zoomInDuration">Time during zooming in</param>
    /// <param name="zoomStayDuration">Time to stayed zoomed in</param>
    /// <param name="zoomOutDuration">Time during zooming out</param>
    /// <param name="zoomLocation">Place to come on to</param>
    /// <returns></returns>
    public void Zoom(float zoomedInSize, float zoomedOutSize, float zoomInTime, float zoomedInStayDuration, float zoomOutTime, Vector3 zoomLocation)
    {
        zoomedIn = zoomedInSize;
        zoomedOut = zoomedOutSize;
        zoomInDuration = zoomInTime;
        zoomStayDuration = zoomedInStayDuration;
        zoomOutDuration = zoomOutTime;

        if (_CurrentZoomRoutine != null)
        {
            StopCoroutine(_CurrentZoomRoutine); // Stop current flash
        }

        _CurrentZoomRoutine = StartCoroutine(ZoomCoRut(zoomLocation)); // Start zoom
    }
    public IEnumerator ZoomCoRut(Vector3 zoomLocation)
    {
        float timer = zoomInDuration;
        zoomLocation.z = _cam.transform.position.z;
        Vector3 oldCamPos = _cam.transform.position;
        while (timer > 0) // Zoom to in
        {
            timer -= Time.deltaTime;

            float Easing(float x)
            {
                return 1 - Mathf.Pow(1 - x, 3);
            }

            float t = Easing(1 - (timer / zoomInDuration));

            _cam.transform.position = Vector3.LerpUnclamped(oldCamPos, zoomLocation, t);

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
        yield return new WaitForSeconds(zoomStayDuration); // Wait zoomed in

        timer = zoomOutDuration;
        while (timer > 0) // Zoom to normal
        {
            timer -= Time.deltaTime;

            float Easing(float x)
            {
                return x * x * x;
            }

            float t = Easing(1 - (timer / zoomOutDuration));

            _cam.transform.position = Vector3.Lerp(zoomLocation, oldCamPos, t);
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