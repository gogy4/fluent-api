using System.Reflection;
using ObjectPrinting.HomeWork.PrintUtils.Helpers;
using ObjectPrinting.HomeWork.PrintUtils.Strategies.Interfaces;
using ObjectPrinting.HomeWork.RuleUtils.Dto;
using ObjectPrinting.HomeWork.RuleUtils.Interfaces;

namespace ObjectPrinting.HomeWork.PrintUtils.Strategies.Implementations;

public class SimplePrinterStrategy(IRuleProcessor ruleProcessor) : IPrintStrategy
{
    public bool CanHandle(Type type) => SimpleHelper.IsSimple(type);

    public string Print(object obj, int nestingLevel, HashSet<object> visited,
        Func<object?, int, HashSet<object>, string> recursivePrinter)
    {
        return Format(obj);
    }

    private string Format(object obj, PropertyInfo? propInfo = null)
    {
        var outcome = ruleProcessor.ApplyRule(obj, propInfo);
        if (outcome.Action == RuleResult.Skip)
        {
            return "null";
        }
        return outcome.Value ?? obj.ToString() ?? string.Empty;
    }
}