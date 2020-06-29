using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using EEMod.Extensions;

namespace EEMod.Autoloading
{
    public static class AutoloadingManager
    {
        public const BindingFlags FLAGS_ANY = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
        public const BindingFlags FLAGS_STATIC = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
        public const BindingFlags FLAGS_INSTANCE = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        internal static void LoadManager(EEMod a)
        {
            DoMagik<FieldInitAttribute>(a, out var memebers); // initialize fields
            DoMagik<FieldAutoInitUnloadAttribute>(memebers, true);
            DoMagik<LoadingMethodAttribute>(memebers); // call loading methods
        }

        internal static void UnloadManager(EEMod a)
        {
            DoMagik<UnloadingMethodAttribute>(a, out var members); // call unloading methods
            DoMagik<FieldAutoInitUnloadAttribute>(members, false);
            DoMagik<FieldUnloadAttribute>(members); // then initialize fields
        }

        private static IEnumerable<MemberInfo> MembersAll(Type type) => type.GetMembers(FLAGS_ANY);
        public static void DoMagik<T>(Mod formod, out IEnumerable<MemberInfo> members, object o = null) where T : Attribute, IMemberHandler => 
            DoMagik<T>(formod.Code ?? formod.GetType().Assembly, out members, o); // vv

        public static void DoMagik<T>(Assembly forAssembly, out IEnumerable<MemberInfo> members, object o = null) where T : Attribute, IMemberHandler => 
            DoMagik<T>(forAssembly.GetTypesSafe(), out members, o); // vv

        public static void DoMagik<T>(IEnumerable<Type> fromtypes, out IEnumerable<MemberInfo> members, object o = null) where T : Attribute, IMemberHandler => 
            DoMagik<T>(members = fromtypes.SelectMany(MembersAll), o); // --

        //public static void DoMagik(Mod forMod, params Type[] HandlerAttributesTypes) => DoMagik(forMod.Code ?? forMod.GetType().Assembly, HandlerAttributesTypes); // vv
        //public static void DoMagik(Assembly assembly, params Type[] HandlerAttributesTypes) => DoMagik(assembly.GetTypesSafe(), HandlerAttributesTypes); // vv
        //public static void DoMagik(IEnumerable<Type> types, params Type[] HandlerAttributesTypes) => DoMagik(types.SelectMany(MembersAll), HandlerAttributesTypes); // --
        
        //public static void DoMagik(IEnumerable<MemberInfo> members, IEnumerable<Type> handlerAttributesTypes)
        //{
        //    foreach (var member in members)
        //        DoMagik(member, handlerAttributesTypes);
        //}
        //public static void DoMagik(MemberInfo member, IEnumerable<Type> handlerAttributesTypes)
        //{
        //    foreach (var type in handlerAttributesTypes)
        //        DoMagik(member, type);
        //}
        //public static void DoMagik(MemberInfo member, Type handlerAttributeType)
        //{
        //    if (!handlerAttributeType.IsSubclassOf(typeof(Attribute)))
        //        throw new ArgumentException($"The type {handlerAttributeType.Name} does not inherit from {nameof(Attribute)}");
        //    if (!handlerAttributeType.ImplementsInterface<IMemberHandler>())
        //        throw new ArgumentException($"The type {handlerAttributeType.Name} does not implement the interface {nameof(IMemberHandler)}");

        //    DoMagik(member, (IMemberHandler)member.GetCustomAttribute(handlerAttributeType));
        //}

        public static void DoMagik<T>(IEnumerable<MemberInfo> members, object o = null) where T : Attribute, IMemberHandler
        {
            foreach (var member in members)
                DoMagik<T>(member, o);
        }

        //public static void DoMagik(MemberInfo member, IEnumerable<IMemberHandler> handlers)
        //{
        //    foreach (var handler in handlers)
        //        DoMagik(member, handler);
        //}

        public static void DoMagik<T>(MemberInfo member, object o = null) where T : Attribute, IMemberHandler
        {
            if (member.TryGetCustomAttribute(out T attribute))
                DoMagik(member, attribute, o);
        }

        public static void DoMagik(MemberInfo member, IMemberHandler handler, object o = null)
        {
            var targetMembers = handler.HandlingMembers;

            if(targetMembers == MemberTypes.All || targetMembers.HasFlag(member.MemberType)) // if it can handle what it specifies
                if (handler.IsValid(member)) // if it allows it
                    handler.HandleMember(member, o); // handle it
        }
    }
}
/*
        public const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
        public const BindingFlags flags0 = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

        internal static void LoadManager(EEMod mod)
        {
            Assembly assembly = mod.Code ?? mod.GetType().Assembly;
            Type[] types = assembly.GetTypesSafe();
            IEnumerable<MemberInfo> members = types.SelectMany(i => i.GetMembers(flags));

            Loading(members, mod);
        }
        private static void Loading(IEnumerable<MemberInfo> members, EEMod mod)
        {
            foreach (MemberInfo member in members)
            {
                // nested types also appear from Assembly.GetTypes()
                if (member is MethodInfo m)
                    LoadInvoke(m, mod);
            }
        }
        private static void LoadInvoke(MethodInfo m, EEMod mod)
        {
            if (!ValidMethod(m, out LoadingAttribute attribute)) // if it has the attribute
                return;
            if (!ValidInCurrent(attribute.Loadmode, Main.dedServ))
                return;

            ParameterInfo[] parameters = m.GetParameters();
            Type modtype = mod.GetType();
            if (parameters is null || parameters.Length == 0)
            {
                m.Invoke(null, null);
            }
            else if (parameters.Length == 1 && parameters[0].ParameterType.IsAssignableFrom(modtype))
            {
                m.Invoke(null, new object[] { mod });
            }
        }
        public static bool ValidMethod<T>(MethodInfo m, out T attribute) where T : Attribute
        {
            attribute = null;
            return m.IsStatic && !(m.IsAbstract || m.IsGenericMethod || m.IsGenericMethodDefinition || m.GetMethodBody()?.GetILAsByteArray() is null) && m.TryGetCustomAttribute(out attribute);
        }
        private static bool ValidInCurrent(LoadingMode mode, bool server) => mode == LoadingMode.Both || mode == LoadingMode.Server == server;



        internal static void UnloadManager(EEMod mod)
        {
            Assembly assembly = mod.Code ?? mod.GetType().Assembly;
            Type[] types = assembly.GetTypesSafe();
            IEnumerable<MemberInfo> members = types.SelectMany(i => i.GetMembers(flags));

            Unloading(members, mod);
        }
        private static void Unloading(IEnumerable<MemberInfo> members, EEMod mod)
        {
            foreach (MemberInfo member in members)
            {
                if (member is MethodInfo m)
                    UnloadInvoke(m, mod);
                else if (member is FieldInfo field)
                    UnloadField(field);
            }
        }
        private static void UnloadInvoke(MethodInfo method, EEMod mod)
        {
            if (!ValidMethod(method, out UnloadingAttribute attribute)) // if it has the attribute
                return;
            if (!ValidInCurrent(attribute.Loadmode, Main.dedServ))
                return;

            ParameterInfo[] parameters = method.GetParameters();
            Type modtype = mod.GetType();
            if (parameters is null || parameters.Length == 0)
            {
                method.Invoke(null, null);
            }
            else if (parameters.Length == 1 && parameters[0].ParameterType.IsAssignableFrom(modtype))
            {
                method.Invoke(null, new object[] { mod });
            }
        }
        private static void UnloadField(FieldInfo field)
        {
            Type fieldtype = field.FieldType;
            if (!field.TryGetCustomAttribute(out UnloadingAttribute _))
                return;
            if (!field.IsStatic || field.IsInitOnly || field.IsLiteral || fieldtype.IsValueType || Nullable.GetUnderlyingType(fieldtype) != null)
                return;
            field.SetValue(null, null);
        }
*/
