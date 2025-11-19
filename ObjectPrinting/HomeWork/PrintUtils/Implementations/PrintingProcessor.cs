using ObjectPrinting.HomeWork.PrintUtils.Interfaces;
using ObjectPrinting.HomeWork.PrintUtils.Strategies.Implementations;
using ObjectPrinting.HomeWork.PrintUtils.Strategies.Interfaces;

namespace ObjectPrinting.HomeWork.PrintUtils.Implementations;

public class PrintingProcessor(
    IEnumerable<IPrintStrategy> strategies)
    : IPrintingProcessor
{
    private readonly List<IPrintStrategy> strategies = strategies.ToList();

    public string Print(object? obj, int nestingLevel, HashSet<object> visited)
    {
        if (obj == null) return "null";

        var type = obj.GetType();

        foreach (var strategy in strategies.OrderByDescending(strategy => strategy is CycleFormatterStrategy))
        {
            if (!strategy.CanHandle(type)) continue;

            var result = strategy.Print(obj, nestingLevel, visited, Print);
            if (result != null) 
                return result;
        }

        throw new InvalidOperationException($"No strategy could print object of type {type.Name}");
    }

}