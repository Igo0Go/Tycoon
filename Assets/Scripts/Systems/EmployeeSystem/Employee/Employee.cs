using System;

[Serializable]
public class Employee
{
    #region ������� ��������������

    public string Name { get; private set; }
    public float BaseSalary { get; private set; }
    public float CostOfAttracting { get; private set; }
    public EmployeeStatsPack EmployeeStatsPack { get; private set; }
    public EmployeeSpeachPack EmployeeSpeachPack { get; private set; }

    #endregion

    #region ���������� ��������������

    /// <summary>
    /// ����������� ����������
    /// </summary>
    public int Stress
    {
        get
        {
            return _stress;
        }
        private set
        {
            _stress = Math.Clamp(value, 0, EmployeeStatsPack.stressThresholdValue);
            EmployeeInfoChanged?.Invoke();
        }
    }
    private int _stress;

    /// <summary>
    /// ��������� ����������
    /// </summary>
    public int Fatigue
    {
        get
        {
            return _fatigue;
        }
        private set
        {
            _fatigue = Math.Clamp(value, 0, EmployeeStatsPack.fatigueThresholdValue);
            EmployeeInfoChanged?.Invoke();
        }
    }
    private int _fatigue;

    public string SalaryStrategyName => salaryStrategy.StateName;
    private ISalaryStrategy salaryStrategy = SalaryStrategySingleton.BaseSalaryStatus;

    public int ExperienceHours => workMinutes / 60;
    private int workMinutes = 0;

    public EmployeeTask CurrentTask
    {
        get => _currentTask;
        set
        {
            _currentTask = value;
            TaskChanged?.Invoke();
        }
    }
    private EmployeeTask _currentTask;

    #endregion

    #region �������


    public string DayState
    {
        get
        {
            return _dayState switch
            {
                EmployeeDayState.work => "��������",
                EmployeeDayState.lunch => "����",
                EmployeeDayState.home => "����",
                _ => "����",
            };
        }
    }
    private EmployeeDayState _dayState;

    public bool OverTime { get; private set; }

    public bool IsActive => _dayState == EmployeeDayState.work;
    public bool IsDoTask => IsActive && CurrentTask != null;


    #endregion

    #region �������
    public event Action EmployeeInfoChanged;
    public event Action<Employee> EmployeeMaxStress;
    public event Action<Employee> EmployeeMaxFatigue;
    public event Action<Employee> EmployeeRecruting;
    public event Action DoTask;
    public event Action TaskChanged;
    #endregion

    public Employee(string name, float baseSalary, float costOfAttractiong, int experienceInHour,
        EmployeeStatsPack statsPack, EmployeeSpeachPack speachPack)
    {
        Name = name;
        BaseSalary = baseSalary;
        CostOfAttracting = costOfAttractiong;
        workMinutes = experienceInHour * 60;

        EmployeeStatsPack = (EmployeeStatsPack)statsPack.Clone();
        EmployeeSpeachPack = speachPack;
    }

    #region ���������� � ����������


    /// <summary>
    /// �������� ������ ���������� ����� � ������ ������� ���������
    /// </summary>
    /// <returns>������ ���������� ����� � ���������</returns>
    public float GetSalary()
    {
        return (float)Math.Round(salaryStrategy.CalculateSalary(this), 2);
    }
    /// <summary>
    /// �������� ������� ���������� � ���������� ����� � ������ ���������
    /// </summary>
    /// <returns>������: ������� � �� ��� �������� ��������</returns>
    public string GetSalaryInfo()
    {
        return salaryStrategy.GetSalaryInfo(this);
    }

    /// <summary>
    /// ��������� ���������� ��������� ������������ ������ ��� ������� �� ����, ����� ������� ��������� ������
    /// </summary>
    /// <param name="value">�������� - ������ �� ��������� �������� �����������</param>
    public void SetOvertimeState(bool value)
    {
        if (value)
        {
            SetOvertimeSalaryStatus();
            OverTime = true;
        }
        else
        {
            SetBaseSalaryStatus();
            OverTime = false;
        }
    }

    /// <summary>
    /// ���������� ������� ��������� ������� ��
    /// </summary>
    public void SetBaseSalaryStatus()
    {
        salaryStrategy = SalaryStrategySingleton.BaseSalaryStatus;
        EmployeeInfoChanged?.Invoke();
    }
    /// <summary>
    /// ���������� ��������� ������� ����������
    /// </summary>
    public void SetHospitalSalaryStatus()
    {
        salaryStrategy = SalaryStrategySingleton.HospitalSalaryStatus;
        EmployeeInfoChanged?.Invoke();
    }
    /// <summary>
    /// ���������� ��������� ������� �� �������� ������������
    /// </summary>
    public void SetOvertimeSalaryStatus()
    {
        salaryStrategy = SalaryStrategySingleton.OvertimeSalaryStatus;
        EmployeeInfoChanged?.Invoke();
    }

    /// <summary>
    /// ��������� �� �� ��������� ��������
    /// </summary>
    /// <param name="value">�������� � �� � ������ ������</param>
    public void PlusSalary(float value)
    {
        BaseSalary += value;
        Stress--;
        EmployeeInfoChanged?.Invoke();
    }
    /// <summary>
    /// �������� ��
    /// </summary>
    /// <param name="value">�������� �� � ������ ������</param>
    public void MinusSalary(float value)
    {
        BaseSalary -= value;
        Stress += 2;
        EmployeeInfoChanged?.Invoke();
    }

    /// <summary>
    /// ��������� ������� ��������� �� ��������� ����� ������
    /// </summary>
    /// <param name="fatiguePoints">���������� ������� ���������. � ��� ����� ���������� ���������� ���������������
    ///  ��������� ������ ����������</param>
    public void FatigueLevelUP(int fatiguePoints)
    {
        Fatigue += (fatiguePoints + EmployeeStatsPack.addingFatigueGrowthPoints) * 
            EmployeeStatsSettings.baseFatigueGrowthSpeed ;
    }
    /// <summary>
    /// ��������� ������� ��������� �� ��������� ����� ������
    /// </summary>
    /// <param name="fatiguePoints">���������� ������� ���������. � ��� ����� ���������� ���������� ���������������
    ///  ��������� ������ ����������</param>
    public void FatigueLevelLower(int fatiguePoints)
    {
        Fatigue -= (fatiguePoints + EmployeeStatsPack.addingFatigueLoweringPoints) * 
            EmployeeStatsSettings.baseFatigueLoweringSpeed;
    }

    /// <summary>
    /// ��������� ������� ������� �� ��������� ����� ������
    /// </summary>
    /// <param name="stressPoints">���������� ������� ���������. � ��� ����� ���������� ���������� ���������������
    ///  ��������� ������ ����������</param>
    public void StressLevelUP(int stressPoints)
    {
        Stress += (stressPoints + EmployeeStatsPack.addingStressGrowthPoints) *
            EmployeeStatsSettings.baseStreesGrowthSpeed;
    }
    /// <summary>
    /// ��������� ������� ��������� �� ��������� ����� ������
    /// </summary>
    /// <param name="stressPoints">���������� ������� ���������. � ��� ����� ���������� ���������� ���������������
    ///  ��������� ������ ����������</param>
    public void StressLevelLower(int stressPoints)
    {
        Stress -= (stressPoints + EmployeeStatsPack.addingStressLoweringPoints) *
            EmployeeStatsSettings.baseStreesLoweringSpeed;
    }

    /// <summary>
    /// �������� ����� ��� - �������� �������������� � ������������ � ���, ������� �������� �������
    /// </summary>
    public void EndDayResult()
    {
        FatigueLevelUP(EmployeeStatsSettings.dailyFatigueGrowth);
        StressLevelUP(EmployeeStatsSettings.dailyStreesGrowth);

        if (OverTime)
        {
            FatigueLevelLower(EmployeeStatsPack.fatigueThresholdValue / EmployeeStatsSettings.dailyFatigueOvertimeLowering);
            StressLevelUP(EmployeeStatsSettings.dailyStreesOvertimeGrowth);
        }
        else
        {
            FatigueLevelLower(EmployeeStatsPack.fatigueThresholdValue / EmployeeStatsSettings.dailyFatigueLowering);
            StressLevelLower(EmployeeStatsSettings.dailyStreesLowering);
        }
    }
    /// <summary>
    /// ���������, ������ �� ������� �� ������ � ������ ���.
    /// </summary>
    /// <returns>������ - ������, ���� - ���. ������� �� ������, ���� ������ �� ����������� �������. 
    /// �� ��������. ������� ��������� ����, ���� ������ �� ����������� �������
    /// </returns>
    public bool EmployeeMaxStatsCheck()
    {
        if (Stress >= EmployeeStatsPack.GetThresholdStress())
        {
            EmployeeMaxStress?.Invoke(this);
            return false;
        }
        if (Fatigue >= EmployeeStatsPack.GetThresholdFatigue())
        {
            EmployeeMaxFatigue?.Invoke(this);
        }
        return true;
    }
    #endregion

    #region �������� ����������

    /// <summary>
    /// ������ ����������. �� ������ �������������� �����
    /// </summary>
    public void Recruting()
    {
        GameUICenter.messageQueue.PrepareMessage(EmployeeSpeachPack.recrutingSpeach.Header,
            EmployeeSpeachPack.recrutingSpeach.Message);
        EmployeeRecruting?.Invoke(this);
    }

    /// <summary>
    /// ��������� ���������� ��������
    /// </summary>
    public void GoToWork()
    {
        if (Fatigue < EmployeeStatsPack.GetThresholdFatigue())
        {
            _dayState = EmployeeDayState.work;
            EmployeeInfoChanged?.Invoke();
        }
    }

    /// <summary>
    /// ��������� ���������� �� ���� � �������� ��� ������ � ��������� �� �������� ��������������
    /// </summary>
    public void GoToLunch()
    {
        FatigueLevelLower(EmployeeStatsSettings.lunchFatigueLowering);
        StressLevelLower(EmployeeStatsSettings.lunchStreesLowering);
        _dayState = EmployeeDayState.lunch;
        EmployeeInfoChanged?.Invoke();
    }

    /// <summary>
    /// ��������� ���������� �����
    /// </summary>
    public void GoHome()
    {
        _dayState = EmployeeDayState.home;
        EmployeeInfoChanged?.Invoke();
    }

    /// <summary>
    /// ��������� ����� ��������� ������ ������, ���� ������� � ���� ������. 
    /// � ����� ����� ����������� �������� ������ ��������� � ����������� ����� �����. 
    /// ��������� ����� �������� ��������� ������, ��������� ������� ������
    /// </summary>
    public void DoWork()
    {
        if (IsDoTask)
        {
            int workSpeed;
            if (CurrentTask.Type == EmployeeTaskType.Testing)
            {
                workSpeed = EmployeeTask.defaultTaskTestSpeedPerMinute + EmployeeStatsPack.GetTaskSpeed(EmployeeTaskType.Testing);
            }
            else
            {
                workSpeed = EmployeeTask.defaultTaskWorkSpeedPerMinute + EmployeeStatsPack.GetTaskSpeed(CurrentTask.Type);
            }
            workMinutes++;
            CurrentTask.CompleteTaskTime += workSpeed;

            if(CurrentTask.CompleteTaskTime % 60 == 0)
            {
                FatigueLevelUP(CurrentTask.Complexity);
            }
            DoTask?.Invoke();
        }
    }

    #endregion
}

public enum EmployeeDayState
{
    home,
    work,
    lunch
}
