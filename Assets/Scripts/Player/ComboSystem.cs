using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboSystem
{
    private List<ComboInput> inputs = new List<ComboInput>();
    private float bufferTime = 1f;
    private float timer;

    public void SetBufferTime(float time)
    {
        bufferTime = time;
    }

    public void AddInput(ComboInput input)
    {
        inputs.Add(input);
        timer = bufferTime;
    }

    public void UpdateTimer()
    {
        if (inputs.Count == 0) return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            inputs.Clear();
        }
    }

    public bool Match(params ComboInput[] sequence)
    {
        if (inputs.Count < sequence.Length)
            return false;

        for (int i = 0; i < sequence.Length; i++)
        {
            if (inputs[inputs.Count - sequence.Length + i] != sequence[i])
                return false;
        }

        return true;
    }

    public void Clear()
    {
        inputs.Clear();
    }

    public string GetBufferDebug()
    {
        if (inputs.Count == 0)
            return "Empty";

        string result = "";
        for (int i = 0; i < inputs.Count; i++)
        {
            result += inputs[i].ToString();

            if (i < inputs.Count - 1)
                result += " → ";
        }

        return result;
    }
    public int BufferCount()
    {
        return inputs.Count;
    }
    public bool Contains(ComboInput input)
    {
        return inputs.Contains(input);
    }
    public bool MatchIgnoring(ComboInput ignore, params ComboInput[] sequence)
    {
        // Build a filtered list (ignore Movement etc)
        List<ComboInput> filtered = new List<ComboInput>();
        for (int i = 0; i < inputs.Count; i++)
        {
            if (inputs[i] != ignore)
                filtered.Add(inputs[i]);
        }

        if (filtered.Count < sequence.Length)
            return false;

        // compare last N
        for (int i = 0; i < sequence.Length; i++)
        {
            if (filtered[filtered.Count - sequence.Length + i] != sequence[i])
                return false;
        }

        return true;
    }
    public float GetTimer()
    {
        return timer;
    }

    public bool HasInputs()
    {
        return inputs.Count > 0;
    }

}
