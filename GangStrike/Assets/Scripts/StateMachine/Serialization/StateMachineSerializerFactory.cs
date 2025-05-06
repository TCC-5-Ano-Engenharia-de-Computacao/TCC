using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using StateMachine.Actions;
using StateMachine.Attributes;
using StateMachine.Conditions;
using StateMachine.Model;


// ------------------------------------------------------------------------------------------------
//  SERIALIZAÇÃO dinâmica – aplica OCP usando Reflection + XmlAttributeOverrides
// ------------------------------------------------------------------------------------------------

namespace StateMachine.Serialization
{
    public static class StateMachineSerializerFactory
    {
        private static XmlSerializer _cached;

        public static XmlSerializer Get()
        {
            if (_cached != null) return _cached;

            // Descobre as subclasses concretas
            var actionTypes    = DiscoverSubtypes<ActionBase>();
            var conditionTypes = DiscoverSubtypes<ConditionBase>();

            // Configura overrides
            var ovs = new XmlAttributeOverrides();
            Register(ovs, typeof(StateModel),      nameof(StateModel.BeforeEnter), actionTypes);
            Register(ovs, typeof(StateModel),      nameof(StateModel.OnEnter),     actionTypes);
            Register(ovs, typeof(StateModel),      nameof(StateModel.OnStay),      actionTypes);
            Register(ovs, typeof(StateModel),      nameof(StateModel.OnLeave),     actionTypes);
            Register(ovs, typeof(TransitionModel), nameof(TransitionModel.Conditions), conditionTypes);

            _cached = new XmlSerializer(typeof(Model.StateMachineModel), ovs);
            return _cached;
        }

        private static IEnumerable<(Type Type, string Tag)> DiscoverSubtypes<TBase>()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => !t.IsAbstract && typeof(TBase).IsAssignableFrom(t))
                .Select(t => (t, t.GetCustomAttribute<XmlTagAttribute>()?.ElementName ?? t.Name));
        }

        private static void Register(XmlAttributeOverrides ovs, Type owner, string propName,
            IEnumerable<(Type Type, string Tag)> types)
        {
            var attrs = new XmlAttributes();
            foreach (var (type, tag) in types)
                attrs.XmlArrayItems.Add(new XmlArrayItemAttribute(tag) { Type = type });
            ovs.Add(owner, propName, attrs);
        }
    }
}