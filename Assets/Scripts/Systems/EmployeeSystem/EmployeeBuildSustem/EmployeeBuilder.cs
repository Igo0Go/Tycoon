public static class EmployeeBuilder 
{
    public static Employee GetEmployee(EmployeeBuilderInfo info)
    {
        return  new Employee(info.name, info.baseSalary, info.overtimeSalaryMultiplier, info.hospitalSalaryMultiplier,
            info.costOfAttracting);
    }
}
