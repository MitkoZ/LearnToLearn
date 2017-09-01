using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;

namespace Repositories
{
    public class CourseRepository : BaseRepository<Course, int>
    {
        public CourseRepository(LearnToLearnContext context) : base(context)
        {
        }
    }
}
