namespace GrandDevs.ExtremeScooling
{
    public interface IServiceLocator
    {
        T GetService<T>();
        void Update();
    }
}