using System.Reflection;
using ObjectPrinting.HomeWork.RuleUtils.Dto;
using ObjectPrinting.HomeWork.Strategies.Interfaces;

namespace ObjectPrinting.HomeWork.Strategies.Implementations;

public class SerializationRule<T>(Func<T, string> serializer, PropertyInfo? property = null) : ISerializationRule
{
    public bool CanApply(PropertyInfo propertyInfo)
    {
        var rightValue = propertyInfo.PropertyType is T;
        return property is null ? rightValue : property == propertyInfo && rightValue;
    }

    public RuleOutcome Apply(object value)
    {
        return new RuleOutcome(RuleResult.Print, serializer((T)value));
    }
}