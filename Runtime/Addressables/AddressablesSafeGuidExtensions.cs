// SPDX-License-Identifier: MPL-2.0
/*
 * Copyright (c) 2024-2026 Saltbox Games Cooperative
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

#if ADDRESSABLES_2
using System;
using System.Collections.Generic;
using SaltboxGames.Core.Shims;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace SaltboxGames.Unity
{
    /// <summary>
    /// Addressables resource locator that treats <see cref="SafeGuid"/> keys as string addresses.
    /// </summary>
    public sealed class SafeGuidPassthroughLocator : IResourceLocator
    {
        /// <inheritdoc />
        public string LocatorId => "SafeGuidPassthrough";

        /// <inheritdoc />
        public IEnumerable<object> Keys { get { yield break; } }

        /// <inheritdoc />
        public IEnumerable<IResourceLocation> AllLocations => Array.Empty<IResourceLocation>();

        /// <inheritdoc />
        public bool Locate(object key, Type type, out IList<IResourceLocation> locations)
        {
            locations = null;
            if (key is not SafeGuid guid)
            {
                return false;
            }

            string addr = guid.ToString(); // must exactly match your address format
            foreach (IResourceLocator loc in UnityEngine.AddressableAssets.Addressables.ResourceLocators)
            {
                if (ReferenceEquals(loc, this))
                {
                    continue;
                }

                if (loc.Locate(addr, type, out locations) && locations != null && locations.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
#endif
