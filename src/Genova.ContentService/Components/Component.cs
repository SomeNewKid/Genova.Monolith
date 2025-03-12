using Genova.ContentService.Fields;
using Genova.ContentService.Templates;

namespace Genova.ContentService.Components;

/// <summary>
/// A base class that implements <see cref="IComponent"/>, holding
/// a unique <see cref="Key"/>, a collection of <see cref="IField"/>s,
/// and an ordered list of child components.
/// </summary>
public abstract class Component : IComponent
{
    private readonly List<IField> _fields = new();
    private readonly List<IComponent> _children = new();

    /// <summary>
    /// Gets a unique key for this component within its parent context,
    /// used in dotted paths (e.g., "article.title").
    /// This property is read-only externally, but can be set internally.
    /// </summary>
    public string Key { get; private set; } = "";

    /// <summary>
    /// A string identifier that specifies the kind of component this is,
    /// such as "ArticleComponent", "GalleryComponent", etc.
    /// Must be provided by a derived class.
    /// </summary>
    public abstract string ComponentType { get; }

    /// <summary>
    /// A collection of fields that store individual data points,
    /// such as "Title" or "Summary". This is read-only externally,
    /// but derived classes can manage it internally.
    /// </summary>
    public IReadOnlyList<IField> Fields => _fields;

    /// <summary>
    /// An ordered list of child components, each of which also
    /// has a key and fields. This is read-only externally, but
    /// derived classes can modify it internally as needed.
    /// </summary>
    public IReadOnlyList<IComponent> Children => _children;

    /// <summary>
    /// Allows derived classes to add a field to the collection.
    /// The field's <see cref="IField.Key"/> should be unique within this component.
    /// </summary>
    protected void AddField(IField field)
    {
        if (field == null)
            throw new ArgumentNullException(nameof(field));

        // Check if any existing field matches the new field's key (ignoring case).
        bool exists = _fields.Any(f =>
            f.Key.Equals(field.Key, StringComparison.OrdinalIgnoreCase)
        );

        if (exists)
        {
            throw new InvalidOperationException(
                $"A field with key '{field.Key}' already exists in this component " +
                "(key comparison is case-insensitive)."
            );
        }

        _fields.Add(field);
    }

    /// <summary>
    /// Allows derived classes to add a child component to the collection.
    /// The child component's <see cref="IComponent.Key"/> should be unique
    /// within this component's <see cref="Children"/>.
    /// </summary>
    public void AddChild(IComponent child)
    {
        if (child == null)
            throw new ArgumentNullException(nameof(child));

        // 1) Validate the child before any further checks
        var childErrors = child.Validate(ValidationMode.Definition).ToList();
        if (childErrors.Any())
        {
            var combined = string.Join("; ", childErrors);
            throw new InvalidOperationException(
                $"Cannot add child component '{child.Key}' because it is invalid. Errors: {combined}"
            );
        }

        // 2) Check if any existing child has the same key (ignoring case).
        bool exists = _children.Any(c =>
            c.Key.Equals(child.Key, StringComparison.OrdinalIgnoreCase)
        );

        if (exists)
        {
            throw new InvalidOperationException(
                $"A child component with key '{child.Key}' already exists in this component " +
                "(key comparison is case-insensitive)."
            );
        }

        // 3) Prevent child of the same component type
        if (string.Equals(this.ComponentType, child.ComponentType, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                $"Cannot add a child of the same ComponentType '{child.ComponentType}' " +
                $"as the parent '{this.ComponentType}'."
            );
        }

        // 4) If this component is NOT an ITemplate, it can't add a child that has its own children
        if (this is not ITemplate && child.Children.Any())
        {
            throw new InvalidOperationException(
                "A non-template component cannot add a child component which already has children."
            );
        }

        // If we reach here, the child is valid and passes all other rules
        _children.Add(child);
    }

    public void SetKey(string key)
    {
        if (!string.IsNullOrEmpty(Key))
            throw new InvalidOperationException("Key is already set; cannot overwrite it.");

        Key = key;
    }

    /// <summary>
    /// Validates this component (e.g., ensures Key is set),
    /// then recursively validates all child components.
    /// Subclasses can override this if they need more specialized checks.
    /// </summary>
    /// <returns>An IEnumerable of error messages; empty if the component is valid.</returns>
    public virtual IEnumerable<string> Validate(ValidationMode validationMode)
    {
        // Basic check: Key must not be empty
        if (string.IsNullOrWhiteSpace(this.Key))
        {
            yield return $"Component of type '{this.ComponentType}' has no Key set.";
        }

        // Recurse into children
        foreach (var child in _children)
        {
            foreach (var childError in child.Validate(validationMode))
            {
                yield return childError;
            }
        }
    }
}