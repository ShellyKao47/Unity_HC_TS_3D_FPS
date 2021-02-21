using UnityEngine;

public class FPSController : MonoBehaviour
{
    #region 基本欄位
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
    #endregion

    #region 開槍欄位
    [Header("生成子彈的位置")]
    public Transform pointFire;
    [Header("子彈")]
    public GameObject bullet;
    [Header("子彈目前數量")]
    public int bulletCurrent = 30;
    [Header("子彈總數")]
    public int bulletTotal = 150;
    [Header("子彈速度")]
    public int bulletSpeed = 450;
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawSphere(transform.position + floorOffset, floorRadius);
    }

    private void Awake()
    {
        Cursor.visible = false;             // 隱藏滑鼠
        ani = GetComponent<Animator>();
        rig = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Move();
        Jump();
        Fire();
    }

    /// <summary>
    /// 開槍
    /// </summary>
    private void Fire()
    {
        // 按下左鍵
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            // 暫存子彈 = 生成(物件，座標，角度)
            GameObject temp = Instantiate(bullet, pointFire.position, pointFire.rotation);
            // 暫存子彈.取得剛體.添加推力(生成點的前方 * 速度)
            temp.GetComponent<Rigidbody>().AddForce(pointFire.up * bulletSpeed);
        }
    }

    /// <summary>
    /// 跳躍
    /// </summary>
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