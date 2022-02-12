using System;
using System.Collections.Generic;
using Terraria;

namespace EEMod.Seamap.Core
{
    public class ComponentManager // implementation might change to use static arrays
    {
        public Dictionary<Type, object> components = new();
        // Ref<T> are used because struct components
        // prevents having to box/unbox (cast to object and back to T)
        // and allow to get a reference to the component

        /// <summary>
        /// The object these components belong to.
        /// </summary>
        public SeamapObject Parent { get; protected set; }

        /// <summary>
        /// All the current components registered.
        /// </summary>
        public IEnumerable<Type> Components => components.Keys;

        public ComponentManager(SeamapObject parent)
        {
            Parent = parent;
        }

        /// <summary>
        /// Sets or adds a component of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value of the component.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void Set<T>(T value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (typeof(T) != value.GetType())
                throw new ArgumentException("typeof(T) != value.GetType()");

            components[value.GetType()] = new Ref<T>(value);
        }

        /// <summary>
        /// Sets or adds a component of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="wrapper">The wrapper for the component</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void Set<T>(Ref<T> wrapper)
        {
            if (wrapper == null)
                throw new ArgumentNullException(nameof(wrapper));

            components[typeof(T)] = wrapper; 
        }

        /// <summary>
        /// Removes a component of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>If the component was succesfully removed</returns>
        public bool Remove<T>()
        {
            return components.Remove(typeof(T));
        }

        /// <summary>
        /// Attempts to get the <seealso cref="Ref{T}"/> wrapper of a component of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="component">The component wrapper.</param>
        /// <returns>If the component was successfully taken.</returns>
        public bool TryGet<T>(out Ref<T> component)
        {
            if(!components.TryGetValue(typeof(T), out object obj))
            {
                component = null;
                return false;
            }

            component = (Ref<T>)obj;
            return true;
        }

        /// <summary>
        /// Attempts to get the reference of a component of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="success">If the component was successfully taken.</param>
        /// <param name="throwIfNotFound">Throw if the component is not present.</param>
        /// <returns>
        /// The reference to the component. If the component is not found and <paramref name="throwIfNotFound"/> is false<br />
        /// returns <seealso cref="RefNull{T}._null"/>.
        /// </returns>
        /// <exception cref="InvalidOperationException"></exception>
        public ref T GetRef<T>(out bool success, bool throwIfNotFound = true)
        {
            if(!components.TryGetValue(typeof(T), out object obj))
            {
                if(throwIfNotFound)
                    throw new InvalidOperationException($"No component of type {typeof(T)} is registered");
                success = false;
                return ref RefNull<T>._null;
            }

            success = true;
            return ref ((Ref<T>)obj).Value;
        }

        /// <summary>
        /// Attempts to get the <see cref="Ref{T}"/> wrapper of a component of type <paramref name="componentType"/>
        /// </summary>
        /// <param name="componentType">The type of the component.</param>
        /// <param name="componentWrapper">The <see cref="Ref{T}"/> wrapper as <see cref="object"/></param>
        /// <returns>If the component exists and was succesfully taken.</returns>
        public bool TryGet(Type componentType, out object componentWrapper)
        {
            return components.TryGetValue(componentType, out componentWrapper);
        }

        /// <summary>
        /// If there's a component of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool Has<T>()
        {
            return components.ContainsKey(typeof(T));
        }

        /// <summary>
        /// If there's a component of type <paramref name="componentType"/>.
        /// </summary>
        /// <param name="componentType">The type of the component.</param>
        /// <returns></returns>
        public bool Has(Type componentType)
        {
            return components.ContainsKey(componentType);
        }
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

