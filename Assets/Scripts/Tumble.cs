using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class Tumble : MonoBehaviour
    {
        private Rigidbody2D rb;

        private CircleCollider2D cCollider;

        [SerializeField]
        private float speed = 2f;

        [SerializeField]
        private Transform spawn;

        // Use this for initialization
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            spawn = GetComponent<Transform>();
        }

        // Update is called once per frame
        void Update()
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.name == "Wall")
            {
                Object.Instantiate(this.gameObject, spawn);
                Object.Destroy(this.gameObject);
            }
        }
    }
}