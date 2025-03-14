using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripte.Test3
{
    [ExecuteInEditMode]
    public class LauncherMachine : MonoBehaviour
    {
        public GameObject bullet;
        //public Quaternion lookRotation;

        private void Update()
        {
            //lookRotation = Quaternion.LookRotation(bullet.transform.position - transform.position);
            //transform.rotation = lookRotation;
        }
        private void OnMouseDown()
        {
            GenerateBullet();
        }
        public void GenerateBullet()
        {
            GameObject g = Instantiate(bullet, transform.position, transform.rotation);
            g.AddComponent<Rigidbody>().AddForce(
                Quaternion.Euler(transform.rotation.z, 0, 0) * new Vector3(25, 0, 0)
                , ForceMode.VelocityChange
                );
        }
    }
}