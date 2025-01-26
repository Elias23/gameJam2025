using Assets.RequiredField.Scripts;
using Core;
using UnityEngine;

namespace Objects
{
    using Unity.Collections;

    public class Garbage : MonoBehaviour
    {
        [SerializeField] public float baseProbability = 0.5f;
        [SerializeField] public float weightClass = 1f;
        [SerializeField] public int HitsNeededToFLoat = 2;

        private bool hasHitBottom = false;
        [SerializeField] private int hitCounter = 0;
        private GameManager gameManager;
        [SerializeField, RequiredField] public int damageMultiplier = 10;
        private Rigidbody2D rb;

        private void Start()
        {
            hitCounter = 0;
            gameManager = GameManager.Instance;
            rb = GetComponent<Rigidbody2D>();
            rb.mass = weightClass;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var collisionGameObject = collision.gameObject;
            if (hasHitBottom)
                return;

            switch (collisionGameObject.tag)
            {
                case "Ship":
                    if (hitCounter == 0)
                        return; // only collide with ship once we have been hit by a bubble

                    //Destroy Garbage
                    Destroy(gameObject);
                    // gameManager.HandleShipDamage(damageMultiplier * GetWeight());
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

            forceComponent.force += new Vector2(0, CalculateForceForBouyancy() );
        }

        private float CalculateForceForBouyancy()
        {
            if (hitCounter == HitsNeededToFLoat)
                return 10f; // massive bonus when counter reached

            var gravitationalForce = rb.mass * Physics2D.gravity.y;
            var upforce = -gravitationalForce / (float)HitsNeededToFLoat;
            return upforce * 1.2f;
        }

        public float GetWeight()
        {
            return weightClass;
        }
    }
}