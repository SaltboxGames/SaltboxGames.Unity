/*
 * Copyright (c) 2024 SaltboxGames, Jonathan Gardner
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */


using System;
using UnityEngine;

namespace SaltboxGames.Unity
{
    [Serializable]
    public abstract class TypeReferenceBase : ISerializationCallbackReceiver
    {
        [SerializeField, HideInInspector]
        protected string _typeName;

        protected Type type;

        public Type ReferenceType => type;

        public virtual void OnBeforeSerialize()
        {
            _typeName = type != null ? type.AssemblyQualifiedName : "";
        }

        public virtual void OnAfterDeserialize()
        {
            type = !string.IsNullOrEmpty(_typeName) ? Type.GetType(_typeName) : null;
        }
    }
    
    [Serializable]
    public class TypeReference<T> : TypeReferenceBase
    {
        public TypeReference() { }

        public TypeReference(Type t)
        {
            if (t != null && !typeof(T).IsAssignableFrom(t))
                throw new InvalidOperationException($"Type {t} is not assignable to {typeof(T)}");
            type = t;
            _typeName = t?.AssemblyQualifiedName;
        }

        public override void OnBeforeSerialize()
        {
            if (type != null && !typeof(T).IsAssignableFrom(type))
                throw new InvalidOperationException($"Type {type} is not assignable to {typeof(T)}");
            base.OnBeforeSerialize();
        }
    }
}
