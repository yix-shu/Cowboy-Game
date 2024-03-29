﻿using System.Collections;
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
        private Vector3 spawn = new Vector3(-170.0f, 1.0f, -4111.0f); //should change relative to screen

        private float rotateSpeed = -0.3f;

        // Use this for initialization
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            StartCoroutine(WaitAfterCloning());
        }

        IEnumerator Continue()
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            this.gameObject.transform.Rotate(0.0f, 0.0f, rotateSpeed);
            speed = Random.Range(20, 30);
            yield return null;
            StartCoroutine(Continue());
        }

        private void OnCollisionEnter2D(Collision2D collision)
        { 
            if (collision.gameObject.name == "Wall")
            {
                Debug.Log("Collided");
                Instantiate(tumblePrefab, spawn, Quaternion.identity);
                Object.Destroy(this.gameObject);
            }
            rotateSpeed = Random.Range(-0.4f, -0.2f);
            speed -= 5.0f;
        }
        IEnumerator WaitAfterCloning()
        {
            yield return new WaitForSeconds(Random.Range(5, 10));
            this.gameObject.transform.position = spawn;
            StartCoroutine(Continue());
        }
    }
}