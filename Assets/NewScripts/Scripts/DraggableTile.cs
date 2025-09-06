using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class DraggableTile : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Tile Data")]
    public string tileValue; // Set manually in Inspector
    public TileType tileType;

    [Header("UI References")]
    public TextMeshProUGUI label;

    private Vector3 defaultPosition; // Default position of tiles

    [HideInInspector] public Transform homeParent;
    [HideInInspector] public Transform dragLayer;
    [HideInInspector] public Canvas rootCanvas;

    private RectTransform rect;
    private CanvasGroup cg;
    private bool placedInSlot = false;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        cg = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        // Save default position & parent
        defaultPosition = transform.position;
        homeParent = transform.parent;

        // Find DragLayer + Canvas automatically if not set
        if (dragLayer == null)
            dragLayer = GameObject.Find("DragLayer").transform;
        if (rootCanvas == null)
            rootCanvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        placedInSlot = false;
        cg.blocksRaycasts = false;
        transform.SetParent(dragLayer, true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rect.anchoredPosition += eventData.delta / rootCanvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        cg.blocksRaycasts = true;

        if (!placedInSlot)
        {
            ReturnHome();
        }
    }




    public void MarkDroppedInto(Transform slotTransform)
    {
        placedInSlot = true;

        // parent to the slot (local space)
        transform.SetParent(slotTransform, false);

        // ensure no scale inherited issues
        rect.localScale = Vector3.one;

        // center the tile's anchors & pivot so anchoredPosition=zero means center
        rect.anchorMin = rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);

        // center it
        rect.anchoredPosition = Vector2.zero;

        // match the slot's current pixel size
        RectTransform slotRect = slotTransform.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(slotRect.rect.width, slotRect.rect.height);
    }


    public void ReturnHome()
    {
        placedInSlot = false;
        transform.SetParent(homeParent, false);
        rect.anchoredPosition = Vector2.zero;
    }
    
    // reset tiles positions
    public void ResetToDefault()
    {
        placedInSlot = false;
        transform.SetParent(homeParent, false);
        rect.anchoredPosition = Vector2.zero;
        rect.sizeDelta = new Vector2(100, 100); // or whatever your default tile size is
    }

}

public enum TileType { Word, Number, Symbol }
