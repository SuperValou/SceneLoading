using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.LoadingSystems.CrossSceneObjects
{
    public abstract class CrossSceneSet<TItem> : ScriptableObject
    {
        private readonly ICollection<TItem> _items = new HashSet<TItem>();
        
        public IEnumerable<TItem> Items => _items;

        public void Add(TItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (_items.Contains(item))
            {
                throw new InvalidOperationException($"Item '{item}' cannot be added to '{this.name}' ({nameof(CrossSceneSet<TItem>)}) " +
                                                    $"because it is already present.");
            }

            _items.Add(item);
        }

        public void Remove(TItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (!_items.Contains(item))
            {
                throw new InvalidOperationException($"Item '{item}' cannot be removed from '{this.name}' ({nameof(CrossSceneSet<TItem>)}) " +
                                                    $"because it was not present in the first place. Did you forget a call to the {nameof(Add)} method?");
            }

            _items.Remove(item);
        }

        void OnDisable()
        {
            if (_items.Count == 0)
            {
                return;
            }

            Debug.LogError($"{this.name} ({this.GetType().Name}) is being disabled, but {_items.Count} {typeof(TItem).Name}(s) are still referenced. " +
                           $"Did you forgot some calls to the {nameof(Remove)} method?");
            _items.Clear();
        }
    }
}