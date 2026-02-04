using System;
using UnityEngine;
using DG.Tweening;

public class SpinInPlace : MonoBehaviour
{
 [Header("Rotation")] 
 [SerializeField] private Vector3 rotationAxis = Vector3.up;
 [SerializeField] private float rotationDuration = 1f;

 private Tween spinTween;

 private void OnEnable()
 {
  spinTween = transform.DORotate(rotationAxis * 360f, rotationDuration, RotateMode.LocalAxisAdd).SetEase(Ease.Linear)
   .SetLoops(-1, LoopType.Restart);
 }

 private void OnDisable()
 {
  spinTween?.Kill();
 }
}
