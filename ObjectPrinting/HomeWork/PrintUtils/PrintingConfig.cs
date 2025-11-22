using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using ObjectPrinting.HomeWork.PrintUtils.Interfaces;
using ObjectPrinting.HomeWork.RuleUtils.Interfaces;
using ObjectPrinting.HomeWork.RuleUtils.Strategies.Implementations;

namespace ObjectPrinting.HomeWork.PrintUtils;

public class PrintingConfig<TOwner>(IRuleProcessor ruleProcessor, IPrintingProcessor printingProcessor)
{
    public string PrintToString(TOwner obj)
    {
        return printingProcessor.Print(obj, 0, []);
    }

    public PrintingConfig<TOwner> Exclude<T>()
    {
        ruleProcessor.AddRule(new ExcludeRule(typeof(T)));
        return this;
    }

    public PrintingConfig<TOwner> SetFormattingCulture(CultureInfo culture)
    {
        ruleProcessor.AddRule(new CultureRule(culture));
        return this;
    }

    public PrintingConfig<TOwner> Serialize<TType>(Func<TType, string> serializer)
    {
        ruleProcessor.AddRule(new SerializationRule<TType>(serializer));
        return this;
    }

    public PrintingConfig<TOwner> Serialize<TProperty>(
        Expression<Func<TOwner, TProperty>> property,
        Func<TProperty, string> serializer)
    {
        var propertyInfo = GetPropertyInfo(property);
        ruleProcessor.AddRule(new SerializationRule<TProperty>(serializer, propertyInfo));
        return this;
    }

    public PrintingConfig<TOwner> Trim(Expression<Func<TOwner, string>> property, int length)
    {
        var propertyInfo = GetPropertyInfo(property);
        ruleProcessor.AddRule(new TrimStringRule(propertyInfo, length));
        return this;
    }

    public PrintingConfig<TOwner> Exclude<TProperty>(Expression<Func<TOwner, TProperty>> property)
    {
        var propertyInfo = GetPropertyInfo(property);
        ruleProcessor.AddRule(new ExcludeRule(propertyInfo));
        return this;
    }
    
    private static PropertyInfo GetPropertyInfo<TProperty>(
        Expression<Func<TOwner, TProperty>> propertyExpression)
    {
        if (propertyExpression.Body is not MemberExpression memberExpr)
            throw new ArgumentException(
                $"Expression '{propertyExpression}' must be a simple property access",
                nameof(propertyExpression));

        if (memberExpr.Member is not PropertyInfo propertyInfo)
            throw new ArgumentException(
                $"Expression '{propertyExpression}' must refer to a property, not a field/method",
                nameof(propertyExpression));

        return propertyInfo;
    }
}
