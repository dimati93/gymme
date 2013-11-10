using Gymme.Data.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymme.ViewModel
{
    public class WorkoutVM : Base.ViewModelBase
    {
        private Workout _workout;

        private string _title;
        private string _note;

        private bool _exercisesLoaded;

        public WorkoutVM()
            : this (new Workout())
        {
        }

        public WorkoutVM(Workout workout)
        {
            _workout = workout;
            RollBack();
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
            Title = _workout.Title;
            Note = _workout.Note;
        }        
    }
}
