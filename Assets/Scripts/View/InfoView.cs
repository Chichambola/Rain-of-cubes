using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfoView<T> : MonoBehaviour where T : PoolableObject 
{
    [SerializeField] private Spawner<T> _spawner;
    [SerializeField] private TextMeshProUGUI _text;

    private void OnEnable()
    {
        _spawner.ValuesChanged += UpdateValues;
    }

    private void OnDisable()
    {
        _spawner.ValuesChanged -= UpdateValues;
    }

    private void UpdateValues(SpawnerInfo info)
    {
        _text.text = $"{info.ObjectName}\n" +
            $"Spawned: {info.SpawnedObjects}\n" +
            $"Created: {info.CreatedObjects}\n" +
            $"Active: {info.ActiveObjects}";
    }
}
