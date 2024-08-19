using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class CustomerController : MonoBehaviour
{
    RecipeSO recipe;
    [SerializeField] SpriteLibrary library;
    SpriteRenderer spriteRenderer;
    bool isWalking = true;
    bool isFirstFrame;

    List<Transform> path;
    List<Transform> exitPath;
    Vector3 destination;
    Vector3 newPos;
    int index = 0;
    float speed = 3;

    public RecipeSO Recipe { get => recipe; set => recipe = value; }
    public List<Transform> Path { get => path; set => path = value; }
    public SpriteLibrary Library { get => library; set => library = value; }
    public List<Transform> ExitPath { get => exitPath; set => exitPath = value; }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(CustomerAnimation());
        StartCoroutine(WalkPath(path));
        DeliveryController.Instance.OnRecipeCompleted += Instance_OnRecipeCompleted;
    }

    private void OnDestroy()
    {
        DeliveryController.Instance.OnRecipeCompleted -= Instance_OnRecipeCompleted;
    }

    private void Instance_OnRecipeCompleted(object sender, DeliveryController.OnRecipeCompletedEventArgs e)
    {
        Debug.Log(e.RecipeSO  + "  "+ recipe);
        if(e.RecipeSO == recipe)
            StartCoroutine(WalkPath(exitPath));
    }

    private IEnumerator WalkPath(List<Transform> currentPath)
    {
        index = 0;
        isWalking = true;
        while (index < currentPath.Count)
        {
            destination = currentPath[index].position;
            transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, destination) <= 0.05f)
            {
                index++;               
            }
            yield return null;
        }
        isWalking = false;
        if (currentPath == exitPath)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator CustomerAnimation()
    {
        while (true)
        {
            spriteRenderer.sprite = library.GetSprite(GetCategory(), GetLabel());
            isFirstFrame = !isFirstFrame;

            yield return new WaitForSeconds(0.3f);
        }
    }

    private string GetLabel()
    {
        return isFirstFrame ? "0" : "1";
    }

    private string GetCategory()
    {
        return isWalking ? "Walking" : "Idle";
    }
}