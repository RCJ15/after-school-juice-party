using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour  //Emma
{
    //private float timer = 0;

    //Hur mycket fr�n den f�rsta fienden p� raden som den andra fienden ska placeras
    private int enemyPlace = 0;

    //Nummer p� formation
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
    //Antagligen den l�taste
    public void FormationOne()
    {
        for (int i = 0; i < 5; i++)
        {
            //F�rsta "raden". Tv� fiender.
            if (i < 2)
            {
                Instantiate(enemy, new Vector3(-3 + enemyPlace, 5.5f, 0), Quaternion.identity);

                enemyPlace += 2;

                //enemyPlace blir noll inf�r placeringen av rad 2
                if (i == 1)
                {
                    enemyPlace = 0;
                }
            }
            //Andra "raden". Tre fiender.
            else if (i >= 2)
            {
                Instantiate(enemy, new Vector3(-4 + enemyPlace, 7.5f, 0), Quaternion.identity);

                enemyPlace += 2;
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
                Instantiate(enemy, new Vector3(-2 + enemyPlace, 5.5f, 0), Quaternion.identity);
                
            }
            //Andra "raden": Tv� fiender.
            else if (i >= 1 && i < 3)
            {
                Instantiate(enemy, new Vector3(-3 + enemyPlace, 7.5f, 0), Quaternion.identity);

                enemyPlace += 2;
                
                if (i == 2)
                {
                    enemyPlace = 0; 
                }
            }
            //Tre fiender
            else if (i >= 3 && i < 6)
            {
                Instantiate(enemy, new Vector3(-4 + enemyPlace, 9.5f, 0), Quaternion.identity);

                enemyPlace += 2;

                if (i == 5)
                {
                    enemyPlace = 0;
                }
            }
            //�tta fiender
            else if (i >= 6 && i < 14)
            {
                Instantiate(enemy, new Vector3(-9 + enemyPlace, 11.5f, 0), Quaternion.identity);

                enemyPlace += 2;
                
                if(i == 13)
                {
                    enemyPlace = 0;
                }

            }
            //Nio fiender
            else if (i >= 14)
            {
                Instantiate(enemy, new Vector3(-10 + enemyPlace, 13.5f, 0), Quaternion.identity);

                enemyPlace += 2;

            }
        }
    }
}
