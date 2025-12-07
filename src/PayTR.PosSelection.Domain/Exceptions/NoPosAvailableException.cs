namespace PayTR.PosSelection.Domain.Exceptions;

public class NoPosAvailableException : Exception
{
    public NoPosAvailableException(string message) : base(message)
    {
    }
}
