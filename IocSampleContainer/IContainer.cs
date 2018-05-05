namespace IocSampleContainer
{
    public interface IContainer
    {
        void Register<TIn, TOut>(RegistrationKind registrationKind);

        T Resolve<T>();

        T Resolve<T>(IScope scope);

        IScope StartNewScope();
    }
}