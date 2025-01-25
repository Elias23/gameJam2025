using Core;
using UnityEngine;

public class Garbage : MonoBehaviour
{
    private bool hasHitBottom = false;
    private GameManager gameManager;
    public int Damage = 1;
    public float BubbleDecayRate = 0.1f;
    public float IntialMass = 1.0f;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Garbage has collided with " + collision.gameObject.name);
        if (hasHitBottom)
        {
            return;
        }
        //get Name of Collision Object
        string collisionName = collision.gameObject.name;
        switch (collisionName)
        {
            case "Ship":
                //Destroy Garbage
                Destroy(gameObject);
                Debug.Log("Garbage has hit the ship");
                gameManager.HandleShipDamage(Damage);
                break;
            case BorderManager.Top:
                //Destroy Garbage
                Destroy(gameObject);
                Debug.Log("Garbage has hit the top");
                break;
            case "Crabby Player":
            case BorderManager.Bottom:
                //Lose Life
                hasHitBottom = true;
                Debug.Log("Garbage has hit the bottom");
                gameManager.HandleGarbageDropped();
                break;
            default:
                break;
        }

    }
}