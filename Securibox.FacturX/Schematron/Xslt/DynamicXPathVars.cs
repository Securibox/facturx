namespace Securibox.FacturX.Schematron.Xslt
{
    using System.Collections.Concurrent;
    using System.Dynamic;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Xml;

    public class DynamicXPathVariables : DynamicObject
    {
        private static readonly ConcurrentDictionary<string, Type> _typeCache = new();
        private static readonly ConcurrentDictionary<
            Type,
            Dictionary<string, PropertyInfo>
        > _propertyCache = new();

        public static object BuildDynamicProps(Dictionary<XmlQualifiedName, object> lets)
        {
            ArgumentNullException.ThrowIfNull(lets, nameof(lets));
            if (lets.Count == 0)
            {
                return new object();
            }

            var typeKey = GenerateTypeKey(lets);
            var dynamicType = _typeCache.GetOrAdd(typeKey, _ => CreateType(lets));
            var properties = _propertyCache.GetOrAdd(
                dynamicType,
                type =>
                    lets.Keys.Select(k => new
                        {
                            Name = k.ToString(),
                            Prop = type.GetProperty(k.ToString()),
                        })
                        .Where(x => x.Prop is not null)
                        .ToDictionary(x => x.Name, x => x.Prop!)
            );

            var instance = Activator.CreateInstance(dynamicType);
            foreach (var let in lets)
            {
                var propertyName = let.Key.ToString();
                if (properties.TryGetValue(propertyName, out var prop) && prop?.CanWrite == true)
                {
                    try
                    {
                        prop.SetValue(instance, let.Value);
                    }
                    catch (ArgumentException ex)
                    {
                        throw new InvalidOperationException(
                            $"Failed to set property {propertyName}: {ex.Message}",
                            ex
                        );
                    }
                }
            }

            return instance;
        }

        private static Type CreateType(Dictionary<XmlQualifiedName, object> lets)
        {
            var typeBuilder = CreateTypeBuilder($"DynamicXPathVars_{Guid.NewGuid():N}");
            foreach (var let in lets)
            {
                var propertyName = let.Key.ToString();
                var propertyType = let.Value?.GetType() ?? typeof(object);
                CreateAutoProperty(typeBuilder, propertyName, propertyType);
            }

            return typeBuilder.CreateType();
        }

        private static string GenerateTypeKey(Dictionary<XmlQualifiedName, object> lets)
        {
            var sortedProps = lets.OrderBy(kv => kv.Key.ToString())
                .Select(kv => $"{kv.Key}:{(kv.Value?.GetType().FullName ?? "object")}");

            return string.Join('|', sortedProps);
        }

        private static TypeBuilder CreateTypeBuilder(string typeName)
        {
            var assemblyName = new AssemblyName("DynamicXPathAssembly");
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(
                assemblyName,
                AssemblyBuilderAccess.Run
            );
            var moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");
            return moduleBuilder.DefineType(
                typeName,
                TypeAttributes.Sealed | TypeAttributes.Public
            );
        }

        private static void CreateAutoProperty(
            TypeBuilder typeBuilder,
            string propertyName,
            Type propertyType
        )
        {
            var fieldBuilder = typeBuilder.DefineField(
                "_" + propertyName,
                propertyType,
                FieldAttributes.Private
            );
            var propertyBuilder = typeBuilder.DefineProperty(
                propertyName,
                PropertyAttributes.HasDefault,
                propertyType,
                null
            );
            var getMethod = typeBuilder.DefineMethod(
                "get_" + propertyName,
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                propertyType,
                Type.EmptyTypes
            );

            var getIL = getMethod.GetILGenerator();
            getIL.Emit(OpCodes.Ldarg_0);
            getIL.Emit(OpCodes.Ldfld, fieldBuilder);
            getIL.Emit(OpCodes.Ret);

            var setMethodBuilder = typeBuilder.DefineMethod(
                "set_" + propertyName,
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                null,
                [propertyType]
            );

            var setIL = setMethodBuilder.GetILGenerator();
            setIL.Emit(OpCodes.Ldarg_0);
            setIL.Emit(OpCodes.Ldarg_1);
            setIL.Emit(OpCodes.Stfld, fieldBuilder);
            setIL.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(getMethod);
            propertyBuilder.SetSetMethod(setMethodBuilder);
        }
    }
}
