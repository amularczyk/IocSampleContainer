namespace IocSampleContainer
{
    public interface IContainer
    {
        void Register<TIn, TOut>(bool isSingleton);

        T Resolve<T>();
    }
}