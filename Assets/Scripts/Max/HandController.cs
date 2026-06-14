using UnityEngine;

public class HandController : MonoBehaviour
{
    public Camera mainCamera;
    public BoxCollider movementBounds;

    [Header("Height")]
    public float handHeight = 1f;
    public float minHeight = 0.3f;
    public float maxHeight = 3f;
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
        handHeight = Mathf.Clamp(handHeight, minHeight, maxHeight);
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

        Vector3 localPoint = movementBounds.transform.InverseTransformPoint(worldPoint);

        Vector3 localCenter = movementBounds.center;
        Vector3 halfSize = movementBounds.size * 0.5f;

        localPoint.x = Mathf.Clamp(localPoint.x, localCenter.x - halfSize.x, localCenter.x + halfSize.x);
        localPoint.y = Mathf.Clamp(localPoint.y, localCenter.y - halfSize.y, localCenter.y + halfSize.y);
        localPoint.z = Mathf.Clamp(localPoint.z, localCenter.z - halfSize.z, localCenter.z + halfSize.z);

        return movementBounds.transform.TransformPoint(localPoint);
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