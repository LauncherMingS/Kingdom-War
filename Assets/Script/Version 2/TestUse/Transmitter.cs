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

        //仰角計算公式，給定欲落地的地點，計算可否落至指定地點
        //目前有個問題是，如果落點在發射點的左側，
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

        //實作投射物，並發射
        private void LaunchProjectile(float velocityX, float velocityY)
        {
            Projectile t_projectile = Instantiate(m_projectile, transform.position, Quaternion.identity)
                .GetComponent<Projectile>();

            //t_projectile.Initialize(velocityX, velocityY);
        }

        //利用平行速度與計算好的拋物線整個過程的時間，計算落點，並標記
        private void MarkDropPoint(float velocityX, float time)
        {
            Vector3 t_offset = new Vector3(velocityX * time, -m_offsetY, 0f);
            m_fallMark.transform.position = transform.position + t_offset;
        }

        //利用向量化的速度與計算好的拋物線整個過程的時間，模擬拋物線，並均化時間，計算每個時間點投射物的位置，
        //傳給Line Renderer渲染
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
                //取當前時刻與下一時刻垂直速度之間的平均速度，再去乘時間
                //v1 = v0 + gt, v平 = (v0 + v1) / 2 => (2v0 + gt) / 2 => v0 + 0.5f * gt
                t_position.y += (velocityY + 0.5f * m_gravity * t_intervalTime) * t_intervalTime;

                lineRenderer.SetPosition(i, t_position);
                velocityY += m_gravity * t_intervalTime;
            }
        }

        //計算投射物從發射到落地整個過程的時間
        private float CalculateParabolaTime(float velocityY)
        {
            float t_upTime = -velocityY / m_gravity;
            float t_height = 0.5f * t_upTime * t_upTime * -m_gravity + m_offsetY;
            float t_downTime = Mathf.Sqrt(t_height * 2 / -m_gravity);
            return t_upTime + t_downTime;
        }

        //計算發射投射物所能投射的最遠距離，[此方法計算的前提是發射點與落地點高度是一致的]
        private float CalculateMaxRange()
        {
            return m_initialVelocity * m_initialVelocity / -m_gravity;
        }

        //將滑鼠點擊的螢幕座標轉換成世界座標
        private Vector3 MousePositionToWorldPosition(float AxisZ = 50)
        {
            Vector3 t_screenPosition = Input.mousePosition;
            t_screenPosition.z = AxisZ;
            Vector3 t_worldPosition = Camera.main.ScreenToWorldPoint(t_screenPosition);

            return t_worldPosition;
        }

        //利用三角函數向量化發射的初始速度
        private Vector3 CalculateInitialVelocity(float radian)
        {
            if (radian < 0f)
            {
                radian += Mathf.PI;
            }

            return new Vector3(m_initialVelocity * Mathf.Cos(radian), m_initialVelocity * Mathf.Sin(radian), 0f);
        }

        //利用向量向量化發射的初始速度
        private Vector3 CalculateInitialVelocity(Vector3 direction)
        {
            return m_initialVelocity * direction;
        }

        private void Update()
        {
            //根據設定的發射速度m_initialVelocity，計算拋物線
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 t_mousePosition = MousePositionToWorldPosition();
                Vector3 t_velocity;
                //將轉換的世界座標正規化成要發射的向量，再用向量化的初始發射速度計算拋物線。
                //鍵盤空白鍵可以切換發射計算的方式
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
                //將轉換的世界座標作為公式的參數傳入，預先計算發射後，投射物是否可以落至滑鼠點擊的位置，
                //如果可以，計算發射的高仰角與低仰角，之後再透過這兩個角度使用三角函數，直接計算向量化的速度，
                //最後計算拋物線，會發射兩次(高仰角與低仰角)。
                //m_isHeightLimited限制公式傳入的高度，true為限定傳入的高度為地板，false為傳入滑鼠點擊的高度。
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
            //根據設定的角度(degree)，計算拋物線
            else if (Input.GetKeyDown(KeyCode.A))
            {
                m_lineRenderer2.enabled = false;

                float t_radian = m_launchDegree * Mathf.Deg2Rad;
                Vector3 t_velocity = CalculateInitialVelocity(t_radian);
                StartLaunch (t_velocity, m_lineRenderer1);

                m_velocity1 = t_velocity;
            }
            //拋物線計算方式的切換器
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                m_switch = !m_switch;
            }
        }
    }
}