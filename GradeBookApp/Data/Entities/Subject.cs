﻿using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace GradeBookApp.Data.Entities;

[Index(nameof(Name), IsUnique = true)]
[Index(nameof(ShortName), IsUnique = true)]
public class Subject
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString(); // lub użyj int, jeśli prostszy klucz

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string ShortName { get; set; } = string.Empty;

    public ICollection<Grade> Grades { get; set; } = new List<Grade>();
    public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
    public ICollection<TeacherSubject> TeacherSubjects { get; set; } = new List<TeacherSubject>();
}