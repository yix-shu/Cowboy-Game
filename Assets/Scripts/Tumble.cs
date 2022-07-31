using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class Tumble : MonoBehaviour
    {
        private Rigidbody2D rb;

        [SerializeField]
        private float speed = 2f;

        // Use this for initialization
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }
    }
}