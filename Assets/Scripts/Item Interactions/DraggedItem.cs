using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DraggedItem : MonoBehaviour
{
    public static DraggedItem Instance { get; private set; }

    [SerializeField] 
    private Image itemImage;

    private ItemInstance currentItemInstance;
    public ItemInstance CurrentItemInstance => currentItemInstance;
    public bool HasItem => currentItemInstance != null;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
            
        Instance = this;
        Hide();
    }

    private void Update()
    {
        if (!HasItem || Mouse.current == null) return;

        transform.position = Mouse.current.position.ReadValue();
    }

    public void SetItem(ItemInstance item)
    {
        currentItemInstance = item;
        if (item != null)
        {
            gameObject.SetActive(true);
            itemImage.sprite = item.data.icon;
            itemImage.enabled = true;
        }
        else
            Hide();
    }

    public void Hide()
    {
        currentItemInstance = null;
        itemImage.enabled = false;
        gameObject.SetActive(false);
    }

}

