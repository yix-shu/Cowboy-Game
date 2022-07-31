using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class Tumble : MonoBehaviour
    {
        public GameObject tumblePrefab;

        private Rigidbody2D rb;

        [SerializeField]
        private float speed = 20.0f;

        [SerializeField]
        private Vector3 spawn = new Vector3(-97.2f, 1.0f, -4111.0f);

        private float rotateSpeed = -0.3f;

        // Use this for initialization
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            this.gameObject.transform.Rotate(0.0f, 0.0f, rotateSpeed);
            speed = Random.Range(20, 50);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        { 
            if (collision.gameObject.name == "Wall")
            {
                Debug.Log("Collided");
                Instantiate(tumblePrefab, spawn, Quaternion.identity);
                Object.Destroy(this.gameObject);
            }
            rotateSpeed = Random.Range(-0.1f, -0.3f);
            speed += 10.0f;
        }
    }
}