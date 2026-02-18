using Serilog;

namespace MyCrmSampleClient.Console;

public sealed record SampleState
{
    public int? LastContactId { get; init; }
    public int? LastContactGroupId { get; init; }
    public int? LastDealId { get; init; }
        
    public bool TryGetDealId(string sampleName, out int dealId)
    {
        if (LastDealId.HasValue)
        {
            dealId = LastDealId.Value;
            return true;
        }

        dealId = 0;
        Log.Warning(
            "Sample '{SampleName}' requires LastDealId",
            sampleName);
        return false;
    }

    public bool TryGetContactId(string sampleName, out int contactId)
    {
        if (LastContactId.HasValue)
        {
            contactId = LastContactId.Value;
            return true;
        }

        contactId = 0;
        Log.Warning(
            "Sample '{SampleName}' requires LastContactId",
            sampleName);
        return false;
    }

    public bool TryGetContactGroupId(string sampleName, out int contactGroupId)
    {
        if (LastContactGroupId.HasValue)
        {
            contactGroupId = LastContactGroupId.Value;
            return true;
        }

        contactGroupId = 0;
        Log.Warning(
            "Sample '{SampleName}' requires LastContactGroupId",
            sampleName);
        return false;
    }
}