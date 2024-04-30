using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UIController.Models;
using UIController.Views;

namespace UIController.Presenters
{
    public class TeacherPresenter
    {
        private IView view;
        private IRepository repository;
        private BindingSource teacherBindingSource;
        private IEnumerable<TeacherModel> teacherList;

        public TeacherPresenter(IView view, IRepository repository)
        {
            this.teacherBindingSource = new BindingSource();
            this.view = view;
            this.repository = repository;

            //Подписываемся на события

            this.view.SearchEvent += SearchTeacher;
            this.view.AddEvent += AddNewTeacher;
            this.view.EditEvent += LoadSelectedTeacherToEdit;
            this.view.DeleteEvent += DeleteSelectedTeacher;
            this.view.SaveEvent += SaveTeacher;
            this.view.CancelEvent += CancelAction;

            //Устанавливаем привязку к ресурсам
            //this.view.SetTeacherListBindingSource(teacherBindingSource);

            this.view.Show();

            LoadAllTeacherList();

        }

        private void LoadAllTeacherList()
        {
            teacherList = repository.GetAll();
            teacherBindingSource.DataSource = teacherList;
        }

        private void CancelAction(object sender, EventArgs e)
        {
            CleanviewFilds();

        }

        private void SaveTeacher(object sender, EventArgs e)
        {
            var model = new TeacherModel();
            model.Id = Convert.ToInt32(view.TeacherId);
            model.Name = view.Name;
            model.Age = Convert.ToInt32(view.Age);
            // делаем валидацию в Presenter -> Common
            try
            {
                new Common.ModelDataValidation().Validate(model);
                if (view.IsEdit)
                {
                    repository.Edit(model);
                    view.Message = "Преподаватель изменён успешно.";
                }
                else
                {
                    repository.Add(model);
                    view.Message = "Преподаватель создан!";
                }
                view.IsSuccessful = true;
                LoadAllTeacherList();
                CleanviewFilds();

            }
            catch (Exception ex)
            {
                view.IsSuccessful = false;
                view.Message = ex.Message;
            }

        }

        private void CleanviewFilds()
        {
            view.TeacherId = "0";
            view.Name = "";
            view.Age = "";
        }

        private void DeleteSelectedTeacher(object sender, EventArgs e)
        {
            try
            {
                var teacher = (TeacherModel)teacherBindingSource.Current;
                repository.Delete(teacher.Id);
                view.IsSuccessful = true;
                view.Message = "Препадователь удалён.";
                LoadAllTeacherList();

            }
            catch (Exception ex)
            {
                view.IsSuccessful = false;
                view.Message = "При удалении что-то пошло не так.";
            }
        }

        private void LoadSelectedTeacherToEdit(object sender, EventArgs e)
        {
            var teacher = (TeacherModel)teacherBindingSource.Current;
            view.TeacherId = teacher.Id.ToString();
            view.Name = teacher.Name;
            view.Age = teacher.Age.ToString();
            view.IsEdit = true;
        }

        private void AddNewTeacher(object sender, EventArgs e)
        {
            view.IsEdit = false;
        }

        private void SearchTeacher(object sender, EventArgs e)
        {
            bool emptyValue = string.IsNullOrWhiteSpace(this.view.SearchValue);
            if (emptyValue == false)
                teacherList = repository.GetByValue(this.view.SearchValue);
            else teacherList = repository.GetAll();
            teacherBindingSource.DataSource = teacherList;

        }
    }
}

