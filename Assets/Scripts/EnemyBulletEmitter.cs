using UnityEngine;

public class EnemyBulletEmitter : MonoBehaviour
{

    [SerializeField]
    private PoolableObject _BulletPrefab = default;

    [SerializeField]
    private Transform _FirePointsAnchor = default;

    [SerializeField]
    private float _BulletVelocity = 10f;

    [SerializeField]
    private bool _FacePlayerOnFire = true;

    [Space]

    [SerializeField]
    private AudioSource _Audio_OnEmit = default;

    //

    public void Emit()
    {
        Quaternion baseLocalRotation = _FirePointsAnchor.localRotation;

        if (_FacePlayerOnFire)
        {
            Vector3 delta = Player.GetPosition() - transform.position;
            _FirePointsAnchor.up = new Vector3(delta.x, delta.y, 0f).normalized;
        }

        Transform firePoint;
        for (int i = 0; i < _FirePointsAnchor.childCount; i ++)
        {
            firePoint = _FirePointsAnchor.GetChild(i);
            Pool.Get(_BulletPrefab, firePoint.position, firePoint.up).GetComponent<EnemyBullet>().Launch(firePoint.up * _BulletVelocity);
        }

        _FirePointsAnchor.localRotation = baseLocalRotation;

        if (_Audio_OnEmit != null) _Audio_OnEmit.Play();
    }

}
