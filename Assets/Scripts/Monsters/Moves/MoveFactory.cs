using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class MoveFactory
{
    private static Dictionary<string, Type> _movesByName;
    private static bool IsInitialized => _movesByName != null;

    private static void InitializeFactory()
    {
        if (IsInitialized)
            return;

        var moveTypes = Assembly.GetAssembly(typeof(Move)).GetTypes().
            Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(Move)));

        _movesByName = new Dictionary<string, Type>();

        foreach (var type in moveTypes)
        {
            var tempEffect = Activator.CreateInstance(type) as Move;
            _movesByName.Add(tempEffect.Name, type);
        }
    }

    public static Move GetAbility(string moveName)
    {
        InitializeFactory();

        if (_movesByName.ContainsKey(moveName))
        {
            Type type = _movesByName[moveName];
            var ability = Activator.CreateInstance(type) as Move;
            return ability;
        }

        return null;
    }
}
