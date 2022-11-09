using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionController : MonoBehaviour
{
    [SerializeField] bool startTransition;
    [SerializeField] float startFadeIn, startStay, startFadeout;

    private Coroutine cor;
    private UnityEngine.UI.Image image;

    private void Start()
    {
        image = GetComponent<UnityEngine.UI.Image>();
        if (startTransition)
        {
            Transition(startFadeIn, startStay, startFadeout);
        }
    }

    /// <summary>
    /// Fade the screen to a image and run a corutine while faded
    /// </summary>
    /// <param name="fadeInTime">Time to fade in</param>
    /// <param name="stayTime">Time to stay faded</param>
    /// <param name="fadeoutTime">Time to fade image out</param>
    /// <param name="function">Function to run in the middle of transition</param>
    public void Transition(float fadeInTime, float stayTime, float fadeoutTime, System.Action function = null)
    {
        if (cor != null) // Stop previous transition
        {
            StopCoroutine(cor);
        }

        cor = StartCoroutine(Fade(fadeInTime, stayTime, fadeoutTime, function)); // Call Function
    }
    private IEnumerator Fade(float fadeInTime, float stayTime, float fadeoutTime, System.Action function = null)
    {
        float timer = fadeInTime;
        Color clr = image.color;

        while (timer > 0) // Fade in
        {
            timer -= Time.deltaTime;
            float t = 1 - (timer / fadeInTime);
            clr = image.color;
            image.color = new Color(clr.r, clr.g, clr.b, t);
            yield return null;
        }
        image.color = new Color(clr.r, clr.g, clr.b, 1); // Incase timer <= 0

        if (stayTime > 0)
        {
            yield return new WaitForSecondsRealtime(stayTime); // Stay in transition
        }

        function?.Invoke();

        timer = fadeoutTime;

        while (timer > 0) // Fade out
        {
            timer -= Time.deltaTime;
            float t = timer / fadeoutTime;
            clr = image.color;
            image.color = new Color(clr.r, clr.g, clr.b, t);
            yield return null;
        }
        image.color = new Color(clr.r, clr.g, clr.b, 0); // Incase timer <= 0
    }
}