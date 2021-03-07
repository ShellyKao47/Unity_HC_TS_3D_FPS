using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    /// <summary>
    /// 玩家的變形資訊
    /// </summary>
    private Transform player;
    /// <summary>
    ///  代理器
    /// </summary>
    private NavMeshAgent nav;
    /// <summary>
    /// 動畫控制器
    /// </summary>
    private Animator ani;

    [Header("移動速度"), Range(0, 30)]
    public float speed = 2.5f;
    [Header("攻擊範圍"), Range(2, 100)]
    public float rangeAttack = 5f;
    [Header("生成子彈位置")]
    public Transform point;
    [Header("子彈")]
    public GameObject bullet;
    [Header("子彈速度"), Range(0, 3000)]
    public float speedBullet = 500;
    [Header("開槍間隔"), Range(0f, 5f)]
    public float interval = 0.5f;

    private float timer;

    private void Awake()
    {
        player = GameObject.Find("玩家").transform;       // 取得玩家變形資訊
        ani = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();                 // 取得導覽代理器
        nav.speed = speed;                                  // 速度
        nav.stoppingDistance = rangeAttack;                 // 停止距離
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);                // 指定顏色
        Gizmos.DrawSphere(transform.position, rangeAttack);     // 繪製球體
    }

    private void Update()
    {
        Track();
    }

    /// <summary>
    /// 追蹤
    /// </summary>
    private void Track()
    {
        nav.SetDestination(player.position);                    // 代理器.設定目的地(角色.座標)

        if (nav.remainingDistance > rangeAttack)                // 如果 剩餘的距離 > 攻擊範圍
        {
            ani.SetBool("跑步開關", true);                      // 就 跑步
        }
        else
        {
            Fire();                                             // 否則 就 開槍
        }
    }

    /// <summary>
    /// 開槍
    /// </summary>
    private void Fire()
    {
        ani.SetBool("跑步開關", false);

        if (timer >= interval)                                          // 如果 計時器 >= 間隔
        {
            timer = 0;                                                  // 歸零
            Instantiate(bullet, point.position, point.rotation);
        }
        else
        {
            timer += Time.deltaTime;                                    // 否則 累家時間
        }
    }
}
