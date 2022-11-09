using UnityEngine;



public class Bullet : MonoBehaviour
{
    [SerializeField] float life = 2;
    [SerializeField] float speed = 50;
    [SerializeField] LayerMask hitLayer;
    [SerializeField] GameObject spawnOnEnable, spawnOnHit;
    bool asHit;



    void OnEnable()
    {
        asHit = false;

        if (spawnOnEnable)
            Instantiate(spawnOnEnable, transform.position, transform.rotation);

        Invoke("Destroy", life);
    }

    void OnDisable()
    {
        CancelInvoke();
    }

    void Update()
    {
        if (!asHit)
            Move();
    }


    void Move()
    {
        float dist = speed * Time.deltaTime;

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, dist, hitLayer))
        {
            asHit = true;
            transform.position = hit.point;

            if (spawnOnHit)
                Instantiate(spawnOnHit, transform.position, Quaternion.LookRotation(Vector3.Reflect(transform.forward, hit.normal)));

            CancelInvoke("Destroy");
            Invoke("Destroy", 1);
            return;
        }

        transform.position += transform.forward * dist;
    }

    void Destroy() => Destroy(gameObject);
}
