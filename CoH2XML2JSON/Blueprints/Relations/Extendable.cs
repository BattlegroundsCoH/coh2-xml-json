using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace CoH2XML2JSON.Blueprints.Relations;

/// <summary>
/// Provides a base class for extendable objects. Extendable objects can inherit from other objects of the same type, allowing them to share properties and values.
/// </summary>
/// <typeparam name="T">The type of the extendable object.</typeparam>
public abstract class Extendable<T> where T : Extendable<T> {

    private readonly Dictionary<string, object?> _impl = new();

    /// <inheritdoc/>
    [JsonIgnore]
    public T? Extends { get; set; }

    private TFieldType? GetSuperValue<TFieldType>(string key)
        => Extends is not null ? Extends.GetValue<TFieldType>(key) is TFieldType value ? value : default : default;

    /// <summary>
    /// Gets the value of a property with the specified key. If the property is not defined in this instance, 
    /// returns the value of the property with the same key in the extended blueprint, if any.
    /// </summary>
    /// <param name="key">The key of the property to get.</param>
    /// <returns>The value of the property, or null if the property is not defined.</returns>
    protected TFieldType? GetValue<TFieldType>([CallerMemberName] string key = "")
        => _impl.TryGetValue(key, out object? obj) && obj is TFieldType objval ? objval : GetSuperValue<TFieldType>(key);

    /// <summary>
    /// Gets the value of a property with the specified key. If the property is not defined in this instance, or uses the specified default value,
    /// returns the value of the property with the same key in the extended blueprint, if any.
    /// </summary>
    /// <param name="key">The key of the property to get.</param>
    /// <returns>The value of the property, or null if the property is not defined.</returns>
    protected TFieldType? GetValueWhereDefaultIs<TFieldType>(TFieldType? def, [CallerMemberName] string key = "")
        => GetValueWhereDefaultMatches<TFieldType>(x => def?.Equals(x) ?? (x is null), key);

    /// <summary>
    /// Gets the value of a property with the specified key. If the property is not defined in this instance, or uses the specified default value,
    /// returns the value of the property with the same key in the extended blueprint, if any.
    /// </summary>
    /// <param name="key">The key of the property to get.</param>
    /// <returns>The value of the property, or null if the property is not defined.</returns>
    protected TFieldType? GetValueWhereDefaultMatches<TFieldType>(Predicate<TFieldType?> defaultTest, [CallerMemberName] string key = "") {
        if (_impl.TryGetValue(key, out object? obj) && obj is TFieldType objval) {
            return defaultTest(objval) ? GetSuperValue<TFieldType>(key) : objval;
        }
        return GetSuperValue<TFieldType>(key);
    }

    /// <summary>
    /// Sets the value of a property with the specified key.
    /// </summary>
    /// <param name="value">The value to set the property to.</param>
    /// <param name="key">The key of the property to set.</param>
    protected void SetValue(object? value, [CallerMemberName] string key = "") => _impl[key] = value;

}
