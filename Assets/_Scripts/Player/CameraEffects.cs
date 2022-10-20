using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffects : MonoBehaviour
{
    [SerializeField] Camera camera;
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
        _StartPos = camera.transform.position; // Start position of camera
        flashImage.color = new Color(flashImage.color.r, flashImage.color.g, flashImage.color.b, 0); // Hide flash
    }

    // Update is called once per frame
    void Update()
    {
        if (shake)
        {
            Vector3 pos = Random.insideUnitCircle * intensity; // Shake
            pos.z = _StartPos.z; // Remove z axis value
            camera.transform.position = pos; // Apply to camera
        }
        else if(camera.transform.position != _StartPos) // If position is not start position
        {
            camera.transform.position = _StartPos; // Reset position
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
            zoom = false;
            if (_CurrentZoomRoutine != null)
            {
                StopCoroutine(_CurrentZoomRoutine); // Stop current flash
            }
            _CurrentZoomRoutine = StartCoroutine(Zoom()); // Start flash
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
            flashImage.color = new Color(flashImage.color.r, flashImage.color.g, flashImage.color.b, timer/fadeDuration); // Apply fade
            yield return null;
        }

        flashImage.color = new Color(flashImage.color.r, flashImage.color.g, flashImage.color.b, 0); // Set alpha to zero after done to be sure
    }
    IEnumerator Zoom()
    {
        float timer = zoomInDuration; ;
        while (timer >0) // Zoom to in
        {
            timer -= Time.deltaTime;
            camera.orthographicSize = Mathf.Lerp(zoomedIn, zoomedOut, timer/zoomInDuration);
            yield return null;
        }
        yield return new WaitForSeconds(zoomStayDuration); // Wait zoomed in
        timer = zoomOutDuration;
        while (timer > 0) // Zoom to normal
        {
            timer -= Time.deltaTime;
            camera.orthographicSize = Mathf.Lerp(zoomedOut, zoomedIn, timer/zoomOutDuration);
            yield return null;
        }
        camera.orthographicSize = zoomedOut;
    }
}
