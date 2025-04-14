using System;
using System.Collections.Generic;

[Serializable]
public class Employee
{
    public string Name { get; private set; }
    public float BaseSalary { get; private set; }

    public string DissmissSpeach { get; private set; }

    public float CostOfAttracting { get; private set; }

    public string WorkExperience => (workMinutes / 60) + "ч.";
    private int workMinutes = 0;

    public const float overtimeSalaryMultiplier = 2;
    public float hospitalSalaryMultiplier = 0.5f;

    public string SalaryState => status.StateName;
    public string DayState
    {
        get
        {
            return _dayState switch
            {
                EmployeeDayState.work => "Работает",
                EmployeeDayState.lunch => "Обед",
                EmployeeDayState.home => "Дома",
                _ => "Дома",
            };
        }
    }

    private EmployeeDayState _dayState;

    public bool OverTime => status.Overtime;
    public bool IsActive => _dayState == EmployeeDayState.work && status.IsActive;

    /// <summary>
    /// Нервозность сотрудника
    /// </summary>
    public int Stress
    {
        get
        {
            return _stress;
        }
        set
        {
            _stress = Math.Clamp(value, 0, 100);
            employeeChanged?.Invoke();
        }
    }
    private int _stress;

    /// <summary>
    /// Усталость сотрудника
    /// </summary>
    public int Fatigue
    {
        get
        {
            return _fatigue;
        }
        set
        {
            _fatigue = Math.Clamp(value, 0, 200);
            employeeChanged?.Invoke();
        }
    }
    private int _fatigue;

    private const int fatigueThresholdValue = 90;

    private int stressMultiplier = 1;
    private int fatigueMultiplier = 1;

    private SalaryStatus status = SalaryStatusSingleton.baseSalaryStatus;

    public EmployeeTask CurrentTask
    {
        get => _currentTask;
        set
        {
            _currentTask = value;
            taskChanged?.Invoke();
        }
    }
    private EmployeeTask _currentTask;
    public Dictionary<EmployeeTaskType, int> employeeTaskSpeed;

    public event Action employeeChanged;
    public event Action<Employee> employeeMaxStress;
    public event Action<Employee> employeeMaxFatigue;
    public event Action<Employee> employeeRecruting;
    public event Action doTask;
    public event Action taskChanged;

    public Employee(string name, float baseSalary, float costOfAttractiong, string dissmissSpeach)
    {
        Name = name;
        BaseSalary = baseSalary;
        CostOfAttracting = costOfAttractiong;
        DissmissSpeach = dissmissSpeach;

        employeeTaskSpeed = new Dictionary<EmployeeTaskType, int>();
        employeeTaskSpeed.Add(EmployeeTaskType.Code, 0);
        employeeTaskSpeed.Add(EmployeeTaskType.Docs, 0);
        employeeTaskSpeed.Add(EmployeeTaskType.Testing, 0);
    }

    public float GetSalary()
    {
        return (float)Math.Round(status.CalculateSalary(this), 2);
    }
    public string GetSalaryInfo()
    {
        return status.GetSalaryInfo(this);
    }

    public void SetBaseSalaryStatus()
    {
        status = SalaryStatusSingleton.baseSalaryStatus;
        employeeChanged?.Invoke();
    }
    public void SetHospitalSalaryStatus()
    {
        status = SalaryStatusSingleton.hospitalSalaryStatus;
        employeeChanged?.Invoke();
    }
    public void SetOvertimeSalaryStatus()
    {
        status = SalaryStatusSingleton.overtimeSalaryStatus;
        employeeChanged?.Invoke();
    }

    public void ToWork()
    {
        if(Fatigue < fatigueThresholdValue)
        {
            _dayState = EmployeeDayState.work;
            employeeChanged?.Invoke();
        }
    }
    public void ToLunch()
    {
        _dayState = EmployeeDayState.lunch;
        employeeChanged?.Invoke();
    }
    public void GoHome()
    {
        _dayState = EmployeeDayState.home;
        employeeChanged?.Invoke();
    }



    public void PlusSalary(float value)
    {
        BaseSalary += value;
        Stress--;
        employeeChanged?.Invoke();
    }
    public void MinusSalary(float value)
    {
        BaseSalary -= value;
        Stress += 2;
        employeeChanged?.Invoke();
    }

    public void SetCharacteristicMultiplier(int stress, int fatigue)
    {
        stressMultiplier = stress;
        fatigueMultiplier = fatigue;
    }

    public void DoWork()
    {
        if(!status.IsActive || !(_dayState == EmployeeDayState.work))
        {
            return;
        }

        if(CurrentTask != null)
        {
            int workSpeed = 0;
            if (CurrentTask.Type == EmployeeTaskType.Testing)
            {
                workSpeed = EmployeeTask.defaultTaskTestSpeedPerMinute + employeeTaskSpeed[EmployeeTaskType.Testing];
            }
            else
            {
                workSpeed = EmployeeTask.defaultTaskWorkSpeedPerMinute + employeeTaskSpeed[CurrentTask.Type];
            }
            workMinutes ++;
            CurrentTask.CompleteTaskTime += workSpeed;
            doTask?.Invoke();
        }
    }
    public void FatigueLevelUP(int fatiguePoints)
    {
        Fatigue += fatiguePoints * fatigueMultiplier;
    }

    public void EndDayResult()
    {
        Fatigue += 10 * fatigueMultiplier;
        Stress += 5 * stressMultiplier;
        if(OverTime)
        {
            Fatigue -= 5 * fatigueMultiplier;
            Stress += 5 * stressMultiplier;
        }
        else
        {
            Fatigue -= 15 * fatigueMultiplier;
            Stress -= 7 * stressMultiplier;
        }
    }
    public bool EmployeeWantFireCheck()
    {
        if(Stress >= 100)
        {
            employeeMaxStress?.Invoke(this);
            return false;
        }
        if (Fatigue >= fatigueThresholdValue)
        {
            employeeMaxFatigue?.Invoke(this);
        }
        return true;
    }

    public void Recruting()
    {
        employeeRecruting?.Invoke(this);
    }
}

public enum EmployeeDayState
{
    home,
    work,
    lunch
}
