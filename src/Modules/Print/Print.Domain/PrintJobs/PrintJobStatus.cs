namespace LimonikOne.Modules.Print.Domain.PrintJobs;

public enum PrintJobStatus
{
    Queued = 0,
    Claimed = 1,
    Completed = 2,
    Failed = 3,
}
