using UnityEngine;

public class SpriteRotator : MonoBehaviour
{
    public Transform target;

    void Start()
    {
        target = FindObjectOfType<PlayerMove>().transform;
    }

    void Update()
    {
        transform.LookAt(target);
    }
}
