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
        var type = typeof(T);
        ruleProcessor.AddRule(new ExcludeRule(type));
        return this;
    }

    public PrintingConfig<TOwner> SetNumericCulture(CultureInfo culture)
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
        Expression<Func<TOwner, TProperty>> property, Func<TProperty, string> serializer)
    {
        var body = property.Body as MemberExpression;
        var propertyInfo = body?.Member as PropertyInfo;
        ruleProcessor.AddRule(new SerializationRule<TProperty>(serializer, propertyInfo));
        return this;
    }

    public PrintingConfig<TOwner> Trim(Expression<Func<TOwner, string>> property, int length)
    {
        var body = property.Body as MemberExpression;
        if (body?.Member is not PropertyInfo propertyInfo)
        {
            throw new NullReferenceException();
        }

        ruleProcessor.AddRule(new TrimStringRule(propertyInfo, length));
        return this;
    }

    public PrintingConfig<TOwner> Exclude<TProperty>(Expression<Func<TOwner, TProperty>> property)
    {
        var body = property.Body as MemberExpression;
        var propertyInfo = body?.Member as PropertyInfo;
        ruleProcessor.AddRule(new ExcludeRule(propertyInfo));
        return this;
    }
}