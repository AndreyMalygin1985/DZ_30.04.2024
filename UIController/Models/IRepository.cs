using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIController.Models
{
    public interface IRepository
    {
        void Add(TeacherModel studentModel);
        void Edit(TeacherModel studentModel);
        void Delete(int id);
        IEnumerable<TeacherModel> GetAll();
        IEnumerable<TeacherModel> GetByValue(string value);

    }
}
