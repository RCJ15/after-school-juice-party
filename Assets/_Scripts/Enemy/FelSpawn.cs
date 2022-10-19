using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FelSpawn : MonoBehaviour  //Emma
    //Antagligen fel (�r till felEnemy). R�tt finns p� Spawn.
{
    private float timer = 0;

    [SerializeField]
    GameObject enemy;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 5)
        {
            //Spawn:ar en fiende
            Instantiate(enemy, transform.position, Quaternion.identity);

            timer = 0;
        }
    }
}
