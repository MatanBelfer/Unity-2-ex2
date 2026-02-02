using System;
using UnityEngine;

public class AgentAnimator : MonoBehaviour
{
    private Animator _animator;
    private int _areaIndex;
    private float _speed;


    private void Start()
    {
        _animator = GetComponent<Animator>();
        
    }
    
    
}