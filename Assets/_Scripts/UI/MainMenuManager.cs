using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject mainPanel;
    [SerializeField] GameObject optionPanel;
    [SerializeField] GameObject transition;
    TransitionControler transControl;
    [SerializeField] Image background;
    [SerializeField] float timetimer;
    [SerializeField] float fadeTimer;
    bool _Fade = true;
    [SerializeField] GameObject blueScreen;

    private Coroutine _currentEpilepsy;
    private void Start()
    {
        transControl = transition.GetComponent<TransitionControler>();
        mainPanel.SetActive(false);
        optionPanel.SetActive(false);

        // Start Song
        transControl.Transition(0, 5, 10, ActivatePanel());
        IEnumerator ActivatePanel()
        {
            mainPanel.SetActive(true); ;
            yield return null;
        }
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
        else if (_Fade)
        {
            if (_currentEpilepsy != null)
            {
                StopCoroutine(_currentEpilepsy);
            }

            _currentEpilepsy = StartCoroutine(Fade(background));
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

            yield return new WaitForSecondsRealtime(0.025f);
        }

        //background.CrossFadeColor(Color.white, 0.5f, true, false);

        Color startColor = background.color;

        float currentTimer = timetimer;

        while (currentTimer > 0)
        {
            currentTimer -= Time.unscaledDeltaTime;

            float t = currentTimer / timetimer;

            background.color = Color.Lerp(Color.white, startColor, t);

            yield return null;
        }

        _Fade = true;
    }
    private IEnumerator Fade(Image img)
    {
        float currentTimer = fadeTimer;
        Color startColor = img.color;

        while (currentTimer < 0)
        {
            currentTimer -= Time.unscaledDeltaTime;

            float t = currentTimer / fadeTimer;

            img.color = Color.Lerp(Random.ColorHSV(), startColor, t);

            yield return null;
        }

        img.CrossFadeColor(Random.ColorHSV(), fadeTimer, true, false);
        yield return new WaitForSecondsRealtime(fadeTimer);
        _Fade = true;
    }

    public void Begin()
    {
        transControl.Transition(0.75f, 1, 1, ChageScene(1));
    }
    public void Options()
    {
        transControl.Transition(1, 1, 1, ActivatePanel());
        IEnumerator ActivatePanel()
        {
            optionPanel.SetActive(true);
            mainPanel.SetActive(false);
            yield return null;
        }
    }
    public void Back()
    {
        transControl.Transition(1, 1, 1, ActivatePanel());
        IEnumerator ActivatePanel()
        {
            optionPanel.SetActive(false);
            mainPanel.SetActive(true);
            yield return null;
        }
    }
    public void Crash()
    {
        transControl.Transition(1, 3, 0,BlueScreenOfDeath());
        // Save stuff

        IEnumerator BlueScreenOfDeath()
        {
        blueScreen.SetActive(true); // Show
            yield return new WaitForSeconds(10); // Wait
        Application.Quit(); // Turn off game
        }
    }
    IEnumerator ChageScene(int scene)
    {
        SceneManager.LoadScene(scene);
        yield return null;
    }
}