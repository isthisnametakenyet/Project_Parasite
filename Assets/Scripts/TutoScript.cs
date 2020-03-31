using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoScript : MonoBehaviour
{
    public int actualStage = 0;
    public bool move_1 = false;
    public bool trespas_2 = false;
    public bool pickup_3 = false;
    public bool attack_4 = false;
    public bool throw_5 = false;
    public bool parasite_6 = false;
    public bool suicide_7 = false;

    public GameObject Dummy;
    private DummyController DummyScript;
    public GameObject Pickup;
    public GameObject Saw;

    public GameObject TriggerMove1;
    public GameObject TriggerTres1;
    public GameObject TriggerTres2;
    public GameObject TriggerPick1;
    public GameObject TriggerAttacknThrow;
    public GameObject TriggerSuicide;

    void Start()
    {
        Dummy.SetActive(false);
        Pickup.SetActive(false);
        Saw.SetActive(false);
        //ENSEÑAR TEXTO EXPLICANDO EL MOVIMIENTO & SALTO
    }

    void Update()
    {
        if (move_1 == true && actualStage == 0)
        {
            //ESCONDER TEXTO EXPLICANDO EL MOVIMIENTO & SALTO
            actualStage = 1;
            TriggerMove1.SetActive(false);
            //ENSEÑAR TEXTO EXPLICANDO COMO TRASPASAR PLATAFORMAS
            move_1 = false;
        }
        if (trespas_2 == true && actualStage == 1)
        {
            //ESCONDER TEXTO EXPLICANDO COMO TRASPASAR PLATAFORMAS
            actualStage = 2;
            TriggerTres1.SetActive(false);
            TriggerTres2.SetActive(false);
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
            TriggerPick1.SetActive(false);
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
            TriggerAttacknThrow.SetActive(false);
            parasite_6 = false;
        }
        if (suicide_7 == true && actualStage == 6)
        {
            //ESCONDER TEXTO EXPLICANDO COMO SUICIDARSE / GANAR
            //end
            TriggerSuicide.SetActive(false);
            suicide_7 = false;
        }
    }
}
