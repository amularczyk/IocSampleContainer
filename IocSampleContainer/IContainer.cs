namespace IocSampleContainer
{
    public interface IContainer
    {
        void Register<TIn, TOut>(RegistrationKind registrationKind);

        T Resolve<T>();
    }
}