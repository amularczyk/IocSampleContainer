namespace IocSampleContainer
{
    public interface IContainer
    {
        void Register<T>();

        T Resolve<T>();
    }
}