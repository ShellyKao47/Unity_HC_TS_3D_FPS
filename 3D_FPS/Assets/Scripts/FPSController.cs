using UnityEngine;

public class FPSController : MonoBehaviour
{
    [Header("移動"), Range(0, 2000)]
    public float speed;
    [Header("旋轉"), Range(0, 2000)]
    public float turn;
    [Header("跳越高度"), Range(0, 2000)]
    public float jump = 100;
    [Header("地板偵測位移")]
    public Vector3 floorOffset;
    [Header("地板偵測半徑"), Range(0, 20)]
    public float floorRadius = 1;

    private Animator ani;
    private Rigidbody rig;

    private void Awake()
    {
        Cursor.visible = false;             // 隱藏滑鼠
        ani = GetComponent<Animator>();
        rig = GetComponent<Rigidbody>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawSphere(transform.position + floorOffset, floorRadius);
    }

    private void Update()
    {
        Move();
        Jump();
    }

    private void Jump()
    {
        // 3D 模式霧裡碰撞偵測
        // 物理.覆蓋球體(中心點 + 位移，半徑，1 << 圖層編號)
        Collider[] hits = Physics.OverlapSphere(transform.position + floorOffset, floorRadius, 1 << 8);

        // 如果 碰撞物件有一個以上 並且 碰撞物件存在 並且 按下空白鍵
        if (hits.Length > 0 && hits[0] && Input.GetKeyDown(KeyCode.Space))
        {
            // 添加推力(0，跳越高度，0)
            rig.AddForce(0, jump, 0);
        }
    }

    /// <summary>
    /// 移動
    /// </summary>
    private void Move()
    {
        float v = Input.GetAxis("Vertical");                    // W S 上 下 - 前進 1，後退 -1
        float h = Input.GetAxis("Horizontal");                  // A D 左 右

        rig.MovePosition(transform.position + transform.forward * v * speed * Time.deltaTime + transform.right * h * speed * Time.deltaTime);

        float x = Input.GetAxis("Mouse X");                     // 滑鼠左右的值
        transform.Rotate(0, x * Time.deltaTime * turn, 0);      // 旋轉
    }
}