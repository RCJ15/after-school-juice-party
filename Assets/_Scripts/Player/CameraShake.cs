using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] float intensity = 0;
    [SerializeField] Camera camera;
    [SerializeField] bool shake;

    Vector3 _StartPos;

    // Start is called before the first frame update
    void Start()
    {
        _StartPos = camera.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (shake)
        {
            Vector3 pos = Random.insideUnitCircle * intensity;
            pos.z = _StartPos.z;
            camera.transform.position = pos;
        }
        else if(camera.transform.position != _StartPos)
        {
            camera.transform.position = _StartPos;
        }
    }
}
