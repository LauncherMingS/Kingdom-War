using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test {
    public class ArrowTest : MonoBehaviour
    {
        public float power = 10f;
        public float angle = 45f;
        public float gravity = -9.8f;

        public Vector3 moveSpeed;
        public Vector3 gravitySpeed = Vector3.zero;
        void Start()
        {
            moveSpeed = Quaternion.Euler(new Vector3(-angle, 0, 0)) * Vector3.forward * power;
        }
        void Update()
        {

        }
    }
}