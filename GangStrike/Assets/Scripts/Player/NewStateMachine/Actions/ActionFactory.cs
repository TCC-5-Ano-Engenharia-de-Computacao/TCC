// ActionFactory.cs

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using StateMachine;
using UnityEngine;

namespace Player.NewStateMachine.Actions
{
    public static class ActionFactory
    {
        private static readonly Dictionary<string, Func<XElement, Transform, PlayerRoot, Task<ActionBase>>> _map
            = new(StringComparer.Ordinal);

        public static void Register(string tag, Func<XElement, Transform, PlayerRoot, Task<ActionBase>> ctor)
            => _map[tag] = ctor;

        public static async Task<ActionBase> CreateAsync(XElement node, Transform parent, PlayerRoot player)
        {
            var tag = node.Name.LocalName;
            if (_map.TryGetValue(tag, out var ctor)) return await ctor(node, parent, player);
            Debug.LogError($"[ActionFactory] Tag '{tag}' não registrada.");
            return null;
        }
    }
}