using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCounter : MonoBehaviour
{
    [SerializeField] GameObject highlight;
    Counter counter;
    // Start is called before the first frame update
    void Start()
    {
        counter = GetComponent<Counter>();
        PlayerController.OnSelectedCounterChanged += PlayerController_OnSelectedCounterChanged;
    }

    private void OnDestroy()
    {
        PlayerController.OnSelectedCounterChanged -= PlayerController_OnSelectedCounterChanged;
    }

    private void PlayerController_OnSelectedCounterChanged(object sender, PlayerController.OnSelectedCounterChangedEventArgs e)
    {       
        if(e.selectedCounter == counter)
        {
            highlight.SetActive(true);
        }
        else
        {
            highlight.SetActive(false);
        }
    }
}