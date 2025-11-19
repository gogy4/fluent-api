using System.Reflection;
using ObjectPrinting.HomeWork.RuleUtils.Dto;
using ObjectPrinting.HomeWork.Strategies.Interfaces;

namespace ObjectPrinting.HomeWork.Strategies.Implementations;

public class TrimStringRule(PropertyInfo property, int length) : ISerializationRule
{
    public bool CanApply(PropertyInfo propertyInfo)
    {
        return property == propertyInfo && propertyInfo.PropertyType == typeof(string);
    }

    public RuleOutcome Apply(object value)
    {
        return new RuleOutcome(RuleResult.Print, ((string)value)[..Math.Min(length, ((string)value).Length)]);
    }
}