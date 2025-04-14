using UnityEngine;

public class EmployeeTask
{
    public string Name { get; private set; }
    public string Description { get; private set; }

    public int AllTaskTime { get; private set; }
    public int CompleteTaskTime { get; set; }
    public int TestingTime
    {
        get
        {
            int result = AllTaskTime / 10;
            if (result < 15)
            {
                result = 15;
            }
            return result;
        }
    }
    public int Progress => CalculateProgressPercent();

    public EmployeeTaskType Type { get; private set; }
    public bool Testing { get; set; } = false;

    public bool IsCorrectTask { get; private set; }

    public int Complexity { get; private set; }

    private Employee worker;
    private Employee tester;

    public const int defaultTaskWorkSpeedPerMinute = 1;
    public const int defaultTaskTestSpeedPerMinute = 2;

    public string GenerateErrorMessage()
    {
        return "Тестировщик " + tester.Name + " сообщает, что задача " + Name + " выполнена с ошибкой. Эта задача раньше была" +
            " отдана работнику " + worker.Name;
    }

    public EmployeeTask(EmployeeTaskInfo info)
    {
        Name = info.taskName; 
        Description = info.taskDescription;
        AllTaskTime = info.workTimeHours * 60 + info.workTimeMinutes;
        Type = info.taskType;
        Complexity = info.complexity;

        CompleteTaskTime = 0;
        IsCorrectTask = true;
    }

    public void StartThisTask(Employee worker)
    {
        this.worker = worker;
    }
    public void EndWorkForThisTask(Employee employee)
    {
        ResetProgress();

        int difficultLevel = Random.Range(1, 10) * Complexity;
        int employeeLevel = 100 - employee.Fatigue - employee.Stress / 2;

        IsCorrectTask =  employeeLevel > difficultLevel;
        Testing = true;
        employee.FatigueLevelUP(Complexity);
    }
    public void StartTestingThisTask(Employee tester)
    {
        this.tester = tester;
    }
    public bool EndTestingThisTask(Employee tester)
    {
        Testing = false;
        tester.FatigueLevelUP(Complexity/2);
        if (!IsCorrectTask)
        {
            ResetProgress();
        }

        return IsCorrectTask;
    }
    public void ResetProgress()
    {
        CompleteTaskTime = 0;
    }

    public bool IsReady()
    {
        if (Testing)
        {
            return CompleteTaskTime >= TestingTime;
        }
        else
        {
            return CompleteTaskTime >= AllTaskTime;
        }
    }

    private int CalculateProgressPercent()
    {
        if(Testing)
        {
            return (int)Mathf.Clamp(Mathf.Round(CompleteTaskTime / TestingTime * 100), 0, 100);
        }
        else
        {
            return (int)Mathf.Clamp(Mathf.Round(CompleteTaskTime / AllTaskTime * 100), 0, 100);
        }
    }
}
