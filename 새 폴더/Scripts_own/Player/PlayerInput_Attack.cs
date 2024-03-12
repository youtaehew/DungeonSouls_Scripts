using System;
using System.Collections;
using System.Collections.Generic;
using Invector.vCharacterController;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput_Attack : MonoBehaviour
{
    private PlayerInput playerInput;
    private vThirdPersonController controller;

    public PlayerInput.MouseActions onBehaviour;
    // Start is called before the first frame update
    private void Awake()
    {
        controller = GetComponent<vThirdPersonController>();
        playerInput = new PlayerInput();
        onBehaviour = playerInput.Mouse;
        

    }

    private void OnEnable()
    {
        // onBehaviour.Attack.started += controller.Attack;
        onBehaviour.Attack.performed += controller.Attack;
        onBehaviour.Attack.canceled += controller.Attack;
        onBehaviour.Shield.performed += controller.Shield;
        // onBehaviour.Shield.canceled += controller.Shield;
        onBehaviour.Heal.performed += ctx => controller.Heal();
        onBehaviour.Enable();
    }

    private void OnDisable()
    {
        // onBehaviour.Attack.started -= controller.Attack;
        onBehaviour.Attack.performed -= controller.Attack;
        onBehaviour.Attack.canceled -= controller.Attack;
        onBehaviour.Shield.performed -= controller.Shield;
        // onBehaviour.Shield.canceled -= controller.Shield;
        onBehaviour.Heal.performed -= ctx => controller.Heal();
        onBehaviour.Disable();
    }

  
}
