using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bullet : MonoBehaviour
{

    public event Action OnArrive;

    public float speed = 3.0f;

    public Vector3 target;

    public void SetTarget(Vector3 target)
    {
        this.target = target;
    }

    private void Update()
    {
        float sqrDis = Vector3.SqrMagnitude(transform.position - target);
        float move = speed * Time.deltaTime;
        if (sqrDis > move * move)
        {
            transform.position += (target - transform.position).normalized * move;
        }
        else
        {
            OnArrive?.Invoke();
            GameManager.Instance.RecycleBullet(this);
        }
    }

}
