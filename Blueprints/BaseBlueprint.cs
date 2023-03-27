using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace CoH2XML2JSON.Blueprints;

/// <summary>
/// Base class for blueprint classes that can be extended by another blueprint.
/// </summary>
/// <typeparam name="T">The type of the blueprint that can be extended.</typeparam>
public abstract class BaseBlueprint<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)]  T> : IExtendableBlueprint<T> where T : IBlueprint {

    private readonly Dictionary<string, object?> _impl = new();
    private readonly Dictionary<string, PropertyInfo> _properties = new();

    /// <summary>
    /// Gets or sets the blueprint that this instance extends, if any.
    /// </summary>
    public T? Extends { get; set; }

    private object? GetSuperValue(string key) {
        if (Extends is null) {
            return null;
        }
        if (_properties.TryGetValue(key, out PropertyInfo? pinfo) && pinfo is not null) {
            return pinfo.GetValue(Extends);
        }
        PropertyInfo? vInfo = typeof(T).GetProperty(key, BindingFlags.Public | BindingFlags.Instance)
            ?? throw new Exception($"Failed to get property '{key}' on type '{typeof(T)}'");
        _properties[key] = vInfo;
        return vInfo.GetValue(Extends);
    }

    private TFieldType? GetSuperValue<TFieldType>(string key)
        => GetSuperValue(key) is TFieldType value ? value : default;

    /// <summary>
    /// Gets the value of a property with the specified key. If the property is not defined in this instance, 
    /// returns the value of the property with the same key in the extended blueprint, if any.
    /// </summary>
    /// <param name="key">The key of the property to get.</param>
    /// <returns>The value of the property, or null if the property is not defined.</returns>
    protected object? GetValue(string key)
        => _impl.TryGetValue(key, out object? obj) ? obj : GetSuperValue(key);

    /// <summary>
    /// Gets the value of a property with the specified key. If the property is not defined in this instance, 
    /// returns the value of the property with the same key in the extended blueprint, if any.
    /// </summary>
    /// <param name="key">The key of the property to get.</param>
    /// <returns>The value of the property, or null if the property is not defined.</returns>
    protected TFieldType? GetValue<TFieldType>(string key)
        => _impl.TryGetValue(key, out object? obj) && obj is TFieldType objval ? objval : GetSuperValue<TFieldType>(key);

    /// <summary>
    /// Sets the value of a property with the specified key.
    /// </summary>
    /// <param name="key">The key of the property to set.</param>
    /// <param name="value">The value to set the property to.</param>
    protected void SetValue(string key, object? value) => _impl[key] = value;

}
