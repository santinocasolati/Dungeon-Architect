using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopRefresh : MonoBehaviour
{
    [SerializeField] private ObjectsDatabaseSO database;
    [SerializeField] private PlacementSystem placementSystem;
    [SerializeField] private GameObject btnPrefab;
    [SerializeField] private List<ShopSection> sections;

    private void Awake()
    {
        DungeonLevelManager.instance.levelModified += RefreshShop;
    }

    private void RefreshShop(int level)
    {
        foreach (ObjectData item in database.objects)
        {
            if (item.UnlockLevel <= level)
            {
                ShopSection section = sections.FirstOrDefault(s => s.type == item.Type);

                if (section != null)
                {
                    GameObject btn = Instantiate(btnPrefab, section.container.transform);
                    TMP_Text text = btn.GetComponentInChildren<TMP_Text>();
                    text.text = item.Name;

                    btn.GetComponent<Button>().onClick.AddListener(() => ClickAction(item.Id));
                }
            }
        }
    }

    private void ClickAction(int id)
    {
        placementSystem.StartPlacement(id);
    }
}

[System.Serializable]
public class ShopSection
{
    public ObjectType type;
    public GameObject container;
}
