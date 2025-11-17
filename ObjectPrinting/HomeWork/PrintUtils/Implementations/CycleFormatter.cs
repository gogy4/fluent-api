using System.Reflection;
using ObjectPrinting.HomeWork.PrintUtils.Interfaces;
using ObjectPrinting.HomeWork.RuleUtils.Interfaces;

namespace ObjectPrinting.HomeWork.PrintUtils.Implementations;

public class CycleFormatter(IRuleProcessor ruleProcessor) : ICycleFormatter
{
    private readonly HashSet<object> visited = [];
    public string FormatReference(object obj)
    {
        var type = obj.GetType();
        //TODO доделать реализацию, сейчас есть ошибка в логике. Если поле хотят exclude тогда, неясно будет, в цикле, что нам указывать
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var basicProperty = properties.Length > 1 ? properties[1] :
            properties.Length > 0 ? properties[0] : null;
        var basicValue = basicProperty != null
            ? ruleProcessor.ApplyRule(basicProperty.GetValue(obj), basicProperty).Value 
            : "?";
        return $"<see {obj.GetType().Name} {basicValue}>";
    }
    
    public bool TryMark(object obj)
    {
        return visited.Add(obj);
    }
}