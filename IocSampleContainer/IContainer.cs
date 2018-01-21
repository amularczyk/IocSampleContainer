namespace IocSampleContainer
{
    public interface IContainer
    {
        void Register<T>(bool isSingleton);

        T Resolve<T>();
    }
}