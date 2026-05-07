using System.Collections.Generic;
using UnityEngine;

public class GuideTrail : MonoBehaviour
{
    [Header("Path")]
    public List<Transform> points;

    [Header("Checkpoint system")]
    public Transform player;
    public Transform currentCheckpoint;

    [Header("Movement")]
    public float moveSpeed = 3f;
    public float hoverAmplitude = 0.3f;
    public float hoverSpeed = 2f;

    private int index = 0;
    private bool waiting = false;

    void Start()
    {
        if (points.Count > 0)
        {
            transform.position = points[0].position;
            SetNextCheckpoint();
        }
    }

    void Update()
    {
        if (points.Count == 0) return;

        if (!waiting)
        {
            MoveToPoint();
        }
        else
        {
            Hover();

            if (PlayerPassedCheckpoint())
            {
                Advance();
            }
        }
    }

    void MoveToPoint()
    {
        Transform target = points[index];

        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, target.position) < 0.05f)
        {
            waiting = true;
        }
    }

    void Hover()
    {
        Vector3 pos = transform.position;
        pos.y += Mathf.Sin(Time.time * hoverSpeed) * hoverAmplitude * Time.deltaTime;
        transform.position = pos;
    }

    bool PlayerPassedCheckpoint()
    {
        if (player == null || currentCheckpoint == null) return false;

        // simple "passed point" check (z-axis example, adjust if needed)
        return player.position.z > currentCheckpoint.position.z;
    }

    void Advance()
    {
        waiting = false;

        if (index < points.Count - 1)
        {
            index++;
            SetNextCheckpoint();
        }
    }

    void SetNextCheckpoint()
    {
        // checkpoint is simply the current waypoint position reference
        currentCheckpoint = points[index];
    }
}
