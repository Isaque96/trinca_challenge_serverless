namespace Domain.Entities;

public enum ChurrasStatus : int
{
    New = 1,
    PendingConfirmations = 2,
    Confirmed = 3,
    ItsNotGonnaHappen = 4
}