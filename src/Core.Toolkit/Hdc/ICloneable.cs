namespace Core
{
    public interface ICloneable<out T>
    {
        T Clone();
    }
}