using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoScript : MonoBehaviour
{
    public int actualStage = 0;
    public bool move_1 = false;
    public bool trespas_2 = false;
    public bool attack_4 = false;
    public bool throw_5 = false;
    public bool parasite_6 = false;
    public bool suicide_7 = false;

    public GameObject Dummy;
    public GameObject Pickup;
    public GameObject Saw;

    public GameObject TriggerMove1;
    public GameObject TriggerTres1;
    public GameObject TriggerTres2;
    public GameObject TriggerAttacknThrow;
    public GameObject TriggerSuicide;

    public GameObject TextBlock1;
    public GameObject TextBlock2;
    public GameObject TextBlock3;

    public GameObject TextMove1;
    public GameObject TextMove2;
    public GameObject TextMove3;
    public GameObject TextMove4;

    public GameObject TextTrespass1;
    public GameObject TextTrespass2;
    public GameObject TextTrespass3;

    public GameObject TextPickup1;
    public GameObject TextPickup2;

    public GameObject TextAttack1;
    public GameObject TextAttack2;

    public GameObject TextThrow1;
    public GameObject TextThrow2;

    public GameObject TextPrasite1;
    public GameObject TextPrasite2;

    public GameObject TextSuicide1;

    void Start()
    {
        Dummy.SetActive(false);
        Pickup.SetActive(false);
        Saw.SetActive(false);
        //ENSEÑAR TEXTO EXPLICANDO EL MOVIMIENTO & SALTO
        TextBlock1.SetActive(true);
        TextMove1.SetActive(true);
        TextMove2.SetActive(true);
        TextMove3.SetActive(true);
        TextMove4.SetActive(true);

        TextBlock2.SetActive(false);
        TextTrespass1.SetActive(false);
        TextTrespass2.SetActive(false);
        TextTrespass3.SetActive(false);
        TextPickup1.SetActive(false);
        TextPickup2.SetActive(false);
        TextAttack1.SetActive(false);
        TextAttack2.SetActive(false);
        TextThrow1.SetActive(false);
        TextThrow2.SetActive(false);
        TextPrasite1.SetActive(false);
        TextPrasite2.SetActive(false);
        TextSuicide1.SetActive(false);
        TextBlock3.SetActive(false);
    }

    void Update()
    {
        if (move_1 == true && actualStage == 0)
        {
            //ESCONDER TEXTO EXPLICANDO EL MOVIMIENTO & SALTO
            TextMove1.SetActive(false);
            TextMove2.SetActive(false);
            TextMove3.SetActive(false);
            TextMove4.SetActive(false);
            actualStage = 1;
            TriggerMove1.SetActive(false);
            //ENSEÑAR TEXTO EXPLICANDO COMO TRASPASAR PLATAFORMAS
            TextTrespass1.SetActive(true);
            TextTrespass2.SetActive(true);
            TextTrespass3.SetActive(true);
            TextBlock1.SetActive(false);
            move_1 = false;
        }
        if (trespas_2 == true && actualStage == 1)
        {
            //ESCONDER TEXTO EXPLICANDO COMO TRASPASAR PLATAFORMAS
            TextTrespass1.SetActive(false);
            TextTrespass2.SetActive(false);
            TextTrespass3.SetActive(false);
            actualStage = 2;
            //ENSEÑAR TEXTO EXPLICANDO COMO PICKUP WEAPONS
            TextBlock2.SetActive(true);
            TextPickup1.SetActive(true);
            TextPickup2.SetActive(true);
            //SET ACTVIVE PICKUP
            Pickup.SetActive(true);
            trespas_2 = false;
        }
        if (!Pickup && actualStage == 2)
        {
            //ESCONDER TEXTO EXPLICANDO COMO PICKUP WEAPONS
            TextPickup1.SetActive(false);
            TextPickup2.SetActive(false);
            actualStage = 3;
            //ENSEÑAR TEXTO EXPLICANDO COMO ATACAR
            TextAttack1.SetActive(true);
            TextAttack2.SetActive(true);
            //SET ACTVIVE DUMMY
            Dummy.SetActive(true);
        }
        if (attack_4 == true && actualStage == 3)
        {
            //ESCONDER TEXTO EXPLICANDO COMO ATACAR
            TextAttack1.SetActive(false);
            TextAttack2.SetActive(false);
            actualStage = 4;
            //ENSEÑAR TEXTO EXPLICANDO COMO THROWEAR
            TextThrow1.SetActive(true);
            TextThrow2.SetActive(true);
            attack_4 = false;
        }
        if (throw_5 == true && actualStage == 4)
        {
            //ESCONDER TEXTO EXPLICANDO COMO THROWEAR 
            TextThrow1.SetActive(false);
            TextThrow2.SetActive(false);
            actualStage = 5;
            //ENSEÑAR TEXTO EXPLICANDO COMO PARASITAR Y PORQ SE HA DESTRUIDO EL ARMA (usos)
            TextPrasite1.SetActive(true);
            TextPrasite2.SetActive(true);
            throw_5 = false;
        }
        if (parasite_6 == true && actualStage == 5)
        {
            //ESCONDER TEXTO EXPLICANDO COMO PARASITAR
            TextPrasite1.SetActive(false);
            TextPrasite2.SetActive(false);
            actualStage = 6;
            //ENSEÑAR TEXTO EXPLICANDO COMO SUICIDARSE / GANAR
            TextSuicide1.SetActive(true);
            //SET ACTVIVE SAW
            Saw.SetActive(true);
            TriggerAttacknThrow.SetActive(false);
            parasite_6 = false;
        }
        if (suicide_7 == true && actualStage == 6)
        {
            //ESCONDER TEXTO EXPLICANDO COMO SUICIDARSE / GANAR
            TextBlock2.SetActive(false);
            TextSuicide1.SetActive(false);
            //end
            TriggerSuicide.SetActive(false);
            suicide_7 = false;
            PlayerManager.Instance.ScorePlayer1 = 1; 
            PlayerManager.Instance.Round = 1;
            PlayerManager.Instance.GameEnd = true;
            PlayerManager.Instance.DeleteProps = true;
            TextBlock3.SetActive(true);
        }
    }
}
