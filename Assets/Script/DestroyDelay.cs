using UnityEngine;


public class DestroyDelay : MonoBehaviour
{
    [SerializeField] float delay = 1;

    void Awake()
    {
        Destroy(gameObject, delay);
    }
}
