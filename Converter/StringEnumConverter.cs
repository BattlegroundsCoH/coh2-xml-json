using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CoH2XML2JSON.Converter;

/// <summary>
/// Provides a custom JSON converter that serializes and deserializes an enumeration to and from a string representation.
/// </summary>
public sealed class StringEnumConverter : JsonConverterFactory {

    /// <summary>
    /// Provides a concrete implementation of the <see cref="JsonConverter{T}"/> class that converts the specified enumeration to and from a string representation.
    /// </summary>
    /// <typeparam name="T">The enumeration type to convert.</typeparam>
    public class ConcreteConverter<T> : JsonConverter<T> {
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => throw new NotSupportedException();
        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options) => writer.WriteStringValue(value?.ToString());
    }

    /// <summary>
    /// Determines whether the specified type can be converted by this converter.
    /// </summary>
    /// <param name="typeToConvert">The type to check.</param>
    /// <returns>true if the type is an enumeration; otherwise, false.</returns>
    public override bool CanConvert(Type typeToConvert) => typeToConvert.IsEnum;

    /// <summary>
    /// Creates a custom JSON converter that can convert the specified enumeration to and from a string representation.
    /// </summary>
    /// <param name="typeToConvert">The enumeration type to convert.</param>
    /// <param name="options">The serialization options to use.</param>
    /// <returns>A custom JSON converter that can serialize and deserialize the specified enumeration type.</returns>
    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        => Activator.CreateInstance(typeof(ConcreteConverter<>).MakeGenericType(typeToConvert)) as JsonConverter ?? throw new Exception();

}
