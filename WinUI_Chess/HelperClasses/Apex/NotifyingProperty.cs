//
//
namespace CyberFeedForward.WinUI_Chess.HelperClasses.Apex;

using System;

/// <summary>
/// The NotifyingProperty class - represents a property of a viewmodel that
/// can be wired into the notification system.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="NotifyingProperty"/> class.
/// </remarks>
/// <param name="name">The name.</param>
/// <param name="type">The type.</param>
/// <param name="value">The value.</param>
public class NotifyingProperty(string name, Type type, object value)
{

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>The name.</value>
    public string Name { get; set; } = name;

    /// <summary>
    /// Gets or sets the type.
    /// </summary>
    /// <value>The type.</value>
    public Type Type { get; set; } = type;

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    /// <value>The value.</value>
    public object Value { get; set; } = value;
}