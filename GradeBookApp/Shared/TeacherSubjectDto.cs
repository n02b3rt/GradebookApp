﻿using System.ComponentModel.DataAnnotations;

namespace GradeBookApp.Shared;

public class TeacherSubjectDto
{
    public string Id { get; set; } = "";
    [Required]
    public string TeacherId { get; set; } = "";
    [Required]
    public string SubjectId { get; set; } = "";
    public int ClassId { get; set; }
}