using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class InputBuffer : MonoBehaviour
{
    private InputPerformedEventController _inputPerformedEventController;
    
    private readonly Dictionary <string, bool> _instantaneousInput = new Dictionary<string, bool>();
    private readonly Dictionary <string, float> _inputQueue = new Dictionary<string, float>();
    
    [SerializeField] private float inputLingerDuration = 0.5f;

    private void Awake()
    {
        _inputPerformedEventController = FindFirstObjectByType<InputPerformedEventController>();
    }

    private void OnEnable()
    {
        if(_inputPerformedEventController == null)
        {
            Debug.LogError("InputTest not found in the scene. Please add an InputTest component to a GameObject.",this);
            return;
        }
        _inputPerformedEventController.inputPerformedEvent.AddListener(OnInputPerformedRegisterInstantInput);
    }

    private void OnDisable()
    {
        _inputPerformedEventController.inputPerformedEvent.RemoveListener(OnInputPerformedRegisterInstantInput);
    }

    private void Update()
    {
        foreach (var key in _inputQueue.Keys)
        {
            _inputQueue[key] -= Time.deltaTime;
            if (_inputQueue[key] <= 0)
            {
                _inputQueue.Remove(key);
                Debug.Log("Input expired: " + key);
            }
        }
    }

    private void OnInputPerformedRegisterInstantInput(string inputName)
    {
        if (_inputQueue.ContainsKey(inputName))
        {
            _inputQueue.Remove(inputName);
            Debug.Log("Input removed from queue: " + inputName + "Because of instantaneous input override");
        }
        _instantaneousInput[inputName] = true;
        Debug.Log("Starting instantaneous input transition for: " + inputName);
        StartCoroutine(InstantInputTransition(inputName));
    }
    
    private IEnumerator InstantInputTransition(string inputName)
    {
        Debug.Log("Waiting for End of Frame");
        yield return new WaitForEndOfFrame();
        Debug.Log("End of Frame reached, transfering input to queue: " + inputName);
        TransferInputToQueue(inputName, inputLingerDuration);
    }
    
    private void TransferInputToQueue(string inputName, float duration)
    {
        Debug.Log("Check if input has been consumed before transfering to queue");
        if(_instantaneousInput[inputName] == true)
        {
            _instantaneousInput[inputName] = false;
            _inputQueue.Add(inputName, duration);
            Debug.Log("Input transfered to queue: " + inputName);
        }
    }

    public bool IsInputInstantaneous(string inputName)
    {
        Debug.Log("Checking if input is instantaneous: " + inputName + " - " + _instantaneousInput[inputName]);
        return _instantaneousInput[inputName];
    }
    
    public bool IsInputInQueue(string inputName)
    {
        Debug.Log("Starting Coroutine to remove input from queue next frame if in queue: " + inputName);
        StartCoroutine(OnObserveRemoveInputFromQueue(inputName));
        return _inputQueue.ContainsKey(inputName);
    }

    private IEnumerator OnObserveRemoveInputFromQueue(string inputName)
    {
        Debug.Log("Waiting for End of Frame to remove input from queue: " + inputName);
        yield return new WaitForEndOfFrame();
        Debug.Log("End of Frame reached, Checking if input is in queue: " + inputName + " - " +
                  _inputQueue.ContainsKey(inputName));
        if (_inputQueue.ContainsKey(inputName))
        {
            Debug.Log("Input is in queue, removing: " + inputName);
            _inputQueue.Remove(inputName);
        }
    }
    
    public void ConsumeInput(string inputName)
    {
        Debug.Log("Checking if input is instantaneous: " + inputName + " - " + _instantaneousInput[inputName]);
        if(_instantaneousInput[inputName] == true) 
        {
            _instantaneousInput[inputName] = false;
            Debug.Log("Input consumed: " + inputName);
        }
        else
        {
            Debug.Log("Checking if input is in queue: " + inputName + " - " + _inputQueue.ContainsKey(inputName));
            if (_inputQueue.ContainsKey(inputName))
            {
                _inputQueue.Remove(inputName);
                Debug.Log("Input consumed from queue: " + inputName);
            }
            else
            {
                Debug.Log("Input failed to consume: " + inputName);
            }
        }
    }
}