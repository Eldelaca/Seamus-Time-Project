using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    private bool collidingWithItem;
    public bool holdingItem;
    private string collidingWithItemName;
    private GameObject itemHoldingGameObject;
    public string itemHoldingName;
    [SerializeField] Image itemImage;
    public Transform itemDropPos;
    
    
    [Header("Items Prefabs")]
    [SerializeField] private GameObject keyRedPrefab;
    [SerializeField] private GameObject keyBluePrefab;
    [SerializeField] private GameObject keyGreenPrefab;

    [Header("Item Icons")]
    [SerializeField] private Sprite keyRedIcon;
    [SerializeField] private Sprite keyBlueIcon;
    [SerializeField] private Sprite keyGreenIcon;

    private void Start()
    {
        itemImage.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Holdable"))
        {
            collidingWithItem = true;
            collidingWithItemName = other.gameObject.transform.root.name;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Holdable"))
        {
            collidingWithItem = false;
            collidingWithItemName = "";
        }
    }

    public void PickUpItem()
    {
        if (collidingWithItem && !holdingItem && itemHoldingName == "")
        {
            holdingItem = true;
            itemHoldingName = collidingWithItemName;
            itemHoldingGameObject = GameObject.Find(itemHoldingName);
            
            itemImage.sprite = GetImageForItem(itemHoldingName);
            itemImage.enabled = true; 
            
            Destroy(itemHoldingGameObject);
            collidingWithItem = false;
            collidingWithItemName = "";
        }
    }
    public void DropItem()
    {
        GameObject prefabToDrop = GetPrefabForItem(itemHoldingName);
        GameObject droppedItem = Instantiate(prefabToDrop, itemDropPos.position, itemDropPos.rotation);
        Rigidbody itemRb = droppedItem.GetComponent<Rigidbody>();
        itemRb.AddForce(itemDropPos.forward * 100f, ForceMode.Impulse); // Adjust force as needed
        itemRb.angularDamping = 2f;
        droppedItem.name = itemHoldingName; 
        
        itemImage.enabled = false;
        
        holdingItem = false;
        itemHoldingName = "";
        itemHoldingGameObject = null;   
    }
    
    private Sprite GetImageForItem(string itemName)
    {
        switch (itemName)
        {
            case "KeyRed":
                return keyRedIcon;
            case "KeyBlue":
                return keyBlueIcon;
            case "KeyGreen":
                return keyGreenIcon;
            default:
                return null;
        }
    }
    
    private GameObject GetPrefabForItem(string itemName)
    {
        switch (itemName)
        {
            case "KeyRed":
                return keyRedPrefab;
            case "KeyBlue":
                return keyBluePrefab;
            case "KeyGreen":
                return keyGreenPrefab;
            default:
                return null;
        }
    }
}
