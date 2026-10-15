using UnityEngine;
using UnityEngine.EventSystems;

public sealed partial class WordfallPrototype
{
    private sealed class ButtonPressFeedback : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        private Vector3 baseScale;

        private void Awake()
        {
            baseScale = transform.localScale;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            transform.localScale = baseScale * 0.94f;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            transform.localScale = baseScale;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            transform.localScale = baseScale * 1.02f;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.localScale = baseScale;
        }
    }

    private sealed class FallingWord
    {
        public readonly string word;
        public readonly WordType type;
        public readonly RectTransform rect;
        public readonly GameObject objectToDestroy;
        public readonly float speed;

        public FallingWord(string word, WordType type, RectTransform rect, GameObject objectToDestroy, float speed)
        {
            this.word = word;
            this.type = type;
            this.rect = rect;
            this.objectToDestroy = objectToDestroy;
            this.speed = speed;
        }
    }
}
