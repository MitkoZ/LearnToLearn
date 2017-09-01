using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;

namespace Repositories
{
    public class EnrollmentRepository : BaseRepository<Enrollment, int>
    {
        public EnrollmentRepository(LearnToLearnContext context) : base(context)
        {
        }
    }
}
