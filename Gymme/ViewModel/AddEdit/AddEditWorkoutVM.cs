using Gymme.Data.Models;
using Gymme.Data.Repository;

namespace Gymme.ViewModel.AddEdit
{
    public class AddEditWorkoutVM : AddEditVM
    {
        private readonly Workout _item;

        private string _title = string.Empty;
        private string _note = string.Empty;

        public AddEditWorkoutVM()
        {
            _item = new Workout();
            PageName = Resources.AppResources.AddEdit_NewWorkout;
        }

        public string Title
        {
            get
            {
                return _title;
            }

            set
            {
                if (_title != value)
                {
                    _title = value;
                    NotifyPropertyChanged("Title");
                }
            }

        }

        public string Note
        {
            get
            {
                return _note;
            }

            set
            {
                if (_note != value)
                {
                    _note = value;
                    NotifyPropertyChanged("Note");
                }
            }
        }

        private void RollBack()
        {
            Title = _item.Title;
            Note = _item.Note;
        }

        public override void Commit()
        {
            _item.Title = Title;
            _item.Note = Note;

            RepoWorkout.Instance.Save(_item);
        }
    }
}
