using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Linq;
using StateMachine;
using UnityEngine;

namespace Player.NewStateMachine
{
    /// <summary>
    ///  Utilitário para invocar métodos estáticos assíncronos de construção a partir de XML.
    /// </summary>
    public static class StaticXmlAsyncCtor
    {
        /// Espera que o método tenha a assinatura:
        public static async Task<TOut> InvokeAsync<TOut>(
            Type t, XElement node, Transform parent, PlayerRoot player) where TOut : Component
        {
            var mi = t.GetMethod(
                "ConstructFromXmlAsync",
                BindingFlags.Public | BindingFlags.Static,
                null,
                new[] { typeof(XElement), typeof(Transform), typeof(PlayerRoot) },
                null);

            if (mi == null)
                throw new MissingMethodException(
                    $"{t.Name} precisa de: public static Task<{typeof(TOut).Name}> ConstructFromXmlAsync(XElement, Transform, PlayerRoot)");

            var taskObj = mi.Invoke(null, new object[] { node, parent, player }) as Task<TOut>;
            return await taskObj;
        }
    }
}