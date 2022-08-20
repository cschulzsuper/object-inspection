using Super.Paula.RuntimeData;
using Super.Paula.Shared;
using Super.Paula.Shared.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Super.Paula.Application.Orchestration;

public class WorkerRuntimeManager : IWorkerRuntimeManager
{
    private readonly IRuntimeCache<WorkerRuntime> _workerRuntimeCache;

    private readonly IDictionary<string, string[]> _stateMachine = new Dictionary<string, string[]>
    {
        ["starting"] = new string[] { "running", "failed", "finished" },
        ["running"] = new string[] { "running", "failed", "finished" },
        ["failed"] = new string[] { "starting" },
        ["finished"] = new string[] { "starting" },
    };

    public WorkerRuntimeManager(IRuntimeCache<WorkerRuntime> workerRuntimeCache)
    {
        _workerRuntimeCache = workerRuntimeCache;
    }

    public bool TrySetState(string worker, string state)
    {
        EnsureSetable(state);

        var successful = true;

        _workerRuntimeCache
            .CreateOrUpdate(
                () => new WorkerRuntime
                {
                    Worker = worker,
                    State = state
                },
                workerRuntime =>
                {
                    if (!_stateMachine[workerRuntime.State].Contains(state))
                    {
                        successful = false;
                    }
                    else
                    {
                        var globalNow = DateTimeNumbers.GlobalNow;

                        workerRuntime.State = state;
                        workerRuntime.HeartbeatDate = globalNow.Date;
                        workerRuntime.HeartbeatTime = globalNow.Time;
                    }
                },
                worker);

        return successful;
    }

    public string GetState(string worker)
    {
        var workerRuntime = _workerRuntimeCache.GetOrDefault(worker);

        if (workerRuntime == null)
        {
            return string.Empty;
        }

        var workerRuntimeTimestamp =
            new DateTimeNumbers(
                workerRuntime.HeartbeatDate,
                workerRuntime.HeartbeatTime)
            .ToGlobalDateTime();

        var tenSecondsAgo = DateTime.UtcNow.AddSeconds(-10);

        if (workerRuntimeTimestamp < tenSecondsAgo)
        {
            return string.Empty;
        }

        return workerRuntime.State;
    }

    private static void EnsureSetable(string state)
        => Validator.Ensure($"worker state '{state}'",
            WorkerRuntimeValidator.StateIsNotNull(state),
            WorkerRuntimeValidator.StateHasValidValue(state));


}