using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Kogane.Internal
{
    [DisallowMultipleComponent]
    [ExecuteAlways]
    [RequireComponent( typeof( TMP_Text ) )]
    [RequireComponent( typeof( ContentSizeFitter ) )]
    internal sealed class ContentSizeFitterDisableDirty : MonoBehaviour
    {
        private TMP_Text          m_tmpTextCache;
        private ContentSizeFitter m_contentSizeFitterCache;

        private TMP_Text          TmpText           => m_tmpTextCache != null ? m_tmpTextCache : m_tmpTextCache = GetComponent<TMP_Text>();
        private ContentSizeFitter ContentSizeFitter => m_contentSizeFitterCache != null ? m_contentSizeFitterCache : m_contentSizeFitterCache = GetComponent<ContentSizeFitter>();

        private bool   m_isInitialize;
        private string m_text;
        private float  m_fontSize;

        private static bool IsPlaying =>
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying;
#else
            true;
#endif

        private void Reset()
        {
            ContentSizeFitter.enabled = false;
        }

        private void Awake()
        {
            if ( !IsPlaying ) return;

            ContentSizeFitter.enabled = true;
        }

        private void Update()
        {
            if ( IsPlaying ) return;

            var currentText     = TmpText.text;
            var currentFontSize = TmpText.fontSize;

            if ( !m_isInitialize )
            {
                m_isInitialize = true;
                m_text         = currentText;
                m_fontSize     = currentFontSize;
                return;
            }

            if ( m_text != currentText || 0.001f < Math.Abs( m_fontSize - currentFontSize ) )
            {
                Apply();
            }

            m_text     = currentText;
            m_fontSize = currentFontSize;
        }

        private void Apply()
        {
            ContentSizeFitter.enabled = true;
            Canvas.ForceUpdateCanvases();
            ContentSizeFitter.enabled = false;
        }
    }
}