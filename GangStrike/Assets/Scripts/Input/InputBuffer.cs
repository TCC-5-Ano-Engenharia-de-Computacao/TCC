using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Input;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class InputBuffer : MonoBehaviour
{
    private InputController _inputController;
    
    private readonly Dictionary <string, bool> _instantaneousInput = new Dictionary<string, bool>();
    private readonly Dictionary <string, float> _inputQueue = new Dictionary<string, float>();
    
    [SerializeField] private float inputLingerDuration = 0.5f;

    private void Awake()
    {
        _inputController = FindFirstObjectByType<InputController>();
    }

    private void OnEnable()
    {
        if(_inputController == null)
        {
            Debug.LogError("InputTest not found in the scene. Please add an InputTest component to a GameObject.",this);
            return;
        }
        _inputController.inputPerformedEvent.AddListener(OnInputPerformedRegisterInstantInput);
    }

    private void OnDisable()
    {
        _inputController.inputPerformedEvent.RemoveListener(OnInputPerformedRegisterInstantInput);
    }

    /*private void LateUpdate()
    {
        float a = 0;
        for (int i = 0; i < 1000000; i++)
        {
            a += math.sqrt(i);
        }
    }*/

    private void Update()
    {
        foreach (var key in _inputQueue.Keys.ToList())
        {
            _inputQueue[key] -= Time.deltaTime;
            if (_inputQueue[key] <= 0)
            {
                // Debug.Log("Input expired: " + key);
                // print(DateTime.Now.Millisecond);
                _inputQueue.Remove(key);
            }
        }
    }

    private void OnInputPerformedRegisterInstantInput(string inputName)
    {
        if (_inputQueue.ContainsKey(inputName))
        {
            _inputQueue.Remove(inputName);
            // Debug.Log("Input removed from queue: " + inputName + " Because of instantaneous input override");
            // print(DateTime.Now.Millisecond);

        }
        _instantaneousInput[inputName] = true;
        // Debug.Log("Starting instantaneous input transition for: " + inputName);
        // print(DateTime.Now.Millisecond);

        StartCoroutine(InputTransitionCoroutine(inputName));
    }
    
    private IEnumerator InputTransitionCoroutine(string inputName)
    {
        // Debug.Log($"start Waiting for End of Frame ...: {DateTime.Now.Millisecond}");
        yield return new WaitForEndOfFrame();
        // print($"finish waiting for the end of frame: {DateTime.Now.Millisecond}");
        
        if(_instantaneousInput[inputName] == true)
        {
            _instantaneousInput[inputName] = false;
            _inputQueue.Add(inputName, inputLingerDuration);
            // Debug.Log("Input transfered to queue: " + inputName);
            // print(DateTime.Now.Millisecond);

        }
    }
   

    public bool IsInputInstantaneous(string inputName)
    {
        // Debug.Log("Checking if input is instantaneous: " + inputName + " - " + _instantaneousInput[inputName]);
        return _instantaneousInput[inputName];
    }
    
    public bool IsInputInQueue(string inputName)
    {
        // Debug.Log("Starting Coroutine to remove input from queue next frame if in queue: " + inputName);
        StartCoroutine(OnObserveRemoveInputFromQueue(inputName));
        return _inputQueue.ContainsKey(inputName);
    }

    private IEnumerator OnObserveRemoveInputFromQueue(string inputName)
    {
        // Debug.Log("Waiting for End of Frame to remove input from queue: " + inputName);
        yield return new WaitForEndOfFrame();
        // Debug.Log("End of Frame reached, Checking if input is in queue: " + inputName + " - " +_inputQueue.ContainsKey(inputName));
        if (_inputQueue.ContainsKey(inputName))
        {
            // Debug.Log("Input is in queue, removing: " + inputName);
            _inputQueue.Remove(inputName);
            // print(DateTime.Now.Millisecond);
        }
    }
    
    public void ConsumeInput(string inputName)
    {
        // Debug.Log("Checking if input is instantaneous: " + inputName + " - " + _instantaneousInput[inputName]);
        if(_instantaneousInput[inputName] == true) 
        {
            _instantaneousInput[inputName] = false;
            // Debug.Log("Input consumed: " + inputName);
        }
        else
        {
            // Debug.Log("Checking if input is in queue: " + inputName + " - " + _inputQueue.ContainsKey(inputName));
            if (_inputQueue.ContainsKey(inputName))
            {
                _inputQueue.Remove(inputName);
                // Debug.Log("Input consumed from queue: " + inputName);
            }
            else
            {
                // Debug.Log("Input failed to consume: " + inputName);
            }
        }
    }
}