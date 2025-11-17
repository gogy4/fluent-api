using ObjectPrinting.HomeWork.PrintUtils;
using ObjectPrinting.HomeWork.PrintUtils.Implementations;
using ObjectPrinting.HomeWork.PrintUtils.Interfaces;
using ObjectPrinting.HomeWork.RuleUtils.Implementations;

namespace ObjectPrinting.HomeWork;

public class ObjectPrinter
{
    public static PrintingConfig<T> For<T>()
    {
        var ruleProcessor = new RuleProcessor();
        var cycleForammter = new CycleFormatter(ruleProcessor);
        return new PrintingConfig<T>(ruleProcessor, new PrintingProcessor(ruleProcessor, cycleForammter));
    }
}