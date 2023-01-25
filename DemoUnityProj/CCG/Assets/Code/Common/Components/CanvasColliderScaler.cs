using System;
using UnityEngine;

namespace CCG.Common.Components
{
    
    [ExecuteInEditMode]
    public class CanvasColliderScaler : MonoBehaviour
    {

        private BoxCollider2D _collider;
        private RectTransform _cachedTransform;
        
#if UNITY_EDITOR
        private void Update()
        {
            if (!Application.isPlaying)
            {
                SetSize();
            }
        }
#endif

        private void FixedUpdate()
        {
            SetSize();
        }

        private void SetSize()
        {
            if (_collider is null)
            {
                var col = GetComponent<BoxCollider2D>();
                if (col is null)
                {
                    Debug.LogException(new Exception("There is not box2d collider attached to the object."));
                    return;
                }

                if (col != null)
                {
                    _collider = col;
                }
                else
                {
                    return;
                }
            }
            
            if (_cachedTransform is null)
            {
                var t = (RectTransform)transform;
                if (t != null)
                {
                    _cachedTransform = t;
                }
                else
                {
                    return;
                }
            }

            _collider.size = _cachedTransform.rect.size;
        }
    }
}
