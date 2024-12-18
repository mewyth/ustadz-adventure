using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    private TextMeshProUGUI keyText;

    private void Start()
    {
        keyText = GetComponent<TextMeshProUGUI>();
    }

    public void updateKey(PlayerInventory playerInventory)
    {
        keyText.text = playerInventory.keys.ToString();
    }
}
