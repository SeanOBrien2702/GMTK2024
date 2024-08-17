using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCounterVisuals : MonoBehaviour
{
    [SerializeField] Transform platePrefab;
    Stack<GameObject> plateStack = new Stack<GameObject>();

    private void Start()
    {
        PlateCounter.OnPlateSpawned += PlateCounter_OnPlateSpawned;
        PlateCounter.OnPlateRemoved += PlateCounter_OnPlateRemoved;
    }

    private void OnDestroy()
    {
        PlateCounter.OnPlateSpawned -= PlateCounter_OnPlateSpawned;
        PlateCounter.OnPlateRemoved -= PlateCounter_OnPlateRemoved;
    }

    private void PlateCounter_OnPlateRemoved(object sender, EventArgs e)
    {
        Destroy(plateStack.Pop());
    }

    private void PlateCounter_OnPlateSpawned(object sender, EventArgs e)
    {
        Transform plateVisualTransform = Instantiate(platePrefab, gameObject.transform);

        float plateOffsetY = .15f;
        plateVisualTransform.localPosition = new Vector3(0, plateOffsetY * plateStack.Count, 0);

        plateStack.Push(plateVisualTransform.gameObject);
    }
}
