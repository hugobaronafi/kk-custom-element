﻿using System;

namespace Core.GitHub.Models
{
    public class Person
    {
        public string? Name { get; set; }

        public string? Email { get; set; }

        public DateTimeOffset Date { get; set; }
    }
}