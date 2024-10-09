using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;

    // 앞으로 이동
    public void MoveForward()
    {
        transform.Translate(transform.forward * moveSpeed * Time.deltaTime);
    }

    // 뒤로 이동
    public void MoveBackward()
    {
        transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
    }

    // 왼쪽으로 이동
    public void MoveLeft()
    {
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
    }

    // 오른쪽으로 이동
    public void MoveRight()
    {
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
    }
}
