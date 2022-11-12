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
    TransitionController transControl;
    [SerializeField] Image background;
    [SerializeField] float timetimer;
    [SerializeField] float fadeTimer;
    //bool _Fade = true;
    [SerializeField] GameObject blueScreen;

    private Coroutine _currentEpilepsy;
    private void Start()
    {
        transControl = transition.GetComponent<TransitionController>();
        mainPanel.SetActive(false);
        optionPanel.SetActive(false);

        //transControl.Transition(0, 0, 1);

        mainPanel.SetActive(true);
    }

    private void Update()
    {
        /*
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
        */
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

    /*
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
    */

    public void Begin()
    {
        transControl.Transition(0.75f, 1, 1, () => ChageScene(1));
    }
    public void Options()
    {
        /*
        transControl.Transition(1, 1, 1, ActivatePanel);
        void ActivatePanel()
        {
            optionPanel.SetActive(true);
            mainPanel.SetActive(false);
        }
        */
        optionPanel.SetActive(true);
        mainPanel.SetActive(false);
    }
    public void Back()
    {
        /*
        transControl.Transition(1, 1, 1, ActivatePanel);
        void ActivatePanel()
        {
            optionPanel.SetActive(false);
            mainPanel.SetActive(true);
        }
        */
        optionPanel.SetActive(false);
        mainPanel.SetActive(true);
    }
    public void Crash()
    {
        StartCoroutine(ActualCrash());
    }

    private IEnumerator ActualCrash()
    {
        Time.timeScale = 0;
        foreach (var juice in FindObjectsOfType<ButtonJuice>())
        {
            juice.enabled = false;
        }

        yield return new WaitForSecondsRealtime(0.46f);

        transControl.Transition(0, 0.389f, 0, () => StartCoroutine(BlueScreenOfDeath()));

        // Save stuff

        IEnumerator BlueScreenOfDeath()
        {
            blueScreen.SetActive(true); // Show
            yield return new WaitForSecondsRealtime(3.534f); // Wait
            Application.Quit(); // Turn off game

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }

    void ChageScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }
}