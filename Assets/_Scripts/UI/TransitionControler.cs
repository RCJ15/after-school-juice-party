using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionControler : MonoBehaviour
{
    [SerializeField] bool startTransition;
    [SerializeField] float startFadeIn, startStay, startFadeout;

    float _fadeIn;
    float _stay;
    float _fadeout;
    IEnumerator _function;
    UnityEngine.UI.Image image;

    private void Start()
    {
        image = GetComponent<UnityEngine.UI.Image>();
        if (startTransition)
        {
            Transition(startFadeIn, startStay, startFadeout);
        }
    }
    /// <summary>
    /// Fade the screen to a image and runa corutine while fided
    /// </summary>
    /// <param name="fadeInTime">Time to fade in</param>
    /// <param name="stayTime">Time to stay faded</param>
    /// <param name="fadeoutTime">Time to fade image out</param>
    /// <param name="function">Function to run in the middel of transition</param>
    public void Transition(float fadeInTime, float stayTime, float fadeoutTime, IEnumerator function = null)
    {
        _fadeIn = fadeInTime;
        _stay = stayTime;
        _fadeout = fadeoutTime;
        _function = function;
        StartCoroutine(Fade()); // Call Function
    }
    private IEnumerator Fade()
    {
        float timer = _fadeIn;
        Color clr = image.color;

        while (timer > 0) // Fade in
        {
            timer -= Time.deltaTime;
            float t = Easing.Same(1 - (timer / _fadeIn));
            clr = image.color;
            image.color = new Color(clr.r, clr.g, clr.b, Mathf.Lerp(0, 1, t));
            yield return null;
        }
        image.color = new Color(clr.r, clr.g, clr.b, 1); // Incase timer <= 0

        yield return new WaitForSeconds(_stay); // Stay in transition

        if (_function != null)
        {
            StartCoroutine(_function); // Call a function in the middel of transition
        }
        timer = _fadeout;

        while (timer > 0) // Fade out
        {
            timer -= Time.deltaTime;
            float t = Easing.Same(1 - (timer / _fadeout));
            clr = image.color;
            image.color = new Color(clr.r, clr.g, clr.b, Mathf.Lerp(1, 0, t));
            yield return null;
        }
        image.color = new Color(clr.r, clr.g, clr.b, 0); // Incase timer <= 0

        // Reset value
        _fadeIn = -1;
        _stay = -1;
        _fadeout = -1;
        _function = null;
    }
}