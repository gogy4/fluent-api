namespace ObjectPrinting.HomeWork.PrintUtils.Interfaces;

public interface ICycleFormatter
{
    string FormatReference(object obj);
    public bool TryMark(object obj);
}