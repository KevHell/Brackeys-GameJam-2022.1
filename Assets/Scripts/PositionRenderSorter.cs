using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionRenderSorter : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool _updateOnce;
    [Space(10)]
    [SerializeField] private int _sortingOrderBase = 5000;
    [SerializeField] private float _offset = 0;
    
    private SpriteRenderer _spriteRenderer;
    private float _timer;
    private float _maxTimer = 0.1f;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        _timer -= Time.deltaTime;
        if (!(_timer <= 0)) return;
        
        _spriteRenderer.sortingOrder = (int)(_sortingOrderBase - transform.position.y - _offset);
        if (_updateOnce) Destroy(this);
    }
}
