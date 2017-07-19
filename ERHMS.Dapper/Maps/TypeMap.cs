﻿using Dapper;
using ERHMS.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ERHMS.Dapper
{
    public class TypeMap : SqlMapper.ITypeMap, IEnumerable<PropertyMap>
    {
        private static readonly Regex PrefixPattern = new Regex(@"^.+\.");

        private DefaultTypeMap @base;
        private IDictionary<PropertyInfo, PropertyMap> propertyMaps;

        public Type Type { get; private set; }
        public string TableName { get; set; }

        public TypeMap(Type type)
        {
            @base = new DefaultTypeMap(type);
            Type = type;
            TableName = type.Name;
            propertyMaps = new Dictionary<PropertyInfo, PropertyMap>();
            foreach (PropertyInfo property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                Set(property, property.Name);
            }
        }

        public PropertyMap Set(PropertyInfo property, string columnName)
        {
            PropertyMap propertyMap = new PropertyMap(columnName, property);
            propertyMaps[property] = propertyMap;
            return propertyMap;
        }

        public PropertyMap Set(string propertyName, string columnName)
        {
            return Set(Type.GetProperty(propertyName), columnName);
        }

        public PropertyMap Get(PropertyInfo property)
        {
            return propertyMaps[property];
        }

        public PropertyMap Get(string propertyName)
        {
            return Get(Type.GetProperty(propertyName));
        }

        public PropertyMap GetId()
        {
            return propertyMaps.Values.Single(propertyMap => propertyMap.Id);
        }

        public IEnumerable<PropertyMap> GetInsertable()
        {
            return propertyMaps.Values.Where(propertyMap => !propertyMap.Computed);
        }

        public IEnumerable<PropertyMap> GetUpdatable()
        {
            return propertyMaps.Values.Where(propertyMap => !propertyMap.Id && !propertyMap.Computed);
        }

        public ConstructorInfo FindConstructor(string[] names, Type[] types)
        {
            return @base.FindConstructor(names, types);
        }

        public ConstructorInfo FindExplicitConstructor()
        {
            return @base.FindExplicitConstructor();
        }

        public SqlMapper.IMemberMap GetConstructorParameter(ConstructorInfo constructor, string columnName)
        {
            return @base.GetConstructorParameter(constructor, columnName);
        }

        private bool TryGetMember(string columnName, out SqlMapper.IMemberMap result)
        {
            result = propertyMaps.Values.SingleOrDefault(propertyMap => propertyMap.ColumnName.EqualsIgnoreCase(columnName));
            return result != null;
        }

        public SqlMapper.IMemberMap GetMember(string columnName)
        {
            SqlMapper.IMemberMap propertyMap;
            if (TryGetMember(columnName, out propertyMap))
            {
                return propertyMap;
            }
            if (PrefixPattern.IsMatch(columnName))
            {
                if (TryGetMember(PrefixPattern.Replace(columnName, ""), out propertyMap))
                {
                    return propertyMap;
                }
            }
            return null;
        }

        public IEnumerator<PropertyMap> GetEnumerator()
        {
            return propertyMaps.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}