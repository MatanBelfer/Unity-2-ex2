using System;
using UnityEngine;
using DG.Tweening;

public class MoveObject : MonoBehaviour
{
    [Header("Movement")] [SerializeField] private float duration = 0.2f;
    [SerializeField] private float distance = 0.5f;


    private Tween move;
    private Vector3 startLocalPos;

    private void Awake()
    {
        startLocalPos = transform.localPosition;
    }

    private void OnEnable()
    {
        StartMoving();
    }

    private void OnDisable()
    {
        move?.Kill();
    }

    private void StartMoving()
    {
        move = transform.DOLocalMoveX(startLocalPos.x + distance, duration)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Yoyo);
    }
}