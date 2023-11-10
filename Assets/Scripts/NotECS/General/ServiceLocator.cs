using System.Collections.Generic;
using System.Linq;

public static class ServiceLocator
{
    private static readonly HashSet<object> _registeredObjects;

    static ServiceLocator()
    {
        _registeredObjects = new HashSet<object>();
    }

    public static void Register<T>(T obj) where T : class
    {
        _registeredObjects.Add(obj);
    }

    public static void Unregister<T>(T obj) where T : class
    {
        _registeredObjects.Remove(obj);
    }

    public static T Resolve<T>() where T : class
    {
        var obj = _registeredObjects.SingleOrDefault(x => x is T);
        return obj as T;
    }
}