using UnityEngine;

public class Rotator : MonoBehaviour
{

    [SerializeField]
    private PoolableObject _PoolableObjectReference = default;

    [SerializeField]
    private Vector3 _Rotation = new Vector3(0f, 360f, 0f);

    //

    private Quaternion _baseLocalRotation;

    //

    private void Awake()
    {
        _baseLocalRotation = transform.localRotation;

        _PoolableObjectReference.Ev_OnPoolableGet += OnPoolableGet;
    }

    private void OnPoolableGet()
    {
        transform.localRotation = _baseLocalRotation;
    }

    private void Update()
    {
        transform.Rotate(_Rotation * Time.deltaTime, Space.Self);
    }

}
