using Assets.RequiredField.Scripts;
using Core;
using UnityEngine;

namespace Objects
{
    public class Garbage : MonoBehaviour
    {
        [Header("Garbage")] [SerializeField] private float moveSpeed = 5f;
        private float baseWeight = 0.08f;
        [SerializeField] public float baseProbability = 0.5f;
        [SerializeField] public float weightClass = 1f;

        private bool hasHitBottom = false;
        private GameManager gameManager;
        [SerializeField, RequiredField] public int damageMultiplier = 10;
        private Rigidbody2D rb;

        private void Start()
        {
            gameManager = GameManager.Instance;
            rb = GetComponent<Rigidbody2D>();
            rb.mass = baseWeight*weightClass;
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
                    gameManager.HandleShipDamage(damageMultiplier * GetWeight());
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
                    Destroy(GetComponent<Rigidbody2D>());
                    Destroy(GetComponent<PolygonCollider2D>());
                    break;
                case "Bubble":
                    HandleBubbleHit(collisionGameObject);
                    rb.linearVelocityY = 0;
                    break;
                default:
                    Debug.Log("Garbage has hit something else");
                    break;
            }
        }

        private void HandleBubbleHit(GameObject collisionGameObject)
        {
            //Attach Bubble to Garbage
            Debug.Log("Bubble has hit the garbage");
            collisionGameObject.transform.parent = transform;
            var bubbleProjectile = collisionGameObject.GetComponent<BubbleProjectile>();
            float bubbleSize = bubbleProjectile.GetSize();
            Destroy(bubbleProjectile);
            Destroy(collisionGameObject.GetComponent<CircleCollider2D>());

            var force = gameObject.GetComponent<ConstantForce2D>();
            if (!force) force = gameObject.AddComponent<ConstantForce2D>();
            float upforce = 0.25f;
            float weightFactor = Mathf.Clamp(bubbleSize/weightClass,0.1f,10f);
            force.force += new Vector2(0, upforce*weightFactor);
        }
        
        public float GetWeight()
        {
            return baseWeight * weightClass;
        }
    }
}