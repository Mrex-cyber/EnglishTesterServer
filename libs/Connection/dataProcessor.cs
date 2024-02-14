using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Connection
{
    public class DataProvider
    {
        /// <summary>
        /// Преобразовать таблицу в стписок 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<T> ConvertTableToList<T>(DataTable dataTable)
        {
            if (!typeof(T).IsClass)
            {
                return DataTableConverters.ConvertTableToListStruct<T>(dataTable);
            }

            var isScalar = typeof(IScalarTypeMapper).IsAssignableFrom(typeof(T));

            var list = new List<T>();
            var names = dataTable.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToArray();
            var props = dataTable.Columns.Cast<DataColumn>().Select((c, i) => typeof(T).GetProperty(isScalar ? string.Concat("Value", i) : c.ColumnName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)).ToArray();
            var types = props.Select(p => p == null ? null : p.PropertyType.IsGenericType ? p.PropertyType.GetGenericArguments()[0].Name + "?" : p.PropertyType.Name).ToArray();
            var delegs = props.Select(p => p == null ? null : CreateDelegate<T>(p)).ToArray();

            foreach (DataRow dr in dataTable.Rows)
            {
                var t = (T)Activator.CreateInstance(typeof(T));

                for (var i = 0; i < names.Length; i++)
                {
                    if (props[i] == null) continue;

                    switch (types[i])
                    {
                        case "String":
                            ((SetValue<T, string>)delegs[i])(t, dr[i] is DBNull ? null : dr[i].ToString());
                            break;
                        case "Guid":
                            ((SetValue<T, Guid>)delegs[i])(t, (Guid)dr[i]);
                            break;

                        case "Boolean":
                            ((SetValue<T, bool>)delegs[i])(t, Convert.ToBoolean(dr[i]));
                            break;
                        case "Byte":
                            ((SetValue<T, byte>)delegs[i])(t, byte.Parse(dr[i].ToString()));
                            break;
                        case "Int16":
                            ((SetValue<T, short>)delegs[i])(t, (short)dr[i]);
                            break;
                        case "Int32":
                            ((SetValue<T, int>)delegs[i])(t, dr.Table.Columns[i].DataType != null ? (int)dr[i] : -1);
                            break;
                        case "Int64":
							//((SetValue<T, long>)delegs[i])(t, dr.Table.Columns[i].DataType == typeof(int) ? (int)dr[i] : (long)dr[i]);
							((SetValue<T, long>)delegs[i])(t, dr[i] is DBNull ? -1 : (dr.Table.Columns[i].DataType == typeof(int) ? (dr[i] is DBNull ? -1 : (int)dr[i]) : (dr[i] is DBNull ? -1 : (long)dr[i])));
							break;
                        case "UInt16":
                            ((SetValue<T, ushort>)delegs[i])(t, (ushort)dr[i]);
                            break;
                        case "UInt32":
                            ((SetValue<T, uint>)delegs[i])(t, (uint)dr[i]);
                            break;
                        case "UInt64":
                            ((SetValue<T, ulong>)delegs[i])(t, (ulong)dr[i]);
                            break;
                        case "DateTime":
                            if (dr[i] == DBNull.Value) {
								((SetValue<T, DateTime>)delegs[i])(t, DateTime.MinValue);
							}
                            else
                            {
                                ((SetValue<T, DateTime>)delegs[i])(t, (DateTime)dr[i]);
                            }
                            break;
                        case "TimeSpan":
                            ((SetValue<T, TimeSpan>)delegs[i])(t, (TimeSpan)dr[i]);
                            break;
                        case "Double":
                            ((SetValue<T, double>)delegs[i])(t, (double)dr[i]);
                            break;
                        case "Single":
                            ((SetValue<T, float>)delegs[i])(t, (float)dr[i]);
                            break;
                        case "Decimal":
                            ((SetValue<T, decimal>)delegs[i])(t, (decimal)dr[i]);
                            break;

                        case "Boolean?":
                            ((SetValue<T, bool?>)delegs[i])(t, dr[i] as bool?);
                            break;
                        case "Byte?":
                            ((SetValue<T, byte?>)delegs[i])(t, dr[i] as byte?);
                            break;
                        case "Int16?":
                            ((SetValue<T, short?>)delegs[i])(t, dr[i] as short?);
                            break;
                        case "Int32?":
                            ((SetValue<T, int?>)delegs[i])(t, dr[i] as int?);
                            break;
                        case "Int64?":
                            ((SetValue<T, long?>)delegs[i])(t, dr[i] as long?);
                            break;
                        case "UInt16?":
                            ((SetValue<T, ushort?>)delegs[i])(t, dr[i] as ushort?);
                            break;
                        case "UInt32?":
                            ((SetValue<T, uint?>)delegs[i])(t, dr[i] as uint?);
                            break;
                        case "UInt64?":
                            ((SetValue<T, ulong?>)delegs[i])(t, dr[i] as ulong?);
                            break;
                        case "DateTime?":
                            ((SetValue<T, DateTime?>)delegs[i])(t, dr[i] as DateTime?);
                            break;
                        case "TimeSpan?":
                            ((SetValue<T, TimeSpan?>)delegs[i])(t, dr[i] as TimeSpan?);
                            break;
                        case "Double?":
                            ((SetValue<T, double?>)delegs[i])(t, dr[i] as double?);
                            break;
                        case "Single?":
                            ((SetValue<T, float?>)delegs[i])(t, dr[i] as float?);
                            break;
                        case "Decimal?":
                            ((SetValue<T, decimal?>)delegs[i])(t, dr[i] as decimal?);
                            break;

                        case "Guid?":
                            ((SetValue<T, Guid?>)delegs[i])(t, dr[i] as Guid?);
                            break;
                    }
                }
                list.Add(t);
            }
            return list;
        }

        /// <summary>
        /// Преобразовать строку таблицы в класс
        /// </summary>
        /// <typeparam name="T">Класс в который нужно преобразовать строку</typeparam>
        /// <param name="dr">Строка таблицы</param>
        /// <returns></returns>
        public static T ConvertRowToObject<T>(DataRow dr) where T : new()
        {
            var i = 0;
            var props = new List<PropertyInfo>();
	
			try
            {

                if (!typeof(T).IsClass)
                {
                    return new T();
                }

                var isScalar = typeof(IScalarTypeMapper).IsAssignableFrom(typeof(T));

                var list = new List<T>();
                var names = dr.Table.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToArray();
                props = dr.Table.Columns.Cast<DataColumn>().Select((c, i) => typeof(T).GetProperty(isScalar ? string.Concat("Value", i) : c.ColumnName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)).ToList();
                var types = props.Select(p => p == null ? null : p.PropertyType.IsGenericType ? p.PropertyType.GetGenericArguments()[0].Name + "?" : p.PropertyType.Name).ToArray();
                var delegs = props.Select(p =>  p == null || p.SetMethod==null ? null : CreateDelegate<T>(p)).ToArray();


                var t = (T)Activator.CreateInstance(typeof(T));

                for (i=0; i < names.Length; i++)
                {
                    if (props[i] == null || props[i].SetMethod==null) continue;

                    switch (types[i])
                    {
                        case "String":
                            ((SetValue<T, string>)delegs[i])(t, dr[i] is DBNull ? "" : dr[i].ToString());
                            break;
                        case "Guid":
                            ((SetValue<T, Guid>)delegs[i])(t, dr[i] is DBNull ? Guid.Empty : (Guid)dr[i]);
                            break;

                        case "Boolean":
                            ((SetValue<T, bool>)delegs[i])(t, Convert.ToBoolean(dr[i]));
                            break;
                        case "Byte":
                            ((SetValue<T, byte>)delegs[i])(t, byte.Parse(dr[i].ToString()));
                            break;
                        case "Int16":
                            ((SetValue<T, short>)delegs[i])(t, (short)dr[i]);
                            break;
                        case "Int32":
                            ((SetValue<T, int>)delegs[i])(t, dr.Table.Columns[i].DataType != null ? (int)dr[i] : -1);
                            break;
                        case "Int64":
                            ((SetValue<T, long>)delegs[i])(t, dr[i] is DBNull ? -1 : (dr.Table.Columns[i].DataType == typeof(int) ? (dr[i] is DBNull ? -1 : (int)dr[i]) :(dr[i] is DBNull ? -1: (long)dr[i])));
                            break;
                        case "UInt16":
                            ((SetValue<T, ushort>)delegs[i])(t, (ushort)dr[i]);
                            break;
                        case "UInt32":
                            ((SetValue<T, uint>)delegs[i])(t, (uint)dr[i]);
                            break;
                        case "UInt64":
                            ((SetValue<T, ulong>)delegs[i])(t, (ulong)dr[i]);
                            break;
                        case "DateTime":

                            if (dr[i] != DBNull.Value)
                            {
                                ((SetValue<T, DateTime>)delegs[i])(t, (DateTime)dr[i]);
                            }
                            break;
                        case "TimeSpan":
                            ((SetValue<T, TimeSpan>)delegs[i])(t, (TimeSpan)dr[i]);
                            break;
                        case "Double":
                            ((SetValue<T, double>)delegs[i])(t, (double)dr[i]);
                            break;
                        case "Single":
                            ((SetValue<T, float>)delegs[i])(t, (float)dr[i]);
                            break;
                        case "Decimal":
                            ((SetValue<T, decimal>)delegs[i])(t, (decimal)dr[i]);
                            break;

                        case "Boolean?":
                            ((SetValue<T, bool?>)delegs[i])(t, dr[i] as bool?);
                            break;
                        case "Byte?":
                            ((SetValue<T, byte?>)delegs[i])(t, dr[i] as byte?);
                            break;
                        case "Int16?":
                            ((SetValue<T, short?>)delegs[i])(t, dr[i] as short?);
                            break;
                        case "Int32?":
                            ((SetValue<T, int?>)delegs[i])(t, dr[i] as int?);
                            break;
                        case "Int64?":
                            ((SetValue<T, long?>)delegs[i])(t, dr[i] as long?);
                            break;
                        case "UInt16?":
                            ((SetValue<T, ushort?>)delegs[i])(t, dr[i] as ushort?);
                            break;
                        case "UInt32?":
                            ((SetValue<T, uint?>)delegs[i])(t, dr[i] as uint?);
                            break;
                        case "UInt64?":
                            ((SetValue<T, ulong?>)delegs[i])(t, dr[i] as ulong?);
                            break;
                        case "DateTime?":
                            ((SetValue<T, DateTime?>)delegs[i])(t, dr[i] as DateTime?);
                            break;
                        case "TimeSpan?":
                            ((SetValue<T, TimeSpan?>)delegs[i])(t, dr[i] as TimeSpan?);
                            break;
                        case "Double?":
                            ((SetValue<T, double?>)delegs[i])(t, dr[i] as double?);
                            break;
                        case "Single?":
                            ((SetValue<T, float?>)delegs[i])(t, dr[i] as float?);
                            break;
                        case "Decimal?":
                            ((SetValue<T, decimal?>)delegs[i])(t, dr[i] as decimal?);
                            break;

                        case "Guid?":
                            ((SetValue<T, Guid?>)delegs[i])(t, dr[i] is DBNull ? Guid.Empty: dr[i] as Guid?);
                            break;
                    }
                }
                return t;
            }
            catch (Exception error) {
                throw new Exception($"{@error.Message }, parameter {props[i]}");
            }
        }

        private static Delegate CreateDelegate<T>(PropertyInfo pi)
        {
            if (pi.PropertyType == typeof(string))
                return Delegate.CreateDelegate(typeof(SetValue<T, string>), pi.GetSetMethod());

            if (pi.PropertyType == typeof(Guid))
                return Delegate.CreateDelegate(typeof(SetValue<T, Guid>), pi.GetSetMethod());

            if (pi.PropertyType == typeof(DateTime))
                return Delegate.CreateDelegate(typeof(SetValue<T, DateTime>), pi.GetSetMethod());

            if (pi.PropertyType == typeof(TimeSpan))
                return Delegate.CreateDelegate(typeof(SetValue<T, TimeSpan>), pi.GetSetMethod());

            if (pi.PropertyType == typeof(short))
                return Delegate.CreateDelegate(typeof(SetValue<T, short>), pi.GetSetMethod());
            if (pi.PropertyType == typeof(int))
                return Delegate.CreateDelegate(typeof(SetValue<T, int>), pi.GetSetMethod());
            if (pi.PropertyType == typeof(long))
                return Delegate.CreateDelegate(typeof(SetValue<T, long>), pi.GetSetMethod());

            if (pi.PropertyType == typeof(ushort))
                return Delegate.CreateDelegate(typeof(SetValue<T, ushort>), pi.GetSetMethod());
            if (pi.PropertyType == typeof(uint))
                return Delegate.CreateDelegate(typeof(SetValue<T, uint>), pi.GetSetMethod());
            if (pi.PropertyType == typeof(ulong))
                return Delegate.CreateDelegate(typeof(SetValue<T, ulong>), pi.GetSetMethod());

            if (pi.PropertyType == typeof(float))
                return Delegate.CreateDelegate(typeof(SetValue<T, float>), pi.GetSetMethod());
            if (pi.PropertyType == typeof(double))
                return Delegate.CreateDelegate(typeof(SetValue<T, double>), pi.GetSetMethod());
            if (pi.PropertyType == typeof(decimal))
                return Delegate.CreateDelegate(typeof(SetValue<T, decimal>), pi.GetSetMethod());

            if (pi.PropertyType == typeof(byte))
                return Delegate.CreateDelegate(typeof(SetValue<T, byte>), pi.GetSetMethod());

            if (pi.PropertyType == typeof(bool))
                return Delegate.CreateDelegate(typeof(SetValue<T, bool>), pi.GetSetMethod());


            if (pi.PropertyType == typeof(DateTime?))
                return Delegate.CreateDelegate(typeof(SetValue<T, DateTime?>), pi.GetSetMethod());
            if (pi.PropertyType == typeof(TimeSpan?))
                return Delegate.CreateDelegate(typeof(SetValue<T, TimeSpan?>), pi.GetSetMethod());

            if (pi.PropertyType == typeof(short?))
                return Delegate.CreateDelegate(typeof(SetValue<T, short?>), pi.GetSetMethod());
            if (pi.PropertyType == typeof(int?))
                return Delegate.CreateDelegate(typeof(SetValue<T, int?>), pi.GetSetMethod());
            if (pi.PropertyType == typeof(long?))
                return Delegate.CreateDelegate(typeof(SetValue<T, long?>), pi.GetSetMethod());

            if (pi.PropertyType == typeof(ushort?))
                return Delegate.CreateDelegate(typeof(SetValue<T, ushort?>), pi.GetSetMethod());
            if (pi.PropertyType == typeof(uint?))
                return Delegate.CreateDelegate(typeof(SetValue<T, uint?>), pi.GetSetMethod());
            if (pi.PropertyType == typeof(ulong?))
                return Delegate.CreateDelegate(typeof(SetValue<T, ulong?>), pi.GetSetMethod());

            if (pi.PropertyType == typeof(float?))
                return Delegate.CreateDelegate(typeof(SetValue<T, float?>), pi.GetSetMethod());
            if (pi.PropertyType == typeof(double?))
                return Delegate.CreateDelegate(typeof(SetValue<T, double?>), pi.GetSetMethod());
            if (pi.PropertyType == typeof(decimal?))
                return Delegate.CreateDelegate(typeof(SetValue<T, decimal?>), pi.GetSetMethod());


            if (pi.PropertyType == typeof(byte?))
                return Delegate.CreateDelegate(typeof(SetValue<T, byte?>), pi.GetSetMethod());

            if (pi.PropertyType == typeof(bool?))
                return Delegate.CreateDelegate(typeof(SetValue<T, bool?>), pi.GetSetMethod());

            //added nullable guid type
            if (pi.PropertyType == typeof(Guid?))
                return Delegate.CreateDelegate(typeof(SetValue<T, Guid?>), pi.GetSetMethod());


            throw new Exception("UNSUPPORTED TYPE!!!");
        }

        private delegate void SetValue<in TObject, in TValue>(TObject obj, TValue value);
    }
    public class DataTableConverters
    {
        private const BindingFlags Flags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;

        private delegate object CreateObject();

        private static readonly ConcurrentDictionary<Type, EntryField[]> TypeFields = new ConcurrentDictionary<Type, EntryField[]>();
        private static readonly ConcurrentDictionary<Type, CreateObject> Instances = new ConcurrentDictionary<Type, CreateObject>();

        private static EntryField[] GetFields<T>()
        {
            return TypeFields.GetOrAdd(typeof(T), _ =>
                new Lazy<EntryField[]>(GetFieldsFromType<T>
                ).Value
            );
        }

        private static CreateObject GetInstance<T>()
        {
            return Instances.GetOrAdd(typeof(T), _ =>
                new Lazy<CreateObject>(CreateInstance<T>
                ).Value
            );
        }

        private static CreateObject CreateInstance<T>()
        {
            var type = typeof(T);
            var method = new DynamicMethod("_", typeof(object), null);
            var ilGen = method.GetILGenerator();
            var lv = ilGen.DeclareLocal(type);
            ilGen.Emit(OpCodes.Ldloca_S, lv);
            ilGen.Emit(OpCodes.Initobj, type);
            ilGen.Emit(OpCodes.Ldloc_0);
            ilGen.Emit(OpCodes.Box, type);
            ilGen.Emit(OpCodes.Ret);
            return (CreateObject)method.CreateDelegate(typeof(CreateObject));
        }

        private static EntryField[] GetFieldsFromType<T>()
        {
            var type = typeof(T);
            var properties = type.GetProperties(Flags);
            var fields = type.GetFields(Flags);
            var list = new EntryField[fields.Length + properties.Length];
            int i;
            for (i = 0; i < properties.Length; i++)
            {
                list[i] = new EntryField(properties[i]);
            }

            for (i = properties.Length; i < properties.Length + fields.Length; i++)
            {
                list[i] = new EntryField(fields[i - properties.Length]);
            }

            return list;
        }

        public static List<T> ConvertTableToListStruct<T>(DataTable table)
        {
            var list = new List<T>();
            var columns = table.Columns.Cast<DataColumn>().Select(c => c.ColumnName.ToLowerInvariant()).ToArray();
            var fields = GetFields<T>().Where(f => columns.Contains(f.Name.ToLowerInvariant())).ToArray();
            var instance = GetInstance<T>();

            foreach (DataRow row in table.Rows)
            {
                var entry = instance();

                foreach (var field in fields)
                {
                    field.SetValue(entry, row[field.Name]);
                }

                list.Add((T)entry);
            }

            return list;
        }
    }
    public struct EntryField
    {
        private readonly PropertyInfo _property;
        private readonly FieldInfo _field;

        public EntryField(PropertyInfo property)
        {
            _property = property;
            _field = null;
        }

        public EntryField(FieldInfo field)
        {
            _property = null;
            _field = field;
        }

        public void SetValue(object entry, object value)
        {
            if (_field != null) _field.SetValue(entry, value is DBNull ? null : value);
            if (_property != null) _property.SetValue(entry, value is DBNull ? null : value);
        }

        public string Name
        {
            get
            {
                if (_property != null) return _property.Name;
                if (_field != null) return _field.Name;
                throw new NotImplementedException();
            }
        }
    }
    internal class ScalarValueTypeMapper<T> : IScalarTypeMapper
    {
        public T Value0 { get; set; }
    }

    internal class Scalar2ValueTypeMapper<T1, T2> : IScalarTypeMapper
    {
        public T1 Value0 { get; set; }
        public T2 Value1 { get; set; }
    }

    internal interface IScalarTypeMapper
    {

    }
}
