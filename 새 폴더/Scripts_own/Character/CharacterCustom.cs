using System;
using System.Collections;
using System.Collections.Generic;
using PsychoticLab;
using UnityEngine;
using UnityEngine.Rendering.UI;

public enum Human
{
    Head=0,
    Eye=1,
    Hair,
    Body,
    ArmUp_right,
    ArmDown_right,
    Hand_right,
    Hip,
    Leg_right,
    ArmUp_left,
    ArmDown_left,
    Hand_left,
    Leg_left,
}

public class CharacterCustom : MonoBehaviour
{
    private int index = 0;
    private Human humanState;

    [SerializeField] private CharacterRandomizer characterRandomizer;
    private GameObject[] humanPart;
    
    [SerializeField] private GameObject[] Button;
    private Gender gender;

    [SerializeField] private CharacterRandomizer custom;
    int defaultMax = 11;
    [SerializeField]
    private GameObject genderPanel;
    [SerializeField]
    private GameObject rawImage;

    [SerializeField] private GameObject finishPanel;


    public void Chooseman()
    {
        CustomSound.instance.choosePlaySound();
        genderPanel.SetActive(false);
        rawImage.SetActive(true);
        index = 0;
        gender = Gender.Male;
        GameManager.instance.data.gender = gender;
        characterRandomizer.ChangeGender(gender);
        changeButton(Human.Head);
    }

    public void Choosewoman()
    {
        CustomSound.instance.choosePlaySound();
        genderPanel.SetActive(false);
        rawImage.SetActive(true);
        
        index = 0;
        gender = Gender.Female;
        characterRandomizer.ChangeGender(gender);
        GameManager.instance.data.gender = gender;
        changeButton(Human.Head);
    }

    private void changeButton(Human s)
    {
        for (int i = 0; i < Button.Length; i++)
        {
            Button[i].SetActive(false);
        }

        Button[(byte)s].SetActive(true);
       
    }

    private int backIndex = 0;

    public void Back()
    {
        CustomSound.instance.cancelPlaySound();
        index = backIndex;
        humanState -=1;
        changeButton(humanState);
    }

    public void End()
    {
        CustomSound.instance.movePlaySound();
        backIndex = index;
        
        index = 0;
        if (humanState == Human.Leg_right)
        {
            finishPanel.SetActive(true);
            return;
        }
        humanState += 1;
        changeButton(humanState);
    }

    public void Left()
    {
        CustomSound.instance.movePlaySound();
        index -= 1;
        if (index < 0)
        {
            index = defaultMax;
            
            custom.ChangeAppearance(gender, humanState, index, 0);
        }
        else
        {
            custom.ChangeAppearance(gender, humanState, index, 1);
        }
            print(humanState);
            print(index);
        GameManager.instance.data.costume[humanState] = index;
     
    }

    public void Right()
    {
        CustomSound.instance.movePlaySound();
        index += 1;
        if (index > defaultMax)
        {
            index = 0;
            custom.ChangeAppearance(gender, humanState, index, 0);
        }
        else
        {
            
            custom.ChangeAppearance(gender, humanState, index, -1);
        }
        print(humanState);
        print(index);

        GameManager.instance.data.costume[humanState] = index;
    }

    public void finishCustom()
    {
        
        finishPanel.SetActive(false);
    }

    
}