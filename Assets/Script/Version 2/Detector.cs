using UnityEngine;

namespace Assets.Version2
{
    public class Detector : MonoBehaviour
    {
        private static Collider[] detectedColliders = new Collider[64];

        [SerializeField] private float m_detectRange = 3f;
        [SerializeField] private int m_targetLayerMask;

        public Transform DetectClosestTarget(out float targetSquaredDistance)
        {
            int t_detectLength = Physics.OverlapSphereNonAlloc(transform.position
                , m_detectRange, detectedColliders, m_targetLayerMask, QueryTriggerInteraction.Ignore);
            Transform t_detectedTarget;
            Transform t_target = null;
            float t_squaredDistance;
            targetSquaredDistance = float.MaxValue;

            for (int i = 0;i < t_detectLength;i++)
            {
                t_detectedTarget = detectedColliders[i].transform;
                t_squaredDistance = Vector3.SqrMagnitude(transform.position - t_detectedTarget.position);
                if (targetSquaredDistance > t_squaredDistance)
                {
                    targetSquaredDistance = t_squaredDistance;
                    t_target = t_detectedTarget;
                }
            }

            return t_target;
        }

        public void Initialize(int group)
        {
            if (group == GameManager.Instance.SYWS)
            {
                m_targetLayerMask = GameManager.Instance.NLI;
            }
            else
            {
                m_targetLayerMask = GameManager.Instance.SYWS;
            }
        }
    }
}