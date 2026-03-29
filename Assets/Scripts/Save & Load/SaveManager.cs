using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    private string savePath;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        savePath = Path.Combine(Application.persistentDataPath, "save.json");
    }

    // =========================
    // SAVE
    // =========================
    public void SaveGame()
    {
        SaveData data = new()
        {
            // Scene
            sceneName = SceneManager.GetActiveScene().name
        };

        // Player
        if (PlayerData.Instance != null)
        {
            data.money = PlayerData.Instance.Money;
            data.purchasedUpgrades = new List<string>(PlayerData.Instance.PurchasedUpgrades);

            // Inventory
            data.inventory = new List<InventoryItemSave>();

            foreach (var item in PlayerData.Instance.Inventory.Items)
            {
                if (item == null)
                {
                    data.inventory.Add(null);
                    continue;
                }

                data.inventory.Add(new InventoryItemSave
                {
                    itemID = item.data.itemID, 
                    value = item.value,
                    isSellable = item.isSellable,
                    isEraseable = item.isEraseable
                });
            }
        }

        // Ship
        if (GameStateManager.Instance != null && GameStateManager.Instance.HasSavedShipTransform)
        {
            var pos = GameStateManager.Instance.SavedShipPosition;
            var rot = GameStateManager.Instance.SavedShipRotation;

            data.shipPosition = new float[] { pos.x, pos.y, pos.z };
            data.shipRotation = new float[] { rot.x, rot.y, rot.z, rot.w };
        }

        SaveCelestialStatesForFile(data);

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);

        Debug.Log("Game saved at: " + savePath);
    }

    private void SaveCelestialStatesForFile(SaveData data)
    {
        if (GameStateManager.Instance == null)  return;

        if (GameStateManager.Instance.IsInSpaceScene)
        {
            CacheCurrentLiveCelestialStates();
        }

        data.celestialStates = GameStateManager.Instance.ExportCelestialStates();
    }

    private void CacheCurrentLiveCelestialStates()
    {
        if (GameStateManager.Instance == null) return;

        CelestialStateId[] stateIds = FindObjectsByType<CelestialStateId>(FindObjectsSortMode.None);

        GameStateManager.Instance.ClearCelestialStates();

        foreach (CelestialStateId stateId in stateIds)
        {
            if (stateId == null) continue;
            if (string.IsNullOrEmpty(stateId.UniqueId)) continue;
            if (!stateId.TryGetComponent<Rigidbody>(out var rb)) continue;

            GameStateManager.Instance.SaveCelestialState(stateId.UniqueId, rb);
        }
    }

    public bool HasSave()
    {
        return File.Exists(savePath);
    }

    public void DeleteSave()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("Save file deleted.");
        }
    }

    // =========================
    // LOAD ENTRY (MENU CALLS THIS)
    // =========================
    public void LoadGameAndChangeScene()
    {
        if (!File.Exists(savePath))
        {
            Debug.Log("No save found.");
            return;
        }

        string json = File.ReadAllText(savePath);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        PreloadGlobalState(data);

        // Load scene first
        SceneManager.LoadScene(data.sceneName);

        // Then apply data AFTER scene loads
        StartCoroutine(LoadAfterScene(data));
    }

    private void PreloadGlobalState(SaveData data)
    {
        if (data == null || GameStateManager.Instance == null)
            return;

        if (data.celestialStates != null)
            GameStateManager.Instance.ImportCelestialStates(data.celestialStates);

        if (data.shipPosition != null && data.shipPosition.Length >= 3 &&
            data.shipRotation != null && data.shipRotation.Length >= 4)
        {
            Vector3 pos = new(
                data.shipPosition[0],
                data.shipPosition[1],
                data.shipPosition[2]
            );

            Quaternion rot = new(
                data.shipRotation[0],
                data.shipRotation[1],
                data.shipRotation[2],
                data.shipRotation[3]
            );

            if (rot == new Quaternion(0f, 0f, 0f, 0f))
                rot = Quaternion.identity;
            else
                rot = Quaternion.Normalize(rot);

            GameStateManager.Instance.SaveShipTransform(pos, rot);
        }
    }

    // =========================
    // LOAD AFTER SCENE
    // =========================
    private IEnumerator LoadAfterScene(SaveData data)
    {
        yield return null; // wait 1 frame

        ApplySaveData(data);
    }

    // =========================
    // APPLY DATA
    // =========================
    private void ApplySaveData(SaveData data)
    {
        if (data == null) return;

        // ===== PLAYER =====
        if (PlayerData.Instance != null)
        {
            PlayerData.Instance.Money = data.money;

            PlayerData.Instance.PurchasedUpgrades = new HashSet<string>(data.purchasedUpgrades);

            // Reapply upgrades 
            PlayerData.Instance.ReapplyPurchasedUpgrades();
        }

        // ===== INVENTORY =====
        if (PlayerData.Instance != null && data.inventory != null)
        {
            var inventory = PlayerData.Instance.Inventory;

            for (int i = 0; i < inventory.Items.Count; i++)
            {
                inventory.Items[i] = null;

                if (i >= data.inventory.Count) continue;

                var savedItem = data.inventory[i];
                if (savedItem == null) continue;

                ItemData itemData = ItemDatabase.Instance.GetById(savedItem.itemID);
                if (itemData == null) continue;

                ItemInstance instance = new(itemData)
                {
                    value = savedItem.value,
                    isSellable = savedItem.isSellable,
                    isEraseable = savedItem.isEraseable
                };

                inventory.Items[i] = instance;
            }
        }

        // ===== SHIP =====
        if (GameStateManager.Instance != null && data.shipPosition != null)
        {
            Vector3 pos = new(
                data.shipPosition[0],
                data.shipPosition[1],
                data.shipPosition[2]
            );

            Quaternion rot = new(
                data.shipRotation[0],
                data.shipRotation[1],
                data.shipRotation[2],
                data.shipRotation[3]
            );

            GameStateManager.Instance.SaveShipTransform(pos, rot);
        }

        Debug.Log("Game loaded successfully.");
    }
}