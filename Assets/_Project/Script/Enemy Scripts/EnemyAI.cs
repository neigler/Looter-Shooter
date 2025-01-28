using UnityEngine;
using Pathfinding;
using System.Collections;
using Unity.VisualScripting;


public enum State
{
    Attacking,
    Pursuing,
    Wandering
}
public class EnemyAI : MonoBehaviour
{
    public Transform target;
    public float radius = 10;
    public float tooCloseRadius = 10;
    public State state;

    [Header("FOV")]
    public float fovRadius = 5f;
    [Range(1, 360)] public float fovAngle = 45f;
    public LayerMask targetLayer;
    public LayerMask obstructionLayer;

    public GameObject playerRef;
    public bool CanSeePlayer { get; private set; }


    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = true;
    Vector2 direction;
    Vector2 temporarytarget;

    public float notSeenSpeed;
    public float seenSpeed;

    Seeker seeker;
    AIPath ai;
    Rigidbody2D rb;

    private void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        ai = GetComponent<AIPath>();

        StartCoroutine(FOVCheck());

        //InvokeRepeating("UpdatePath", 0f, 0.2f);
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    private void Update()
    {
        if (target == null)
        {
            target = GameObject.Find("Player").GetComponent<Transform>();
        }
        if (playerRef == null)
        {
            playerRef = GameObject.Find("Player").GetComponent<GameObject>();
        }
        if (state == State.Pursuing)
        {
            ai.maxSpeed = seenSpeed;
            StartCoroutine(pathToPlayer());
        }
        if (state == State.Wandering)
        {
            ai.maxSpeed = notSeenSpeed;
            if (ai.reachedEndOfPath)
            {
                if (state == State.Wandering)
                {
                    temporarytarget = PickRandomPoint();
                }
            }
            if (reachedEndOfPath == true)
                temporarytarget = PickRandomPoint();
            seeker.StartPath(rb.position, new Vector2(temporarytarget.x, temporarytarget.y), OnPathComplete);
        }

        if (CanSeePlayer)
        {
            state = State.Pursuing;
        }
    }

    private void FixedUpdate()
    {
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }
    }

    Vector2 PickRandomPoint()
    {
        var point = Random.insideUnitSphere * radius;

        point += transform.position;
        return point;
    }

    private IEnumerator pathToPlayer()
    {
        yield return new WaitForSeconds(0.2f);
        UpdatePath();
    }

    private IEnumerator FOVCheck()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FOV();
        }
    }

    private void FOV()
    {
        Collider2D[] rangeCheck = Physics2D.OverlapCircleAll(transform.position, radius, targetLayer);

        if (rangeCheck.Length > 0)
        {
            Transform target = rangeCheck[0].transform;
            Vector2 directionToTarget = (target.position - transform.position).normalized;

            if (Vector2.Angle(transform.up, directionToTarget) < fovAngle / 2)
            {
                float distanceToTarget = Vector2.Distance(transform.position, target.position);

                if (!Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionLayer))
                    CanSeePlayer = true;
                else
                    CanSeePlayer = false;
            }
            else
                CanSeePlayer = false;
        }
        else if (CanSeePlayer)
            CanSeePlayer = false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, fovRadius);

        Vector3 angle01 = DirectionFromAngle(-transform.eulerAngles.z, -fovAngle / 2);
        Vector3 angle02 = DirectionFromAngle(-transform.eulerAngles.z, fovAngle / 2);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + angle01 * fovRadius);
        Gizmos.DrawLine(transform.position, transform.position + angle02 * fovRadius);

        if (CanSeePlayer)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, playerRef.transform.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            state = State.Pursuing;
        }
    }

    private Vector2 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector2(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
