using UnityEngine;

namespace Assets.Version2
{
    public class DetectionHandler : MonoBehaviour
    {
        private static readonly Collider[] detectedColliders = new Collider[64];

        [SerializeField] private int m_targetLayer;
        [SerializeField] private int m_targetLayerMask;

        public int TargetLayer => m_targetLayer;

        public Transform DetectClosestTarget(Vector3 position, float detectionRadius, out float targetSquaredDistance)
        {
            Transform t_target = null;
            targetSquaredDistance = float.MaxValue;

            int t_detectLength = Physics.OverlapSphereNonAlloc(position, detectionRadius, detectedColliders
                , m_targetLayerMask, QueryTriggerInteraction.Ignore);

            Transform t_detectedTarget;
            float t_squaredDistance;
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

        public void Initialize()
        {
            m_targetLayer = GameManager.Instance.GetOppositeGroupLayer(gameObject.layer);
            m_targetLayerMask = 1 << m_targetLayer;
        }
    }
}