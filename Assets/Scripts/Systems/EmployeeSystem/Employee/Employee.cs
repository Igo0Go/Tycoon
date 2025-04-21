using System;

[Serializable]
public class Employee
{
    #region Базовые характеристики

    public string Name { get; private set; }
    public float BaseSalary { get; private set; }
    public float CostOfAttracting { get; private set; }
    public EmployeeStatsPack EmployeeStatsPack { get; private set; }
    public EmployeeSpeachPack EmployeeSpeachPack { get; private set; }

    #endregion

    #region Изменяемые характеристики

    /// <summary>
    /// Нервозность сотрудника
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
    /// Усталость сотрудника
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

    #region Статусы


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

    public bool OverTime { get; private set; }

    public bool IsActive => _dayState == EmployeeDayState.work;
    public bool IsDoTask => IsActive && CurrentTask != null;


    #endregion

    #region События
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

    #region Информация о сотруднике


    /// <summary>
    /// Получить размер заработной платы с учётом текущей стратегии
    /// </summary>
    /// <returns>Размер заработной платы с копейками</returns>
    public float GetSalary()
    {
        return (float)Math.Round(salaryStrategy.CalculateSalary(this), 2);
    }
    /// <summary>
    /// Получить текущую информацию о заработной плате с учётом стратегии
    /// </summary>
    /// <returns>Строка: сколько и за что получает работник</returns>
    public string GetSalaryInfo()
    {
        return salaryStrategy.GetSalaryInfo(this);
    }

    /// <summary>
    /// Назначить сотруднику состояние сверхурочной работы или вывести из него, также изменяя стратегию оплаты
    /// </summary>
    /// <param name="value">значение - должен ли сотрудник работать сверхурочно</param>
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
    /// Установить базовую стратегию выплаты зп
    /// </summary>
    public void SetBaseSalaryStatus()
    {
        salaryStrategy = SalaryStrategySingleton.BaseSalaryStatus;
        EmployeeInfoChanged?.Invoke();
    }
    /// <summary>
    /// Установить стратегию выплаты больничных
    /// </summary>
    public void SetHospitalSalaryStatus()
    {
        salaryStrategy = SalaryStrategySingleton.HospitalSalaryStatus;
        EmployeeInfoChanged?.Invoke();
    }
    /// <summary>
    /// Установить стратегию выплаты по правилам сверхурочных
    /// </summary>
    public void SetOvertimeSalaryStatus()
    {
        salaryStrategy = SalaryStrategySingleton.OvertimeSalaryStatus;
        EmployeeInfoChanged?.Invoke();
    }

    /// <summary>
    /// Увеличить ЗП на указанное значение
    /// </summary>
    /// <param name="value">прибавка к ЗП с учётом копеек</param>
    public void PlusSalary(float value)
    {
        BaseSalary += value;
        Stress--;
        EmployeeInfoChanged?.Invoke();
    }
    /// <summary>
    /// Понизить ЗП
    /// </summary>
    /// <param name="value">Урезание ЗП с учётом копеек</param>
    public void MinusSalary(float value)
    {
        BaseSalary -= value;
        Stress += 2;
        EmployeeInfoChanged?.Invoke();
    }

    /// <summary>
    /// Увеличить уровень усталости на указанное число единиц
    /// </summary>
    /// <param name="fatiguePoints">количество пунктов усталости. К ним потом применятся показатели восприимчивости
    ///  усталости самого сотрудника</param>
    public void FatigueLevelUP(int fatiguePoints)
    {
        Fatigue += (fatiguePoints + EmployeeStatsPack.addingFatigueGrowthPoints) * 
            EmployeeStatsSettings.baseFatigueGrowthSpeed ;
    }
    /// <summary>
    /// уменьшить уровень усталости на указанное число единиц
    /// </summary>
    /// <param name="fatiguePoints">количество пунктов усталости. К ним потом применятся показатели восприимчивости
    ///  усталости самого сотрудника</param>
    public void FatigueLevelLower(int fatiguePoints)
    {
        Fatigue -= (fatiguePoints + EmployeeStatsPack.addingFatigueLoweringPoints) * 
            EmployeeStatsSettings.baseFatigueLoweringSpeed;
    }

    /// <summary>
    /// Увеличить уровень стресса на указанное число единиц
    /// </summary>
    /// <param name="stressPoints">количество пунктов усталости. К ним потом применятся показатели восприимчивости
    ///  усталости самого сотрудника</param>
    public void StressLevelUP(int stressPoints)
    {
        Stress += (stressPoints + EmployeeStatsPack.addingStressGrowthPoints) *
            EmployeeStatsSettings.baseStreesGrowthSpeed;
    }
    /// <summary>
    /// уменьшить уровень усталости на указанное число единиц
    /// </summary>
    /// <param name="stressPoints">количество пунктов усталости. К ним потом применятся показатели восприимчивости
    ///  усталости самого сотрудника</param>
    public void StressLevelLower(int stressPoints)
    {
        Stress -= (stressPoints + EmployeeStatsPack.addingStressLoweringPoints) *
            EmployeeStatsSettings.baseStreesLoweringSpeed;
    }

    /// <summary>
    /// Подвести итоги дня - сместить характеристики в соответствии с тем, сколько отдохнул человек
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
    /// Проверить, выйдет ли человек на работу в старте дня.
    /// </summary>
    /// <returns>Истина - выйдет, ложт - нет. Человек не выйдет, если стресс на критической отметке. 
    /// Он уволится. Человек пропустит день, если стресс на критической отметке
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

    #region Действия сотрудника

    /// <summary>
    /// Нанять сотрудника. Он скажет приветственную фразу
    /// </summary>
    public void Recruting()
    {
        GameUICenter.messageQueue.PrepareMessage(EmployeeSpeachPack.recrutingSpeach.Header,
            EmployeeSpeachPack.recrutingSpeach.Message);
        EmployeeRecruting?.Invoke(this);
    }

    /// <summary>
    /// Отправить сотрудника работать
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
    /// Отправить сотрудника на обед и понизить его стресс и усталость на заданные характеристики
    /// </summary>
    public void GoToLunch()
    {
        FatigueLevelLower(EmployeeStatsSettings.lunchFatigueLowering);
        StressLevelLower(EmployeeStatsSettings.lunchStreesLowering);
        _dayState = EmployeeDayState.lunch;
        EmployeeInfoChanged?.Invoke();
    }

    /// <summary>
    /// Отправить сотрудника домой
    /// </summary>
    public void GoHome()
    {
        _dayState = EmployeeDayState.home;
        EmployeeInfoChanged?.Invoke();
    }

    /// <summary>
    /// Сотрудник будет выполнять работу дальше, если активен и есть задача. 
    /// К этому будет применяться скорость работы сотрудник с определённым типом задач. 
    /// Сотрудник будет уставать настолько быстро, насколько сложная задача
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
