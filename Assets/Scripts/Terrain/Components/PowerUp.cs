using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{



  
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {

            int number = Random.Range(1,4);

            switch (number)
            {


                case 1:

                    PlayerController.incBullet = true;
                    Debug.Log("Double bullets");
                    break;

                case 2:

                    PlayerController.collision++;
                    Debug.Log("Collision increases");
                    break;

                case 3:

                    PlayerController.incSpeed += 30;
                    Debug.Log(PlayerController.incSpeed);
                    Debug.Log("Bullet Speed increases");
                    break;




            }

            Destroy(gameObject);


        }

        }

}
