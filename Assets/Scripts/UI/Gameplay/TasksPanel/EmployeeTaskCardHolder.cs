using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EmployeeTaskCardHolder : TooltipElement
{
    [SerializeField]
    private TMP_Text employeeNameText;
    [SerializeField]
    private TMP_Text employeeExperienceText;
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
        employee.DoTask += OnChangeTaskProgress;
        employee.TaskChanged += OnChangeTask;
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
        employeeExperienceText.text = "���� ������: " + Employee.ExperienceHours;
        OnChangeTask();
    }

    private void OnChangeTask()
    {
        if (Employee.CurrentTask != null)
        {
            currentTaskNameText.text = Employee.CurrentTask.Name + (Employee.CurrentTask.Testing ? 
                " T[" + (Employee.CurrentTask.IsCorrectTask? "+":"x") + "]" : "");

            currentTaskProgressSlider.maxValue = Employee.CurrentTask.Testing? 
                Employee.CurrentTask.TestingTime : Employee.CurrentTask.AllTaskTime;

            OnChangeTaskProgress();
            taskContainer.DestroyCardWithThisTask(Employee.CurrentTask);
        }
        else
        {
            currentTaskNameText.text = "��� ������";
            currentTaskProgressText.text = "�������������";
            currentTaskProgressSlider.maxValue = 1;
            currentTaskProgressSlider.value = 0;
        }
    }

    private void OnChangeTaskProgress()
    {
        employeeExperienceText.text = "���� ������: " + Employee.ExperienceHours;

        int totalTime = 0;

        if(Employee.CurrentTask.Testing)
        {
            totalTime = Employee.CurrentTask.TestingTime - Employee.CurrentTask.CompleteTaskTime;
        }
        else
        {
            totalTime = Employee.CurrentTask.AllTaskTime - Employee.CurrentTask.CompleteTaskTime;
        }


        int hour = totalTime / 60;
        int minute = totalTime % 60;

        currentTaskProgressText.text = "��� " + hour + "�. " + minute + "�.";
        currentTaskProgressSlider.value = Employee.CurrentTask.CompleteTaskTime;
    }

    protected override string GetStringForTooltip()
    {
        string s = Employee.Name + "\n";
        s += "���� ������: " + Employee.ExperienceHours + "\n";
        s += "���������: " + Employee.Fatigue + "\n";
        s += "������: " + Employee.Stress + "\n";
        s += Employee.DayState + "\n";
        s += "\n";
        s += "������: " + Employee.GetSalaryInfo() + "\n";
        s += "\n";
        if (Employee.CurrentTask != null)
        {
            s += "������� ������: " + Employee.CurrentTask.Name + "\n";
            s += Employee.CurrentTask.Description + "\n";
            if(Employee.CurrentTask.Testing)
            {
                s += "������������";
            }
        }
        return s;
    }
}
