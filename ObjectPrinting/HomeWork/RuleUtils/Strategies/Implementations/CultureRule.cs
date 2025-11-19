using System.Globalization;
using System.Reflection;
using ObjectPrinting.HomeWork.RuleUtils.Dto;
using ObjectPrinting.HomeWork.Strategies.Interfaces;

namespace ObjectPrinting.HomeWork.Strategies.Implementations;

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