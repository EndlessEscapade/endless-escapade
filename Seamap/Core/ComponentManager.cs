using EEMod.Seamap.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

#nullable enable

namespace EEMod.Seamap.Core
{
    public class ComponentManager
    {
        /// <summary> The object this component manager belongs to </summary>
        public SeamapObject Parent { get; }

        public Dictionary<Type, object> Components { get; } = new();

        public ComponentManager(SeamapObject parent)
        {
            Parent = parent;
        }

        /// <summary>
        /// Sets a component
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="component"></param>
        public void Set<T>(T component) where T : Component
        {
            Components[typeof(T)] = component;
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
            if (Components.TryGetValue(typeof(T), out var entry))
                return (T)entry;

            if (throwsIfNotPresent)
                throw new KeyNotFoundException($"Component of type {typeof(T).FullName} was not found");

            return null!;
        }

        /// <summary>
        /// Attempts to get a component
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="component"></param>
        /// <returns><see langword="true"/> if the component is found, <see langword="false"/> otherwise </returns>
        public bool TryGet<T>([MaybeNullWhen(false)] out T component) where T : Component
        {
            if (Components.TryGetValue(typeof(T), out var entry))
            {
                component = (T)entry;
                return true;
            }
            component = null;
            return false;
        }

        /// <summary>
        /// If the given component is present
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns><see langword="true"/> if the component is found, <see langword="false"/> otherwise </returns>
        public bool Has<T>() where T : Component
            => Components.ContainsKey(typeof(T));

        /// <summary>
        /// Removes a component
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns><see langword="true"/> if the component was removed, <see langword="false"/> otherwise </returns>
        public bool Remove<T>() where T : Component
            => Components.Remove(typeof(T));

        /// <summary>
        /// Returns the components of the specified type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IEnumerable<T> ComponentsOfType<T>() where T : Component
            => Components.Values.OfType<T>();
    }

    static class RefNull<T>
    {
        public static T _null;
    }
}

//public ref T Get<T>()
//{
//    if (!components.TryGetValue(typeof(T), out var obj))
//        throw new InvalidOperationException($"No component of type {typeof(T)} is registered");
//    return ref ((Ref<T>)obj).Value;
//}

//public ref T Get<T>(out bool success)
//{
//    if (!components.TryGetValue(typeof(T), out var obj))
//    {
//        success = false;
//        return ref RefNull<T>._null;
//    }
//    success = true;
//    return ref ((Ref<T>)obj).Value;
//}
