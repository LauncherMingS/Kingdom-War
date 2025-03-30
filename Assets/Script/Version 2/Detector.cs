using UnityEngine;

namespace Assets.Version2
{
    public class Detector : MonoBehaviour
    {
        private static Collider[] detectedColliders = new Collider[64];

        [SerializeField] private float m_detectRange = 3f;
        [SerializeField] private int m_targetLayerMask = 128;

        public Transform DetectClosestTarget(out float targetDistance)
        {
            int t_detectLength = Physics.OverlapSphereNonAlloc(transform.position
                , m_detectRange, detectedColliders, m_targetLayerMask, QueryTriggerInteraction.Ignore);
            Transform t_detectedTarget;
            Transform t_target = null;
            float t_distance;
            targetDistance = float.MaxValue;

            for (int i = 0;i < t_detectLength;i++)
            {
                t_detectedTarget = detectedColliders[i].transform;
                t_distance = (transform.position - t_detectedTarget.position).sqrMagnitude;
                if (targetDistance > t_distance)
                {
                    targetDistance = t_distance;
                    t_target = t_detectedTarget;
                }
            }

            return t_target;
        }
    }
}