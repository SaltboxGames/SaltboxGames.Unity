// SPDX-License-Identifier: MPL-2.0
/*
 * Copyright (c) 2024-2026 Saltbox Games Cooperative
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

using System;
using UnityEngine;

namespace SaltboxGames.Unity
{
    /// <summary>
    /// Stores a serialized assembly-qualified type name and restores it to a <see cref="Type"/> after deserialization.
    /// </summary>
    [Serializable]
    public abstract class TypeReferenceBase : ISerializationCallbackReceiver
    {
        /// <summary>
        /// Serialized assembly-qualified type name.
        /// </summary>
        [SerializeField, HideInInspector]
        protected string _typeName;

        /// <summary>
        /// Resolved runtime type.
        /// </summary>
        protected Type type;

        /// <summary>
        /// Gets the resolved runtime type.
        /// </summary>
        public Type ReferenceType => type;

        /// <inheritdoc />
        public virtual void OnBeforeSerialize()
        {
            _typeName = type != null ? type.AssemblyQualifiedName : "";
        }

        /// <inheritdoc />
        public virtual void OnAfterDeserialize()
        {
            type = !string.IsNullOrEmpty(_typeName) ? Type.GetType(_typeName) : null;
        }
    }

    /// <summary>
    /// Stores a serializable reference to a concrete type assignable to <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The base type or interface the referenced type must be assignable to.</typeparam>
    [Serializable]
    public class TypeReference<T> : TypeReferenceBase
    {
        /// <summary>
        /// Creates an empty type reference.
        /// </summary>
        public TypeReference() { }

        /// <summary>
        /// Creates a type reference for the provided type.
        /// </summary>
        /// <param name="t">The type to reference.</param>
        /// <exception cref="InvalidOperationException">Thrown when <paramref name="t"/> is not assignable to <typeparamref name="T"/>.</exception>
        public TypeReference(Type t)
        {
            if (t != null && !typeof(T).IsAssignableFrom(t))
                throw new InvalidOperationException($"Type {t} is not assignable to {typeof(T)}");
            type = t;
            _typeName = t?.AssemblyQualifiedName;
        }

        /// <inheritdoc />
        public override void OnBeforeSerialize()
        {
            if (type != null && !typeof(T).IsAssignableFrom(type))
                throw new InvalidOperationException($"Type {type} is not assignable to {typeof(T)}");
            base.OnBeforeSerialize();
        }
    }
}
