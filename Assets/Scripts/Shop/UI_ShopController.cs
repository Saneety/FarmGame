using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class UI_ShopController : MonoBehaviour
{
    [System.Serializable]
    public class ShopItemData
    {
        public Sprite itemSprite;
        public Item item;
        public int itemCost;
        public string displayedName;
    }

    private Transform container;
    private Transform shopItemTemplate;

    [SerializeField] private MoneyController money;
    [SerializeField] private GameObject toolbarPanel;
    [SerializeField] private GameObject inventoryPanel;

    [SerializeField] private List<ShopItemData> shopItems;

    public Button btn;
    public bool isOpen;

    private void Awake()
    {
        container = transform.Find("container");
        shopItemTemplate = container.Find("shopItemTemplate");
    }

    private void Start()
    {
        for (int i = 0; i < shopItems.Count; i++)
        {
            CreateItemButton(shopItems[i], i);
        }

        gameObject.SetActive(false);
        Hide();
    }

    private void CreateItemButton(ShopItemData data, int positionIndex)
    {
        Transform shopItemTransform = Instantiate(shopItemTemplate, container);
        RectTransform shopItemRectTransform = shopItemTransform.GetComponent<RectTransform>();

        float shopItemHeight = 60f;
        shopItemRectTransform.anchoredPosition = new Vector2(0, 150 + (-shopItemHeight * positionIndex));

        shopItemTransform.Find("nameText").GetComponent<TextMeshProUGUI>().SetText(data.displayedName);
        shopItemTransform.Find("priceText").GetComponent<TextMeshProUGUI>().SetText(data.itemCost.ToString());
        shopItemTransform.Find("itemIcon").GetComponent<Image>().sprite = data.itemSprite;

        btn = shopItemTransform.GetComponent<Button>();
        btn.onClick.AddListener(() => TaskWithParameters(data.itemCost, data.item));
    }

    void TaskWithParameters(long itemCost, Item item)
    {
        if (money.canBuyItems(itemCost))
        {
            money.substractMoney(itemCost);
            FindObjectOfType<SoundManager>().Play("Money");

            // 🔥 СТАРАЯ ЛОГИКА СОХРАНЕНА
            if (item.Name.Contains("Seeds_Corn"))
            {
                GameManager.instance.inventoryContainer.Add(item, 4);
            }
            else if (item.Name.Contains("Seeds_Tomato"))
            {
                GameManager.instance.inventoryContainer.Add(item, 3);
            }
            else if (item.Name.Contains("Seeds_Strawberry"))
            {
                GameManager.instance.inventoryContainer.Add(item, 6);
            }
            else if (item.Name.Contains("Seeds_Parsley"))
            {
                GameManager.instance.inventoryContainer.Add(item, 3);
            }
            else if (item.Name.Contains("Seeds_Potato"))
            {
                GameManager.instance.inventoryContainer.Add(item, 1);
            }
            else
            {
                GameManager.instance.inventoryContainer.Add(item);
            }
        }

        toolbarPanel.SetActive(!toolbarPanel.activeInHierarchy);
        toolbarPanel.SetActive(true);
    }

    public void Show()
    {
        isOpen = true;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        isOpen = false;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        inventoryPanel.SetActive(false);
        toolbarPanel.SetActive(true);
    }
}