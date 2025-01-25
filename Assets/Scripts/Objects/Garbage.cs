using Core;
using UnityEngine;

public class Garbage : MonoBehaviour
{
    private bool hasHitBottom = false;
    private GameManager gameManager;
    public int Damage = 1;
    public float IntialMass = 1.0f;

    private void Start()
    {
        gameManager = GameManager.Instance;
        GetComponent<Rigidbody2D>().mass = IntialMass;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collisionGameObject = collision.gameObject;
        Debug.Log("Garbage has collided with " + collisionGameObject.name);
        if (hasHitBottom)
        {
            return;
        }
        //get Name of Collision Object
        string collisionName = collisionGameObject.tag;
        switch (tag)
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
            case "Player":
            case BorderManager.Bottom:
                //Lose Life
                hasHitBottom = true;
                Debug.Log("Garbage has hit the bottom");
                gameManager.HandleGarbageDropped();
                break;
            case "Bubble":
                //Attach Bubble to Garbage
                Debug.Log("Bubble has hit the garbage");
                collisionGameObject.transform.parent = transform;
                collisionGameObject.GetComponent<BubbleProjectile>().enabled = false;
                break;
        }

    }
}