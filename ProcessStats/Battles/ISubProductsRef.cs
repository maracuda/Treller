namespace ProcessStats.Battles
{
    public interface ISubProductsRef
    {
        string[] GetSubproductIds();
        string GetSubProductName(string subProductId);
    }
}