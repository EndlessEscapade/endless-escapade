using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using Terraria;
using Terraria.ModLoader;

#nullable enable

namespace EEMod.Seamap.SeamapContent
{
    public class ComponentManager
    {
        public SeamapObject Parent { get; }

        public Dictionary<Type, object> Components { get; } = new();

        public ComponentManager(SeamapObject parent)
        {
            Parent = parent;
        }

        /// <summary>
        /// Gets a component
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>The component, or <see langword="null"/> if its not present</returns>
        public T Get<T>() where T : Component 
            => Components.TryGetValue(typeof(T), out var component) ? (T)component : null!;

        public T Get<T>(bool throwsIfNotPresent) where T : Component
        {
            if(Components.TryGetValue(typeof(T), out var entry))
                return (T)entry;
            
            if (throwsIfNotPresent)
                throw new KeyNotFoundException($"Component of type {typeof(T).FullName} was not found");

            return null!;
        }

        public bool TryGet<T>([MaybeNullWhen(false)] out T component) where T : Component
        {
            if(Components.TryGetValue(typeof(T), out var entry))
            {
                component = (T)entry;
                return true;
            }
            component = null;
            return false;
        }

        public bool Has<T>() where T : Component
            => Components.ContainsKey(typeof(T));

        public bool Remove<T>() where T : Component
            => Components.Remove(typeof(T));

        public IEnumerable<T> ComponentsOfType<T>() where T : Component
            => Components.Values.OfType<T>();
    }
}