using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace CompanyApp
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Position> Positions { get; set; } = new();
    }

    public class Position
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public List<Employee> Employees { get; set; } = new();
    }

    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PositionId { get; set; }
        public Position Position { get; set; }
    }

    public class CompanyContext : DbContext
    {
        public DbSet<Department> Departments { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=CompanyDB;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>()
                .HasMany(d => d.Positions)
                .WithOne(p => p.Department)
                .HasForeignKey(p => p.DepartmentId);

            modelBuilder.Entity<Position>()
                .HasMany(p => p.Employees)
                .WithOne(e => e.Position)
                .HasForeignKey(e => e.PositionId);
        }
    }

    class Program
    {
        static void Main()
        {
            using var context = new CompanyContext();
            context.Database.EnsureCreated();

            while (true)
            {
                Console.WriteLine("\n1. Добавить отдел\n2. Добавить должность\n3. Добавить сотрудника\n4. Показать данные\n5. Выход");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddDepartment(context);
                        break;
                    case "2":
                        AddPosition(context);
                        break;
                    case "3":
                        AddEmployee(context);
                        break;
                    case "4":
                        DisplayData(context);
                        break;
                    case "5":
                        return;
                }
            }
        }

        static void AddDepartment(CompanyContext context)
        {
            Console.Write("Название отдела: ");
            var name = Console.ReadLine();
            context.Departments.Add(new Department { Name = name });
            context.SaveChanges();
        }

        static void AddPosition(CompanyContext context)
        {
            Console.WriteLine("Доступные отделы:");
            foreach (var d in context.Departments)
                Console.WriteLine($"{d.Id}. {d.Name}");

            Console.Write("ID отдела: ");
            var departmentId = int.Parse(Console.ReadLine());

            Console.Write("Название должности: ");
            var title = Console.ReadLine();

            context.Positions.Add(new Position
            {
                Title = title,
                DepartmentId = departmentId
            });
            context.SaveChanges();
        }

        static void AddEmployee(CompanyContext context)
        {
            Console.WriteLine("Доступные должности:");
            foreach (var p in context.Positions.Include(p => p.Department))
                Console.WriteLine($"{p.Id}. {p.Title} ({p.Department.Name})");

            Console.Write("ID должности: ");
            var positionId = int.Parse(Console.ReadLine());

            Console.Write("Имя сотрудника: ");
            var name = Console.ReadLine();

            context.Employees.Add(new Employee
            {
                Name = name,
                PositionId = positionId
            });
            context.SaveChanges();
        }

        static void DisplayData(CompanyContext context)
        {
            var departments = context.Departments
                .Include(d => d.Positions)
                .ThenInclude(p => p.Employees);

            foreach (var d in departments)
            {
                Console.WriteLine($"\nОтдел: {d.Name}");
                foreach (var p in d.Positions)
                {
                    Console.WriteLine($"  Должность: {p.Title}");
                    foreach (var e in p.Employees)
                        Console.WriteLine($"    Сотрудник: {e.Name}");
                }
            }
        }
    }
}