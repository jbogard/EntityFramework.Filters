namespace EntityFramework.Filters
{
    public interface IFilter
    {
        string FilterName { get; }
        IFilter SetParameter(string parameter, object value);
    }
}