using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    PlateKitchenObject scaledPlate;
    bool isPlayingMinigame = false;
    int level = 0;

    public bool IsPlayingMinigame { get => isPlayingMinigame; set => isPlayingMinigame = value; }
    public PlateKitchenObject PlateKitchenObject { get => scaledPlate; set => scaledPlate = value; }
    public int Level { get => level; set => level = value; }

    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
