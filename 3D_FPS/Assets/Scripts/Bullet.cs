using UnityEngine;

public class Bullet : MonoBehaviour
{
    /// <summary>
    /// 子彈的傷害
    /// </summary>
    public float attack;

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
