using Assets.RequiredField.Scripts;
using Core;
using UnityEngine;

namespace Objects
{
    public class Garbage : MonoBehaviour
    {
        private float baseWeight = 0.08f;
        [SerializeField] public float baseProbability = 0.5f;
        [SerializeField] public float weightClass = 1f;

        private bool hasHitBottom = false;
        private int hitCounter = 0;
        private GameManager gameManager;
        [SerializeField, RequiredField] public int damageMultiplier = 10;
        private Rigidbody2D rb;

        private void Start()
        {
            gameManager = GameManager.Instance;
            rb = GetComponent<Rigidbody2D>();
            rb.mass = baseWeight * weightClass;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var collisionGameObject = collision.gameObject;
            if (hasHitBottom)
                return;


            //get Name of Collision Object
            switch (collisionGameObject.tag)
            {
                case "Ship":
                    if (hitCounter == 0)
                        return; // only collide with ship once we have been hit by a bubble

                    //Destroy Garbage
                    Destroy(gameObject);
                    gameManager.HandleShipDamage(damageMultiplier * GetWeight());
                    break;
                case BorderManager.Top:
                    if (hitCounter == 0)
                        return; // only collide once we have been hit by a bubble

                    //Destroy Garbage
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
            hitCounter++;
            Debug.Log("Bubble has hit the garbage");
            collisionGameObject.transform.parent = transform;
            var bubbleProjectileScript = collisionGameObject.GetComponent<BubbleProjectile>();
            float bubbleSize = bubbleProjectileScript.GetSize();
            Destroy(bubbleProjectileScript);
            Destroy(collisionGameObject.GetComponent<CircleCollider2D>());

            // add buoyant force
            var forceComponent = gameObject.GetComponent<ConstantForce2D>();
            if (!forceComponent) forceComponent = gameObject.AddComponent<ConstantForce2D>();

            float upforce = 0.25f;
            float weightFactor = Mathf.Clamp(bubbleSize / weightClass, 0.1f, 10f);
            forceComponent.force += new Vector2(0, upforce * weightFactor);
        }

        public float GetWeight()
        {
            return baseWeight * weightClass;
        }
    }
}