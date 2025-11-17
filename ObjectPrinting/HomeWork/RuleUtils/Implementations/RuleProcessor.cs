using System.Reflection;
using ObjectPrinting.HomeWork.RuleUtils.Dto;
using ObjectPrinting.HomeWork.RuleUtils.Interfaces;
using ObjectPrinting.HomeWork.Strategies.Interfaces;

namespace ObjectPrinting.HomeWork.RuleUtils.Implementations;

public class RuleProcessor : IRuleProcessor
{
    private readonly List<ISerializationRule> rules = [];

    public void AddRule(ISerializationRule rule)
    {
        rules.Add(rule);
    }
    
    public RuleOutcome ApplyRule(object propertyValue, PropertyInfo? propertyInfo)
    {
        foreach (var rule in rules)
        {
            if (propertyInfo == null || !rule.CanApply(propertyInfo)) continue;

            var result = rule.Apply(propertyValue);
            if (result.Action == RuleResult.Skip)
                return new RuleOutcome(RuleResult.Skip, null);

            propertyValue = result.Value;
        }

        return new RuleOutcome( RuleResult.Print, propertyValue.ToString());
    }
}