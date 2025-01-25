using UnityEngine;

public class Garbage : MonoBehaviour
{
    private bool hasHitBottom = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasHitBottom)
        {
            return;
        }
        //get Name of Collision Object
        string collisionName = collision.gameObject.name;
        switch (collisionName)
        {
            case "ship":
                //Destroy Garbage
                Destroy(gameObject);
                Debug.Log("Garbage has hit the ship");
                break;
            case "Crabby Player":
                //Destroy Garbage
                Destroy(gameObject);
                break;
            case "bottom":
                //Lose Life
                hasHitBottom = true;
                Debug.Log("Garbage has hit the bottom");
                break;
            case "top":
                //Destroy Garbage
                Destroy(gameObject);
                Debug.Log("Garbage has hit the top");
                break;
            default:
                break;
        }

    }
}