using System.Globalization;
using System.Reflection;
using ObjectPrinting.HomeWork.RuleUtils.Dto;
using ObjectPrinting.HomeWork.RuleUtils.Strategies.Interfaces;

namespace ObjectPrinting.HomeWork.RuleUtils.Strategies.Implementations;

public class CultureRule(CultureInfo cultureInfo) : ISerializationRule
{
    public bool CanApply(PropertyInfo propertyInfo)
    {
        return typeof(IFormattable).IsAssignableFrom(propertyInfo.PropertyType);
    }

    public RuleOutcome Apply(object value)
    {
        return new RuleOutcome(RuleResult.Print, ((IFormattable)value).ToString(null, cultureInfo));
    }
}