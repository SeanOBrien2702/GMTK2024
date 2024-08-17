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

            //DeliveryManager.Instance.DeliverRecipe(plateKitchenObject);
            Destroy(player.GetKitchenObject());

        }
    }
}