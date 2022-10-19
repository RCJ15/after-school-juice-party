using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuJuiceManager : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Image background;
    [SerializeField] float timer;
    [SerializeField] float fadeTimer;
    bool _Fade = true;

    private Coroutine _currentEpilepsy;

    void Start()
    {

    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            if (_currentEpilepsy != null)
            {
                StopCoroutine(_currentEpilepsy);
            }

            _currentEpilepsy = StartCoroutine(Epilepsy());
        } 
        else if(_Fade)
        {
            if (_currentEpilepsy != null)
            {
                StopCoroutine(_currentEpilepsy);
            }

            _currentEpilepsy = StartCoroutine(Fade());
            _Fade = false;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        /*
        // Old epilepsy thingy
        if (Input.anykey)
        {
            background.color = Random.ColorHSV();
            faded = false;
        }
        else if(!faded)
        {
            background.CrossFadeColor(Color.white, 0.5f,true,false);

            faded = true;
        }
        */
    }

    private IEnumerator Epilepsy()
    {
        for (int i = 0; i < 2; i++)
        {
            background.color = Random.ColorHSV();

            yield return new WaitForSecondsRealtime (0.025f);
        }

        //background.CrossFadeColor(Color.white, 0.5f, true, false);

        Color startColor = background.color;

        float currentTimer = timer;

        while (currentTimer > 0)
        {
            currentTimer -= Time.unscaledDeltaTime;

            float t = currentTimer / timer;

            background.color = Color.Lerp(Color.white, startColor, t);

            yield return null;
        }

        _Fade = true;
    }
    private IEnumerator Fade()
    {
        float currentTimer = fadeTimer;
        Color startColor = background.color;

        while (currentTimer < 0)
        {
            currentTimer -= Time.unscaledDeltaTime;

            float t = currentTimer / fadeTimer;

            background.color = Color.Lerp(Random.ColorHSV(), startColor, t);

            yield return null;
        }

        background.CrossFadeColor(Random.ColorHSV(), fadeTimer, true, false);
        yield return new WaitForSecondsRealtime(fadeTimer);
        _Fade = true;
    }
}
