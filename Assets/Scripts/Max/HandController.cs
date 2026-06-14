using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HandController : MonoBehaviour
{
    public Camera mainCamera;
    public Rigidbody rb;
    public BoxCollider movementBounds;

    [Header("Height")]
    public float handHeight = 1f;
    public float minHeight = 0.3f;
    public float maxHeight = 3f;
    public float scrollHeightSpeed = 0.5f;

    [Header("Movement")]
    public float moveSpeed = 20f;
    public float maxSpeed = 25f;
    public float stopDistance = 0.1f;
    public float slowRadius = 1.5f;
    public float velocityDamping = 8f;

    [Header("Debug")]
    public Vector3 targetPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        targetPosition = transform.position;
        handHeight = transform.position.y;

        rb.useGravity = false;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    private void Update()
    {
        if (mainCamera == null || InputHandler.Instance == null) return;

        UpdateHeight();
        UpdateTargetPosition();
    }

    private void FixedUpdate()
    {
        MoveHandPhysics();
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
            hitPoint.y = handHeight;

            targetPosition = ClampPointToBounds(hitPoint);
        }
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

    public void MoveHandPhysics()
    {
        Vector3 toTarget = targetPosition - rb.position;
        float distance = toTarget.magnitude;

        if (distance <= stopDistance)
        {
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, Vector3.zero, velocityDamping * Time.fixedDeltaTime);
            return;
        }

        float speedFactor = 1f;

        if (distance < slowRadius)
            speedFactor = distance / slowRadius;

        Vector3 desiredVelocity = toTarget.normalized * moveSpeed * speedFactor;
        Vector3 newVelocity = Vector3.Lerp(rb.linearVelocity, desiredVelocity, velocityDamping * Time.fixedDeltaTime);

        if (newVelocity.magnitude > maxSpeed)
            newVelocity = newVelocity.normalized * maxSpeed;

        rb.linearVelocity = newVelocity;
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