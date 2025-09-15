using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Movement")]
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;
    public float waitTime = 1f;

    private Transform target;
    private float waitTimer;
    private bool isWaiting = false;

    void Start()
    {
        target = pointB;

        if (pointA == null || pointB == null)
        {
            CreateDefaultPoints();
        }
    }

    void Update()
    {
        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0)
            {
                isWaiting = false;
                target = (target == pointA) ? pointB : pointA;
            }
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            isWaiting = true;
            waitTimer = waitTime;
        }
    }

    void CreateDefaultPoints()
    {
        GameObject pointAObj = new GameObject("PointA");
        pointAObj.transform.SetParent(transform);
        pointAObj.transform.position = transform.position + Vector3.left * 3f;
        pointA = pointAObj.transform;

        GameObject pointBObj = new GameObject("PointB");
        pointBObj.transform.SetParent(transform);
        pointBObj.transform.position = transform.position + Vector3.right * 3f;
        pointB = pointBObj.transform;
    }

    void OnDrawGizmos()
    {
        if (pointA != null && pointB != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(pointA.position, pointB.position);
            Gizmos.DrawWireSphere(pointA.position, 0.3f);
            Gizmos.DrawWireSphere(pointB.position, 0.3f);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}