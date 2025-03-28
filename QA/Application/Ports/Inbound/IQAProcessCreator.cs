namespace QA.Application.Ports.Inbound
{
    public interface IQAProcessCreator<T>
    {
        Task CreateProcess(T messageBody);
    }
}
