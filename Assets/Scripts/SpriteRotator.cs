using UnityEngine;

public class SpriteRotator : MonoBehaviour
{
    public Transform target;

    void Start()
    {
        target = FindFirstObjectByType<PlayerMove>().transform;
    }

    void Update()
    {
        transform.LookAt(target);
    }
}
