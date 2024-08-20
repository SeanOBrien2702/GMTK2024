using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IKitchenObjectParent
{
    public static event EventHandler OnPickUp;
    public static event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    Counter selectedCounter;
    Animator animator;
    SpriteRenderer spriteRenderer;

    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public Counter selectedCounter;
    }

    Rigidbody2D body;

    Vector2 direction = Vector2.zero;
    Vector2 lastDirection = Vector2.zero;
    private KitchenObject kitchenObject;

    [SerializeField] float runSpeed = 0.5f;
    [SerializeField] LayerMask counterMask;
    [SerializeField] Transform carryPosition;
    [SerializeField] float carryDistance = 2.5f;

    float footstepTimer;
    float footstepCooldown = 0.3f;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();
        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;
    }

    private void OnDestroy()
    {
        TrashCounter.OnAnyObjectTrashed -= TrashCounter_OnAnyObjectTrashed;
    }

    void Update()
    {
        if (GameManager.Instance.IsPlayingMinigame) return;

        direction.x = Input.GetAxisRaw("Horizontal");
        direction.y = Input.GetAxisRaw("Vertical");
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log("player " + kitchenObject);
        }
        Interactions();
        LookAtDirection();
        if (direction.magnitude > 0)
        {
            animator.SetBool("IsWalking", true);
            footstepTimer += Time.deltaTime;
            if (footstepTimer > footstepCooldown)
            {
                footstepTimer = 0;
                SFXController.Instance.PlayFootstepsSound();              
            }
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }
    }

    void LookAtDirection()
    {
        if (direction != lastDirection) return;
        if(lastDirection.x >= 0)
        {
            carryPosition.localPosition = new Vector3(carryDistance, 0, 0);
            spriteRenderer.flipX = true;
        }
        else
        {
            carryPosition.localPosition = new Vector3(carryDistance * -1, 0, 0);
            spriteRenderer.flipX = false;
        }
    }

    void FixedUpdate()
    {
        body.velocity = direction.normalized * runSpeed;
       
    }

    void Interactions()
    {
        if (direction != Vector2.zero)
        {
            lastDirection = direction;
        }

        float interactDistance = 4f;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, lastDirection, interactDistance, counterMask);
        Debug.DrawRay(transform.position, lastDirection * interactDistance, Color.green);
        if (hit.collider != null)
        {
            if (hit.transform.TryGetComponent(out Counter baseCounter))
            {
                // Has ClearCounter
                if (baseCounter != selectedCounter)
                {
                    SetSelectedCounter(baseCounter);
                }
            }
            else
            {
                SetSelectedCounter(null);

            }
        }
        else
        {
            SetSelectedCounter(null);
        }

        if (selectedCounter == null) return; 
        if(Input.GetKeyDown(KeyCode.E))
        {
            selectedCounter.Interact(this);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void SetSelectedCounter(Counter selectedCounter)
    {
        this.selectedCounter = selectedCounter;
        
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = selectedCounter
        });
    }

    public void SetKitchenObject(KitchenObject newKitchenObject)
    {
        kitchenObject = newKitchenObject;
        
        if (kitchenObject != null)
        {
            kitchenObject.transform.parent = carryPosition;
            kitchenObject.transform.localPosition = Vector3.zero;
            OnPickUp?.Invoke(this, EventArgs.Empty);
        }
    }

    private void TrashCounter_OnAnyObjectTrashed(object sender, EventArgs e)
    {
        Destroy(kitchenObject.gameObject);
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    internal bool HasPlate()
    {
        return kitchenObject is PlateKitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public Transform GetParentTransform()
    {
        return transform;
    }
}