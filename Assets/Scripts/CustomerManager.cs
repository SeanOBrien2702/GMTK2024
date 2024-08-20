using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public enum CustomerType
{
    Small,
    Medium,
    large
}

public class Seat
{
    public Transform Position;
    public bool IsFree;
    public RecipeSO RecipeSO;

    public Seat(Transform position, bool isFree)
    {
        Position = position;
        IsFree = isFree;
    }
}

public class CustomerManager : MonoBehaviour
{
    [SerializeField] List<CustomerSO> customerSOs;

    [SerializeField] CustomerController CustomerPrefab;
    [SerializeField] List<Transform> entrance;
    [SerializeField] List<Transform> exit;
    [SerializeField] List<Transform> seatPositions;
    [SerializeField] Transform test;

    List<Seat> seats = new List<Seat>();

    void Start()
    {

        foreach (var seat in seatPositions)
        {
            seats.Add(new Seat(seat, true));       
        }
        DeliveryController.Instance.OnRecipeSpawned += Instance_OnRecipeSpawned;
        DeliveryController.Instance.OnRecipeCompleted += Instance_OnRecipeCompleted;   
    }

    private void OnDestroy()
    {
        DeliveryController.Instance.OnRecipeSpawned -= Instance_OnRecipeSpawned;
        DeliveryController.Instance.OnRecipeCompleted -= Instance_OnRecipeCompleted;
    }

    private void Instance_OnRecipeSpawned(object sender, DeliveryController.OnRecipeSpawnedEventArgs e)
    {
        CustomerController customer = Instantiate(CustomerPrefab);
        customer.Library.spriteLibraryAsset = customerSOs[UnityEngine.Random.Range(0, customerSOs.Count)].SpriteLibrary;
        customer.Recipe = e.RecipeSO;

        List<Transform> entrancePath = new List<Transform>(entrance);
        entrancePath.Add(GetFreeSeat(e.RecipeSO));
        customer.Path = entrancePath;
        customer.ExitPath = exit;
        customer.transform.position = entrancePath[0].position;
    }

    private Transform GetFreeSeat(RecipeSO recipe)
    {
        foreach (Seat seat in seats)
        {
            if(seat.IsFree)
            {
                seat.IsFree = false;
                seat.RecipeSO = recipe;
                return seat.Position;
            }
        }
        return test;
    }

    private void Instance_OnRecipeCompleted(object sender, DeliveryController.OnRecipeCompletedEventArgs e)
    {
        seats.Find(item => item.RecipeSO == e.RecipeSO).IsFree = true;
        seats.Find(item => item.RecipeSO == e.RecipeSO).RecipeSO = null;
    }
}