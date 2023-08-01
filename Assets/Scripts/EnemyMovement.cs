using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    private Enemy _enemy;

    private Vector2 _startPosition;
    private Vector2 _toPosition;

    private float _startTime;
    private float _duration;

    //

    public void BeginMovement(Vector2 startPosition, Vector2 toPosition, float startTime, float duration)
    {
        _startPosition = startPosition;
        _toPosition = toPosition;

        transform.position = startPosition;
        if (toPosition == startPosition)
        {
            transform.up = Vector3.up;
        } else
        {
            transform.up = (toPosition - startPosition).normalized;
        }

        _startTime = startTime;
        _duration = duration;
    }

    //

    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
    }

    private void FixedUpdate()
    {
        float age = ViewPort.SweepTimePosition - _startTime;
        if (age > _duration)
        {
            _enemy.Remove();
        } else
        {
            _enemy.MyRigidbody2D.MovePosition(Vector2.Lerp(_startPosition, _toPosition, age / _duration));
        }
    }

}
