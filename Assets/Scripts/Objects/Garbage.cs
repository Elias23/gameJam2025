using Core;
using UnityEngine;

namespace Objects
{
    public class Garbage : MonoBehaviour
    {
        [Header("Garbage")] [SerializeField] private float moveSpeed = 5f;
        [SerializeField] public float baseWeight = 1f;
        [SerializeField] public float baseProbability = 0.5f;

        private bool hasHitBottom = false;
        private GameManager gameManager;
        public int Damage = 1;

        private void Start()
        {
            gameManager = GameManager.Instance;
            GetComponent<Rigidbody2D>().mass = baseWeight;
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
                case "Player":
                case BorderManager.Bottom:
                    //Lose Life
                    hasHitBottom = true;
                    Debug.Log("Garbage has hit the bottom");
                    gameManager.HandleGarbageDropped();
                    Destroy(gameObject);
                    break;
                case "Bubble":
                    //Attach Bubble to Garbage
                    Debug.Log("Bubble has hit the garbage");
                    collisionGameObject.transform.parent = transform;
                    collisionGameObject.GetComponent<BubbleProjectile>().enabled = false;
                    break;
                default:
                    Debug.Log("Garbage has hit something else");
                    break;
            }

        }
    }
}