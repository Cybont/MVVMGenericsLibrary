namespace GenericsLibrary
{
    //For TDTO's and Models
    public interface IKey<TKey>
    {
        TKey Key { get; set; }
    }
}