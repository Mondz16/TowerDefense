using System;
using System.Collections;
using System.Collections.Generic;
using TowerDefense.PathFinding;
using UnityEngine;
using UnityEngine.EventSystems;

public class DefenderDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private RectTransform _rectTransform;

    private bool _isDragging = false;
    private UIManager _uiManager;

    public void InjectUIManager(UIManager uiManager)
    {
        _uiManager = uiManager;
    }

    private void Update()
    {
        if (_isDragging)
        {
            Vector3 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(MousePos, Vector3.zero);
            if (hit.transform != null)
            {
                if (hit.transform.TryGetComponent(out Node node))
                {
                    if(node.isWalkable)
                        node.ShowHighlightIndicator();
                }
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _isDragging = true;
        _canvasGroup.alpha = .6f;
        _canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log($"#{GetType().Name}# On Drop!");
        
        Vector3 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(MousePos, Vector3.zero);
        if (hit.transform != null)
        {
            if (hit.transform.TryGetComponent(out Node node))
            {
                _uiManager.OnDefenderDropEventTriggered();
            }
        }
        
        _canvasGroup.alpha = 1f;
        _canvasGroup.blocksRaycasts = true;
        _rectTransform.localPosition = Vector2.zero;
        _isDragging = false;
    }
}
