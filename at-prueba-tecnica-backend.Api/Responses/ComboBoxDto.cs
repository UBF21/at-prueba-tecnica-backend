namespace at_prueba_tecnica_backend.Api.Responses;

/// <summary>
/// Response DTO for combobox/dropdown lists (value-label pairs).
/// </summary>
public record ComboBoxDto(
    string Value,
    string Label
);
