using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TutoTrigger { MOVE, TRESPASS, PICKUP, SUICIDE };

public class TriggerScript : MonoBehaviour
{
    public TutoTrigger trigger = TutoTrigger.MOVE;

    public GameObject Tuto;
    private TutoScript TutoScript;

    void Start()
    {
        TutoScript = Tuto.GetComponent<TutoScript>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            switch (trigger)
            {
                default:
                    break;
                case TutoTrigger.MOVE:
                    TutoScript.move_1 = true;
                    break;
                case TutoTrigger.TRESPASS:
                    TutoScript.trespas_2 = true;
                    break;
                case TutoTrigger.PICKUP:
                    TutoScript.pickup_3 = true;
                    break;
                case TutoTrigger.SUICIDE:
                    TutoScript.suicide_7 = true;
                    break;
            }
        }

        if (collision.gameObject.tag == "Attacking")
        {
            TutoScript.attack_4 = true;
        }

        if (collision.gameObject.tag == "Throwing")
        {
            TutoScript.throw_5 = true;
        }

        if (collision.gameObject.tag == "FlyingHead")
        {
            TutoScript.parasite_6 = true;
        }
    }

    private void OnCollision(Collision2D collision)
    {

    }
}
