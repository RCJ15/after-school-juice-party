using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonJuice : MonoBehaviour
{
    [Header("Transform")]
    [SerializeField] Vector2 rngspeedX;
    [SerializeField] Vector2 rngspeedY;
    [SerializeField] Vector2 rngintensityX;
    [SerializeField] Vector2 rngintensityY;
    [Space(10)]
    [SerializeField] Vector2 rndSizeX;
    [SerializeField] Vector2 rndSizeY;
    [Space(5)]
    [SerializeField] Vector2 rndSizeIntensityX;
    [SerializeField] Vector2 rndSizeIntensityY;
    [Space]
    [Header("Rotation")]
    [SerializeField] Vector2 rngspeedRot;
    [SerializeField] Vector2 rngintensityRot;
    [Space]
    [Header("Offset")]
    [SerializeField] float timer;
    [SerializeField] float fadeTimer;
    [SerializeField] bool devideHundred = false;

    float _SpeedY, _SpeedX, _IntensityY, _IntensityX, _SizeX, _SizeY, _SizeIntensityX, _SizeIntensityY, _SpeedRot, _IntensityRot;

    private Vector3 _StartPos;
    private Vector3 _StartScale;
  [  SerializeField]  UnityEngine.UI.Image background;
    bool _Fade = true;

    private void Start()
    {
        // Assign random number
        _SpeedY = Random.Range(rngspeedY.x, rngspeedY.y);
        _SpeedX = Random.Range(rngspeedX.x, rngspeedX.y);
        _IntensityY = Random.Range(rngintensityY.x, rngintensityY.y);
        _IntensityX = Random.Range(rngintensityX.x, rngintensityX.y);
        _SpeedRot = Random.Range(rngspeedRot.x, rngspeedRot.y);
        _IntensityRot = Random.Range(rngintensityRot.x, rngintensityRot.y);
        _SizeX = Random.Range(rndSizeX.x, rndSizeX.y);
        _SizeY = Random.Range(rndSizeY.x, rndSizeY.y);
        _SizeIntensityX = Random.Range(rndSizeIntensityX.x, rndSizeIntensityX.y);
        _SizeIntensityY = Random.Range(rndSizeIntensityY.x, rndSizeIntensityY.y);

        // Save position and scale
        _StartPos = transform.position;
        _StartScale = transform.localScale;
        
        if(background == null)
        {
            // Get background image
            background = transform.GetComponent<UnityEngine.UI.Image>(); 
        }
    }
    private void Update()
    {
        timer += Time.unscaledDeltaTime;



        float TempIntensityY = _IntensityY;
        float TempIntensityRot = _IntensityRot;
        float TempIntensityX = _IntensityX;
        float TempSizeIntensityY = _SizeIntensityY;
        float TempSizeIntensityX = _SizeIntensityX;

        float TempSpeedY = _SpeedY;
        float TempSpeedRot = _SpeedRot;
        float TempSpeedX = _SpeedX;
        float TempSizeX = _SizeX;
        float TempSizeY = _SizeY;


        if (devideHundred)
        {
            TempIntensityY = _IntensityY / 100;
            TempIntensityRot = _IntensityRot / 100;
            TempIntensityX = _IntensityX / 100;
            TempSizeIntensityY = _SizeIntensityY / 100;
            TempSizeIntensityX = _SizeIntensityX / 100;

            TempSpeedY = _SpeedY / 100;
            TempSpeedRot = _SpeedRot / 100;
            TempSpeedX = _SpeedX / 100;
            TempSizeX = _SizeX / 100;
            TempSizeY = _SizeY / 100;
        }

        //Get sin wave
        float sinMoveY = Mathf.Sin(timer * TempSpeedY) * TempIntensityY;
        float sinRot = Mathf.Sin(timer * TempSpeedRot) * TempIntensityRot;
        float sinMoveX = Mathf.Sin(timer * TempSpeedX) * TempIntensityX;
        float sinScaleX = Mathf.Sin(timer * TempSizeX) * TempSizeIntensityX;
        float sinScaleY = Mathf.Sin(timer * TempSizeY) * TempSizeIntensityY;

        // Position
        transform.position = new Vector3(_StartPos.x + sinMoveX, _StartPos.y + sinMoveY, _StartPos.z);

        // Rotation
        Vector3 pos = transform.localEulerAngles;
        transform.localRotation = Quaternion.Euler(pos.x, pos.y, sinRot);

        // Size
        transform.localScale = new Vector3(_StartScale.x + sinScaleX, _StartScale.y + sinScaleY, _StartScale.z);

        if (_Fade) // Fade background
        {
            StartCoroutine(Fade());
            _Fade = false;
        }
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
        yield return new WaitForSecondsRealtime (fadeTimer);
        _Fade = true;
    }
}