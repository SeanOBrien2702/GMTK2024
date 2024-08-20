using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    private IKitchenObjectParent kitchenObjectParent;

    protected virtual void Awake()
    {
        GetComponent<SpriteRenderer>().sprite = kitchenObjectSO.Sprite;
    }

    public KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObjectSO;
    }

    public void SetKitchenObjectParent(IKitchenObjectParent parent)
    {
        if (kitchenObjectParent != null)
        {
           
            kitchenObjectParent.ClearKitchenObject();
        }
        parent.SetKitchenObject(this);
        kitchenObjectParent = parent;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    public void ClearKitchenObjectOnParent()
    {
        //kitchenObjectParent.ClearKitchenObject();
    }

    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject)
    {
        if (this is PlateKitchenObject)
        {
            plateKitchenObject = this as PlateKitchenObject;
            return true;
        }
        else
        {
            plateKitchenObject = null;
            return false;
        }
    }
}