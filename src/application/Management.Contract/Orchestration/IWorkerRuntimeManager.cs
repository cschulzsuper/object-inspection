﻿namespace ChristianSchulz.ObjectInspection.Application.Orchestration;

public interface IWorkerRuntimeManager
{
    bool TrySetState(string worker, string state);

    string GetState(string worker);
}