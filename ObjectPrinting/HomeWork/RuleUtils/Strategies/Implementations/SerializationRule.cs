using System.Reflection;
using ObjectPrinting.HomeWork.RuleUtils.Dto;
using ObjectPrinting.HomeWork.Strategies.Interfaces;

namespace ObjectPrinting.HomeWork.Strategies.Implementations;

public class SerializationRule<T>(Func<T, string> serializer, PropertyInfo? property = null) : ISerializationRule
{
    public bool CanApply(PropertyInfo propertyInfo)
    {
        if (propertyInfo is null) return true;
        var rightValue = propertyInfo.PropertyType == typeof(T);
        return property is null ? rightValue : property == propertyInfo && rightValue;
    }

    public RuleOutcome Apply(object value)
    {
        if (value is T typedValue)
        {
            return new RuleOutcome(RuleResult.Print, serializer(typedValue));
        }

        return new RuleOutcome(RuleResult.Print, value.ToString());
    }
}