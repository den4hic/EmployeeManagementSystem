namespace Application.Common;

public abstract class BaseDto<TId>
{
    public TId Id { get; set; }
}
