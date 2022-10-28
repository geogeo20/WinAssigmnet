using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayCard : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField]
    private Image cardImage = null;
    [SerializeField]
    private Image holder = null;
    [SerializeField]
    private Animation anim = null;
    [SerializeField]
    private CardFlipper cardFlipper = null;
    [SerializeField]
    private float animationTime = 0;
    [SerializeField]
    private RectTransform rect = null;

    public int CardValue { get; private set; }
    public bool WildCard { get; private set; }
    public bool IsAce { get; private set; }


    private Action<PlayCard> onCardAttached;
    private Vector3 startPosition;
    private Transform targetSlot;

    public void Init(CardData cardData, Action<PlayCard> onCardAttached, Transform targetSlot)
    {
        enabled = true;
        WildCard = cardData.isWildCard;
        CardValue = cardData.cardValue;
        IsAce = cardData.isAce;
        cardImage.sprite = cardData.cardVisual;
        holder.raycastTarget = false;
        cardFlipper.Init(cardData.cardVisual);
        this.onCardAttached = onCardAttached;
        this.targetSlot = targetSlot;

        rect.anchoredPosition = new Vector2((rect.sizeDelta.x / 2), rect.anchoredPosition.y);
    }

    public void MoveCard()
    {
        rect.anchoredPosition = new Vector2(rect.anchoredPosition.x + 25, rect.anchoredPosition.y);
    }

    public void DropCard()
    {
        StartCoroutine(DropCardAnimation());
    }

    private IEnumerator DropCardAnimation()
    {
        anim.Play("Flip");
        yield return new WaitForSeconds(animationTime);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = transform.position;
        holder.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.pointerEnter.TryGetComponent<ColumnSlot>(out ColumnSlot slot) && slot.CanAttach())
        {
            onCardAttached?.Invoke(this);
            slot.AttachCard(this);
            enabled = false;
        }
        else
        {
            transform.position = startPosition;
            holder.raycastTarget = true;
        }
    }

    private float speed = 500;

    public void MoveToCurrentSlot()
    {
        StartCoroutine(Move());
        cardFlipper.FlipCard();
    }

    private IEnumerator Move()
    {
        while (Vector3.Distance(transform.position, targetSlot.position) > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetSlot.position, speed * Time.deltaTime);
            yield return null;
        }

        transform.SetParent(targetSlot);
        holder.raycastTarget = true;
    }
}