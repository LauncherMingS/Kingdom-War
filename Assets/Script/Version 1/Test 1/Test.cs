using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripte
{
    [ExecuteInEditMode]
    class Test : MonoBehaviour
    {
        //static int[] Source = new int[] { 2, 7, 5, 1, 6, 8, 3 };

        //private static int Sum(int[] source)
        //{
        //    //return source.Sum(x => x);
        //    return source.Aggregate((total, next) => total + next);
        //}
        //static void Main(String[] args)
        //{
        //    int a = Sum(Source);
        //}
        //public int degree;
        public double radian;
        //public float value;

        public double x;
        public double y;
        public Transform target;
        public Vector3 relative;
        public float angle;
        public float ratio;
        private void Update()
        {
            //degree = degree % 360;
            //radian = degree * Mathf.Deg2Rad;
            //value = Mathf.Tan((float)radian);
            //Debug.Log(Math.Sin(radian));
            //Debug.Log(Math.Cos(radian));



            //radian = Math.Atan(value);
            //degree = radian / Math.PI * 180;

            //radian = Math.Atan2(y, x);
            relative = transform.InverseTransformPoint(target.position);
            angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;
            //ratio = relative.x / relative.z;
            //angle = ratio * Mathf.Rad2Deg;
            transform.Rotate(0, angle, 0);
        }
    }
}
