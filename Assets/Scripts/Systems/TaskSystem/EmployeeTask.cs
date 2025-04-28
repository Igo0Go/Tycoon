using UnityEngine;

/// <summary>
/// Класс задачи сотрудника
/// </summary>
public class EmployeeTask
{
    #region Свойства

    /// <summary>
    /// Название задачи
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Описание задачи
    /// </summary>
    public string Description { get; private set; }

    /// <summary>
    /// Всё время на выполнение задачи
    /// </summary>
    public int AllTaskTime { get; private set; }

    /// <summary>
    /// Время, которое над задачей уже отработали
    /// </summary>
    public int CompleteTaskTime { get; set; }

    /// <summary>
    /// Расчитываемое время на тестирование задачи. Обычно - десятая часть от времени работы над задачей, 
    /// но не меньше 15 минут
    /// </summary>
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

    /// <summary>
    /// Рассчитываемый процент выполнения задачи
    /// </summary>
    public int Progress => CalculateProgressPercent();

    /// <summary>
    /// Тип задачи
    /// </summary>
    public EmployeeTaskType Type { get; private set; }

    /// <summary>
    /// Это задача сейчас на тестировании
    /// </summary>
    public bool Testing { get; set; } = false;

    /// <summary>
    /// Эта задача выполнена без ошибок
    /// </summary>
    public bool IsCorrectTask { get; private set; }

    /// <summary>
    /// Сложность задачи. Влияет на накопление усталости и вероятность ошибки
    /// </summary>
    public int Complexity { get; private set; }

    /// <summary>
    /// Сотрудник, назначенный на выполнение задачи
    /// </summary>
    public Employee Worker { get; private set; }

    /// <summary>
    /// Сотрудник, назначенный на тестирование задачи
    /// </summary>
    public Employee Tester { get; private set; }

    #endregion

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

    #region Методы

    #region Пока не используется

    public string GenerateErrorMessage()
    {
        return "Тестировщик " + Tester.Name + " сообщает, что задача " + Name + " выполнена с ошибкой. Эта задача раньше была" +
            " отдана работнику " + Worker.Name;
    }

    #endregion

    /// <summary>
    /// Назначить сотрудника на эту задачу
    /// </summary>
    /// <param name="worker">Сотрудник</param>
    public void SetWorkerToThisTask(Employee worker)
    {
        Worker = worker;
    }
    /// <summary>
    /// Работник работник завершает работу с этой задачей
    /// </summary>
    public void FinalWorkForThisTask()
    {
        ResetProgress();

        //вычисляем сложность задачи
        int difficultLevel = Random.Range(1, 10) * Complexity;

        //вычисляем способность сотрудника
        int employeeLevel = EmployeeStatsSettings.baseEmployeeTaskLevel + Worker.ExperienceHours -
            (Worker.Fatigue * EmployeeStatsSettings.fatigueTaskDificultyMultiplier +
            Worker.Stress * EmployeeStatsSettings.stressTaskDificultyMultiplier);

        IsCorrectTask = employeeLevel > difficultLevel;
        Testing = true;
        Worker.FatigueLevelUP(Complexity);
    }

    /// <summary>
    /// Назначить тестировщика на эту задачу
    /// </summary>
    /// <param name="tester">Сотрудник - тестировщик</param>
    public void SetTesterToThisTask(Employee tester)
    {
        Tester = tester;
    }
    /// <summary>
    /// Выбранный сотрудник завершает тестирование этой 
    /// </summary>
    /// <returns>Закончена ли эта задача без ошибок</returns>
    public bool FinalTestingThisTask()
    {
        Testing = false;
        Tester.FatigueLevelUP(Complexity / 2);
        if (!IsCorrectTask)
        {
            ResetProgress();
        }

        return IsCorrectTask;
    }

    /// <summary>
    /// Сбросить прогресс по этой задаче
    /// </summary>
    public void ResetProgress()
    {
        CompleteTaskTime = 0;
    }

    /// <summary>
    /// Задача закончена
    /// </summary>
    /// <returns>Завершённое время задачи покрывает всё время задачи</returns>
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
        if (Testing)
        {
            return (int)Mathf.Clamp(Mathf.Round(CompleteTaskTime / TestingTime * 100), 0, 100);
        }
        else
        {
            return (int)Mathf.Clamp(Mathf.Round(CompleteTaskTime / AllTaskTime * 100), 0, 100);
        }
    }

    #endregion
}
