﻿using Microsoft.EntityFrameworkCore;

namespace CollegeApp.Data.Repository
{
    public class StudentRepository : CollegeRepository<Student>, IStudentRepository
    {
        private readonly CollegeDbContext _context;

        public StudentRepository(CollegeDbContext context)
            : base(context)
        {
            _context = context;
        }

        public Task<List<Student>> GetStudentsByFeesStatusAsync(int feeStatus)
        {
            return null;
        }
    }
}
