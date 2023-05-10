using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileScrpt : MonoBehaviour
{
    private Rigidbody2D rigidbody;
    private Quaternion rotateToTarget;
    private Vector3 direction;
    public Transform target;
    private void Start()
    {
        rigidbody= GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //calculating the direction the missile has to go towards
        direction = (target.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rotateToTarget = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation,rotateToTarget,Time.deltaTime * 10f);
        rigidbody.velocity = new Vector2(direction.x * 4,direction.y * 4);
    }
}
