using Assets.Version2;
using UnityEngine;

namespace Assets.TestUse
{
    public class Transmitter : MonoBehaviour
    {
        [Header("Reference")]
        [SerializeField] private GameObject m_projectile;
        [SerializeField] private GameObject m_fallMark;
        [SerializeField] private LineRenderer m_lineRenderer1;
        [SerializeField] private LineRenderer m_lineRenderer2;

        [Header("Data")]
        [SerializeField] private int m_pointCount = 10;
        [SerializeField] private float m_initialVelocity = 12f;
        [SerializeField] private float m_gravity = -9.8f;
        [SerializeField] private float m_floorHeight = 0f;
        [SerializeField] private float m_launchDegree = 45f;

        [Header("Record")]
        [SerializeField] private Vector3 m_velocity1;
        [SerializeField] private Vector3 m_velocity2;
        [SerializeField] private float m_highRadian;
        [SerializeField] private float m_highDegree;
        [SerializeField] private float m_lowRadian;
        [SerializeField] private float m_lowDegree;
        [SerializeField] private float m_maxRange;
        //the height difference between the launch point and the floor
        [SerializeField] private float m_offsetY;

        [Header("Switch")]
        [SerializeField] private bool m_switch;
        [SerializeField] private bool m_isHeightLimited;


        private void StartLaunch(Vector3 velocity, LineRenderer lineRenderer)
        {
            m_maxRange = CalculateMaxRange();

            LaunchProjectile(velocity.x, velocity.y);

            m_offsetY = transform.position.y - m_floorHeight;
            float t_time = CalculateParabolaTime(velocity.y);
            MarkDropPoint(velocity.x, t_time);

            DrawParabola(velocity.x, velocity.y, t_time, lineRenderer);
        }

        //�����p�⤽���A���w�����a���a�I�A�p��i�_���ܫ��w�a�I
        //�ثe���Ӱ��D�O�A�p�G���I�b�o�g�I�������A
        private bool SolveProjectileAngle(Vector3 targetPosition)
        {
            float t_offsetX = targetPosition.x - transform.position.x;
            float t_offsetY = targetPosition.y - transform.position.y;
            float t_g = -m_gravity;

            float t_v2 = m_initialVelocity * m_initialVelocity;
            float t_v4 = t_v2 * t_v2;

            float t_insideSqrt = t_v4 - t_g * (t_g * t_offsetX * t_offsetX + 2 * t_offsetY * t_v2);
            if (t_insideSqrt < 0f)
            {
                Debug.Log("Too far, cannot solve.");
                return false;
            }

            float t_sqrt = Mathf.Sqrt(t_insideSqrt);

            float t_tangentThetaHigh = (t_v2 + t_sqrt) / (t_g * t_offsetX);
            float t_tangentThetaLow = (t_v2 - t_sqrt) / (t_g * t_offsetX);
            m_highRadian = Mathf.Atan(t_tangentThetaHigh);
            m_highDegree = m_highRadian * Mathf.Rad2Deg;
            m_lowRadian = Mathf.Atan(t_tangentThetaLow);
            m_lowDegree = m_lowRadian * Mathf.Rad2Deg;

            return true;
        }

        //��@��g���A�õo�g
        private void LaunchProjectile(float velocityX, float velocityY)
        {
            Projectile t_projectile = Instantiate(m_projectile, transform.position, Quaternion.identity)
                .GetComponent<Projectile>();

            //t_projectile.Initialize(velocityX, velocityY);
        }

        //�Q�Υ���t�׻P�p��n���ߪ��u��ӹL�{���ɶ��A�p�⸨�I�A�üаO
        private void MarkDropPoint(float velocityX, float time)
        {
            Vector3 t_offset = new Vector3(velocityX * time, -m_offsetY, 0f);
            m_fallMark.transform.position = transform.position + t_offset;
        }

        //�Q�ΦV�q�ƪ��t�׻P�p��n���ߪ��u��ӹL�{���ɶ��A�����ߪ��u�A�ç��Ʈɶ��A�p��C�Ӯɶ��I��g������m�A
        //�ǵ�Line Renderer��V
        private void DrawParabola(float velocityX, float velocityY, float time, LineRenderer lineRenderer)
        {
            if (lineRenderer.positionCount < m_pointCount)
            {
                lineRenderer.positionCount = m_pointCount;
            }

            float t_intervalTime = time / (m_pointCount - 1);
            Vector3 t_position = transform.position;

            lineRenderer.SetPosition(0, t_position);
            for (int i = 1; i < m_pointCount; i++)
            {
                t_position.x += velocityX * t_intervalTime;
                //����e�ɨ�P�U�@�ɨ諫���t�פ����������t�סA�A�h���ɶ�
                //v1 = v0 + gt, v�� = (v0 + v1) / 2 => (2v0 + gt) / 2 => v0 + 0.5f * gt
                t_position.y += (velocityY + 0.5f * m_gravity * t_intervalTime) * t_intervalTime;

                lineRenderer.SetPosition(i, t_position);
                velocityY += m_gravity * t_intervalTime;
            }
        }

        //�p���g���q�o�g�츨�a��ӹL�{���ɶ�
        private float CalculateParabolaTime(float velocityY)
        {
            float t_upTime = -velocityY / m_gravity;
            float t_height = 0.5f * t_upTime * t_upTime * -m_gravity + m_offsetY;
            float t_downTime = Mathf.Sqrt(t_height * 2 / -m_gravity);
            return t_upTime + t_downTime;
        }

        //�p��o�g��g���ү��g���̻��Z���A[����k�p�⪺�e���O�o�g�I�P���a�I���׬O�@�P��]
        private float CalculateMaxRange()
        {
            return m_initialVelocity * m_initialVelocity / -m_gravity;
        }

        //�N�ƹ��I�����ù��y���ഫ���@�ɮy��
        private Vector3 MousePositionToWorldPosition(float AxisZ = 50)
        {
            Vector3 t_screenPosition = Input.mousePosition;
            t_screenPosition.z = AxisZ;
            Vector3 t_worldPosition = Camera.main.ScreenToWorldPoint(t_screenPosition);

            return t_worldPosition;
        }

        //�Q�ΤT����ƦV�q�Ƶo�g����l�t��
        private Vector3 CalculateInitialVelocity(float radian)
        {
            if (radian < 0f)
            {
                radian += Mathf.PI;
            }

            return new Vector3(m_initialVelocity * Mathf.Cos(radian), m_initialVelocity * Mathf.Sin(radian), 0f);
        }

        //�Q�ΦV�q�V�q�Ƶo�g����l�t��
        private Vector3 CalculateInitialVelocity(Vector3 direction)
        {
            return m_initialVelocity * direction;
        }

        private void Update()
        {
            //�ھڳ]�w���o�g�t��m_initialVelocity�A�p��ߪ��u
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 t_mousePosition = MousePositionToWorldPosition();
                Vector3 t_velocity;
                //�N�ഫ���@�ɮy�Х��W�Ʀ��n�o�g���V�q�A�A�ΦV�q�ƪ���l�o�g�t�׭p��ߪ��u�C
                //��L�ť���i�H�����o�g�p�⪺�覡
                if (!m_switch)
                {
                    m_lineRenderer2.enabled = false;

                    Vector3 t_direction = Vector3.Normalize(t_mousePosition - transform.position);
                    t_velocity = CalculateInitialVelocity(t_direction);

                    StartLaunch(t_velocity, m_lineRenderer1);

                    m_velocity1 = t_velocity;
                    m_highRadian = Mathf.Atan(t_direction.y / t_direction.x);
                    m_highDegree = m_highRadian * Mathf.Rad2Deg;
                }
                //�N�ഫ���@�ɮy�Ч@���������ѼƶǤJ�A�w���p��o�g��A��g���O�_�i�H���ܷƹ��I������m�A
                //�p�G�i�H�A�p��o�g���������P�C�����A����A�z�L�o��Ө��רϥΤT����ơA�����p��V�q�ƪ��t�סA
                //�̫�p��ߪ��u�A�|�o�g�⦸(�������P�C����)�C
                //m_isHeightLimited������ǤJ�����סAtrue�����w�ǤJ�����׬��a�O�Afalse���ǤJ�ƹ��I�������סC
                else
                {
                    if (m_isHeightLimited)
                    {
                        t_mousePosition.y = m_floorHeight;
                    }
                    if (!SolveProjectileAngle(t_mousePosition))
                    {
                        return;
                    }

                    t_velocity = CalculateInitialVelocity(m_highRadian);
                    StartLaunch(t_velocity, m_lineRenderer1);
                    m_velocity1 = t_velocity;

                    t_velocity = CalculateInitialVelocity(m_lowRadian);
                    m_lineRenderer2.enabled = true;
                    StartLaunch(t_velocity, m_lineRenderer2);
                    m_velocity2 = t_velocity;
                }
            }
            //�ھڳ]�w������(degree)�A�p��ߪ��u
            else if (Input.GetKeyDown(KeyCode.A))
            {
                m_lineRenderer2.enabled = false;

                float t_radian = m_launchDegree * Mathf.Deg2Rad;
                Vector3 t_velocity = CalculateInitialVelocity(t_radian);
                StartLaunch (t_velocity, m_lineRenderer1);

                m_velocity1 = t_velocity;
            }
            //�ߪ��u�p��覡��������
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                m_switch = !m_switch;
            }
        }
    }
}