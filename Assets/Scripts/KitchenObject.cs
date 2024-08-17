using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    private IKitchenObjectParent kitchenObjectParent;
    //private FollowTransform followTransform;

    protected virtual void Awake()
    {
        //followTransform = GetComponent<FollowTransform>();
    }

    public KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObjectSO;
    }

    public void SetKitchenObjectParent(IKitchenObjectParent parent)
    //public void SetKitchenObjectParent()
    {
        if (kitchenObjectParent != null)
        {
           
            kitchenObjectParent.ClearKitchenObject();
        }
        parent.SetKitchenObject(this);
        kitchenObjectParent = parent;
        Debug.Log(kitchenObjectParent.GetParentTransform().name);
        //transform.parent = parent.GetParentTransform();
        //transform.localPosition = Vector3.zero;
        //SetKitchenObjectParent(kitchenObjectParent.GetNetworkObject());
    }

    //[ServerRpc(RequireOwnership = false)]
    //private void SetKitchenObjectParentServerRpc(NetworkObjectReference kitchenObjectParentNetworkObjectReference)
    //{
    //    SetKitchenObjectParentClientRpc(kitchenObjectParentNetworkObjectReference);
    //}

    //[ClientRpc]
    //private void SetKitchenObjectParentClientRpc(NetworkObjectReference kitchenObjectParentNetworkObjectReference)
    //{
    //    kitchenObjectParentNetworkObjectReference.TryGet(out NetworkObject kitchenObjectParentNetworkObject);
    //    IKitchenObjectParent kitchenObjectParent = kitchenObjectParentNetworkObject.GetComponent<IKitchenObjectParent>();

    //    if (this.kitchenObjectParent != null)
    //    {
    //        this.kitchenObjectParent.ClearKitchenObject();
    //    }

    //    this.kitchenObjectParent = kitchenObjectParent;

    //    if (kitchenObjectParent.HasKitchenObject())
    //    {
    //        Debug.LogError("IKitchenObjectParent already has a KitchenObject!");
    //    }

    //    kitchenObjectParent.SetKitchenObject(this);
    //    followTransform.SetTargetTransform(kitchenObjectParent.GetKitchenObjectFollowTransform());
    //}

    //public IKitchenObjectParent GetKitchenObjectParent()
    //{
    //    return kitchenObjectParent;
    //}

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

    //public static void SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent)
    //{
    //    KitchenGameMultiplayer.Instance.SpawnKitchenObject(kitchenObjectSO, kitchenObjectParent);
    //}

    //public static void DestroyKitchenObject(KitchenObject kitchenObject)
    //{
    //    KitchenGameMultiplayer.Instance.DestroyKitchenObject(kitchenObject);
    //}
}