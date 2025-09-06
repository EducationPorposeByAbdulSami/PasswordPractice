using UnityEngine;
using UnityEngine.EventSystems;

public class PasswordSlot : MonoBehaviour, IDropHandler
{
    public string currentValue = "";

    private DraggableTile currentTile;
    public DraggableTile CurrentTile => currentTile;

    private PasswordBar bar;  // reference to the bar

    private void Start()
    {
        bar = FindObjectOfType<PasswordBar>();  // find the bar in scene
    }

    public void OnDrop(PointerEventData eventData)
    {
        DraggableTile tile = eventData.pointerDrag.GetComponent<DraggableTile>();
        if (tile == null) return;

        if (currentTile != null)
        {
            currentTile.ReturnHome();
        }

        currentTile = tile;
        currentValue = tile.tileValue;

        tile.MarkDroppedInto(transform);

        // ✅ tell PasswordBar to update
        bar?.UpdatePassword();

        AudioManager.Instance.PlaySound(AudioManager.Instance.plipHit);
    }

    private void Update()
    {
        if (currentTile != null && currentTile.transform.parent != transform)
        {
            currentTile = null;
            currentValue = "";

            // ✅ tell PasswordBar to update
            bar?.UpdatePassword();
        }
    }
}
