namespace IocSampleContainer
{
    public interface IContainer
    {
        void Register<T>(RegistrationKind registrationKind);

        void Register<TIn, TOut>(RegistrationKind registrationKind);

        T Resolve<T>();

        T Resolve<T>(IScope scope);

        IScope StartNewScope();
    }
}