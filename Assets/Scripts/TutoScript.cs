using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoScript : MonoBehaviour
{
    bool move_1 = false;
    bool trespas_2 = false;
    bool pickup_3 = false;
    bool attack_4 = false;
    bool throw_5 = false;
    bool parasite_6 = false;
    bool suicide_7 = false;
    int actualStage = 0;

    public GameObject Dummy;
    private DummyController DummyScript;
    public GameObject Pickup;
    public GameObject Saw;

    void Start()
    {
        Dummy.SetActive(false);
        Pickup.SetActive(false);
        Saw.SetActive(false);
        //ENSEÑAR TEXTO EXPLICANDO EL MOVIMIENTO & SALTO
    }

    void Update()
    {
        if (move_1 == true)
        {
            //ESCONDER TEXTO EXPLICANDO EL MOVIMIENTO & SALTO
            actualStage = 1;
            //ENSEÑAR TEXTO EXPLICANDO COMO TRASPASAR PLATAFORMAS
            move_1 = false;
        }
        if (trespas_2 == true && actualStage == 1)
        {
            //ESCONDER TEXTO EXPLICANDO COMO TRASPASAR PLATAFORMAS
            actualStage = 2;
            //ENSEÑAR TEXTO EXPLICANDO COMO PICKUP WEAPONS
            //SET ACTVIVE WEAPON
            Pickup.SetActive(true);
            trespas_2 = false;
        }
        if (pickup_3 == true && actualStage == 2)
        {
            //ESCONDER TEXTO EXPLICANDO COMO PICKUP WEAPONS
            actualStage = 3;
            //ENSEÑAR TEXTO EXPLICANDO COMO ATACAR
            //SET ACTVIVE DUMMY
            Dummy.SetActive(true);
            pickup_3 = false;
        }
        if (attack_4 == true && actualStage == 3)
        {
            //ESCONDER TEXTO EXPLICANDO COMO ATACAR
            actualStage = 4;
            //ENSEÑAR TEXTO EXPLICANDO COMO THROWEAR
            attack_4 = false;
        }
        if (throw_5 == true && actualStage == 4)
        {
            //ESCONDER TEXTO EXPLICANDO COMO THROWEAR 
            actualStage = 5;
            //ENSEÑAR TEXTO EXPLICANDO COMO PARASITAR Y PORQ SE HA DESTRUIDO EL ARMA (usos)
            throw_5 = false;
        }
        if (parasite_6 == true && actualStage == 5)
        {
            //ESCONDER TEXTO EXPLICANDO COMO PARASITAR
            actualStage = 6;
            //ENSEÑAR TEXTO EXPLICANDO COMO SUICIDARSE / GANAR
            //SET ACTVIVE SAW
            Saw.SetActive(true);
            parasite_6 = false;
        }
        if (suicide_7 == true && actualStage == 6)
        {
            //ESCONDER TEXTO EXPLICANDO COMO SUICIDARSE / GANAR
            //end
            suicide_7 = false;
        }
    }
}
