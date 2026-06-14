using UnityEngine;

public class HandController : MonoBehaviour
{
    public Camera mainCamera;
    public BoxCollider movementBounds;

    [Header("Height")]
    public float handHeight = 1f;
    public float scrollHeightSpeed = 0.5f;

    [Header("Debug")]
    public Vector3 targetPosition;

    private void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        handHeight = transform.position.y;
        targetPosition = transform.position;
    }

    private void Update()
    {
        if (mainCamera == null || InputHandler.Instance == null) return;

        UpdateHeight();
        UpdateTargetPosition();
        MoveHandInstant();
    }

    public void UpdateHeight()
    {
        handHeight += InputHandler.Instance.scrollDelta * scrollHeightSpeed;
    }

    public void UpdateTargetPosition()
    {
        Plane handPlane = new Plane(Vector3.up, new Vector3(0f, handHeight, 0f));
        Ray ray = mainCamera.ScreenPointToRay(InputHandler.Instance.mouseScreenPosition);

        if (handPlane.Raycast(ray, out float distance))
        {
            Vector3 hitPoint = ray.GetPoint(distance);
            targetPosition = ClampPointToBounds(hitPoint);
        }
    }

    public void MoveHandInstant()
    {
        transform.position = targetPosition;
    }

    public Vector3 ClampPointToBounds(Vector3 worldPoint)
    {
        if (movementBounds == null)
            return worldPoint;

        Vector3 localPoint = movementBounds.transform.InverseTransformPoint(worldPoint) - movementBounds.center;
        Vector3 halfSize = movementBounds.size * 0.5f;

        localPoint.x = Mathf.Clamp(localPoint.x, -halfSize.x, halfSize.x);
        localPoint.y = Mathf.Clamp(localPoint.y, -halfSize.y, halfSize.y);
        localPoint.z = Mathf.Clamp(localPoint.z, -halfSize.z, halfSize.z);

        return movementBounds.transform.TransformPoint(localPoint + movementBounds.center);
    }

    private void OnTriggerEnter(Collider other)
    {
        AlarmClock alarm = other.GetComponentInParent<AlarmClock>();
        if (alarm != null)
        {
            alarm.TurnOff();
            return;
        }

        Cactus cactus = other.GetComponentInParent<Cactus>();
        if (cactus != null)
        {
            cactus.TryDealDamage();
            return;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Cactus cactus = other.GetComponentInParent<Cactus>();
        if (cactus != null)
        {
            cactus.TryDealDamage();
        }
    }
}