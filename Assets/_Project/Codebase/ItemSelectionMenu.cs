using System;
using System.Collections.Generic;
using CustomUI;
using UnityEngine;

namespace _Project.Codebase
{
    public class ItemSelectionMenu : Menu
    {
        private SelectableItem _selectedItem;
        [SerializeField] private Transform _spawnTransform;
        [SerializeField] private GameObject _selectableItemPrefab;

        protected override void Start()
        {
            base.Start();
            /*
            foreach (SelectableItem item in _items)
            {
                //item.SetData(data);
                item.onSelect += OnItemSelected;
            }
            */
        }

        protected void OnItemSelected(object sender, EventArgs eventArgs)
        {
            
        }
    }
}