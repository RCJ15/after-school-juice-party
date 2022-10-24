using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    private float timer = 0;

    private int enemyPlace = 0;

    [SerializeField]
    GameObject enemy;

    // Start is called before the first frame update
    void Start()
    {
        FormationOne();
    }

    // Update is called once per frame
    void Update()
    {
        

        /*timer += Time.deltaTime;

       if (timer >= 5)
       {
           //Spawn:ar en fiende
           Instantiate(enemy, transform.position, Quaternion.identity);

           timer = 0;
       }*/

    }
    //Antagligen den lätaste
    public void FormationOne()
    {
        for (int i = 0; i < 5; i++)
        {
            //Första "raden"
            if (i < 2)
            {
                Instantiate(enemy, new Vector3(-2 + enemyPlace, 5.5f, 0), Quaternion.identity);

                enemyPlace += 2;

                //enemyPlace blir noll inför placeringen av rad 2
                if (i == 1)
                {
                    enemyPlace = 0;
                }
            }
            //Andra "raden"
            else if (i >= 2)
            {
                Instantiate(enemy, new Vector3(-3 + enemyPlace, 7.5f, 0), Quaternion.identity);

                enemyPlace += 2;
            }
            
        }
    }
}
