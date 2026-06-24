namespace MedManage.Core.DTOs.Case;

/// <summary>
/// Result of business rule validation for a case operation
/// </summary>
public class BusinessRuleValidationResult
{
    public bool IsValid => Errors.Count == 0;
    public List<BusinessRuleError> Errors { get; set; } = new();
    public List<BusinessRuleWarning> Warnings { get; set; } = new();

    public static BusinessRuleValidationResult Success() => new();

    public static BusinessRuleValidationResult Failure(string rule, string message)
    {
        var result = new BusinessRuleValidationResult();
        result.Errors.Add(new BusinessRuleError(rule, message));
        return result;
    }

    public void AddError(string rule, string message)
    {
        Errors.Add(new BusinessRuleError(rule, message));
    }

    public void AddWarning(string rule, string message)
    {
        Warnings.Add(new BusinessRuleWarning(rule, message));
    }
}

/// <summary>
/// A business rule violation that prevents the operation
/// </summary>
public record BusinessRuleError(string Rule, string Message);

/// <summary>
/// A business rule warning that does not prevent the operation but should be surfaced to the user
/// </summary>
public record BusinessRuleWarning(string Rule, string Message);
