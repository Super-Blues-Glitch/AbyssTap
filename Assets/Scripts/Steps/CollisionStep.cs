using UnityEngine;
using UnityEngine.Events;

public class CollisionStep : MonoBehaviour
{
    [SerializeField] GameEnums.CollisionType detectionType;
    [SerializeField] UnityEvent collisionEvent;

    [SerializeField] Collider collToDetect;

    private void OnCollisionEnter(Collision collision)
    {
        if (detectionType == GameEnums.CollisionType.CollEnter) Verify(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (detectionType == GameEnums.CollisionType.CollStay) Verify(collision);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (detectionType == GameEnums.CollisionType.CollExit) Verify(collision);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (detectionType == GameEnums.CollisionType.TriggerEnter) Verify(other);
    }

    private void OnTriggerStay(Collider other)
    {
        if (detectionType == GameEnums.CollisionType.TriggerStay) Verify(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (detectionType == GameEnums.CollisionType.TriggerExit) Verify(other);
    }

    private void Verify(Collision collision)
    {
        if (collision.collider == collToDetect)
        {
            collisionEvent?.Invoke();
        }
    }

    private void Verify(Collider collision)
    {
        if (collision == collToDetect)
        {
            collisionEvent?.Invoke();
        }
    }
}
