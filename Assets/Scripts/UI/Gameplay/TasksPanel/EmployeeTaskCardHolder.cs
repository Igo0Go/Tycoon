using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EmployeeTaskCardHolder : MonoBehaviour
{
    [SerializeField]
    private TMP_Text employeeNameText;
    [SerializeField]
    private TMP_Text currentTaskNameText;
    [SerializeField]
    private TMP_Text currentTaskProgressText;
    [SerializeField]
    private Slider currentTaskProgressSlider;
    [SerializeField]
    private UIDropQueueSlot taskContainer;

    public Employee Employee { get; private set; }

    public event Action<Employee, EmployeeTask> taskAdded;
    public event Action<Employee, EmployeeTask> taskRemoved;

    public void Init(Employee employee)
    {
        this.Employee = employee;
        employee.doTask += OnChangeTaskProgress;
        employee.taskChanged += OnChangeTask;
        taskContainer.taskAdd += AddTaskToQueue;
        taskContainer.taskRemove += RemoveTaskForthisEmployee;
        RedrawEmployeeInfo();
    }

    public void AddTaskToQueue(EmployeeTask task)
    {
        taskAdded?.Invoke(Employee, task);
    }

    public void RemoveTaskForthisEmployee(EmployeeTask task)
    {
        taskRemoved?.Invoke(Employee, task);
    }

    private void RedrawEmployeeInfo()
    {
        employeeNameText.text = Employee.Name;
        OnChangeTask();
    }

    private void OnChangeTask()
    {
        if (Employee.CurrentTask != null)
        {
            currentTaskNameText.text = Employee.CurrentTask.Name + (Employee.CurrentTask.Testing ? 
                " test[" + Employee.CurrentTask.IsCorrectTask + "]" : "");
            currentTaskProgressSlider.maxValue = Employee.CurrentTask.AllTaskTime;
            OnChangeTaskProgress();
            taskContainer.DestroyCardWithThisTask(Employee.CurrentTask);
        }
        else
        {
            currentTaskNameText.text = "Нет задачи";
            currentTaskProgressText.text = "бездельничает";
            currentTaskProgressSlider.maxValue = 1;
            currentTaskProgressSlider.value = 0;
        }
    }

    private void OnChangeTaskProgress()
    {
        currentTaskProgressText.text = "ещё" +
    (Employee.CurrentTask.AllTaskTime - Employee.CurrentTask.CompleteTaskTime) + "м.";
        currentTaskProgressSlider.value = Employee.CurrentTask.CompleteTaskTime;
    }
}
