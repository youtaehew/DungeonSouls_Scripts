using System;
using System.Collections;
using System.Collections.Generic;
using PsychoticLab;
using UnityEngine;

public class ChangeAppearance : MonoBehaviour
{
    [SerializeField] private GameObject[] manBody;

    [SerializeField] private GameObject[] girlBody;

    private void Awake()
    {
        for (int i = 0; i < girlBody.Length; i++)
        {
            girlBody[i].SetActive(false);
            manBody[i].SetActive(false);
        }

        GameObject[] body;
        if (GameManager.instance.data.gender == Gender.Male)
        {
            body = manBody;
        }
        else
        {
            body = girlBody;
        }

        for (int i = 0; i < body.Length; i++)
        {
            Human human = (Human)i;
            
            body[i].SetActive(true);
            for (int a = 0; a < 13; a++)
            {
                body[(byte)human].transform.GetChild(a).gameObject.SetActive(false);
            }

            if (human == Human.ArmUp_right ||human ==  Human.ArmUp_left)
            {
                for (int a = 0; a < 13; a++)
                {
                    body[(byte)human].transform.GetChild(a).gameObject.SetActive(false);
                    body[(byte)Human.ArmUp_left].transform.GetChild(a).gameObject.SetActive(false);
                }
                    body[(byte)Human.ArmUp_right].transform.GetChild(GameManager.instance.data.costume[Human.ArmUp_right]).gameObject.SetActive(true);
                    body[(byte)Human.ArmUp_left].transform.GetChild(GameManager.instance.data.costume[Human.ArmUp_right]).gameObject.SetActive(true);

            }
            else if (human == Human.ArmDown_right ||human ==  Human.ArmDown_left)
            {
                for (int a = 0; a < 13; a++)
                {
                    body[(byte)human].transform.GetChild(a).gameObject.SetActive(false);
                    body[(byte)Human.ArmDown_left].transform.GetChild(a).gameObject.SetActive(false);
                }
                    
                    body[(byte)Human.ArmDown_right].transform.GetChild(GameManager.instance.data.costume[Human.ArmDown_right]).gameObject.SetActive(true);
                    body[(byte)Human.ArmDown_left].transform.GetChild(GameManager.instance.data.costume[Human.ArmDown_right]).gameObject.SetActive(true);

        
            }
            else if (human == Human.Hand_right ||human ==  Human.Hand_left)
            {
                for (int a = 0; a < 13; a++)
                {
                    body[(byte)human].transform.GetChild(a).gameObject.SetActive(false);
                    body[(byte)Human.Hand_left].transform.GetChild(a).gameObject.SetActive(false);
                }
                
                body[(byte)Human.Hand_right].transform.GetChild(GameManager.instance.data.costume[Human.Hand_right]).gameObject.SetActive(true);
                body[(byte)Human.Hand_left].transform.GetChild(GameManager.instance.data.costume[Human.Hand_right]).gameObject.SetActive(true);
            }
            else if (human == Human.Leg_right ||human ==  Human.Leg_left)
            {
                for (int a = 0;a < 13; a++)
                {
                    body[(byte)human].transform.GetChild(a).gameObject.SetActive(false);
                    body[(byte)Human.Leg_left].transform.GetChild(a).gameObject.SetActive(false);
                }
                
                body[(byte)Human.Leg_right].transform.GetChild(GameManager.instance.data.costume[Human.Leg_right]).gameObject.SetActive(true);
                body[(byte)Human.Leg_left].transform.GetChild(GameManager.instance.data.costume[Human.Leg_right]).gameObject.SetActive(true);
            }
            else
            {
                for (int a = 0;a < 13; a++)
                {
                    body[(byte)human].transform.GetChild(GameManager.instance.data.costume[human]).gameObject.SetActive(true);

                }
            }

        }
    }
}