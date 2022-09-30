namespace ChristianSchulz.ObjectInspection.Application.Operation;

public interface IConnectionViolationManager
{
    void Trace(string violator);

    bool Verify(string violator);
}