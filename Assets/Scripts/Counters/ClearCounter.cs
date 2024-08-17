public class ClearCounter : Counter 
{
    public override void Interact(PlayerController player) 
    {
        if (!HasKitchenObject()) 
        {
            // There is no KitchenObject here
            if (player.HasKitchenObject()) 
            {
                // Player is carrying something
                player.GetKitchenObject().SetKitchenObjectParent(this);
                player.SetKitchenObject(null);
            } 
            else 
            {
                // Player not carrying anything
            }
        } 
        else 
        {
            // There is a KitchenObject here
            if (player.HasKitchenObject()) 
            {
                // Player is carrying something
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) 
                {
                    // Player is holding a Plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())) 
                    {
                        Destroy(GetKitchenObject());
                    }
                } 
                else 
                {
                    // Player is not carrying Plate but something else
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject)) 
                    {
                        // Counter is holding a Plate
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO())) 
                        {
                            Destroy(player.GetKitchenObject());
                        }
                    }
                }
            } 
            else 
            {
                // Player is not carrying anything
                
                GetKitchenObject().SetKitchenObjectParent(player);
                //player.SetKitchenObject(null);
            }
        }
    }
}