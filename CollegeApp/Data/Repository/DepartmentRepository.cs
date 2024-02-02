namespace CollegeApp.Data.Repository
{
    public class DepartmentRepository : CollegeRepository<Department>, IDepartmentRepository
    {
        private readonly CollegeDbContext _context;
        public DepartmentRepository(CollegeDbContext context)
            :base(context)
        {
            _context = context;
        }
    }
}
