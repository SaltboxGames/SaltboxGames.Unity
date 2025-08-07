using System;
using System.Linq.Expressions;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace SaltboxGames.Unity.Editor.Utilities
{
    public static class EditorReflection
    {
        private static Func<string> _getActiveFolderPath;
        private static Func<SerializedProperty, FieldInfo> _getFieldInfoFromProperty;

        public static string GetActiveFolderPath()
        {
            return _getActiveFolderPath();
        }

        public static FieldInfo GetFieldInfoFromProperty(SerializedProperty property)
        {
            return _getFieldInfoFromProperty(property);
        }

        static EditorReflection()
        {
            _getActiveFolderPath = Create_GetActiveFolderPath();
            _getFieldInfoFromProperty = Create_GetFieldInfoFromProperty();
        }

        private static Func<string> Create_GetActiveFolderPath()
        {
            Type projectWindowUtil = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.ProjectWindowUtil");
            MethodInfo getFolderMethod = projectWindowUtil?.GetMethod("GetActiveFolderPath", BindingFlags.Static | BindingFlags.NonPublic);

            if (getFolderMethod == null || getFolderMethod.ReturnType != typeof(string))
            {
                Debug.LogWarning("[EditorReflectionUtils] Unable to bind GetActiveFolderPath. Falling back to 'Assets/'.");
                return () => "Assets";
            }
            else
            {
                return Expression.Lambda<Func<string>>(Expression.Call(getFolderMethod)).Compile();
            }
        }

        private static Func<SerializedProperty, FieldInfo> Create_GetFieldInfoFromProperty()
        {
            Type scriptAttrUtilType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.ScriptAttributeUtility");
            MethodInfo fieldInfoMethod = scriptAttrUtilType?.GetMethod("GetFieldInfoFromProperty", BindingFlags.Static | BindingFlags.NonPublic);

            if (fieldInfoMethod == null)
            {
                Debug.LogWarning("[EditorReflectionUtils] Unable to bind GetFieldInfoFromProperty.");
                return null;
            }
            
            ParameterExpression propertyParam = Expression.Parameter(typeof(SerializedProperty), "property");
            Expression[] args = new Expression[]
            {
                propertyParam,
                Expression.Constant(null, typeof(Type))
            };

            MethodCallExpression call = Expression.Call(fieldInfoMethod, args);
            return Expression.Lambda<Func<SerializedProperty, FieldInfo>>(call, propertyParam).Compile();
        }
    }
}
