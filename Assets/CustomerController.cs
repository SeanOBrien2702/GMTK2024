using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class CustomerController : MonoBehaviour
{
    //[SerializeField] SpriteLibraryAsset asset;
    SpriteLibrary library;
    SpriteRenderer spriteRenderer;
    [SerializeField ]bool isWalking = false;
    bool isFirstFrame;
    // Start is called before the first frame update
    void Start()
    {
        library = GetComponent<SpriteLibrary>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(CustomerAnimation());
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // every 2 seconds perform the print()
    private IEnumerator CustomerAnimation()
    {
        while (true)
        {
            spriteRenderer.sprite = library.GetSprite(GetCategory(), GetLabel());
            isFirstFrame = !isFirstFrame;


            yield return new WaitForSeconds(0.5f);
            print("WaitAndPrint " + Time.time);
        }
    }

    private string GetLabel()
    {
        return isFirstFrame ? "0" : "1";
    }

    private string GetCategory()
    {
        return !isWalking ? "Idle" : "Walking";
    }
}
