using UnityEngine;

/// <summary>
/// ����� ������ ����������
/// </summary>
public class EmployeeTask
{
    #region ��������

    /// <summary>
    /// �������� ������
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// �������� ������
    /// </summary>
    public string Description { get; private set; }

    /// <summary>
    /// �� ����� �� ���������� ������
    /// </summary>
    public int AllTaskTime { get; private set; }

    /// <summary>
    /// �����, ������� ��� ������� ��� ����������
    /// </summary>
    public int CompleteTaskTime { get; set; }

    /// <summary>
    /// ������������� ����� �� ������������ ������. ������ - ������� ����� �� ������� ������ ��� �������, 
    /// �� �� ������ 15 �����
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
    /// �������������� ������� ���������� ������
    /// </summary>
    public int Progress => CalculateProgressPercent();

    /// <summary>
    /// ��� ������
    /// </summary>
    public EmployeeTaskType Type { get; private set; }

    /// <summary>
    /// ��� ������ ������ �� ������������
    /// </summary>
    public bool Testing { get; set; } = false;

    /// <summary>
    /// ��� ������ ��������� ��� ������
    /// </summary>
    public bool IsCorrectTask { get; private set; }

    /// <summary>
    /// ��������� ������. ������ �� ���������� ��������� � ����������� ������
    /// </summary>
    public int Complexity { get; private set; }

    /// <summary>
    /// ���������, ����������� �� ���������� ������
    /// </summary>
    public Employee Worker { get; private set; }

    /// <summary>
    /// ���������, ����������� �� ������������ ������
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

    #region ������

    #region ���� �� ������������

    public string GenerateErrorMessage()
    {
        return "����������� " + Tester.Name + " ��������, ��� ������ " + Name + " ��������� � �������. ��� ������ ������ ����" +
            " ������ ��������� " + Worker.Name;
    }

    #endregion

    /// <summary>
    /// ��������� ���������� �� ��� ������
    /// </summary>
    /// <param name="worker">���������</param>
    public void SetWorkerToThisTask(Employee worker)
    {
        Worker = worker;
    }
    /// <summary>
    /// �������� �������� ��������� ������ � ���� �������
    /// </summary>
    public void FinalWorkForThisTask()
    {
        ResetProgress();

        //��������� ��������� ������
        int difficultLevel = Random.Range(1, 10) * Complexity;

        //��������� ����������� ����������
        int employeeLevel = EmployeeStatsSettings.baseEmployeeTaskLevel + Worker.ExperienceHours -
            (Worker.Fatigue * EmployeeStatsSettings.fatigueTaskDificultyMultiplier +
            Worker.Stress * EmployeeStatsSettings.stressTaskDificultyMultiplier);

        IsCorrectTask = employeeLevel > difficultLevel;
        Testing = true;
        Worker.FatigueLevelUP(Complexity);
    }

    /// <summary>
    /// ��������� ������������ �� ��� ������
    /// </summary>
    /// <param name="tester">��������� - �����������</param>
    public void SetTesterToThisTask(Employee tester)
    {
        Tester = tester;
    }
    /// <summary>
    /// ��������� ��������� ��������� ������������ ���� 
    /// </summary>
    /// <returns>��������� �� ��� ������ ��� ������</returns>
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
    /// �������� �������� �� ���� ������
    /// </summary>
    public void ResetProgress()
    {
        CompleteTaskTime = 0;
    }

    /// <summary>
    /// ������ ���������
    /// </summary>
    /// <returns>����������� ����� ������ ��������� �� ����� ������</returns>
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
