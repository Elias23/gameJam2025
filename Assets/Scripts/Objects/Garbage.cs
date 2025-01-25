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
        public float IntialMass = 1.0f;

        private void Start()
        {
            gameManager = GameManager.Instance;
            GetComponent<Rigidbody2D>().mass = IntialMass;

            Destroy(gameObject, 10);
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
            switch (collisionGameObject.tag)
            {
                case "Ship":
                    //Destroy Garbage
                    Debug.Log("Garbage has hit the ship");
                    Destroy(gameObject);
                    gameManager.HandleShipDamage(Damage);
                    break;
                case BorderManager.Top:
                    //Destroy Garbage
                    Debug.Log("Garbage has hit the top");
                    Destroy(gameObject);
                    break;
                case "Player":
                case BorderManager.Bottom:
                    //Lose Life
                    Debug.Log("Garbage has hit the bottom");
                    hasHitBottom = true;
                    gameManager.HandleGarbageDropped();
                    break;
                case "Bubble":
                    HandleBubbleHit(collisionGameObject);
                    break;
            }
        }

        private void HandleBubbleHit(GameObject collisionGameObject)
        {
            //Attach Bubble to Garbage
            Debug.Log("Bubble has hit the garbage");
            collisionGameObject.transform.parent = transform;
            Destroy(collisionGameObject.GetComponent<BubbleProjectile>());
            Destroy(collisionGameObject.GetComponent<CircleCollider2D>());

            var force = gameObject.GetComponent<ConstantForce2D>();
            if (!force) force = gameObject.AddComponent<ConstantForce2D>();
            force.force += new Vector2(0, 0.5f);
        }
    }
}