using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour  //Emma. Den här koden spawn:ar fienderna i "Vågor"/"Formationer" ifall vi vill ha en
                                    //konstant ström av fiender måste vi fixa felspawn koden eftersom det är det den har.
{
    //Hur mycket från den första fienden på raden som den andra fienden ska placeras (blir noll mellan varje rad)
    private float enemyPlace = 0;

    //Nummer på formation (Just nu finns det bara 2).
    private int formationNum;

    [SerializeField]
    GameObject enemy;

    // Start is called before the first frame update
    void Start()
    {
        //Väljer slumpmässigt formation
        formationNum = Random.Range(1, 3);
        if (formationNum == 1)
        {
            FormationOne();
        }
        else
        {
            FormationTwo();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    //Antagligen den lättaste (att besegra)
    public void FormationOne()
    {
        for (int i = 0; i < 5; i++)
        {
            //Första "raden". Två fiender.
            if (i < 2)
            {
                Instantiate(enemy, new Vector3(-1 + enemyPlace, 5.5f, 0), Quaternion.identity);

                enemyPlace += 1;

                //enemyPlace blir noll inför placeringen av rad 2
                if (i == 1)
                {
                    enemyPlace = 0;
                }
            }
            //Andra "raden". Tre fiender.
            else if (i >= 2)
            {
                Instantiate(enemy, new Vector3(-1.5f + enemyPlace, 7, 0), Quaternion.identity);

                enemyPlace += 1;

                //Gör båda till 0 på slutet ifall vi vill göra så att en ny våg kommer efter (Vilket vi antagligen vill?)
                //(Just nu kommer det ingen ny våg)
                if (i == 4)
                {
                    enemyPlace = 0;
                    formationNum = 0;
                }
            }
            
        }
    }

    public void FormationTwo()
    {
        for (int i = 0; i < 23; i++)
        {
            //Första "raden". En fiende.
            if (i == 0)
            {
                Instantiate(enemy, new Vector3(-0.5f + enemyPlace, 5.5f, 0), Quaternion.identity);
                
            }
            //Andra "raden". Två fiender.
            else if (i >= 1 && i < 3)
            {
                Instantiate(enemy, new Vector3(-1 + enemyPlace, 7, 0), Quaternion.identity);

                enemyPlace += 1;
                
                if (i == 2)
                {
                    enemyPlace = 0; 
                }
            }
            //Tre fiender
            else if (i >= 3 && i < 6)
            {
                Instantiate(enemy, new Vector3(-1.5f + enemyPlace, 8.5f, 0), Quaternion.identity);

                enemyPlace += 1;

                if (i == 5)
                {
                    enemyPlace = 0;
                }
            }
            //Åtta fiender
            else if (i >= 6 && i < 14)
            {
                Instantiate(enemy, new Vector3(-4f + enemyPlace, 10, 0), Quaternion.identity);

                enemyPlace += 1;
                
                if(i == 13)
                {
                    enemyPlace = 0;
                }

            }
            //Nio fiender
            else if (i >= 14)
            {
                Instantiate(enemy, new Vector3(-4.5f + enemyPlace, 11.5f, 0), Quaternion.identity);

                enemyPlace += 1;

                //Gör båda till 0 på slutet ifall vi vill göra så att en ny våg kommer efter (Vilket vi antagligen vill?)
                //(Just nu kommer det ingen ny våg)
                if (i == 22)
                {
                    enemyPlace = 0;
                    formationNum = 0;
                }

            }
        }
    }
}
