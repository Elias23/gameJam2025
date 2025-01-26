using Core;
using UnityEngine;

namespace Objects
{
    using Unity.Collections;
    using UnityEngine.Serialization;

    public class Garbage : MonoBehaviour
    {
        [FormerlySerializedAs("baseProbability")]
        [SerializeField] public float spawnProbability = 0.5f;
        [SerializeField] public float weightClass = 1f;
        [SerializeField] public int HitsNeededToFLoat = 2;

        private bool hasHitBottom = false;
        private int hitCounter = 0;
        private GameManager gameManager;
        private Rigidbody2D rigidBody;
        private ConstantForce2D forceComponent;

        private void Start()
        {
            hitCounter = 0;
            gameManager = GameManager.Instance;
            rigidBody = GetComponent<Rigidbody2D>();
            rigidBody.mass = weightClass;

            forceComponent = gameObject.GetComponent<ConstantForce2D>();
            if (!forceComponent) forceComponent = gameObject.AddComponent<ConstantForce2D>();
            forceComponent.force = Vector2.zero;
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
                    gameManager.HandleShipDamage(GetWeight());
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
            forceComponent.force += new Vector2(0, CalculateForceForBouyancy() );
        }

        private float CalculateForceForBouyancy()
        {
            if (hitCounter == HitsNeededToFLoat)
                return 10f; // massive bonus when counter reached

            // calculate force per hit to float
            var gravitationalForce = rigidBody.mass * Physics2D.gravity.y;
            var upForce = -gravitationalForce / (float)HitsNeededToFLoat;
            return upForce * 1.2f;
        }

        public float GetWeight()
        {
            return weightClass;
        }
    }
}