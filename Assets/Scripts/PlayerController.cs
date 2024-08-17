using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IKitchenObjectParent
{
    public static event EventHandler OnPickUp;
    public static event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    Counter selectedCounter;

    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public Counter selectedCounter;
    }

    Rigidbody2D body;

    Vector2 direction = Vector2.zero;
    Vector2 lastDirection = Vector2.zero;
    float horizontal;
    float vertical;
    float angle;
    private KitchenObject kitchenObject;

    [SerializeField] float runSpeed = 0.5f;
    [SerializeField] LayerMask counterMask;
    [SerializeField] Transform carryPosition;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;
    }

    private void OnDestroy()
    {
        TrashCounter.OnAnyObjectTrashed -= TrashCounter_OnAnyObjectTrashed;
    }

    void Update()
    {
        direction.x = Input.GetAxisRaw("Horizontal");
        direction.y = Input.GetAxisRaw("Vertical");
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log(kitchenObject);
        }
        Interactions();
        LookAtDirection();
        
            
    }

    void LookAtDirection()
    {
        //if (direction.magnitude < 0) return;
        //if (direction != Vector2.zero)
        //{
        //    lastDirection = direction;
        //}
        if (lastDirection != direction) return;
        //float rotateSpeed = 10f;
        //transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void FixedUpdate()
    {
        body.velocity = direction.normalized * runSpeed;
    }

    void Interactions()
    {
        //Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();

        //Vector3 moveDir = new Vector3(direction.x, 0f, inr.y);

        if (direction != Vector2.zero)
        {
            lastDirection = direction;
        }

        float interactDistance = 2f;
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
        Debug.Log(kitchenObject);
        
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
