namespace IocSampleContainer
{
    public interface IContainer
    {
        void Register<T>(bool isSingleton);

        void Register<TIn, TOut>(bool isSingleton);

        T Resolve<T>();
    }
}