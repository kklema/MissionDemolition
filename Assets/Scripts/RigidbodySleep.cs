using UnityEngine;

public class RigidbodySleep : MonoBehaviour
{
    private void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null) rb.Sleep();
    }
}