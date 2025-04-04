using System.Collections;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using Debug = UnityEngine.Debug;
using CodeMonkey.Utils;
using EmeraldAI;


namespace Invector.vCharacterController
{
    [vClassHeader("THIRD PERSON CONTROLLER", iconName = "controllerIcon")]
    public class vThirdPersonController : vThirdPersonAnimator
    {
        /// <summary>
        /// Move the controller to a specific Position, you must Lock the Input first 
        /// </summary>
        /// <param name="targetPosition"></param>
        public virtual void MoveToPosition(Transform targetPosition)
        {
            MoveToPosition(targetPosition.position);
        }

        /// <summary>
        /// +
        /// Move the controller to a specific Position, you must Lock the Input first 
        /// </summary>
        /// <param name="targetPosition"></param>
        public virtual void MoveToPosition(Vector3 targetPosition)
        {
            Vector3 dir = targetPosition - transform.position;
            dir.y = 0;
            /*dir = dir.normalized * Mathf.Min(1f, dir.magnitude);*/ /*That is to make smootly stop*/

            if (dir.magnitude < 0.1f)
            {
                input = Vector3.zero;
                moveDirection = Vector3.zero;
            }
            else
            {
                input = transform.InverseTransformDirection(dir.normalized);
                moveDirection = dir.normalized;
            }
        }

        /// <summary>
        /// Handle RootMotion movement and specific Actions
        /// </summary>       
        public virtual void ControlAnimatorRootMotion()
        {
            if (!this.enabled)
            {
                return;
            }

            if (isRolling)
            {
                RollBehavior();
                return;
            }

            if (useRootMotion || customAction || lockAnimMovement)
            {
                StopCharacterWithLerp();
                transform.position = animator.rootPosition;
                transform.rotation = animator.rootRotation;
            }

            if (useRootMotion)
            {
                MoveCharacter(moveDirection);
            }
        }

        /// <summary>
        /// Set the Controller movement speed (rigidbody, animator and root motion)
        /// </summary>
        public virtual void ControlLocomotionType()
        {
            if (lockAnimMovement || lockMovement || customAction)
            {
                return;
            }

            if (!lockSetMoveSpeed)
            {
                if (locomotionType.Equals(LocomotionType.FreeWithStrafe) && !isStrafing ||
                    locomotionType.Equals(LocomotionType.OnlyFree))
                {
                    SetControllerMoveSpeed(freeSpeed);
                    SetAnimatorMoveSpeed(freeSpeed);
                }
                else if (locomotionType.Equals(LocomotionType.OnlyStrafe) ||
                         locomotionType.Equals(LocomotionType.FreeWithStrafe) && isStrafing)
                {
                    isStrafing = true;
                    SetControllerMoveSpeed(strafeSpeed);
                    SetAnimatorMoveSpeed(strafeSpeed);
                }
            }

            if (!useRootMotion)
            {
                MoveCharacter(moveDirection);
            }
        }

        /// <summary>
        /// Manage the Control Rotation Type of the Player
        /// </summary>
        public virtual void ControlRotationType()
        {
            if (lockAnimRotation || lockRotation || customAction || isRolling)
            {
                return;
            }

            bool validInput = input != Vector3.zero ||
                              (isStrafing ? strafeSpeed.rotateWithCamera : freeSpeed.rotateWithCamera);

            if (validInput)
            {
                if (lockAnimMovement)
                {
                    // calculate input smooth
                    inputSmooth = Vector3.Lerp(inputSmooth, input,
                        (isStrafing ? strafeSpeed.movementSmooth : freeSpeed.movementSmooth) * Time.deltaTime);
                }

                Vector3 dir =
                    (isStrafing && isGrounded && (!isSprinting || sprintOnlyFree == false) ||
                     (freeSpeed.rotateWithCamera && input == Vector3.zero)) && rotateTarget
                        ? rotateTarget.forward
                        : moveDirection;

                //RotationTest(dir);

                RotateToDirection(dir);
            }
        }

        /// <summary>
        /// Use it to keep the direction the Player is moving (most used with CCV camera)
        /// </summary>
        public virtual void ControlKeepDirection()
        {
            // update oldInput to compare with current Input if keepDirection is true
            if (!keepDirection)
            {
                oldInput = input;
            }
            else if ((input.magnitude < 0.01f || Vector3.Distance(oldInput, input) > 0.9f) && keepDirection)
            {
                keepDirection = false;
            }
        }

        /// <summary>
        /// Determine the direction the player will face based on input and the referenceTransform
        /// </summary>
        /// <param name="referenceTransform"></param>
        public virtual void UpdateMoveDirection(Transform referenceTransform = null)
        {
            if (isRolling && !rollControl /*|| input.magnitude <= 0.01*/)
            {
                moveDirection = Vector3.Lerp(moveDirection, Vector3.zero,
                    (isStrafing ? strafeSpeed.movementSmooth : freeSpeed.movementSmooth) * Time.deltaTime);
                return;
            }

            if (referenceTransform && !rotateByWorld)
            {
                //get the right-facing direction of the referenceTransform
                var right = referenceTransform.right;
                right.y = 0;
                //get the forward direction relative to referenceTransform Right
                var forward = Quaternion.AngleAxis(-90, Vector3.up) * right;
                // determine the direction the player will face based on input and the referenceTransform's right and forward directions
                moveDirection = (inputSmooth.x * right) + (inputSmooth.z * forward);
                var moveDirectionRaw = (input.x * right) + (input.z * forward);
                SetInputDirection(moveDirectionRaw);
            }
            else
            {
                moveDirection = new Vector3(inputSmooth.x, 0, inputSmooth.z);
                var moveDirectionRaw = new Vector3(input.x, 0, input.z);
                SetInputDirection(moveDirectionRaw);
            }
        }

        /// <summary>
        /// Set the isSprinting bool and manage the Sprint Behavior 
        /// </summary>
        /// <param name="value"></param>
        public virtual void Sprint(bool value)
        {
            var sprintConditions = (!isCrouching || (!inCrouchArea && CanExitCrouch())) && (currentStamina > 0 &&
                hasMovementInput &&
                !(isStrafing && (horizontalSpeed >= 0.5 || horizontalSpeed <= -0.5 || verticalSpeed <= 0.1f) &&
                  !sprintOnlyFree));

            if (value && sprintConditions)
            {
                if (currentStamina > (finishStaminaOnSprint ? sprintStamina : 0) && hasMovementInput)
                {
                    finishStaminaOnSprint = false;
                    if (isGrounded && useContinuousSprint)
                    {
                        isCrouching = false;
                        isSprinting = !isSprinting;
                        if (isSprinting)
                        {
                            OnStartSprinting.Invoke();
                            alwaysWalkByDefault = false;
                        }
                        else
                        {
                            OnFinishSprinting.Invoke();
                        }
                    }
                    else if (!isSprinting)
                    {
                        OnStartSprinting.Invoke();

                        alwaysWalkByDefault = false;
                        isSprinting = true;
                    }
                }
                else if (!useContinuousSprint && isSprinting)
                {
                    if (currentStamina <= 0)
                    {
                        finishStaminaOnSprint = true;
                        OnFinishSprintingByStamina.Invoke();
                    }

                    isSprinting = false;
                    OnFinishSprinting.Invoke();
                }
            }
            else if (isSprinting && (!useContinuousSprint || !sprintConditions))
            {
                if (currentStamina <= 0)
                {
                    finishStaminaOnSprint = true;
                    OnFinishSprintingByStamina.Invoke();
                }

                isSprinting = false;
                OnFinishSprinting.Invoke();
            }
        }

        /// <summary>
        /// Manage the isCrouching bool
        /// </summary>
        public virtual void Crouch()
        {
            if (isGrounded && !customAction)
            {
                AutoCrouch();
                if (isCrouching && CanExitCrouch())
                {
                    isCrouching = false;
                }
                else
                {
                    isCrouching = true;
                    isSprinting = false;
                }
            }
        }

        /// <summary>
        /// Set the isStrafing bool
        /// </summary>
        public virtual void Strafe()
        {
            isStrafing = !isStrafing;
        }


        public bool isHoldingAttack = false;
        public bool isHeal = false;
        public int HealCount = 3;

        private bool healing = false;
        // private bool isShield = false;
        // private bool shieldEnd = false;
        // private bool isParyying = false;


        // [SerializeField] private EnemyStats enemyStats;

        [SerializeField] private GameObject _sword;
        [SerializeField] private GameObject _potion;


        [SerializeField] private GameObject shieldCol;
        public Collider parryCol;
        [SerializeField] private Collider Playercollider_Capsule;

        public bool isPursure = false;

        #region InputSystem

        public virtual void Attack(InputAction.CallbackContext Context)
        {
            if (isHeal || isRolling || currentStamina <= 20f || ishiting || isBackAttacking || isFrontAttacking) return;
            if (Context.performed)
            {
                if (Context.interaction is HoldInteraction)
                {
                    weapon.power = 40;
                    animator.CrossFadeInFixedTime("ChargingAttack", 0.1f);
                    movementLock();
                    StartCoroutine(Hold(1f));
                }

                if (Context.interaction is PressInteraction)
                {
                    weapon.power =18;
                    canBackAttack();
                    if (isPursure)
                    {
                        if (isFrontAttack && enemyManager.isStun)
                        {
                            FrontStebAttack();
                        }

                        NormalAttack();
                    }
                    else
                    {
                        if (isBackAttack)
                        {
                            backStebAttack();
                        }
                        else NormalAttack();
                    }
                    // if (isBackAttack) backStebAttack();
                    // else NormalAttack();
                }
            }
        }

        public virtual void Shield(InputAction.CallbackContext Context)
        {
            if (isRolling || currentStamina <= 30 || ishiting || isBackAttacking || isFrontAttacking) return;
            // if (Context.performed)
            // {
            //     if (Context.interaction is HoldInteraction)
            //     {
            //         isShield = true;
            //         animator.CrossFadeInFixedTime("ShieldOn", 0.1f);
            //     }

            if (Context.performed)
            {
                if (Context.interaction is PressInteraction)
                {
                    if (isRolling || currentStamina < 30 || IsAttacking || isHoldingAttack || isHeal) return;
                    Paryying();
                }
            }
            // }

            // if (Context.canceled)
            // {
            //     if (Context.interaction is HoldInteraction)
            //     {
            //         isShield = false;
            //         animator.SetBool("isShield", false);
            //     }
            // }
        }

        #endregion


        private Vector3 dirFromEnemyToPlayer;
        [SerializeField] private float backstabDistae = 2f;
        public bool isBackAttack = false;
        private bool isBackAttacking = false;
        public bool isFrontAttack = false;
        private bool isFrontAttacking = false;
        public bool isParrying = false;
        private Vector3 dirToEnemy;


        [SerializeField] private Weapon weapon;
        [SerializeField] private CinemachineFreeLook cinemachine;
        [SerializeField] private CinemachineImpulseSource cinemachineImpulseSource;

        public void CameraZoom(int a)
        {
            DOTween.To(() => cinemachine.m_Lens.FieldOfView, x => cinemachine.m_Lens.FieldOfView = x, a, 0.2f);
        }


        public void CameraShake(int a)
        {
            cinemachineImpulseSource.GenerateImpulse(1f);
            CameraZoom(35);
        }


        public void EnableCol(int a)
        {
            weapon.EnableCol_animationEvent(a);
        }

        public void SwordEffect(int a)
        {
            swordEffect[a].Play();
        }

        float enemyDistance;
        float minenemyDistance = 100;
        private float detectionRadius = 3f;
        private EnemyBackSteb enemyBackSteb_change;
        [SerializeField] private EnemyBackSteb enemyBackSteb;
        private EnemyManager enemyManager_change;
        [SerializeField] private EnemyManager enemyManager;

        [SerializeField] private LayerMask layerMask;

        [SerializeField] private ParticleSystem[] swordEffect;
        [SerializeField] private HealCount healCount;


        private void Update()
        {
            // if (isShield)
            // {
            //     ReduceStamina(5, true);
            //     currentStaminaRecoveryDelay = 1f;
            // }

            Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, layerMask);
            for (int i = 0; i < colliders.Length; i++)
            {
                enemyBackSteb_change = colliders[i].transform.GetComponent<EnemyBackSteb>();
                enemyManager_change = colliders[i].transform.GetComponent<EnemyManager>();


                enemyBackSteb = colliders[0].GetComponent<EnemyBackSteb>();
                enemyManager = colliders[0].GetComponent<EnemyManager>();

                minenemyDistance = Vector3.Distance(transform.position, colliders[0].transform.position);
                if (enemyBackSteb != null)
                {
                    enemyDistance = Vector3.Distance(colliders[i].transform.position, transform.position);
                    if (enemyDistance < minenemyDistance)
                    {
                        minenemyDistance = enemyDistance;

                        enemyManager = enemyManager_change;
                        enemyBackSteb = enemyBackSteb_change;
                    }
                }
            }
        }


        private void canBackAttack()
        {
            if (enemyBackSteb == null) return;
            Transform enemyTransform = enemyBackSteb.transform;
            dirFromEnemyToPlayer = (transform.position - enemyTransform.position).normalized;
            // float dot = Vector3.Dot(new Vector3(enemyTransform.forward.x,0, enemyTransform.forward.z), new Vector3(dirFromEnemyToPlayer.x, 0, dirFromEnemyToPlayer.z));
            float dot = Vector3.Dot(enemyTransform.forward, dirFromEnemyToPlayer);
            if (dot < -0.9f && Vector3.Distance(transform.position, enemyTransform.position) < backstabDistae)
            {
                isBackAttack = true;
                dirToEnemy = (enemyTransform.position - transform.position).normalized;
                SetTargetForward(dirToEnemy);
                enemyBackSteb.SetTargetForward(dirToEnemy);
                enemyBackSteb.ForceMoveToPos(transform.position + dirToEnemy * 0.7f);
            }
            else if (dot > 0.9f && Vector3.Distance(transform.position, enemyTransform.position) < backstabDistae)
            {
                isFrontAttack = true;
                dirToEnemy = (enemyTransform.position - transform.position).normalized;
                SetTargetForward(dirToEnemy);
                enemyBackSteb.SetTargetBack(dirToEnemy);
                enemyBackSteb.ForceMoveToPos(transform.position + dirToEnemy);
            }
            else isBackAttack = false;
        }


        private void SetTargetForward(Vector3 targetForward)
        {
            Vector3 cameraForward = Camera.main.transform.forward;
            float setRotation = -UtilsClass.GetAngleFromVectorFloatXZ(targetForward) + 90;
            transform.rotation = Quaternion.AngleAxis(setRotation, transform.up);
        }

        private void backStebAttack()
        {
            isBackAttacking = true;
            _rigidbody.useGravity = false;
            Playercollider_Capsule.enabled = false;
            moveDirection = Vector3.zero;
            input = Vector3.zero;
            inputSmooth = Vector3.zero;
            inputMagnitude = 0;
            verticalSpeed = 0;
            horizontalSpeed = 0;
            useRootMotion = true;
            lockMovement = true;
            lockRotation = true;
            lockAnimMovement = true;
            _rigidbody.velocity = Vector3.zero;
            //무적상태

            animator.CrossFadeInFixedTime("BackStabAttack", 0.1f);
            enemyBackSteb.HandleSmoothForwardMovement();
            enemyBackSteb.HandleSmoothForwardRotation();
            enemyBackSteb.PlayBackstabAnimation();
        }

        private void FrontStebAttack()
        {
            isFrontAttacking = true;
            _rigidbody.useGravity = false;
            Playercollider_Capsule.enabled = false;
            moveDirection = Vector3.zero;
            input = Vector3.zero;
            inputSmooth = Vector3.zero;
            inputMagnitude = 0;
            verticalSpeed = 0;
            horizontalSpeed = 0;
            useRootMotion = true;
            lockMovement = true;
            lockRotation = true;
            lockAnimMovement = true;
            _rigidbody.velocity = Vector3.zero;
            //무적상태

            animator.CrossFadeInFixedTime("FrontStebAttack", 0.1f);
            enemyBackSteb.HandleSmoothForwardMovement();
            enemyBackSteb.HandleSmoothForwardRotation();
            enemyBackSteb.PlayFrontstabAnimation();
        }

        private void backStebEnd()
        {
            Playercollider_Capsule.enabled = true;
            _rigidbody.useGravity = true;
            lockAnimMovement = false;
            ComboPossible = false;
            useRootMotion = false;
            lockMovement = false;
            lockRotation = false;
            isBackAttacking = false;
            isFrontAttacking = false;
        }

        #region Paryy

        private void Paryying()
        {
            movementLock();
            ReduceStamina(25, false);
            currentStaminaRecoveryDelay = 2f;
            isParrying = true;
            animator.CrossFadeInFixedTime("Parrying", 0.1f);
        }

        public void FinishParrying()
        {
            parryCol.enabled = false;
            BehaviourEnd();
        }

        private void parryingControll()
        {
            if (parryCol.enabled == false)
            {
                parryCol.enabled = true;
            }
            else
            {
                parryCol.enabled = false;
            }
        }

        #endregion

        // private void Shielding()
        // {
        //     if (isShield)
        //     {
        //         animator.SetBool("isShield", true);
        //         shieldCol.SetActive(true);
        //     }
        //     else
        //     {
        //         ReduceStamina(0, false);
        //         shieldCol.SetActive(false);
        //     }
        // }

        #region Heal

        public virtual void Heal()
        {
            if (isHeal || isHoldingAttack || IsAttacking || isRolling || ishiting || isBackAttacking ||
                isFrontAttacking) return;
            isHeal = true;
            healing = true;
            _sword.SetActive(false);
            if (HealCount <= 0)
            {
                animator.CrossFadeInFixedTime("Potion_Empty", 0.1f);
                return;
            }

            movementLock(false);
            animator.CrossFadeInFixedTime("Potion_Drink", 0.1f);
            HealCount--;
            healCount.HealControll(HealCount);
        }

        public void healPlus()
        {
            GameManager.instance.PotionDrinkSound();
            HpHeal(40f);
        }

        public void healEnd()
        {
            isHeal = false;
            BehaviourEnd();
        }

        public void hideSword()
        {
            if (healing == true)
            {
                _potion.SetActive(true);
                healing = false;
            }
            else
            {
                _potion.SetActive(false);
                _sword.SetActive(true);
            }
        }

        #endregion


        #region Attack

        IEnumerator Hold(float stopTime)
        {
            isHoldingAttack = true;
            yield return new WaitForSeconds(stopTime);
            if (isHoldingAttack == true)
            {
                ChargingAttack();
            }
        }

        private void ChargingAttack()
        {
            animator.SetFloat("Charging", 1f);
            ReduceStamina(30, false);
            currentStaminaRecoveryDelay = 2f;
        }

        public void chargingAttackEnd()
        {
            EnableCol(0);
            isHoldingAttack = false;
            BehaviourEnd();
        }

        [SerializeField] private AudioClip backSoundClip;

        public void BackStebSound(int a)
        {
            if (a == 1)
            {
                GameManager.PlaySound(backSoundClip, 0.6f);
            }
            else
            {
                GameManager.instance.EnemyKillSound();
            }
        }

        private void NormalAttack()
        {
            if (isHoldingAttack || isBackAttacking || isFrontAttacking) return;
            if (IsAttacking)
            {
                if (ComboPossible)
                {
                    ComboCount += 1;
                    ComboPossible = false;
                    IsAttacking = true;
                    animator.CrossFadeInFixedTime("Attack" + ComboCount, 0.1f);
                    onComboRotaion();
                    ReduceStamina(20, false);
                    currentStaminaRecoveryDelay = 2f;
                }
            }
            else
            {
                if (ComboCount == 1)
                {
                    //lockAnimMovement = true;
                    movementLock();
                    _rigidbody.velocity = Vector3.zero;
                    animator.CrossFadeInFixedTime("Attack" + ComboCount, 0.1f);
                    onComboRotaion();
                    ReduceStamina(20, false);
                    currentStaminaRecoveryDelay = 2f;
                }
            }
        }

        public void ComboisTrue()
        {
            EnableCol(0);
            ComboPossible = true;
            IsAttacking = true;
            if (ComboCount >= 3)
            {
                ComboCount = 1;
            }
        }

        public void ComboReset()
        {
            EnableCol(0);
            BehaviourEnd();
            ComboCount = 1;
        }

        private void onComboRotaion(float duration = .2f)
        {
            float targetAngle = Mathf.Atan2(input.x, input.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            transform.DORotateQuaternion(Quaternion.Euler(0, targetAngle, 0), duration);
        }

        #endregion

        #region movementLock

        public void movementLock(bool rootmotion = true)
        {
            moveDirection = Vector3.zero;
            input = Vector3.zero;
            inputSmooth = Vector3.zero;
            inputMagnitude = 0;
            verticalSpeed = 0;
            horizontalSpeed = 0;
            useRootMotion = rootmotion;
            lockMovement = true;
            lockRotation = true;
            IsAttacking = true;
            lockAnimMovement = true;
            _rigidbody.velocity = Vector3.zero;
        }

        private void BehaviourEnd()
        {
            lockAnimMovement = false;
            ComboPossible = false;
            useRootMotion = false;
            lockMovement = false;
            lockRotation = false;
            IsAttacking = false;
            isParrying = false;
        }

        #endregion

        #region Hit

        public void hit()
        {
            ishiting = true;
            moveDirection = Vector3.zero;
            input = Vector3.zero;
            inputSmooth = Vector3.zero;
            inputMagnitude = 0;
            verticalSpeed = 0;
            horizontalSpeed = 0;
            useRootMotion = true;
            lockMovement = true;
            lockRotation = true;
            IsAttacking = true;
            lockAnimMovement = true;
            _rigidbody.velocity = Vector3.zero;
        }

        private void hitEnd()
        {
            ishiting = false;
            lockAnimMovement = false;
            ComboPossible = false;
            useRootMotion = false;
            lockMovement = false;
            lockRotation = false;
            IsAttacking = false;
            ComboReset();
        }

        #endregion


        /// <summary>
        /// Triggers the Jump Animation and set the necessary variables to make the Jump behavior in the <seealso cref="vThirdPersonMotor"/>
        /// </summary>
        /// <param name="consumeStamina">Option to consume or not the stamina</param>
        public virtual void Jump(bool consumeStamina = false)
        {
            // trigger jump behaviour
            jumpCounter = jumpTimer;
            OnJump.Invoke();

            // trigger jump animations
            if (input.sqrMagnitude < 0.1f)
            {
                StartCoroutine(DelayToJump());
                animator.CrossFadeInFixedTime("Jump", 0.1f);
            }
            else
            {
                isJumping = true;
                animator.CrossFadeInFixedTime("JumpMove", .2f);
            }

            // reduce stamina
            if (consumeStamina)
            {
                ReduceStamina(jumpStamina, false);
                currentStaminaRecoveryDelay = 1f;
            }
        }

        protected IEnumerator DelayToJump()
        {
            inJumpStarted = true;
            yield return new WaitForSeconds(jumpStandingDelay);
            isJumping = true;
            inJumpStarted = false;
        }


        public void PlayerDamage(int damage)
        { 
            TakeDamage(new vDamage(damage, false));
        }

        /// <summary>
        /// Triggers the Roll Animation and set the stamina cost for this action
        /// </summary>
        public virtual void Roll()
        {
            if (isBackAttacking || isFrontAttacking) return;
            OnRoll.Invoke();
            isRolling = true;
            animator.CrossFadeInFixedTime("Roll", rollTransition, baseLayer);
            ReduceStamina(rollStamina, false);
            currentStaminaRecoveryDelay = 1.5f;
        }


        #region Check Action Triggers

        /// <summary>
        /// Call this in OnTriggerEnter or OnTriggerStay to check if enter in triggerActions     
        /// </summary>
        /// <param name="other">collider trigger</param>                         
        protected override void OnTriggerStay(Collider other)
        {
            try
            {
                CheckForAutoCrouch(other);
            }
            catch (UnityException e)
            {
                Debug.LogWarning(e.Message);
            }

            base.OnTriggerStay(other);
        }

        /// <summary>
        /// Call this in OnTriggerExit to check if exit of triggerActions 
        /// </summary>
        /// <param name="other"></param>
        protected override void OnTriggerExit(Collider other)
        {
            AutoCrouchExit(other);
            base.OnTriggerExit(other);
        }

        #endregion
    }
}