using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour  //Emma. Den h�r koden spawn:ar fienderna i "V�gor"/"Formationer" ifall vi vill ha en
                                    //konstant str�m av fiender m�ste vi fixa felspawn koden eftersom det �r det den har.
{
    //Hur mycket fr�n den f�rsta fienden p� raden som den andra fienden ska placeras (blir noll mellan varje rad)
    private float enemyPlace = 0;

    //Nummer p� formation (Just nu finns det bara 2).
    private int formationNum;

    [SerializeField]
    GameObject enemy;

    // Start is called before the first frame update
    void Start()
    {
        //V�ljer slumpm�ssigt formation
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
    //Antagligen den l�ttaste (att besegra)
    public void FormationOne()
    {
        for (int i = 0; i < 5; i++)
        {
            //F�rsta "raden". Tv� fiender.
            if (i < 2)
            {
                Instantiate(enemy, new Vector3(-1 + enemyPlace, 5.5f, 0), Quaternion.identity);

                enemyPlace += 1;

                //enemyPlace blir noll inf�r placeringen av rad 2
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

                //G�r b�da till 0 p� slutet ifall vi vill g�ra s� att en ny v�g kommer efter (Vilket vi antagligen vill?)
                //(Just nu kommer det ingen ny v�g)
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
            //F�rsta "raden". En fiende.
            if (i == 0)
            {
                Instantiate(enemy, new Vector3(-0.5f + enemyPlace, 5.5f, 0), Quaternion.identity);
                
            }
            //Andra "raden". Tv� fiender.
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
            //�tta fiender
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

                //G�r b�da till 0 p� slutet ifall vi vill g�ra s� att en ny v�g kommer efter (Vilket vi antagligen vill?)
                //(Just nu kommer det ingen ny v�g)
                if (i == 22)
                {
                    enemyPlace = 0;
                    formationNum = 0;
                }

            }
        }
    }
}
