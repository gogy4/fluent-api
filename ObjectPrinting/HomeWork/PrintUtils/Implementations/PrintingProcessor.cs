using System.Reflection;
using System.Text;
using ObjectPrinting.HomeWork.PrintUtils.Interfaces;
using ObjectPrinting.HomeWork.RuleUtils.Dto;
using ObjectPrinting.HomeWork.RuleUtils.Interfaces;

namespace ObjectPrinting.HomeWork.PrintUtils.Implementations;

public class PrintingProcessor(IRuleProcessor ruleProcessor, ICycleFormatter cycleFormatter) : IPrintingProcessor
{
    public string Print(object? obj, int nestingLevel, HashSet<object> visited)
    {
        var (text, type) = HandleNullOrVisited(obj);
        if (type is null)
        {
            return text;
        }

        var sb = new StringBuilder();
        sb.AppendLine(type.Name);

        var indent = new string('\t', nestingLevel + 1);

        foreach (var prop in type.GetProperties())
        {
            var value = prop.GetValue(obj);
            if (value == null) continue;

            AppendProperty(sb, prop, value, nestingLevel, visited, indent);
        }

        return sb.ToString().TrimEnd();
    }

    private (string? text, Type? type) HandleNullOrVisited(object? obj)
    {
        if (obj == null)
            return ("null", null);

        var firstSeen = cycleFormatter.TryMark(obj);
        if (!firstSeen)
            return (cycleFormatter.FormatReference(obj), null);

        return (null, obj.GetType());
    }

    private void AppendProperty(StringBuilder sb, PropertyInfo prop, object value, int nestingLevel,
        HashSet<object> visited, string indent)
    {
        var typeValue = value.GetType();
        var defaultValue = typeValue.IsValueType ? Activator.CreateInstance(typeValue) : null;
        if (value.Equals(defaultValue)) return;

        var ruleOutcome = ruleProcessor.ApplyRule(value, prop);
        if (ruleOutcome.Action == RuleResult.Skip) return;

        sb.Append(indent + prop.Name + " = ");

        if (!(typeValue.IsPrimitive || typeValue == typeof(string)) && ruleOutcome.Value == value.ToString())
        {
            sb.Append(Print(value, nestingLevel + 1, visited));
        }
        else
        {
            sb.Append(ruleOutcome.Value);
        }

        sb.Append('\n');
    }
}