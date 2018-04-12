using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform target;
    public float speed;

	void Update ()
    {
		transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
	}
}
