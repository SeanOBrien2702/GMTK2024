public class DeliveryCounter : Counter 
{
    public static DeliveryCounter Instance { get; private set; }

    private void Awake() 
    {
        Instance = this;
    }

    public override void Interact(PlayerController player) 
    {
        if (player.HasPlate()) 
        {
            if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
            {
                // Only accepts Plates
                DeliveryController.Instance.DeliverRecipe(plateKitchenObject);
                Destroy(player.GetKitchenObject().gameObject);
                player.SetKitchenObject(null);
            }

        }
    }
}