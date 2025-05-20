class Program
{
    static void Main(string[] args)
    {
        // Инициализация базы данных
        InitializeDatabase();

        Console.WriteLine("Демонстрация различных способов загрузки связанных данных:");

        // 1. Eager Loading (метод Include)
        Console.WriteLine("\n1. Eager Loading (метод Include):");
        EagerLoadingExample();

        // 2. Explicit Loading (явная загрузка)
        Console.WriteLine("\n2. Explicit Loading (явная загрузка):");
        ExplicitLoadingExample();

        // 3. Lazy Loading (ленивая загрузка)
        Console.WriteLine("\n3. Lazy Loading (ленивая загрузка):");
        LazyLoadingExample();
    }

    static void InitializeDatabase()
    {
        using (var context = new CompanyContext())
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Создаем отделы
            var devDepartment = new Department { Name = "Отдел разработки" };
            var designDepartment = new Department { Name = "Отдел дизайна" };

            // Создаем должности
            var csharpPosition = new Position { Title = "Разработчик на C#", Department = devDepartment };
            var javaPosition = new Position { Title = "Разработчик на Java", Department = devDepartment };
            var uiDesignerPosition = new Position { Title = "Дизайнер интерфейсов", Department = designDepartment };

            // Создаем сотрудников
            var emp1 = new Employee { Name = "Иван Иванов", Email = "ivan@example.com", Position = csharpPosition };
            var emp2 = new Employee { Name = "Петр Петров", Email = "petr@example.com", Position = csharpPosition };
            var emp3 = new Employee { Name = "Сидор Сидоров", Email = "sidor@example.com", Position = javaPosition };
            var emp4 = new Employee { Name = "Анна Архипова", Email = "anna@example.com", Position = uiDesignerPosition };

            context.AddRange(devDepartment, designDepartment);
            context.AddRange(csharpPosition, javaPosition, uiDesignerPosition);
            context.AddRange(emp1, emp2, emp3, emp4);

            context.SaveChanges();
        }
    }

    static void EagerLoadingExample()
    {
        using (var context = new CompanyContext())
        {
            // Загружаем сотрудников вместе с должностями и отделами
            var employees = context.Employees
                .Include(e => e.Position)
                    .ThenInclude(p => p.Department)
                .ToList();

            foreach (var emp in employees)
            {
                Console.WriteLine($"Сотрудник: {emp.Name}, Email: {emp.Email}");
                Console.WriteLine($"Должность: {emp.Position.Title}");
                Console.WriteLine($"Отдел: {emp.Position.Department.Name}");
                Console.WriteLine();
            }
        }
    }

    static void ExplicitLoadingExample()
    {
        using (var context = new CompanyContext())
        {
            // Загружаем сотрудника без связанных данных
            var employee = context.Employees.First();
            Console.WriteLine($"Сотрудник: {employee.Name}, Email: {employee.Email}");

            // Явно загружаем должность
            context.Entry(employee)
                .Reference(e => e.Position)
                .Load();

            Console.WriteLine($"Должность: {employee.Position.Title}");

            // Явно загружаем отдел через должность
            context.Entry(employee.Position)
                .Reference(p => p.Department)
                .Load();

            Console.WriteLine($"Отдел: {employee.Position.Department.Name}");
        }
    }

    static void LazyLoadingExample()
    {
        using (var context = new CompanyContext())
        {
            // Загружаем сотрудника без связанных данных
            var employee = context.Employees.First();
            Console.WriteLine($"Сотрудник: {employee.Name}, Email: {employee.Email}");

            // Ленивая загрузка сработает при первом обращении к навигационному свойству
            Console.WriteLine($"Должность: {employee.Position.Title}");

            // Ленивая загрузка для отдела через должность
            Console.WriteLine($"Отдел: {employee.Position.Department.Name}");
        }
    }
}
class Program
{
    static void Main(string[] args)
    {
        // Инициализация базы данных
        InitializeDatabase();

        Console.WriteLine("Демонстрация различных способов загрузки связанных данных:");

        // 1. Eager Loading (метод Include)
        Console.WriteLine("\n1. Eager Loading (метод Include):");
        EagerLoadingExample();

        // 2. Explicit Loading (явная загрузка)
        Console.WriteLine("\n2. Explicit Loading (явная загрузка):");
        ExplicitLoadingExample();

        // 3. Lazy Loading (ленивая загрузка)
        Console.WriteLine("\n3. Lazy Loading (ленивая загрузка):");
        LazyLoadingExample();
    }

    static void InitializeDatabase()
    {
        using (var context = new CompanyContext())
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Создаем отделы
            var devDepartment = new Department { Name = "Отдел разработки" };
            var designDepartment = new Department { Name = "Отдел дизайна" };

            // Создаем должности
            var csharpPosition = new Position { Title = "Разработчик на C#", Department = devDepartment };
            var javaPosition = new Position { Title = "Разработчик на Java", Department = devDepartment };
            var uiDesignerPosition = new Position { Title = "Дизайнер интерфейсов", Department = designDepartment };

            // Создаем сотрудников
            var emp1 = new Employee { Name = "Иван Иванов", Email = "ivan@example.com", Position = csharpPosition };
            var emp2 = new Employee { Name = "Петр Петров", Email = "petr@example.com", Position = csharpPosition };
            var emp3 = new Employee { Name = "Сидор Сидоров", Email = "sidor@example.com", Position = javaPosition };
            var emp4 = new Employee { Name = "Анна Архипова", Email = "anna@example.com", Position = uiDesignerPosition };

            context.AddRange(devDepartment, designDepartment);
            context.AddRange(csharpPosition, javaPosition, uiDesignerPosition);
            context.AddRange(emp1, emp2, emp3, emp4);

            context.SaveChanges();
        }
    }

    static void EagerLoadingExample()
    {
        using (var context = new CompanyContext())
        {
            // Загружаем сотрудников вместе с должностями и отделами
            var employees = context.Employees
                .Include(e => e.Position)
                    .ThenInclude(p => p.Department)
                .ToList();

            foreach (var emp in employees)
            {
                Console.WriteLine($"Сотрудник: {emp.Name}, Email: {emp.Email}");
                Console.WriteLine($"Должность: {emp.Position.Title}");
                Console.WriteLine($"Отдел: {emp.Position.Department.Name}");
                Console.WriteLine();
            }
        }
    }

    static void ExplicitLoadingExample()
    {
        using (var context = new CompanyContext())
        {
            // Загружаем сотрудника без связанных данных
            var employee = context.Employees.First();
            Console.WriteLine($"Сотрудник: {employee.Name}, Email: {employee.Email}");

            // Явно загружаем должность
            context.Entry(employee)
                .Reference(e => e.Position)
                .Load();

            Console.WriteLine($"Должность: {employee.Position.Title}");

            // Явно загружаем отдел через должность
            context.Entry(employee.Position)
                .Reference(p => p.Department)
                .Load();

            Console.WriteLine($"Отдел: {employee.Position.Department.Name}");
        }
    }

    static void LazyLoadingExample()
    {
        using (var context = new CompanyContext())
        {
            // Загружаем сотрудника без связанных данных
            var employee = context.Employees.First();
            Console.WriteLine($"Сотрудник: {employee.Name}, Email: {employee.Email}");

            // Ленивая загрузка сработает при первом обращении к навигационному свойству
            Console.WriteLine($"Должность: {employee.Position.Title}");

            // Ленивая загрузка для отдела через должность
            Console.WriteLine($"Отдел: {employee.Position.Department.Name}");
        }
    }
}