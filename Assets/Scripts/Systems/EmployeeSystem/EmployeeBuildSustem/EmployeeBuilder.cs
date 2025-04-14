public static class EmployeeBuilder 
{
    public static Employee GetEmployee(EmployeeBuilderInfo info)
    {
        return  new Employee(info.name, info.baseSalary, info.costOfAttracting, info.experienceInHour,
            info.statsPack, info.speachPack);
    }
}
