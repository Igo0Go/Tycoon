using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Employee
{
    [SerializeField]
    private string name;
    [Min(1)]
    [SerializeField]
    private float baseSalary;
    [Range(0,2)]
    [SerializeField]
    private float overtimeSalaryMultiplier = 2;
    [Range(0, 2)]
    [SerializeField]
    private float hospitalSalaryMultiplier = 0.5f;

    public string Name => name;
    public float BaseSalary => baseSalary;
    public float OvertimeSalaryMultiplier => overtimeSalaryMultiplier;
    public float HospitalSalaryMultiplier => hospitalSalaryMultiplier;

    public string State
    {
        get
        {
            if (!OverTime && !workTime)
            {
                return "Дома";
            }

            return status.StateName;
        }
    }

    public bool OverTime => status.Overtime;
    public bool IsActive => workTime && status.IsActive;
    private bool workTime;

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
    public event Action doTask;
    public event Action taskChanged;

    public Employee(string name, float baseSalary, float overtimeSalaryMultiplier, float hospitalSalaryMultiplier)
    {
        this.name = name;
        this.baseSalary = baseSalary;
        this.overtimeSalaryMultiplier = overtimeSalaryMultiplier;
        this.hospitalSalaryMultiplier = hospitalSalaryMultiplier;

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
            workTime = true;
            employeeChanged?.Invoke();
        }
    }
    public void StopWorkTime()
    {
        workTime = false;
        employeeChanged?.Invoke();
    }
    public void PlusSalary(float value)
    {
        baseSalary += value;
        Stress--;
        employeeChanged?.Invoke();
    }
    public void MinusSalary(float value)
    {
        baseSalary -= value;
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
        if(!status.IsActive || !workTime)
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
    public bool EmployeeInTheTeam()
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
}
