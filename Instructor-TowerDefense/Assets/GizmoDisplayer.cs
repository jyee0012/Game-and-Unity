﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoDisplayer : MonoBehaviour
{

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(transform.position, .1f);
    }
}